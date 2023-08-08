namespace Game.Snake.PartsPoses
{
    using Unity.Burst;
    using Unity.Collections;
    using Unity.Collections.LowLevel.Unsafe;
    using Unity.Jobs;
    using Unity.Mathematics;
    using UnityEngine;

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
                
            var newRotation = Quaternion.RotateTowards
            (
                partRotation,
                targetRotation,
                Delta * 90
            );

            PartsRotations[index] = newRotation;
        }
    }
}
