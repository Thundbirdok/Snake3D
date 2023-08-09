namespace Game.Snake.PartsPoses
{
    using System;
    using System.Runtime.CompilerServices;
    using Unity.Burst;
    using Unity.Collections;
    using Unity.Collections.LowLevel.Unsafe;
    using Unity.Jobs;
    using Unity.Mathematics;

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
