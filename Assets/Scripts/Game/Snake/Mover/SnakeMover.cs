namespace Game.Snake.Mover
{
    using System;
    using System.Collections.Generic;
    using Unity.Burst;
    using Unity.Collections;
    using Unity.Collections.LowLevel.Unsafe;
    using Unity.Jobs;
    using UnityEngine;
    using Utility;

    [Serializable]
    public class SnakeMover
    {
        public List<SnakePartPose> PartsTargetPose => _partsTargetPoseHandler.PartsTargetPose;

        public Vector3 TailPreviousTargetPosition => _partsTargetPoseHandler.TailPreviousTargetPosition;
        
        public float Delay => timer.Duration;

        [SerializeField]
        private Timer timer;
        
        [SerializeField]
        private float moveTime = 0.25f;

        private Snake _snake;

        private PartsTargetPoseHandler _partsTargetPoseHandler;
        
        private bool _isPartsMoved;
        
        private NativeArray<Vector3> _targetPositions;
        private NativeArray<Quaternion> _targetRotations;
        
        private NativeArray<Vector3> _partsPositions;
        private NativeArray<Quaternion> _partsRotations;
        
        public void Construct(Snake snake)
        {
            _snake = snake;

            _partsTargetPoseHandler = new PartsTargetPoseHandler(snake);

            _snake.OnPartAdded += AddTargetForLastPart;
        }

        public void Dispose()
        {
            if (_snake != null)
            {
                _snake.OnPartAdded -= AddTargetForLastPart;
            }
        }

        public void Setup()
        {
            timer.SetTimeToMax();

            _partsTargetPoseHandler.SetPartsToTargets();
            
            SetNativeArrays();
        }

        public bool IsTimeToSetNewTargetPositions(float time)
        {
            var isTime = timer.AddTime(time);

            var isForceTurnCauseInput = _isPartsMoved && _snake.DirectionController.IsUpdated;

            return isTime || isForceTurnCauseInput;
        } 
        
        public void MoveParts()
        {
            if (_isPartsMoved)
            {
                return;
            }
            
            var delta = Time.fixedDeltaTime / moveTime;

            _isPartsMoved = true;

            for (var i = 0; i < _snake.Parts.Count; i++)
            {
                var target = _partsTargetPoseHandler.PartsTargetPose[i];
                
                _targetPositions[i] = target.Position;
                _targetRotations[i] = target.Rotation;
            }
            
            var positionJob = new MovePositionToTargetJob()
            {
                PartsTargetPositions = _targetPositions,
                PartsPositions = _partsPositions,
                Delta = delta
            }.Schedule(_snake.Parts.Count, 64);

            var rotationJob = new MoveRotationToTargetJob()
            {
                PartsTargetRotations = _targetRotations,
                PartsRotations = _partsRotations,
                Delta = delta
            }.Schedule(_snake.Parts.Count, 64);
            
            positionJob.Complete();
            rotationJob.Complete();
            
            for (var i = 0; i < PartsTargetPose.Count; i++)
            {
                _snake.Parts[i].Position = _partsPositions[i];
                _snake.Parts[i].Rotation = _partsRotations[i];
                
                var position = _snake.Parts[i].Position;
                var rotation = _snake.Parts[i].Rotation;

                var targetPosition = PartsTargetPose[i].Position;
                var targetRotation = PartsTargetPose[i].Rotation;

                if (position != targetPosition
                    || rotation != targetRotation)
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
            
            _partsTargetPoseHandler.SetTargetPositions();
        }

        private void SetNativeArrays()
        {
            _targetPositions = new NativeArray<Vector3>(_snake.Parts.Count, Allocator.Persistent);
            _targetRotations = new NativeArray<Quaternion>(_snake.Parts.Count, Allocator.Persistent);

            _partsPositions = new NativeArray<Vector3>(_snake.Parts.Count, Allocator.Persistent);
            _partsRotations = new NativeArray<Quaternion>(_snake.Parts.Count, Allocator.Persistent);
            
            for (var i = 0; i < _snake.Parts.Count; i++)
            {
                var part = _snake.Parts[i];

                _partsPositions[i] = part.Position;
                _partsRotations[i] = part.Rotation;
            }
        }

        private void AddTargetForLastPart()
        {
            _partsTargetPoseHandler.AddTargetForLastPart();
            
            SetNativeArrays();
        }
    }
    
    [BurstCompile]
    public struct MoveRotationToTargetJob : IJobParallelFor
    {
        [ReadOnly]
        public NativeArray<Quaternion> PartsTargetRotations;

        [NativeDisableContainerSafetyRestriction]
        public NativeArray<Quaternion> PartsRotations;

        [ReadOnly]
        public float Delta;

        [BurstCompile]
        public void Execute(int index)
        {
            var targetRotation = PartsTargetRotations[index];
            var partRotation = PartsRotations[index];
                
            var newRotation = Quaternion.RotateTowards
            (
                partRotation,
                targetRotation,
                Delta * 90
            );

            PartsRotations[index] = newRotation;
        }
    }
        
    [BurstCompile]
    public struct MovePositionToTargetJob : IJobParallelFor
    {
        [ReadOnly]
        public NativeArray<Vector3> PartsTargetPositions;

        [NativeDisableContainerSafetyRestriction]
        public NativeArray<Vector3> PartsPositions;

        [ReadOnly]
        public float Delta;

        [BurstCompile]
        public void Execute(int index)
        {
            var targetPosition = PartsTargetPositions[index];
            var partPosition = PartsPositions[index];

            var newPosition = Vector3.MoveTowards
            (
                partPosition,
                targetPosition,
                Delta
            );

            PartsPositions[index] = newPosition;
        }
    }
}
