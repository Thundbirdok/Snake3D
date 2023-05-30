using System;
using System.Collections.Generic;
using UnityEngine;

namespace Snake
{
    public class Snake : MonoBehaviour
    {
        public event Action OnAddPart;
        public event Action OnSettedNewPosition;

        public Transform Direction => snakeMover.Direction;
        
        public float MoveDelay => snakeMover.Delay;
        
        public List<Vector3> PartsTargetPosition => snakeMover.PartsTargetPosition;
        
        public Transform Head => Parts[0].transform;
        public Vector3 HeadTargetPosition => PartsTargetPosition[0];
        
        [field: SerializeField]
        public List<GameObject> Parts { get; private set; }

        [SerializeField]
        private SnakeMover snakeMover;
        
        [SerializeField]
        private CameraMover cameraMover;

        [SerializeField]
        private SnakeDirectionController directionController;
    
        private void Awake()
        {
            snakeMover.Construct(this);
            cameraMover.Construct(this);
            directionController.Construct(this);
        }

        private void OnDestroy()
        {
            directionController.Dispose();
            //snakeMover.Dispose();
        }

        private void FixedUpdate()
        {
            if (snakeMover.IsTimeToSetNewTargetPositions())
            {
                directionController.UpdateDirection();
                snakeMover.SetTargetPositions();
                
                OnSettedNewPosition?.Invoke();
            }

            snakeMover.MoveParts();
            cameraMover.Move();
        }

        public void AddPart()
        {
            OnAddPart?.Invoke();
        }
    }
}
