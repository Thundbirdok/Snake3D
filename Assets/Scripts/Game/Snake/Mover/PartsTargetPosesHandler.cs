namespace Game.Snake.Mover
{
    using System.Linq;
    using UnityEngine;
    using Unity.Collections;
    using Unity.Mathematics;

    public class PartsTargetPosesHandler
    {
        public NativeArray<float3> Positions;
        public NativeArray<quaternion> Rotations;
        
        public float3 HeadTargetPosition => Positions.First();
        
        public float3 TailTargetPosition => Positions.Last();
        
        public float3 TailPreviousTargetPosition { get; private set; }

        private readonly SnakePartsPosesHandler _partsPosesHandler;
        private readonly SnakeDirectionController _directionController;
        
        public PartsTargetPosesHandler(SnakePartsPosesHandler partsPosesHandler, SnakeDirectionController directionController)
        {
            _partsPosesHandler = partsPosesHandler;
            _directionController = directionController;
        }

        ~PartsTargetPosesHandler()
        {
            Positions.Dispose();
            Rotations.Dispose();
        }
        
        public void SetTargetPositions()
        {
            var head = Positions[0];
    
            var forward = _directionController.Forward;
            var up = _directionController.Up;
    
            var newPosition = head + forward;
    
            var newRotation = 
                Quaternion.LookRotation(forward, up);
    
            TailPreviousTargetPosition = Positions.Last();

            for (var i = 0; i < Positions.Length; i++)
            {
                var position = Positions[i];
                var rotation = Rotations[i];

                Positions[i] = newPosition;
                Rotations[i] = newRotation;

                newPosition = position;
                newRotation = rotation;
            }
        }
    
        public void SetPartsToTargets()
        {
            Positions = new NativeArray<float3>(_partsPosesHandler.PartsPositions.Length, Allocator.Persistent);
            Rotations = new NativeArray<quaternion>(_partsPosesHandler.PartsRotations.Length, Allocator.Persistent);

            NativeArray<float3>.Copy(_partsPosesHandler.PartsPositions, Positions);
            NativeArray<quaternion>.Copy(_partsPosesHandler.PartsRotations, Rotations);
        }
    
        public void AddTargetForLastPart()
        {
            var oldPartTargetPositions = Positions;
            var oldPartTargetRotations = Rotations;
            
            Positions = new NativeArray<float3>(_partsPosesHandler.PartsPositions.Length, Allocator.Persistent);
            Rotations = new NativeArray<quaternion>(_partsPosesHandler.PartsRotations.Length, Allocator.Persistent);
            
            NativeArray<float3>.Copy(oldPartTargetPositions, Positions, oldPartTargetPositions.Length);
            Positions[oldPartTargetPositions.Length] = _partsPosesHandler.TailPosition;
            
            NativeArray<quaternion>.Copy(oldPartTargetRotations, Rotations, oldPartTargetPositions.Length);
            Rotations[oldPartTargetPositions.Length] = _partsPosesHandler.TailRotation;
        }
    }
}
