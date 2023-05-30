using System;
using System.Collections.Generic;
using UnityEngine;

namespace Snake
{
    [Serializable]
    public class SnakeMover
    {
        [field: SerializeField]
        public Transform Direction { get; private set; }
        
        [field: SerializeField]
        public float Delay { get; private set; } = 0.5f;

        [SerializeField]
        private float moveTime = 0.25f;
    
        public List<Vector3> PartsTargetPosition { get; private set; }
        private List<Quaternion> _partsTargetRotation;
    
        private Snake _snake;
    
        private float _timer;

        public void Construct(Snake snake)
        {
            _snake = snake;
        
            PartsTargetPosition = new List<Vector3>(_snake.Parts.Count);
            _partsTargetRotation = new List<Quaternion>(_snake.Parts.Count);
        
            foreach (var part in _snake.Parts)
            {
                PartsTargetPosition.Add(part.transform.localPosition);
                _partsTargetRotation.Add(part.transform.localRotation);
            }
        }

        private void OnAddPart()
        {
            var last = _snake.Parts.Count - 1;
            var part = _snake.Parts[last].transform;

            PartsTargetPosition.Add(part.localPosition);
            _partsTargetRotation.Add(part.localRotation);
        }

        public bool IsTimeToSetNewTargetPositions()
        {
            _timer += Time.fixedDeltaTime;

            if (_timer < Delay)
            {
                return false;
            }

            _timer = 0;

            return true;
        }

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
            var head = _snake.Parts[0].transform;

            var forward = _snake.Direction.forward;

            var newPosition = 
                head.localPosition 
                + forward;

            var newRotation = 
                Quaternion.LookRotation(forward, Vector3.up);

            for (var i = 0; i < _snake.Parts.Count; i++)
            {
                var part = _snake.Parts[i];
            
                var prevPosition = part.transform.localPosition;
                var prevRotation = part.transform.localRotation;

                PartsTargetPosition[i] = newPosition;
                _partsTargetRotation[i] = newRotation;

                newPosition = prevPosition;
                newRotation = prevRotation;
            }
        }
    }
}
