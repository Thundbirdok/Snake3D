namespace Game.Snake
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Game.Snake.Mover;
    using UnityEngine;

    public class Snake : MonoBehaviour
    {
        public event Action OnPartAdded;
        public event Action OnNewPositionSet;

        public bool IsActive { get; set; }
        
        public Vector3 Forward => DirectionController.Forward;
        public Vector3 Up => DirectionController.Up;
        public Vector3 Right => DirectionController.Right;

        public float MoveDelay => snakeMover.Delay;
        
        public IReadOnlyList<Transform> Parts => grower.Parts;
        public List<SnakePartPose> PartsTarget => snakeMover.PartsTargetPose;
        
        public Transform Head => grower.Parts.First().transform;
        public SnakePartPose HeadTarget => PartsTarget.First();

        public Transform Tail => grower.Parts.Last().transform;
        public SnakePartPose TailTarget => PartsTarget.Last();
        
        public Vector3 TailPreviousTargetPosition => snakeMover.TailPreviousTargetPosition;
        
        [SerializeField]
        private SnakeMover snakeMover;
        
        [SerializeField]
        private CameraMover cameraMover;

        [SerializeField]
        private SnakeGrower grower;
        
        [field: SerializeField]
        public SnakeDirectionController DirectionController { get; private set; }

        private bool _isInitialized;

        private void OnDestroy()
        {
            DirectionController.Dispose();
            snakeMover.Dispose();
            grower.Dispose();
        }

        private void FixedUpdate()
        {
            if (IsActive == false)
            {
                return;
            }

            var fixedDeltaTime = Time.fixedDeltaTime;

            if (snakeMover.IsTimeToSetNewTargetPositions(fixedDeltaTime))
            {
                DirectionController.UpdateDirection();
                DirectionController.TakeDirection();
                snakeMover.SetTargetPositions();
                
                OnNewPositionSet?.Invoke();
            }

            snakeMover.MoveParts();
            cameraMover.Move();
        }

        public void Setup()
        {
            if (_isInitialized == false)
            {
                Initialize();
            }
            
            grower.Setup();
            DirectionController.Setup();
            snakeMover.Setup();
            cameraMover.Setup();
        }

        public void Grow()
        {
            grower.Grow();
            OnPartAdded?.Invoke();
        }

        private void Initialize()
        {
            grower.Construct(this);
            DirectionController.Construct();
            snakeMover.Construct(this);
            cameraMover.Construct(this);

            _isInitialized = true;
        }
    }
}
