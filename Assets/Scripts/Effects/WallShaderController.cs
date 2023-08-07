namespace Effects
{
    using System;
    using Game.Snake.Mover;
    using Unity.Mathematics;
    using UnityEngine;
    
    [Serializable]
    public class WallShaderController
    {
        [SerializeField]
        private MeshRenderer meshRenderer;
        
        private static readonly int TargetProperty = Shader.PropertyToID("_Target");

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
        
        public void UpdateTargetPosition(float3 position)
        {
            if (_block == null)
            {
                return;
            }

            if (_position.Equals(position))
            { 
                return;
            }
            
            _position = position;
            
            _block.SetVector(TargetProperty, _position);
            meshRenderer.SetPropertyBlock(_block);
        }
    }
}
