namespace Game.Snake.PartsTargetPoses
{
    using Unity.Burst;
    using Unity.Collections;
    using Unity.Jobs;
    using Unity.Mathematics;

    [BurstCompile]
    public struct SetPartsTargetPositionsJob : IJobParallelFor
    {
        [ReadOnly]
        public NativeArray<float3> OldPositions;
            
        [WriteOnly, NativeDisableParallelForRestriction]
        public NativeArray<float3> NewPositions;
            
        [BurstCompile]
        public void Execute(int index)
        {
            NewPositions[index + 1] = OldPositions[index];
        }
    }
}
