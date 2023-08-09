namespace Game.Snake.PartsTargetPoses
{
    using Unity.Burst;
    using Unity.Collections;
    using Unity.Jobs;
    using Unity.Mathematics;
    using UnityEngine;

    [BurstCompile]
    public struct SetHeadTargetRotationJob : IJob
    {
        [ReadOnly]
        public float3 Forward;

        [ReadOnly]
        public float3 Up;
            
        [WriteOnly, NativeDisableParallelForRestriction]
        public NativeArray<quaternion> NewRotations;
            
        [BurstCompile]
        public void Execute()
        {
            NewRotations[0] = Quaternion.LookRotation
            (
                Forward,
                Up
            );
        }
    }
}
