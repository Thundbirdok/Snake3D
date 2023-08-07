namespace Game.Snake
{
    using System;
    using UnityEngine;

    [Serializable]
    public class SnakeDrawer
    {
        [SerializeField]
        private GameObject partPrefab;
        
        private SnakePartsPosesHandler _partsPosesHandler;

        private Mesh _mesh;

        private Material _material;

        private Matrix4x4 _scaleMatrix;
        
        public void Construct(SnakePartsPosesHandler partsPosesHandler)
        {
            _partsPosesHandler = partsPosesHandler;
            
            _mesh = partPrefab.GetComponent<MeshFilter>().sharedMesh;
            _material = partPrefab.GetComponent<MeshRenderer>().sharedMaterial;
            _scaleMatrix = Matrix4x4.Scale(partPrefab.transform.localScale);
        }
        
        public void Draw()
        {
            for (var i = 0; i < _partsPosesHandler.PartsPositions.Length; i++)
            {
                var position = _partsPosesHandler.PartsPositions[i];
                var rotation = _partsPosesHandler.PartsRotations[i];
                
                var translateMatrix = Matrix4x4.Translate(position);
                var rotationMatrix = Matrix4x4.Rotate(rotation);
                
                var matrix = translateMatrix * rotationMatrix * _scaleMatrix;
                
                Graphics.DrawMesh(_mesh, matrix, _material, 0);
            }
        }
    }
}
