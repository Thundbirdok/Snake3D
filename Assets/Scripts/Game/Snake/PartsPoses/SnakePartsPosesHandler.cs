namespace Game.Snake.PartsPoses
{
    using System;
    using System.Linq;
    using Game.Field;
    using Game.Snake.PartsTargetPoses;
    using Unity.Collections;
    using Unity.Mathematics;
    using UnityEngine;

    [Serializable]
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

        public void Grow()
        {
            var tailPreviousTargetPosition = _targetPosesHandler.TailPreviousTargetPosition;

            var tailTargetPosition = _targetPosesHandler.TailTargetPosition;

            var rotation = Quaternion.LookRotation(tailTargetPosition - tailPreviousTargetPosition);
            
            Grow(tailPreviousTargetPosition, rotation);

            OnPartAdded?.Invoke();
        }

        public void SetNewPoses(NativeArray<float3> positions, NativeArray<quaternion> rotations)
        {
            NativeArray<float3>.Copy(positions, PartsPositions);
            NativeArray<quaternion>.Copy(rotations, PartsRotations);
        }
        
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

        private void Grow(Vector3 position, Quaternion rotation)
        {
            var oldPartPosition = PartsPositions;
            var oldPartRotation = PartsRotations;
            
            PartsPositions = new NativeArray<float3>(oldPartPosition.Length + 1, Allocator.Persistent);
            PartsRotations = new NativeArray<quaternion>(oldPartRotation.Length + 1, Allocator.Persistent);

            if (oldPartPosition.IsCreated)
            {
                if (oldPartPosition.Length > 0)
                {
                    NativeArray<float3>.Copy
                        (oldPartPosition, PartsPositions, oldPartPosition.Length);

                    PartsPositions[oldPartPosition.Length] = position;
                }
                
                oldPartPosition.Dispose();
            }

            if (oldPartPosition.IsCreated)
            {
                if (oldPartPosition.Length > 0)
                {
                    NativeArray<quaternion>.Copy
                        (oldPartRotation, PartsRotations, oldPartPosition.Length);

                    PartsRotations[oldPartRotation.Length] = rotation;
                }

                oldPartRotation.Dispose();
            }
        }
    }
}
