namespace Game.Snake.PartsPoses
{
    using System;
    using System.Linq;
    using Game.Field;
    using Game.Snake.PartsTargetPoses;
    using Unity.Burst;
    using Unity.Collections;
    using Unity.Mathematics;
    using UnityEngine;

    [Serializable, BurstCompile]
    public class SnakePartsPosesHandler
    {
        public event Action OnPartAdded;
        
        public NativeArray<float3> PartsPositions; 
        public NativeArray<quaternion> PartsRotations;

        public float3 HeadPosition => PartsPositions.First();
        public quaternion HeadRotation => PartsRotations.First();
        
        public float3 TailPosition => PartsPositions.Last();
        public quaternion TailRotation => PartsRotations.Last();
        
        [SerializeField]
        private Field field;

        [SerializeField]
        private int startPartsNumber;
        
        private SnakePartsTargetPosesHandler _targetPosesHandler;

        public void Construct(SnakePartsTargetPosesHandler targetPosesHandler)
        {
            _targetPosesHandler = targetPosesHandler;
        }

        public void Dispose()
        {
            ClearParts();
        }
        
        public void Setup()
        {
            ClearParts();
            SetupParts();
        }

        [BurstCompile]
        public void Grow()
        {
            var tailPreviousTargetPosition = _targetPosesHandler.TailPreviousTargetPosition;

            var tailTargetPosition = _targetPosesHandler.TailTargetPosition;

            var rotation = Quaternion.LookRotation(tailTargetPosition - tailPreviousTargetPosition);
            
            Grow(tailPreviousTargetPosition, rotation);

            OnPartAdded?.Invoke();
        }

        [BurstCompile]
        public void SetNewPoses(NativeArray<float3> positions, NativeArray<quaternion> rotations)
        {
            NativeArray<float3>.Copy(positions, PartsPositions);
            NativeArray<quaternion>.Copy(rotations, PartsRotations);
        }
        
        [BurstCompile]
        private void SetupParts()
        {
            var xPartPosition = (float)field.Size.x / 2 - (field.Size.x % 2 == 0 ? 0.5f : 0);
            var yPartPosition = (float)field.Size.y / 2 - (field.Size.y % 2 == 0 ? 0.5f : 0);
            var zPartPosition = (float)field.Size.z / 2 - (field.Size.z % 2 == 0 ? 0.5f : 0);

            var partPosition = new float3(xPartPosition, yPartPosition, zPartPosition);

            if (PartsPositions.IsCreated == false)
            {
                PartsPositions.Dispose();
            }

            if (PartsRotations.IsCreated == false)
            {
                PartsRotations.Dispose();
            }

            PartsPositions = new NativeArray<float3>(startPartsNumber, Allocator.Persistent);
            PartsRotations = new NativeArray<quaternion>(startPartsNumber, Allocator.Persistent);
            
            for (var i = 0; i < startPartsNumber; i++)
            {
                PartsPositions[i] = partPosition;
                PartsRotations[i] = quaternion.identity;
                
                --partPosition.z;
            }
        }

        [BurstCompile]
        private void ClearParts()
        {
            if (PartsPositions.IsCreated == false)
            {
                PartsPositions.Dispose();
            }

            if (PartsRotations.IsCreated == false)
            {
                PartsRotations.Dispose();
            }
        }

        [BurstCompile]
        private void Grow(Vector3 position, Quaternion rotation)
        {
            GrowArray(ref PartsPositions, position);
            GrowArray(ref PartsRotations, rotation);
        }

        [BurstCompile]
        private void GrowArray<T>(ref NativeArray<T> array, T tail) where T : struct
        {
            var oldArray = array;
            
            array = new NativeArray<T>(oldArray.Length + 1, Allocator.Persistent);
            
            if (oldArray.IsCreated == false)
            {
                array[0] = tail;
                
                return;
            }

            if (oldArray.Length > 0)
            {
                NativeArray<T>.Copy(oldArray, array, oldArray.Length);
            }
            
            array[oldArray.Length] = tail;
                
            oldArray.Dispose();
        }
    }
}
