namespace Game.Snake
{
    using System;
    using Game.Snake.Mover;
    using Unity.Collections;
    using Unity.Mathematics;
    using UnityEngine;

    public class Snake : MonoBehaviour
    {
        public event Action OnNewPositionSet;

        public bool IsActive { get; set; }
        
        public NativeArray<float3> PartsTargetPositions => _partsTargetPosesHandler.Positions;
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

        private bool _isNeedGetTargetPositions;
        
        private SnakePartsTargetPosesHandler _partsTargetPosesHandler;

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

                _isNeedGetTargetPositions = true;
            }
            else
            {
                snakeMover.ScheduleMoveParts();
            }
            
            drawer.Draw();
        }

        public void LateUpdate()
        {
            if (IsActive == false)
            {
                return;
            }

            if (_isNeedGetTargetPositions)
            {
                _partsTargetPosesHandler.GetPartsTargetPoses();

                OnNewPositionSet?.Invoke();
                
                _isNeedGetTargetPositions = false;
            }
            else
            {
                snakeMover.MoveParts();
            }
            
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
            
            _partsTargetPosesHandler = new SnakePartsTargetPosesHandler(partsPosesHandler, directionController);
            
            partsPosesHandler.Construct(_partsTargetPosesHandler);
            directionController.Construct();
            snakeMover.Construct(partsPosesHandler, _partsTargetPosesHandler, directionController);
            
            drawer.Construct(partsPosesHandler);
            cameraMover.Construct(partsPosesHandler, directionController, snakeMover);

            _isInitialized = true;
        }
    }
}
