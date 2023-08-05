using UnityEngine;

namespace Game.Snake.Mover
{
    using System.Collections.Generic;
    using System.Linq;

    public class PartsTargetPoseHandler
    {
        public List<SnakePartPose> PartsTargetPose { get; private set; }
        
        public Vector3 TailPreviousTargetPosition { get; private set; }

        private readonly Snake _snake;
        
        public PartsTargetPoseHandler(Snake snake)
        {
            _snake = snake;
            
            PartsTargetPose = new List<SnakePartPose>(_snake.Parts.Count);
        }

        ~PartsTargetPoseHandler()
        {
            PartsTargetPose.Clear();
            PartsTargetPose = null;
        }
        
        public void SetTargetPositions()
        {
            var head = PartsTargetPose[0].Position;
    
            var forward = _snake.Forward;
            var up = _snake.Up;
    
            var newPosition = head + forward;
    
            var newRotation = 
                Quaternion.LookRotation(forward, up);
    
            TailPreviousTargetPosition = PartsTargetPose.Last().Position;
                
            foreach (var pose in PartsTargetPose)
            {
                var position = pose.Position;
                var rotation = pose.Rotation;
    
                pose.Position = newPosition;
                pose.Rotation = newRotation;
                
                newPosition = position;
                newRotation = rotation;
            }
        }
    
        public void SetPartsToTargets()
        {
            if (PartsTargetPose == null)
            {
                return;
            }

            PartsTargetPose.Clear();

            foreach (var part in _snake.Parts)
            {
                SetPartToTargets(part);
            }
        }
    
        public void AddTargetForLastPart()
        {
            var part = _snake.Parts.Last();
    
            SetPartToTargets(part);
        }
    
        private void SetPartToTargets(SnakePartPose part)
        {
            var localPosition = part.Position;
            var localRotation = part.Rotation;

            var newPose = new SnakePartPose(localPosition, localRotation);
            
            PartsTargetPose.Add(newPose);
        }
    }

    public class SnakePartPose
    {
        public Vector3 Position;
        public Quaternion Rotation;

        public SnakePartPose(Vector3 position, Quaternion rotation)
        {
            Position = position;
            Rotation = rotation;
        }
    }
}
