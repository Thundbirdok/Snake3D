namespace Game.Snake.PartsTargetPoses
{
    using Unity.Burst;
    using Unity.Collections;
    using Unity.Jobs;
    using Unity.Mathematics;

    [BurstCompile]
    public struct SetPartsTargetRotationsJob : IJobParallelFor
    {
        [ReadOnly]
        public NativeArray<quaternion> OldRotations;
            
        [WriteOnly, NativeDisableParallelForRestriction]
        public NativeArray<quaternion> NewRotations;
            
        [BurstCompile]
        public void Execute(int index)
        {
            NewRotations[index + 1] = OldRotations[index];
        }
    }
}
