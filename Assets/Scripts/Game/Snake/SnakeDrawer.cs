namespace Game.Snake
{
    using System;
    using UnityEngine;

    [Serializable]
    public class SnakeDrawer
    {
        [SerializeField]
        private GameObject partPrefab;
        
        private SnakeGrower _grower;

        private Mesh _mesh;

        private Material _material;
        
        public void Construct(SnakeGrower grower)
        {
            _grower = grower;
            
            _mesh = partPrefab.GetComponent<MeshFilter>().sharedMesh;
            _material = partPrefab.GetComponent<MeshRenderer>().sharedMaterial;
        }
        
        public void Draw()
        {
            foreach (var part in _grower.Parts)
            {
                var matrix = Matrix4x4.TRS(part.Position, part.Rotation, Vector3.one * 0.9f);
                
                Graphics.DrawMesh(_mesh, matrix, _material, 0);
            }
        }
    }
}
