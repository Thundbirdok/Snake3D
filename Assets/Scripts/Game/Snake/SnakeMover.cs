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

        public void Construct(Snake snake)
        {
            _snake = snake;

            PartsTargetPosition = new List<Vector3>(_snake.Parts.Count);
            _partsTargetRotation = new List<Quaternion>(_snake.Parts.Count);

            _snake.OnAddPart += OnAddPart;
        }

        public void Dispose()
        {
            _snake.OnAddPart -= OnAddPart;
        }

        public void Setup()
        {
            timer.ResetTime();
            
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
        
        private void OnAddPart()
        {
            var part = _snake.Parts.Last();

            SetPartToTargets(part);
        }

        public bool IsTimeToSetNewTargetPositions() => timer.AddTime(Time.fixedDeltaTime);

        public void MoveParts()
        {
            for (var i = 0; i < PartsTargetPosition.Count; i++)
            {
                var t = Time.fixedDeltaTime / moveTime;

                var part = _snake.Parts[i].transform;       

                var newPosition = Vector3.MoveTowards
                (
                    part.localPosition,
                    PartsTargetPosition[i],
                    t
                );
            
                var newRotation = Quaternion.RotateTowards
                (
                    part.localRotation,
                    _partsTargetRotation[i],
                    t * 90
                );
            
                part.SetLocalPositionAndRotation(newPosition, newRotation);
            }
        }

        public void SetTargetPositions()
        {
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

        private void SetPartToTargets(Transform part)
        {
            var localPosition = part.transform.localPosition;

            PartsTargetPosition.Add(localPosition);
            _partsTargetRotation.Add(part.localRotation);
        }
    }
}
