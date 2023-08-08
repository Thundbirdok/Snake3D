namespace Game.Snake.PartsTargetPoses
{
    using System.Linq;
    using Game.Snake.PartsPoses;
    using Unity.Burst;
    using Unity.Collections;
    using Unity.Jobs;
    using Unity.Mathematics;

    [BurstCompile]
    public class SnakePartsTargetPosesHandler
    {
        public NativeArray<float3> Positions;
        public NativeArray<quaternion> Rotations;
        
        public float3 HeadTargetPosition => Positions.First();
        public float3 TailTargetPosition => Positions.Last();
        
        public float3 TailPreviousTargetPosition { get; private set; }

        private readonly SnakePartsPosesHandler _partsPosesHandler;
        private readonly SnakeDirectionController _directionController;

        private NativeArray<float3> _newPositions;
        private NativeArray<quaternion> _newRotations;

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

        ~SnakePartsTargetPosesHandler()
        {
            DisposeArrays();
        }
        
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
                NewPositions = _newPositions
            };
            var positionsJobHandle = positionsJob.Schedule
            (
                Positions.Length - 1,
                64
            );

            var rotationsJob = new SetPartsTargetRotationsJob()
            {
                OldRotations = Rotations,
                NewRotations = _newRotations
            };
            var rotationsJobHandle = rotationsJob.Schedule
            (
                Rotations.Length - 1,
                64
            );

            var headPositionJob = new SetHeadTargetPositionJob()
            {
                OldHeadPosition = head,
                Forward = forward,
                NewPositions = _newPositions
            };
            var headPositionJobHandle = headPositionJob.Schedule(positionsJobHandle);
            
            var headRotationJob = new SetHeadTargetRotationJob()
            {
                Forward = forward,
                Up = up,
                NewRotations = _newRotations
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
            
            NativeArray<float3>.Copy(_newPositions, Positions);
            NativeArray<quaternion>.Copy(_newRotations, Rotations);
        } 
        
        [BurstCompile]
        public void SetPartsToTargets()
        {
            _partsTargetPosesJobHandle.Complete();
            
            DisposeArrays();

            SetArrays();
            
            NativeArray<float3>.Copy(_partsPosesHandler.PartsPositions, Positions);
            NativeArray<quaternion>.Copy(_partsPosesHandler.PartsRotations, Rotations);
        }
    
        [BurstCompile]
        public void AddTargetForLastPart()
        {
            _partsTargetPosesJobHandle.Complete();
            
            _newPositions.Dispose();
            _newRotations.Dispose();
            
            var oldPartTargetPositions = Positions;
            var oldPartTargetRotations = Rotations;
            
            SetArrays();

            NativeArray<float3>.Copy
            (
                oldPartTargetPositions,
                Positions,
                oldPartTargetPositions.Length
            );
            Positions[oldPartTargetPositions.Length] = _partsPosesHandler.TailPosition;
            
            NativeArray<quaternion>.Copy
            (
                oldPartTargetRotations,
                Rotations,
                oldPartTargetPositions.Length
            );
            Rotations[oldPartTargetPositions.Length] = _partsPosesHandler.TailRotation;

            oldPartTargetPositions.Dispose();
            oldPartTargetRotations.Dispose();
        }

        [BurstCompile]
        private void SetArrays()
        {
            Positions = new NativeArray<float3>
            (
                _partsPosesHandler.PartsPositions.Length,
                Allocator.Persistent
            );

            Rotations = new NativeArray<quaternion>
            (
                _partsPosesHandler.PartsRotations.Length,
                Allocator.Persistent
            );

            _newPositions = new NativeArray<float3>
            (
                _partsPosesHandler.PartsPositions.Length,
                Allocator.Persistent
            );

            _newRotations = new NativeArray<quaternion>
            (
                _partsPosesHandler.PartsRotations.Length,
                Allocator.Persistent
            );
        }

        private void DisposeArrays()
        {
            Positions.Dispose();
            Rotations.Dispose();
            _newPositions.Dispose();
            _newRotations.Dispose();
        }
    }
}
