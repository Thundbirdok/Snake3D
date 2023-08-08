namespace Game.Field.Wall
{
    using Effects;
    using Unity.Mathematics;
    using UnityEngine;

    public class Wall : MonoBehaviour
    {
        [SerializeField]
        private WallShaderController shaderController;
        
        public void Initialize() => shaderController.Initialize();
        
        public void UpdateTargetPosition(float3 targetForShader) => shaderController.UpdateTargetPosition(targetForShader);
    }
}
