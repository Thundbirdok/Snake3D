using System;
using System.Collections.Generic;
using UnityEngine;

namespace Snake
{
    public class Snake : MonoBehaviour
    {
        public event Action OnAddPart;
        public event Action OnSettedNewPosition;

        public bool IsActive;
        
        public Transform Direction => snakeMover.Direction;
        
        public float MoveDelay => snakeMover.Delay;
        
        public IReadOnlyList<Transform> Parts => grower.Parts;
        public List<Vector3Int> PartsTargetPosition => snakeMover.PartsTargetPosition;
        
        public Transform Head => grower.Parts[0].transform;
        public Vector3Int HeadTargetPosition => PartsTargetPosition[0];

        public Vector3 TailPreviousTargetPosition => snakeMover.TailPreviousTargetPosition;
        
        [SerializeField]
        private SnakeMover snakeMover;
        
        [SerializeField]
        private CameraMover cameraMover;

        [SerializeField]
        private SnakeGrower grower;
        
        [SerializeField]
        private SnakeDirectionController directionController;
    
        private void Awake()
        {
            grower.Construct(this);
            snakeMover.Construct(this);
            cameraMover.Construct(this);
            directionController.Construct(this);
        }

        private void OnDestroy()
        {
            directionController.Dispose();
            snakeMover.Dispose();
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

        public void Grow()
        {
            grower.Grow();
            OnAddPart?.Invoke();
        }
    }
}
