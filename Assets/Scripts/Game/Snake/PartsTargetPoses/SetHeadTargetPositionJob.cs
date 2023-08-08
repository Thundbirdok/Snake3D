namespace Game.Snake.PartsTargetPoses
{
    using Unity.Burst;
    using Unity.Collections;
    using Unity.Jobs;
    using Unity.Mathematics;

    [BurstCompile]
    public struct SetHeadTargetPositionJob : IJob
    {
        [ReadOnly]
        public float3 OldHeadPosition;
            
        [ReadOnly]
        public float3 Forward;

        [WriteOnly, NativeDisableParallelForRestriction]
        public NativeArray<float3> NewPositions;
            
        [BurstCompile]
        public void Execute()
        {
            NewPositions[0] = OldHeadPosition + Forward;
        }
    }
}
