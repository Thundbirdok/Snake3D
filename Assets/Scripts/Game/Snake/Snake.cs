namespace Game.Snake
{
    using System;
    using Game.Snake.Mover;
    using Unity.Mathematics;
    using UnityEngine;

    public class Snake : MonoBehaviour
    {
        public event Action OnNewPositionSet;

        public bool IsActive { get; set; }
        
        public float3[] PartsTargetPositions => snakeMover.PartsTargetsPositions;
        public float3 HeadTargetPosition => _partsTargetPosesHandler.HeadTargetPosition;
        public float3 HeadPosition => partsPosesHandler.HeadPosition;
        
        [SerializeField]
        private SnakeMover snakeMover;
        
        [SerializeField]
        private CameraMover cameraMover;

        [SerializeField]
        private SnakePartsPosesHandler partsPosesHandler;
        
        [SerializeField]
        private SnakeDrawer drawer;

        [SerializeField]
        private SnakeDirectionController directionController;

        private bool _isInitialized;
        
        private PartsTargetPosesHandler _partsTargetPosesHandler;

        private void OnDestroy()
        {
            directionController.Dispose();
            snakeMover.Dispose();
            partsPosesHandler.Dispose();
        }

        private void Update()
        {
            if (IsActive == false)
            {
                drawer.Draw();
                
                return;
            }
            
            var fixedDeltaTime = Time.fixedDeltaTime;

            if (snakeMover.IsTimeToSetNewTargetPositions(fixedDeltaTime))
            {
                directionController.UpdateDirection();
                directionController.TakeDirection();
                snakeMover.SetTargetPositions();
                
                OnNewPositionSet?.Invoke();
            }

            snakeMover.ScheduleMoveParts();
            drawer.Draw();
        }

        public void LateUpdate()
        {
            if (IsActive == false)
            {
                return;
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
            
            partsPosesHandler.Setup();
            _partsTargetPosesHandler.SetPartsToTargets();
            directionController.Setup();
            snakeMover.Setup();
            cameraMover.Setup();
        }

        public void Grow() => partsPosesHandler.Grow();

        private void Initialize()
        {
            IsActive = false;
            
            _partsTargetPosesHandler = new PartsTargetPosesHandler(partsPosesHandler, directionController);
            
            partsPosesHandler.Construct(_partsTargetPosesHandler);
            directionController.Construct();
            snakeMover.Construct(partsPosesHandler, _partsTargetPosesHandler, directionController);
            
            drawer.Construct(partsPosesHandler);
            cameraMover.Construct(partsPosesHandler, directionController, snakeMover);

            _isInitialized = true;
        }
    }
}
