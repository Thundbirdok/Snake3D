namespace Game.Field.Editor
{
    using UnityEditor;
    using UnityEngine;
    using Editor = UnityEditor.Editor;

    [CustomEditor(typeof(Field))]
    public class FieldEditor : Editor
    {
        private Field _field;

        private bool _showGrid = true;
        private bool _showCells;

        private void Awake() => _field = (Field)target;

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            _showGrid = EditorGUILayout.Toggle("Show Grid", _showGrid);

            if (_showGrid)
            {
                _showCells = EditorGUILayout.Toggle("Show Cells", _showCells);
            }
        }
        
        private void OnSceneGUI()
        {
            if (_showGrid == false)
            {
                return;
            }

            if (_showCells)
            {
                DrawAllCells(_field);
                
                return;
            }
            
            DrawOuterSides(_field);
        }

        private static void DrawOuterSides(Field field)
        {
            var defaultColor = Handles.color;
            Handles.color = Color.yellow;
            
            float maxX = field.Size.x;
            float maxY = field.Size.y;
            float maxZ = field.Size.z;

            var width = new Vector3(maxX, 0, 0);
            var height = new Vector3(0, maxY, 0);
            var length = new Vector3(0, 0, maxZ);

            var fieldPosition = field.transform.position;
            var offset = Vector3Int.zero;
            
            Handles.DrawLine(fieldPosition, fieldPosition + width);
            Handles.DrawLine(fieldPosition + length, fieldPosition + length + width);
            Handles.DrawLine(fieldPosition + height, fieldPosition + height + width);
            Handles.DrawLine(fieldPosition + height + length, fieldPosition + height + length + width);

            for (offset.x = 0; offset.x <= maxX; offset.x++)
            {
                var start = fieldPosition + offset;
                var heightEnd = start + height;
                var lengthEnd = start + length;

                Handles.DrawLine(start, heightEnd);
                Handles.DrawLine(start + length, heightEnd + length);
                Handles.DrawLine(start, lengthEnd);
                Handles.DrawLine(start + height, lengthEnd + height);
            }
            offset.x = 0;

            for (offset.y = 1; offset.y < maxY; offset.y++)
            {
                var start = fieldPosition + offset;
                var widthEnd = start + width;
                var lengthEnd = start + length;

                Handles.DrawLine(start, widthEnd);
                Handles.DrawLine(start + length, widthEnd + length);
                Handles.DrawLine(start, lengthEnd);
                Handles.DrawLine(start + width, lengthEnd + width);
            }
            offset.y = 0;

            for (offset.z = 1; offset.z < maxZ; offset.z++)
            {
                var start = fieldPosition + offset;
                var widthEnd = start + width;
                var heightEnd = start + height;

                Handles.DrawLine(start, widthEnd);
                Handles.DrawLine(start + height, widthEnd + height);
                Handles.DrawLine(start, heightEnd);
                Handles.DrawLine(start + width, heightEnd + width);
            }
            offset.z = 0;
            
            Handles.color = defaultColor;
        }
        
        private static void DrawAllCells(Field field)
        {
            var defaultColor = Handles.color;
            Handles.color = Color.yellow;
            
            for (float x = 0; x <= field.Size.x; x++)
            {
                for (float y = 0; y <= field.Size.y; y++)
                {
                    var startPoint = field.transform.position + new Vector3(x, y, 0);
                    var endPoint = startPoint + new Vector3(0, 0, field.Size.z);

                    Handles.DrawLine(startPoint, endPoint);
                }
            }

            for (float z = 0; z <= field.Size.x; z++)
            {
                for (float y = 0; y <= field.Size.y; y++)
                {
                    var startPoint = field.transform.position + new Vector3(0, y, z);
                    var endPoint = startPoint + new Vector3(field.Size.x, 0, 0);

                    Handles.DrawLine(startPoint, endPoint);
                }
            }

            for (float x = 0; x <= field.Size.x; x++)
            {
                for (float z = 0; z <= field.Size.y; z++)
                {
                    var startPoint = field.transform.position + new Vector3(x, 0, z);
                    var endPoint = startPoint + new Vector3(0, field.Size.y, 0);

                    Handles.DrawLine(startPoint, endPoint);
                }
            }

            Handles.color = defaultColor;
        }
    }
}
