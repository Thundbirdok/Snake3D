namespace Game.Snake
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using Utility;

    [Serializable]
    public class SnakeMover
    {
        public List<Vector3> PartsTargetPosition { get; private set; }

        public Vector3 TailPreviousTargetPosition { get; private set; }
        
        public float Delay => timer.Duration;

        [SerializeField]
        private Timer timer;
        
        [SerializeField]
        private float moveTime = 0.25f;

        private List<Quaternion> _partsTargetRotation;

        private Snake _snake;

        private bool _isPartsMoved;
        
        public void Construct(Snake snake)
        {
            _snake = snake;

            PartsTargetPosition = new List<Vector3>(_snake.Parts.Count);
            _partsTargetRotation = new List<Quaternion>(_snake.Parts.Count);

            _snake.OnPartAdded += PartAdded;
        }

        public void Dispose()
        {
            if (_snake != null)
            {
                _snake.OnPartAdded -= PartAdded;
            }
        }

        public void Setup()
        {
            timer.ResetTime();

            SetPartsToTargets();
        }

        public bool IsTimeToSetNewTargetPositions(float time) => 
            timer.AddTime(time) 
            || _isPartsMoved 
            && _snake.DirectionController.IsUpdated;

        public void MoveParts()
        {
            if (_isPartsMoved)
            {
                return;
            }
            
            var t = Time.fixedDeltaTime / moveTime;

            _isPartsMoved = true;
            
            for (var i = 0; i < PartsTargetPosition.Count; i++)
            {
                var isAtTargetPosition = MovePartToTarget
                (
                    t, 
                    _snake.Parts[i].transform,
                    PartsTargetPosition[i], 
                    _partsTargetRotation[i]
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
            Vector3 targetPosition,
            Quaternion targetRotation
        )
        {
            var localPosition = part.localPosition;
            var localRotation = part.localRotation;

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
            
            var head = PartsTargetPosition[0];

            var forward = _snake.Forward;
            var up = _snake.Up;

            var newPosition = head + forward;

            var newRotation = 
                Quaternion.LookRotation(forward, up);

            TailPreviousTargetPosition = PartsTargetPosition.Last();
            
            for (var i = 0; i < PartsTargetPosition.Count; i++)
            {
                var position = PartsTargetPosition[i];
                var rotation = _partsTargetRotation[i];

                PartsTargetPosition[i] = newPosition;
                _partsTargetRotation[i] = newRotation;

                newPosition = position;
                newRotation = rotation;
            }
        }

        private void SetPartsToTargets()
        {
            if (PartsTargetPosition == null)
            {
                return;
            }

            if (_partsTargetRotation == null)
            {
                return;
            }

            PartsTargetPosition.Clear();
            _partsTargetRotation.Clear();

            foreach (var part in _snake.Parts)
            {
                SetPartToTargets(part);
            }
        }

        private void PartAdded()
        {
            var part = _snake.Parts.Last();

            SetPartToTargets(part);
        }

        private void SetPartToTargets(Transform part)
        {
            var localPosition = part.transform.localPosition;

            PartsTargetPosition.Add(localPosition);
            _partsTargetRotation.Add(part.localRotation);
        }
    }
}
