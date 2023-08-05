namespace Effects
{
    using System;
    using Game.Snake.Mover;
    using UnityEngine;
    
    [Serializable]
    public class WallShaderController
    {
        [SerializeField]
        private MeshRenderer meshRenderer;
        
        private static readonly int TargetProperty = Shader.PropertyToID("_Target");
        
        private SnakePartPose _target;

        private MaterialPropertyBlock _block;

        private Vector3 _position;
        
        public void Initialize()
        {
            if (meshRenderer == null)
            {
                return;
            }
            
            _block = new MaterialPropertyBlock();
            meshRenderer.GetPropertyBlock(_block);
        }

        public void Setup(SnakePartPose target)
        {
            _target = target;
        }
        
        public void UpdateTargetPosition()
        {
            if (_block == null)
            {
                return;
            }

            if (_target == null)
            {
                return;
            }

            if (_position == _target.Position)
            { 
                return;
            }
            
            _position = _target.Position;
            
            _block.SetVector(TargetProperty, _position);
            meshRenderer.SetPropertyBlock(_block);
        }
    }
}
