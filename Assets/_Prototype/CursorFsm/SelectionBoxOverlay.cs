using UnityEngine;

namespace DefaultNamespace
{
    [RequireComponent(typeof(Camera))]
    public class SelectionBoxOverlay : MonoBehaviour
    {
        public Material BorderMaterial;
        public SelectionCursorState SelectionState;
        private void OnPostRender()
        {
            if (SelectionState.IsSelecting)
            {
                var box = SelectionState.SelectionBox;
                var width = Screen.width;
                var height = Screen.height;
                var v1 = new Vector3(box.xMin / width, (height - box.yMin) / height, 0);
                var v2 = new Vector3(box.xMin / width, (height - box.yMax) / height, 0);
                var v3 = new Vector3(box.xMax / width, (height - box.yMax) / height, 0);
                var v4 = new Vector3(box.xMax / width, (height - box.yMin) / height, 0);

                GL.PushMatrix();
                if (BorderMaterial.SetPass(0))
                {
                    GL.LoadOrtho();
                    GL.Begin(GL.LINES);
                    {
                        GL.Color(Color.green);

                        GL.Vertex(v1);
                        GL.Vertex(v2);

                        GL.Vertex(v2);
                        GL.Vertex(v3);

                        GL.Vertex(v3);
                        GL.Vertex(v4);

                        GL.Vertex(v4);
                        GL.Vertex(v1);
                    }
                    GL.End();
                }

                GL.PopMatrix();
            }
        }
    }
}