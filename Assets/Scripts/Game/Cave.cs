using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DSA
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class Cave : MonoBehaviour
    {
        private bool[,]             m_cave = new bool[32, 16];

        private static Cave         sm_instance;

        #region Properties

        public Vector2Int Size => new Vector2Int(m_cave.GetLength(0), m_cave.GetLength(1));

        public static Cave Instance => sm_instance;

        #endregion

        private void OnEnable()
        {
            sm_instance = this;

            GenerateCave();
            CreateCaveMesh();
        }

        public bool HasWall(Vector2Int v)
        {
            if (v.x < 0 ||
                v.y < 0 ||
                v.x >= Size.x ||
                v.y >= Size.y)
            {   
                // defaults to walls outside the level
                return true;
            }

            return m_cave[v.x, v.y];
        }

        public Vector2Int GetRandomPosition()
        {
            List<Vector2Int> freePositions = new List<Vector2Int>();
            for (int y = 0; y < Size.y; y++)
            {
                for (int x = 0; x < Size.x; x++)
                {
                    if (!m_cave[x, y])
                    {
                        freePositions.Add(new Vector2Int(x, y));
                    }
                }
            }

            return freePositions[Random.Range(0, freePositions.Count)];
        }

        protected void CreateCaveMesh()
        {
            List<Vector3> vertices = new List<Vector3>();
            List<int> walls = new List<int>();
            List<int> floor = new List<int>();

            // add floor and move floor in Z
            AddQuad(new Rect(0, 0, m_cave.GetUpperBound(0) + 1, m_cave.GetUpperBound(1) + 1), vertices, floor);
            vertices = vertices.ConvertAll(v => v + Vector3.forward * 0.01f);

            // add grid
            const float GRID_SIZE = 0.02f;
            for (int x = 0; x <= Size.x; x++)
            {
                AddQuad(new Rect(x - GRID_SIZE, 0, GRID_SIZE * 2, Size.y), vertices, walls);
            }
            for (int y = 0; y <= Size.y; y++)
            {
                AddQuad(new Rect(0, y - GRID_SIZE, Size.x, GRID_SIZE * 2), vertices, walls);
            }

            // create walls
            List<RectInt> wallRects = new List<RectInt>();
            GetWallRects(new RectInt(0, 0, Size.x, Size.y), wallRects);
            foreach (RectInt r in wallRects)
            { 
                AddQuad(new Rect(r.x, r.y, r.width, r.height), vertices, walls);
            }

            // create mesh
            Mesh mesh = new Mesh();
            mesh.subMeshCount = 2;
            mesh.vertices = vertices.ToArray();
            mesh.uv = vertices.ConvertAll(v => (Vector2)v * 0.25f).ToArray();
            mesh.SetTriangles(floor.ToArray(), 0);
            mesh.SetTriangles(walls.ToArray(), 1);
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();

            // assign mesh
            GetComponent<MeshFilter>().mesh = mesh;
        }

        private void AddQuad(Rect r, List<Vector3> vertices, List<int> triangles)
        {
            int iStart = vertices.Count;
            vertices.AddRange(new Vector3[]{
                new Vector3(r.x, r.y, 0.0f),
                new Vector3(r.x, r.yMax, 0.0f),
                new Vector3(r.xMax, r.yMax, 0.0f),
                new Vector3(r.xMax, r.y, 0.0f)
            });

            triangles.AddRange(new int[]{
                iStart + 0, iStart + 1, iStart + 2,
                iStart + 2, iStart + 3, iStart + 0
            });
        }

        protected void GenerateCave()
        {
            // STEP 1: Randomly fill m_cave with random bools
            for (int y = 0; y < m_cave.GetLength(1); y++)
            {
                for (int x = 0; x < m_cave.GetLength(0); x++)
                {
                    m_cave[x, y] = Random.value < 0.42f;
                }
            }

            // STEP 2: Cellular Automata
            // https://www.roguebasin.com/index.php/Cellular_Automata_Method_for_Generating_Random_Cave-Like_Levels
            // Follow the steps to produce a cave like level using the 4/5 rule            
            for (int i = 0; i < 3; ++i)
            {
                bool[,] newCave = new bool[Size.x, Size.y];

                // for each tile
                for (int y = 0; y < Size.y; ++y)
                {
                    for (int x = 0; x < Size.x; ++x)
                    {
                        newCave[x, y] = CountWalls(x, y) >= 5;
                    }
                }

                m_cave = newCave;
            }
        }

        protected int CountWalls(int x, int y)
        {
            int iNumWalls = 0;
            for (int y2 = -1; y2 <= 1; ++y2)
            {
                for (int x2 = -1; x2 <= 1; ++x2)
                {
                    Vector2Int v = new Vector2Int(x + x2, y + y2);
                    if (v.x >= 0 && v.y >= 0 &&
                        v.x < Size.x && v.y < Size.y &&
                        m_cave[v.x, v.y])
                    {
                        iNumWalls++;
                    }
                }
            }

            return iNumWalls;
        }

        protected void GetWallRects(RectInt area, List<RectInt> walls)
        {
            // STEP 3: Replace the code below with a recursive function that checks
            // if the entire area is 'walls', if so adds the area rectangle...
            // if not, divides the area in 4 quadrants and recursivly call GetWallRects()

            if (area.width == 0 || area.height == 0)
            {
                return;
            }

            // end the recursion?
            if (area.width == 1 && area.height == 1)
            {
                if (m_cave[area.x, area.y])
                {
                    walls.Add(area);
                }
                return;
            }

            // check if all is wall inside the area
            bool bAllWall = true;
            for (int y = area.y; y < area.yMax && bAllWall; y++)
            {
                for (int x = area.x; x < area.xMax && bAllWall; x++)
                {
                    if (!m_cave[x, y])
                    {
                        bAllWall = false;
                        break;
                    }
                }
            }

            if (bAllWall)
            {
                walls.Add(area);
            }
            else
            {
                Vector2Int vMiddle = new Vector2Int((area.x + area.xMax) / 2,
                                                    (area.y + area.yMax) / 2);

                GetWallRects(new RectInt(area.x, area.y, vMiddle.x - area.x, vMiddle.y - area.y), walls);
                GetWallRects(new RectInt(vMiddle.x, area.y, area.xMax - vMiddle.x, vMiddle.y - area.y), walls);
                GetWallRects(new RectInt(vMiddle.x, vMiddle.y, area.xMax - vMiddle.x, area.yMax - vMiddle.y), walls);
                GetWallRects(new RectInt(area.x, vMiddle.y, vMiddle.x - area.x, area.yMax - vMiddle.y), walls);
            }
        }
    }
}