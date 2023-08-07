namespace Game.Snake.Mover
{
    using System;
    using System.Runtime.CompilerServices;
    using Unity.Burst;
    using Unity.Collections;
    using Unity.Collections.LowLevel.Unsafe;
    using Unity.Jobs;
    using Unity.Mathematics;
    using UnityEngine;
    using Utility;

    [Serializable]
    public class SnakeMover
    {
        public float3[] PartsTargetsPositions => _partsTargetPosesHandler.Positions.ToArray();
        public quaternion[] PartsTargetsRotations => _partsTargetPosesHandler.Rotations.ToArray();
        
        public float Delay => timer.Duration;

        [SerializeField]
        private Timer timer;
        
        [SerializeField]
        private float moveTime = 0.25f;

        private SnakePartsPosesHandler _partsPosesHandler;
        private PartsTargetPosesHandler _partsTargetPosesHandler;
        private SnakeDirectionController _directionController;
        
        private bool _isPartsMoved;

        private JobHandle _positionJob;
        private JobHandle _rotationJob;
        
        public void Construct
        (
            SnakePartsPosesHandler partsPosesHandler,
            PartsTargetPosesHandler partsTargetPosesHandler,
            SnakeDirectionController directionController
        )
        {
            _partsPosesHandler = partsPosesHandler;
            _partsTargetPosesHandler = partsTargetPosesHandler;
            _directionController = directionController;
            
            _partsPosesHandler.OnPartAdded += AddTargetForLastPart;
        }

        public void Dispose()
        {
            if (_partsPosesHandler != null)
            {
                _partsPosesHandler.OnPartAdded -= AddTargetForLastPart;
            }
        }

        public void Setup()
        {
            timer.SetTimeToMax();
        }

        public bool IsTimeToSetNewTargetPositions(float time)
        {
            var isTime = timer.AddTime(time);

            var isForceTurnCauseInput = _isPartsMoved && _directionController.IsUpdated;

            return isTime || isForceTurnCauseInput;
        } 
        
        public void ScheduleMoveParts()
        {
            if (_isPartsMoved)
            {
                return;
            }
            
            var delta = Time.fixedDeltaTime / moveTime;

            _isPartsMoved = true;
            
            _positionJob = new MovePositionToTargetJob()
            {
                PartsTargetPositions = _partsTargetPosesHandler.Positions,
                PartsPositions = _partsPosesHandler.PartsPositions,
                Delta = delta
            }.Schedule(_partsTargetPosesHandler.Positions.Length, 64);

            _rotationJob = new MoveRotationToTargetJob()
            {
                PartsTargetRotations = _partsTargetPosesHandler.Rotations,
                PartsRotations = _partsPosesHandler.PartsRotations,
                Delta = delta
            }.Schedule(_partsTargetPosesHandler.Rotations.Length, 64);
        }

        public void MoveParts()
        {
            _positionJob.Complete();
            _rotationJob.Complete();

            _isPartsMoved = true;
            
            for (var i = 0; i < PartsTargetsPositions.Length; i++)
            {
                var position = _partsPosesHandler.PartsPositions[i];
                var rotation = _partsPosesHandler.PartsRotations[i];

                var targetPosition = PartsTargetsPositions[i];
                var targetRotation = PartsTargetsRotations[i];

                if (position.Equals(targetPosition) == false)
                {
                    _isPartsMoved = false;
                }
                else if (rotation.Equals(targetRotation) == false)
                {
                    _isPartsMoved = false;
                }
            }
        }
        
        public void SetTargetPositions()
        {
            if (_isPartsMoved == false)
            {
                return;
            }
        
            _isPartsMoved = false;
            timer.ResetTime();
            
            _partsTargetPosesHandler.SetTargetPositions();
        }

        private void AddTargetForLastPart()
        {
            _partsTargetPosesHandler.AddTargetForLastPart();
        }
    }
    
    [BurstCompile]
    public struct MoveRotationToTargetJob : IJobParallelFor
    {
        [ReadOnly]
        public NativeArray<quaternion> PartsTargetRotations;

        [NativeDisableContainerSafetyRestriction]
        public NativeArray<quaternion> PartsRotations;

        [ReadOnly]
        public float Delta;

        [BurstCompile]
        public void Execute(int index)
        {
            var targetRotation = PartsTargetRotations[index];
            var partRotation = PartsRotations[index];
                
            var newRotation = RotateTowards
            (
                partRotation,
                targetRotation,
                Delta * 90
            );

            PartsRotations[index] = newRotation;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public quaternion RotateTowards(quaternion from, quaternion to, float maxDegreesDelta)
        {
            var num = Angle(from, to);
            return (double) num == 0.0 ? to : Quaternion.SlerpUnclamped(from, to, Mathf.Min(1f, maxDegreesDelta / num));
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Angle(quaternion a, quaternion b)
        {
            var num = Mathf.Min(Mathf.Abs(Dot(a, b)), 1f);
            return IsEqualUsingDot(num) ? 0.0f : (float) ((double) Mathf.Acos(num) * 2.0 * 57.295780181884766);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool IsEqualUsingDot(float dot) => (double) dot > 0.9999989867210388;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Dot(quaternion a, quaternion b) => 
            (float) ((double) a.value.x * b.value.x 
                     + (double) a.value.y * b.value.y 
                     + (double) a.value.z * b.value.z 
                     + (double) a.value.w * b.value.w);
    }
        
    [BurstCompile]
    public struct MovePositionToTargetJob : IJobParallelFor
    {
        [ReadOnly]
        public NativeArray<float3> PartsTargetPositions;

        [NativeDisableContainerSafetyRestriction]
        public NativeArray<float3> PartsPositions;

        [ReadOnly]
        public float Delta;

        [BurstCompile]
        public void Execute(int index)
        {
            var targetPosition = PartsTargetPositions[index];
            var partPosition = PartsPositions[index];
            
            var newPosition = MoveTowards
            (
                partPosition,
                targetPosition,
                Delta
            );

            PartsPositions[index] = newPosition;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static float3 MoveTowards
        (
            float3 current,
            float3 target,
            double maxDistanceDelta
        )
        {
            var num1 = target.x - current.x;
            var num2 = target.y - current.y;
            var num3 = target.z - current.z;
            
            var d = (double) num1 * num1 + (double) num2 * num2 + (double) num3 * num3;

            if (
                d == 0.0
                || maxDistanceDelta >= 0.0
                && d <= maxDistanceDelta * maxDistanceDelta
                )
            {
                return target;
            }

            var num4 = (float) Math.Sqrt(d);
            
            return new float3
            (
                (float)(current.x + num1 / num4 * maxDistanceDelta), 
                (float)(current.y + num2 / num4 * maxDistanceDelta),
                (float)(current.z + num3 / num4 * maxDistanceDelta)
            );
        }
    }
}
