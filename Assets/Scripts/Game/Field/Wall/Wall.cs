namespace Game.Field.Wall
{
    using Effects;
    using UnityEngine;

    public class Wall : MonoBehaviour
    {
        [SerializeField]
        private WallShaderController shaderController;
        
        public void Initialize() => shaderController.Initialize();

        public void Setup(Transform targetForShader) => shaderController.Setup(targetForShader);
        
        private void Update() => shaderController.UpdateTargetPosition();
    }
}
