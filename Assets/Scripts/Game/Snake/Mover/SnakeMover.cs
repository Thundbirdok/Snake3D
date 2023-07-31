namespace Game.Snake.Mover
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using Utility;

    [Serializable]
    public class SnakeMover
    {
        public List<SnakePartPose> PartsTargetPose => _partsTargetPoseHandler.PartsTargetPose;

        public Vector3 TailPreviousTargetPosition => _partsTargetPoseHandler.TailPreviousTargetPosition;
        
        public float Delay => timer.Duration;

        [SerializeField]
        private Timer timer;
        
        [SerializeField]
        private float moveTime = 0.25f;

        private Snake _snake;

        private PartsTargetPoseHandler _partsTargetPoseHandler;
        
        private bool _isPartsMoved;
        
        public void Construct(Snake snake)
        {
            _snake = snake;

            _partsTargetPoseHandler = new PartsTargetPoseHandler(snake);

            _snake.OnPartAdded += AddTargetForLastPart;
        }

        public void Dispose()
        {
            if (_snake != null)
            {
                _snake.OnPartAdded -= AddTargetForLastPart;
            }
        }

        public void Setup()
        {
            timer.ResetTime();

            _partsTargetPoseHandler.SetPartsToTargets();
        }

        public bool IsTimeToSetNewTargetPositions(float time)
        {
            var isTime = timer.AddTime(time);

            var isForceTurnCauseInput = _isPartsMoved && _snake.DirectionController.IsUpdated;

            return isTime || isForceTurnCauseInput;
        } 
        
        public void MoveParts()
        {
            if (_isPartsMoved)
            {
                return;
            }
            
            var t = Time.fixedDeltaTime / moveTime;

            _isPartsMoved = true;
            
            for (var i = 0; i < PartsTargetPose.Count; i++)
            {
                var isAtTargetPosition = MovePartToTarget
                (
                    t, 
                    _snake.Parts[i].transform,
                    PartsTargetPose[i]
                );

                if (isAtTargetPosition == false)
                {
                    _isPartsMoved = false;
                }
            }
        }

        private bool MovePartToTarget
        (
            float t,
            Transform part,
            SnakePartPose targetSnakePartPose
        )
        {
            var localPosition = part.localPosition;
            var localRotation = part.localRotation;

            var targetPosition = targetSnakePartPose.Position;
            var targetRotation = targetSnakePartPose.Rotation;
            
            var newPosition = Vector3.MoveTowards
            (
                localPosition,
                targetPosition,
                t
            );

            var newRotation = Quaternion.RotateTowards
            (
                localRotation,
                targetRotation,
                t * 90
            );

            part.SetLocalPositionAndRotation(newPosition, newRotation);

            return localPosition == targetPosition 
                   && localRotation == targetRotation;
        }

        public void SetTargetPositions()
        {
            if (_isPartsMoved == false)
            {
                return;
            }
        
            _isPartsMoved = false;
            timer.ResetTime();
            
            _partsTargetPoseHandler.SetTargetPositions();
        }

        private void AddTargetForLastPart()
        {
            _partsTargetPoseHandler.AddTargetForLastPart();
        }
    }
}
