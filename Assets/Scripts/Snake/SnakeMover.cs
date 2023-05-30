using System;
using System.Collections.Generic;
using UnityEngine;

namespace Snake
{
    using System.Linq;

    [Serializable]
    public class SnakeMover
    {
        public List<Vector3Int> PartsTargetPosition { get; private set; }

        public Vector3Int TailPreviousTargetPosition { get; private set; }
        
        [field: SerializeField]
        public Transform Direction { get; private set; }

        [field: SerializeField]
        public float Delay { get; private set; } = 0.5f;

        [SerializeField]
        private float moveTime = 0.25f;

        private List<Quaternion> _partsTargetRotation;

        private Snake _snake;
    
        private float _timer;

        public void Construct(Snake snake)
        {
            _snake = snake;
        
            PartsTargetPosition = new List<Vector3Int>(_snake.Parts.Count);
            _partsTargetRotation = new List<Quaternion>(_snake.Parts.Count);
        
            foreach (var part in _snake.Parts)
            {
                SetPartToTargets(part);
            }
            
            _snake.OnAddPart += OnAddPart;
        }

        public void Dispose()
        {
            _snake.OnAddPart -= OnAddPart;
        }

        private void OnAddPart()
        {
            var part = _snake.Parts.Last();

            SetPartToTargets(part);
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
            var head = FloatToInt(_snake.Parts[0].transform.localPosition);

            var forward = FloatToInt(_snake.Direction.forward);

            var newPosition = head + forward;

            var newRotation = 
                Quaternion.LookRotation(forward, Vector3.up);

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

            var localPositionInt = FloatToInt(localPosition);

            PartsTargetPosition.Add(localPositionInt);
            _partsTargetRotation.Add(part.localRotation);
        }

        private Vector3Int FloatToInt(Vector3 vector3)
        {
            return new Vector3Int
            (
                (int)vector3.x,
                (int)vector3.y,
                (int)vector3.z
            );
        } 
    }
}
