using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TD.Gameplay.Path
{
    [ExecuteInEditMode]
    [RequireComponent (typeof(MeshFilter))]
    [RequireComponent (typeof(MeshCollider))]
    public class PathMaker : MonoBehaviour
    {
        public bool DebugNow = false;
        public bool DebugPathMesh = false;
        public bool DebugForceRotation = false;

        [SerializeField]
        private float _pathThickness = 1.0f;

        [SerializeField]
        private GameObject _pathContainer;

        private List<PathCell> pathCells;

        private void Start()
        {
            LookForPathCells();
            FeedPathCellChildren_ParentProperty();
            GenerateNavMeshPath();
        }

        private void GenerateNavMeshPath()
        {
            Mesh navMeshSource = new Mesh();
            MeshFilter meshFilter = GetComponent<MeshFilter>();
            MeshCollider meshCollider = GetComponent<MeshCollider>();
            meshFilter.mesh = navMeshSource;
            meshCollider.sharedMesh = navMeshSource;

            List<Vector3> vertexPos = new List<Vector3>();
            Vector3[] vertices = new Vector3[ pathCells.Count * 2 ];
            int[] triangles = new int[ (pathCells.Count - 1) * 2 * 3 ];

            int x = 0;
            for (int i = 0; i < pathCells.Count; i++)
            {
                Quaternion keepRotation = pathCells[i].transform.rotation;
                if(i == 0)
                {
                    pathCells[i].transform.LookAt(pathCells[i + 1].transform);
                }
                else if (pathCells.Count > i + 1)
                {
                    pathCells[i].transform.LookAt(pathCells[i + 1].transform.position);
                    Vector3 a = pathCells[i].transform.forward;
                    pathCells[i].transform.LookAt(pathCells[i - 1].transform.position);
                    Vector3 b = pathCells[i].transform.forward;
                    

                    pathCells[i].transform.forward = a-b;
                    
                }
                else
                {
                    pathCells[i].transform.LookAt(pathCells[i - 1].transform);
                    Vector3 mirrorRotation = pathCells[i].transform.rotation.eulerAngles;
                    mirrorRotation.y += 180f;
                    pathCells[i].transform.rotation = Quaternion.Euler(mirrorRotation);
                }

                Vector3 vec0Pos;
                Vector3 vec1Pos;

                // do better checking
                if (!pathCells[i].GetComponent<PathMaker>())
                {
                    vec0Pos = -pathCells[i].transform.right * (_pathThickness / 2);
                    vec0Pos += pathCells[i].transform.localPosition;

                    vec1Pos = pathCells[i].transform.right * (_pathThickness / 2);
                    vec1Pos += pathCells[i].transform.localPosition;
                }
                else
                {
                    vec0Pos = -pathCells[i].transform.right * (_pathThickness / 2);
                    vec0Pos += pathCells[i].transform.localPosition;

                    vec1Pos = pathCells[i].transform.right * (_pathThickness / 2);
                    vec1Pos += pathCells[i].transform.localPosition;
                }
                //vec0Pos += pathCells[i].transform.localPosition;

                //Vector3 vec1Pos = pathCells[i].transform.right * (_pathThickness / 2);
                //vec1Pos += pathCells[i].transform.localPosition;

                vertexPos.Add(vec0Pos);
                vertexPos.Add(vec1Pos);

                vertices[x] = vec0Pos;
                x++;

                vertices[x] = vec1Pos;
                x++;

                if (DebugPathMesh)
                {
                    var vec0Debug = new GameObject();
                    vec0Debug.transform.position = vec0Pos;
                    var vec1Debug = new GameObject();
                    vec1Debug.transform.position = vec1Pos;
                }
                if(DebugForceRotation)
                {
                    pathCells[i].transform.rotation = keepRotation;
                }
                pathCells[i].transform.rotation = keepRotation;                        
                
            }

            x = 0;
            for(int i = 0; i < pathCells.Count - 1; i++)
            {
                triangles[x] = i * 2;
                x++;
                triangles[x] = triangles[x-1] + 2;
                x++;
                triangles[x] = triangles[x-2] + 1;
                x++;
                

                triangles[x] = triangles[x-3] + 1;
                x++;
                triangles[x] = triangles[x-4] + 2;
                x++;
                triangles[x] = triangles[x-5] + 3;
                x++;
            }


            //Debug.Log("vertices: " + vertices.ToString());
            //for (int i = 0; i < vertices.Length; i++)
            //{
            //    Debug.Log("v" + i.ToString() + " " + vertices[i].ToString());
            //}
            //navMeshSource.vertices = vertices;
            //Debug.Log("tris indices: " + triangles.ToString());
            //for (int i = 0; i < triangles.Length; i++)
            //{
            //    Debug.Log("t" + i.ToString() + " " + triangles[i].ToString());
            //}
            navMeshSource.vertices = vertices;
            navMeshSource.triangles = triangles;
            
        }

        private void OnDrawGizmos()
        {
            if (_pathContainer != null)
            {
                LookForPathCells();
                DebugPath();
            }
        }

        private void LookForPathCells()
        {
            pathCells = new List<PathCell>();

            if (GetComponent<PathCell>())
            {
                pathCells.Add(GetComponent<PathCell>());
            }

            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).GetComponent<PathCell>())
                {
                    pathCells.Add(transform.GetChild(i).GetComponent<PathCell>());
                }
            }
        }

        public void FeedPathCellChildren_ParentProperty()
        {
            for (int i = 0; i < pathCells.Count; i++)
            {
                PathMaker currentPathMaker = pathCells[i].GetComponent<PathMaker>();
                if (currentPathMaker != null && currentPathMaker != this)
                {
                    currentPathMaker.FeedPathCellChildren_ParentProperty();
                }

                pathCells[i].parentPath = this;
            }
        }

        private void DebugPath()
        {            
            for (int i = 0; i < pathCells.Count; i++)
            {
                if (i + 1 < pathCells.Count)
                {
                    Transform gizmo0 = pathCells[i].GetComponent<Transform>();
                    Transform gizmo1 = pathCells[i + 1].GetComponent<Transform>();
                    Gizmos.DrawLine(gizmo0.position, gizmo1.position);
                }
            }
        }

        public PathCell GetNextPathCell (PathCell current)
        {
            for (int i = 0; i < pathCells.Count; i++)
            {
                if (i + 1 >= pathCells.Count)
                {
                    return null;
                }

                if (pathCells[i] == current)
                {
                    return pathCells[i + 1];
                }                
            }
            return null;
        }

        private void Update()
        {
            if(DebugNow)
            {
                LookForPathCells();
                FeedPathCellChildren_ParentProperty();
                GenerateNavMeshPath();
                //DebugNow = false;
            }
        }
    }

}
