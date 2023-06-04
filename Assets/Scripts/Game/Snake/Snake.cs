namespace Game.Snake
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class Snake : MonoBehaviour
    {
        public event Action OnAddPart;
        public event Action OnSettedNewPosition;

        public bool IsActive;
        
        public Vector3 Forward => directionController.Forward;
        public Vector3 Up => directionController.Up;
        public Vector3 Right => directionController.Right;

        public float MoveDelay => snakeMover.Delay;
        
        public IReadOnlyList<Transform> Parts => grower.Parts;
        public List<Vector3> PartsTargetPosition => snakeMover.PartsTargetPosition;
        
        public Transform Head => grower.Parts[0].transform;
        public Vector3 HeadTargetPosition => PartsTargetPosition[0];

        public Vector3 TailPreviousTargetPosition => snakeMover.TailPreviousTargetPosition;
        
        [SerializeField]
        private SnakeMover snakeMover;
        
        [SerializeField]
        private CameraMover cameraMover;

        [SerializeField]
        private SnakeGrower grower;
        
        [SerializeField]
        private SnakeDirectionController directionController;

        private bool _isInitialized;

        private void OnDestroy()
        {
            directionController.Dispose();
            snakeMover.Dispose();
            grower.Dispose();
        }

        private void FixedUpdate()
        {
            if (IsActive == false)
            {
                return;
            }
            
            if (snakeMover.IsTimeToSetNewTargetPositions())
            {
                directionController.UpdateDirection();
                snakeMover.SetTargetPositions();
                
                OnSettedNewPosition?.Invoke();
            }

            snakeMover.MoveParts();
            cameraMover.Move();
        }

        public void Setup()
        {
            if (_isInitialized == false)
            {
                grower.Construct(this);
                directionController.Construct();
                snakeMover.Construct(this);
                cameraMover.Construct(this);
                
                _isInitialized = true;
            }
            
            grower.Setup();
            directionController.Setup();
            snakeMover.Setup();
            cameraMover.Setup();
            
            directionController.UpdateDirection();
        }
        
        public void Grow()
        {
            grower.Grow();
            OnAddPart?.Invoke();
        }
    }
}
