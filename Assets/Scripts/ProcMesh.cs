using System.Collections.Generic;
using UnityEngine;

namespace Funnelling
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
    public class ProcMesh : MonoBehaviour
    {
        [SerializeField, Range(5f, 10f)] 
        private float radiusShaft = 7.5f;

        [SerializeField, Range(15f, 45f)]
        private float lengthShaft = 30f;

        [SerializeField, Range(15f, 30f)]
        private float radiusCone = 22.5f;

        [SerializeField, Range(15f, 30f)]
        private float lengthCone = 22.5f;

        [SerializeField, Range(3, 100)] 
        private int angularSegmentCount = 26;

        // 4 for funnel + 2 for cone;
        private int TotalVertexCount => angularSegmentCount * 3;
        private int SubVertexCount => TotalVertexCount / 3;
        
        private Mesh mesh;

        private void Awake()
        {
            mesh = new Mesh();
            mesh.name = "Funnel";
            
            GetComponent<MeshFilter>().sharedMesh = mesh;
            GetComponent<MeshCollider>().sharedMesh = mesh;
        }

        private void Update() => GenerateMesh();

        private void GenerateMesh()
        {
            mesh.Clear();
  
            List<Vector3> funnelVertices = new List<Vector3>();
            for (int i = 0; i < TotalVertexCount; i++)
            {
                float t = i / (float) angularSegmentCount;
                float angleRadius = t * Mathfs.TAU;
                float yOffset = 0f;
                float radiusInner = radiusCone;
        
                if (i >= SubVertexCount)
                {
                    radiusInner = radiusShaft;
                    yOffset -= lengthCone;
                    
                    if (i >= SubVertexCount * 2)
                    {
                        yOffset -= lengthShaft;
                    }
                }
                
                Vector3 yPosition = Mathfs.GetOffsetYPosition(transform.position, yOffset);
                Vector3 point = yPosition + transform.rotation * (Mathfs.GetVectorByAngle(angleRadius) * radiusInner);
                funnelVertices.Add(point);
            }

            List<int> triangles = new List<int>();
            List<Vector3> normals = new List<Vector3>();
            for (int i = 0; i < TotalVertexCount; i++)
            {
                int indexPos = i;

                normals.Add(funnelVertices[i]);
                
                if (indexPos + angularSegmentCount >= TotalVertexCount)
                {
                    continue;
                }
                
                int indexNextPos = indexPos + 1;
                int indexNeighbourPos = indexPos + angularSegmentCount;

                if (indexNextPos % angularSegmentCount == 0)
                {
                    indexNextPos = indexNextPos % angularSegmentCount + angularSegmentCount * (i / angularSegmentCount);
                }
                
                int indexNeighbourNextPos = indexNextPos + angularSegmentCount;

                triangles.Add(indexPos);
                triangles.Add(indexNextPos);
                triangles.Add(indexNeighbourPos);
                
                triangles.Add(indexNextPos);
                triangles.Add(indexNeighbourNextPos);
                triangles.Add(indexNeighbourPos);
            }
            
            mesh.SetVertices(funnelVertices);
            mesh.SetTriangles(triangles, 0);
            mesh.SetNormals(normals);
        }
    }
}