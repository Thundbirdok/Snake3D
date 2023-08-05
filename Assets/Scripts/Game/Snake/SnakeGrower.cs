namespace Game.Snake
{
    using System;
    using System.Collections.Generic;
    using Game.Field;
    using Game.Snake.Mover;
    using UnityEngine;

    [Serializable]
    public class SnakeGrower
    {
        private List<SnakePartPose> _parts = new List<SnakePartPose>();
        public IReadOnlyList<SnakePartPose> Parts => _parts;

        [SerializeField]
        private Field field;

        [SerializeField]
        private int startPartsNumber;
        
        private Snake _snake;

        public void Construct(Snake snake)
        {
            _snake = snake;
        }

        public void Dispose()
        {
            ClearParts();
        }
        
        public void Setup()
        {
            ClearParts();
            SetupParts();
        }

        public void Grow()
        {
            var tailPreviousTargetPosition = _snake.TailPreviousTargetPosition;

            var tailTargetPosition = _snake.TailTarget.Position;

            var rotation = Quaternion.LookRotation(tailTargetPosition - tailPreviousTargetPosition);
            
            Grow(tailPreviousTargetPosition, rotation);
        }
        
        private void SetupParts()
        {
            var xPartPosition = (float)field.Size.x / 2 - (field.Size.x % 2 == 0 ? 0.5f : 0);
            var yPartPosition = (float)field.Size.y / 2 - (field.Size.y % 2 == 0 ? 0.5f : 0);
            var zPartPosition = (float)field.Size.z / 2 - (field.Size.z % 2 == 0 ? 0.5f : 0);

            var partPosition = new Vector3(xPartPosition, yPartPosition, zPartPosition);

            for (var i = 0; i < startPartsNumber; i++)
            {
                Grow(partPosition, Quaternion.identity);
                --partPosition.z;
            }
        }

        private void ClearParts()
        {
            _parts?.Clear();
        }

        private void Grow(Vector3 position, Quaternion rotation)
        {
            var part = new SnakePartPose(position, rotation);
            
            _parts.Add(part);
        }
    }
}
