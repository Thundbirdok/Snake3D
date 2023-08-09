namespace Game.Snake.PartsTargetPoses
{
    using System.Linq;
    using Game.Snake.PartsPoses;
    using Unity.Burst;
    using Unity.Collections;
    using Unity.Jobs;
    using Unity.Jobs.LowLevel.Unsafe;
    using Unity.Mathematics;

    [BurstCompile]
    public class SnakePartsTargetPosesHandler
    {
        private int _posesCurrentArrayIndex;
        private int _posesNewArrayIndex = 1;
        
        private NativeArray<float3>[] _positionsBuffers;
        public ref NativeArray<float3> Positions => ref _positionsBuffers[_posesCurrentArrayIndex];
        private ref NativeArray<float3> NewPositions => ref _positionsBuffers[_posesNewArrayIndex];
        
        private NativeArray<quaternion>[] _rotationsBuffers;
        public ref NativeArray<quaternion> Rotations => ref _rotationsBuffers[_posesCurrentArrayIndex];
        private ref NativeArray<quaternion> NewRotations => ref _rotationsBuffers[_posesNewArrayIndex];
        
        public float3 HeadTargetPosition => Positions.First();
        public float3 TailTargetPosition => Positions.Last();
        
        public float3 TailPreviousTargetPosition { get; private set; }

        private readonly SnakePartsPosesHandler _partsPosesHandler;
        private readonly SnakeDirectionController _directionController;
        
        private JobHandle _partsTargetPosesJobHandle;
        
        public SnakePartsTargetPosesHandler
        (
            SnakePartsPosesHandler partsPosesHandler, 
            SnakeDirectionController directionController
        )
        {
            _partsPosesHandler = partsPosesHandler;
            _directionController = directionController;
        }

        ~SnakePartsTargetPosesHandler() => DisposeArrays();

        [BurstCompile]
        public void SchedulePartsTargetPoses()
        {
            var head = Positions[0];
    
            var forward = _directionController.Forward;
            var up = _directionController.Up;

            TailPreviousTargetPosition = Positions.Last();

            var positionsJob = new SetPartsTargetPositionsJob()
            {
                OldPositions = Positions,
                NewPositions = NewPositions
            };
            var positionsJobHandle = positionsJob.Schedule
            (
                Positions.Length - 1,
                JobsUtility.JobWorkerMaximumCount / 2
            );

            var rotationsJob = new SetPartsTargetRotationsJob()
            {
                OldRotations = Rotations,
                NewRotations = NewRotations
            };
            var rotationsJobHandle = rotationsJob.Schedule
            (
                Rotations.Length - 1,
                JobsUtility.JobWorkerMaximumCount / 2
            );

            var headPositionJob = new SetHeadTargetPositionJob()
            {
                OldHeadPosition = head,
                Forward = forward,
                NewPositions = _positionsBuffers[_posesNewArrayIndex]
            };
            var headPositionJobHandle = headPositionJob.Schedule(positionsJobHandle);
            
            var headRotationJob = new SetHeadTargetRotationJob()
            {
                Forward = forward,
                Up = up,
                NewRotations = _rotationsBuffers[_posesNewArrayIndex]
            };
            var headRotationJobHandle = headRotationJob.Schedule(rotationsJobHandle);

            _partsTargetPosesJobHandle = JobHandle.CombineDependencies
            (
                headPositionJobHandle,
                headRotationJobHandle
            );
        }

        [BurstCompile]
        public void GetPartsTargetPoses()
        {
            _partsTargetPosesJobHandle.Complete();

            (_posesCurrentArrayIndex, _posesNewArrayIndex) 
                = (_posesNewArrayIndex, _posesCurrentArrayIndex);
        } 
        
        [BurstCompile]
        public void SetPartsToTargets()
        {
            _partsTargetPosesJobHandle.Complete();
            
            DisposeArrays();

            CreateArraysBuffers();
            
            NativeArray<float3>.Copy(_partsPosesHandler.PartsPositions, Positions);
            NativeArray<quaternion>.Copy(_partsPosesHandler.PartsRotations, Rotations);
        }
    
        [BurstCompile]
        public void AddTargetForLastPart()
        {
            _partsTargetPosesJobHandle.Complete();

            GrowArray(ref _positionsBuffers, _posesCurrentArrayIndex, _partsPosesHandler.TailPosition);
            GrowArray(ref _rotationsBuffers, _posesCurrentArrayIndex, _partsPosesHandler.TailRotation);
        }

        [BurstCompile]
        private static void GrowArray<T>(ref NativeArray<T>[] arrayBuffers, int currentIndex, T tail) where T : struct
        {
            var oldArrayBuffers = arrayBuffers;
            var oldArray = arrayBuffers[currentIndex];

            CreateArrayBuffers(out arrayBuffers, oldArray.Length + 1);

            NativeArray<T>.Copy(oldArray, arrayBuffers[currentIndex], oldArray.Length);
            arrayBuffers[currentIndex][oldArray.Length] = tail;

            oldArrayBuffers[0].Dispose();
            oldArrayBuffers[1].Dispose();
        }
        
        [BurstCompile]
        private void CreateArraysBuffers()
        {
            CreateArrayBuffers(out _positionsBuffers, _partsPosesHandler.PartsPositions.Length);
            CreateArrayBuffers(out _rotationsBuffers, _partsPosesHandler.PartsRotations.Length);
        }

        [BurstCompile]
        private void DisposeArrays()
        {
            DisposeArrayBuffers(_positionsBuffers);
            DisposeArrayBuffers(_rotationsBuffers);
        }

        [BurstCompile]
        private static void CreateArrayBuffers<T>(out NativeArray<T>[] arrayBuffers, int size) where T : struct
        {
            arrayBuffers = new NativeArray<T>[2];

            arrayBuffers[0] = new NativeArray<T>
            (
                size,
                Allocator.Persistent
            );
            
            arrayBuffers[1] = new NativeArray<T>
            (
                size,
                Allocator.Persistent
            );
        }
        
        [BurstCompile]
        private static void DisposeArrayBuffers<T>(in NativeArray<T>[] arrayBuffers) where T : struct
        {
            if (arrayBuffers == null)
            {
                return;
            }
            
            if (arrayBuffers[0] != null && arrayBuffers[0].IsCreated)
            {
                arrayBuffers[0].Dispose();
            }

            if (arrayBuffers[1] != null && arrayBuffers[1].IsCreated)
            {
                arrayBuffers[1].Dispose();
            }
        }
    }
}
