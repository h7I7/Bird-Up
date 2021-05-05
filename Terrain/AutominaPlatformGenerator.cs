//\===========================================================================================
//\ File: AutominaPlatformGenerator.cs
//\ Author: Morgan James
//\ Brief: Create an platform using cellular automina. Heavily inspired from https://www.youtube.com/watch?v=v7yyZZjF1z4.
//\===========================================================================================

using UnityEngine;
using System.Collections.Generic;

public class AutominaPlatformGenerator : MonoBehaviour
{
	private int m_Width;//How wide the map is.
	private int m_Height;//How tall the map is.

	private string m_Seed;//The seed used to seed the random number gen.

	[Range(0, 100)]
	private int m_RandomFillPercent;//How much of the map should be filled.(higher is less as I inverted the map generation).

	private int[,] m_Map;//This contains the coordinates for each cell.

	private void Start()
	{
		transform.localScale = new Vector3(1, 1, 1);//Sets the scale of the platform to be correct.
		Regen();//Generates a platform.
		gameObject.SetActive(false);//Sets the platform to inactive.
	}

	//Sets all variables and creates a platform with a mesh and a collider.
	private void Regen()
	{
		//Set the variables of the platform from the terrain generator.
		m_Width = Random.Range(GetComponentInParent<TerrainGenerator>().m_PlatformWidthMin, GetComponentInParent<TerrainGenerator>().m_PlatformWidthMax);
		m_Height = Random.Range(GetComponentInParent<TerrainGenerator>().m_PlatformHeightMin, GetComponentInParent<TerrainGenerator>().m_PlatformHeightMax);
		m_RandomFillPercent = Random.Range(GetComponentInParent<TerrainGenerator>().m_PlatformFillMin, GetComponentInParent<TerrainGenerator>().m_PlatformFillMax); 
		m_Seed = GetComponentInParent<TerrainGenerator>().m_Seed.ToString();

		//Generate a platform.
		GenerateMap();

		//Add a 2D collider based on the mesh.
		if (GetComponentInParent<TerrainGenerator>().m_UseFastGeneration == true)
		{
			Gen2DCollisionmesh();//Fast method.
		}
		else
		{
			CreatePolygon2DColliderPoints();//Slow accurate method.
		}
	}

	//Creates a platform with a mesh.
	private void GenerateMap()
	{
		m_Map = new int[m_Width, m_Height];//Create a new map.
		RandomFillMap();//Fill the map

		for (int i = 0; i < 5; i++)
		{
			SmoothMap();//Smooth the map
		}
		
		GetComponent<MeshGenerator>().GenerateMesh(m_Map, 4);//Generate a mesh.
	}

	//Randomly decides whether to fill the map coordinate or not.
	private void RandomFillMap()
	{
		System.Random pseudoRandom = new System.Random(m_Seed.GetHashCode());

		for (int iX = 0; iX < m_Width; iX++)
		{
			for (int iY = 0; iY < m_Height; iY++)
			{
				if (iX == 0 || iX == m_Width - 1 || iY == 0 || iY == m_Height - 1)
				{
					m_Map[iX, iY] = 1;
				}
				else
				{
					m_Map[iX, iY] = (pseudoRandom.Next(0, 100) < m_RandomFillPercent) ? 1 : 0;
				}
			}
		}
	}

	//Makes the map feel more organic based on how many filled cells around each other there are.
	private void SmoothMap()
	{
		for (int iX = 0; iX < m_Width; iX++)
		{
			for (int iY = 0; iY < m_Height; iY++)
			{
				int iNeighbourWallTiles = GetSurroundingWallCount(iX, iY);

				if (iNeighbourWallTiles > 4)
					m_Map[iX, iY] = 1;
				else if (iNeighbourWallTiles < 4)
					m_Map[iX, iY] = 0;

			}
		}
	}

	//Returns how many walls are surrounding a cell.
	private int GetSurroundingWallCount(int a_iGridX, int a_iGridY)
	{
		int iWallCount = 0;
		for (int neighbourX = a_iGridX - 1; neighbourX <= a_iGridX + 1; neighbourX++)
		{
			for (int neighbourY = a_iGridY - 1; neighbourY <= a_iGridY + 1; neighbourY++)
			{
				if (neighbourX >= 0 && neighbourX < m_Width && neighbourY >= 0 && neighbourY < m_Height)
				{
					if (neighbourX != a_iGridX || neighbourY != a_iGridY)
					{
						iWallCount += m_Map[neighbourX, neighbourY];
					}
				}
				else
				{
					iWallCount++;
				}
			}
		}

		return iWallCount;
	}


	//A mesh to 2D polygon collier function From https://forum.unity.com/threads/solved-draw-polygon2d-collider-paths-around-a-2d-mesh.334039/
	//This is fast but sometimes doesn't get all the paths.
	private void Gen2DCollisionmesh()
	{
		if (GetComponent<MeshFilter>() == null)
		{
			return;
		}

		if (GetComponent<PolygonCollider2D>() == true)
		{
			Destroy(GetComponent<PolygonCollider2D>());
		}

		// Get triangles and vertices from mesh
		int[] iaTriangles = GetComponent<MeshFilter>().mesh.triangles;
		Vector3[] v3aVertices = GetComponent<MeshFilter>().mesh.vertices;

		// Get just the outer edges from the mesh's triangles (ignore or remove any shared edges)
		Dictionary<string, KeyValuePair<int, int>> edges = new Dictionary<string, KeyValuePair<int, int>>();
		for (int iI = 0; iI < iaTriangles.Length; iI += 3)
		{
			for (int iE = 0; iE < 3; iE++)
			{
				int iVert1 = iaTriangles[iI + iE];
				int iVert2 = iaTriangles[iI + iE + 1 > iI + 2 ? iI : iI + iE + 1];
				string sEdge = Mathf.Min(iVert1, iVert2) + ":" + Mathf.Max(iVert1, iVert2);
				if (edges.ContainsKey(sEdge))
				{
					edges.Remove(sEdge);
				}
				else
				{
					edges.Add(sEdge, new KeyValuePair<int, int>(iVert1, iVert2));
				}
			}
		}

		// Create edge lookup (Key is first vertex, Value is second vertex, of each edge)
		Dictionary<int, int> lookup = new Dictionary<int, int>();
		foreach (KeyValuePair<int, int> edge in edges.Values)
		{
			if (lookup.ContainsKey(edge.Key) == false)
			{
				lookup.Add(edge.Key, edge.Value);
			}
		}

		// Create empty polygon collider
		PolygonCollider2D polygonCollider = gameObject.AddComponent<PolygonCollider2D>();
		polygonCollider.pathCount = 0;

		// Loop through edge vertices in order
		int iStartVert = 0;
		int iNextVert = iStartVert;
		int iHighestVert = iStartVert;
		List<Vector2> lv2ColliderPath = new List<Vector2>();
		while (true)
		{

			// Add vertex to collider path
			lv2ColliderPath.Add(v3aVertices[iNextVert]);

			// Get next vertex
			iNextVert = lookup[iNextVert];

			// Store highest vertex (to know what shape to move to next)
			if (iNextVert > iHighestVert)
			{
				iHighestVert = iNextVert;
			}

			// Shape complete
			if (iNextVert == iStartVert)
			{

				// Add path to polygon collider
				polygonCollider.pathCount++;
				polygonCollider.SetPath(polygonCollider.pathCount - 1, lv2ColliderPath.ToArray());
				lv2ColliderPath.Clear();

				// Go to next shape if one exists
				if (lookup.ContainsKey(iHighestVert + 1))
				{

					// Set starting and next vertices
					iStartVert = iHighestVert + 1;
					iNextVert = iStartVert;

					// Continue to next loop
					continue;
				}

				// No more verts
				break;
			}
		}
	}

	//A mesh to 2D polygon collier function From https://forum.unity.com/threads/solved-draw-polygon2d-collider-paths-around-a-2d-mesh.334039/
	//This one works on getting all the paths but is very intensive.
	#region API

	private void CreatePolygon2DColliderPoints()
	{
		if (GetComponent<MeshFilter>() == null)
		{
			return;
		}

		if (GetComponent<PolygonCollider2D>() == true)
		{
			Destroy(GetComponent<PolygonCollider2D>());
		}
		gameObject.AddComponent<PolygonCollider2D>();

		var edges = BuildEdgesFromMesh();
		var paths = BuildColliderPaths(edges);
		ApplyPathsToPolygonCollider(paths);
	}

	#endregion

	#region Helper

	private void ApplyPathsToPolygonCollider(List<Vector2[]> paths)
	{
		if (paths == null)
			return;

		GetComponent<PolygonCollider2D>().pathCount = paths.Count;
		for (int i = 0; i < paths.Count; i++)
		{
			var path = paths[i];
			GetComponent<PolygonCollider2D>().SetPath(i, path);
		}
	}

	private Dictionary<Edge2D, int> BuildEdgesFromMesh()
	{
		var mesh = GetComponent<MeshFilter>().sharedMesh;

		if (mesh == null)
			return null;

		var verts = mesh.vertices;
		var tris = mesh.triangles;
		var edges = new Dictionary<Edge2D, int>();

		for (int i = 0; i < tris.Length - 2; i += 3)
		{

			var faceVert1 = verts[tris[i]];
			var faceVert2 = verts[tris[i + 1]];
			var faceVert3 = verts[tris[i + 2]];

			Edge2D[] faceEdges;
			faceEdges = new Edge2D[] {
				new Edge2D{ a = faceVert1, b = faceVert2 },
				new Edge2D{ a = faceVert2, b = faceVert3 },
				new Edge2D{ a = faceVert3, b = faceVert1 },
			};

			foreach (var edge in faceEdges)
			{
				if (edges.ContainsKey(edge))
					edges[edge]++;
				else
					edges[edge] = 1;
			}
		}

		return edges;
	}

	private static List<Edge2D> GetOuterEdges(Dictionary<Edge2D, int> allEdges)
	{
		var outerEdges = new List<Edge2D>();

		foreach (var edge in allEdges.Keys)
		{
			var numSharedFaces = allEdges[edge];
			if (numSharedFaces == 1)
				outerEdges.Add(edge);
		}

		return outerEdges;
	}

	private static List<Vector2[]> BuildColliderPaths(Dictionary<Edge2D, int> allEdges)
	{

		if (allEdges == null)
			return null;

		var outerEdges = GetOuterEdges(allEdges);

		var paths = new List<List<Edge2D>>();
		List<Edge2D> path = null;

		while (outerEdges.Count > 0)
		{

			if (path == null)
			{
				path = new List<Edge2D>();
				path.Add(outerEdges[0]);
				paths.Add(path);

				outerEdges.RemoveAt(0);
			}

			bool foundAtLeastOneEdge = false;

			int i = 0;
			while (i < outerEdges.Count)
			{
				var edge = outerEdges[i];
				bool removeEdgeFromOuter = false;

				if (edge.b == path[0].a)
				{
					path.Insert(0, edge);
					removeEdgeFromOuter = true;
				}
				else if (edge.a == path[path.Count - 1].b)
				{
					path.Add(edge);
					removeEdgeFromOuter = true;
				}

				if (removeEdgeFromOuter)
				{
					foundAtLeastOneEdge = true;
					outerEdges.RemoveAt(i);
				}
				else
					i++;
			}

			//If we didn't find at least one edge, then the remaining outer edges must belong to a different path
			if (!foundAtLeastOneEdge)
				path = null;

		}

		var cleanedPaths = new List<Vector2[]>();

		foreach (var builtPath in paths)
		{
			var coords = new List<Vector2>();

			foreach (var edge in builtPath)
				coords.Add(edge.a);

			cleanedPaths.Add(CoordinatesCleaned(coords));
		}


		return cleanedPaths;
	}

	private static bool CoordinatesFormLine(Vector2 a, Vector2 b, Vector2 c)
	{
		//If the area of a triangle created from three points is zero, they must be in a line.
		float area = a.x * (b.y - c.y) +
			b.x * (c.y - a.y) +
				c.x * (a.y - b.y);

		return Mathf.Approximately(area, 0f);

	}

	private static Vector2[] CoordinatesCleaned(List<Vector2> coordinates)
	{
		List<Vector2> coordinatesCleaned = new List<Vector2>();
		coordinatesCleaned.Add(coordinates[0]);

		var lastAddedIndex = 0;

		for (int i = 1; i < coordinates.Count; i++)
		{

			var coordinate = coordinates[i];

			Vector2 lastAddedCoordinate = coordinates[lastAddedIndex];
			Vector2 nextCoordinate = (i + 1 >= coordinates.Count) ? coordinates[0] : coordinates[i + 1];

			if (!CoordinatesFormLine(lastAddedCoordinate, coordinate, nextCoordinate))
			{

				coordinatesCleaned.Add(coordinate);
				lastAddedIndex = i;

			}

		}

		return coordinatesCleaned.ToArray();

	}

	#endregion

	#region Nested

	private struct Edge2D
	{

		public Vector2 a;
		public Vector2 b;

		public override bool Equals(object obj)
		{
			if (obj is Edge2D)
			{
				var edge = (Edge2D)obj;
				//An edge is equal regardless of which order it's points are in
				return (edge.a == a && edge.b == b) || (edge.b == a && edge.a == b);
			}

			return false;

		}

		public override int GetHashCode()
		{
			return a.GetHashCode() ^ b.GetHashCode();
		}

		public override string ToString()
		{
			return string.Format("[" + a.x + "," + a.y + "->" + b.x + "," + b.y + "]");
		}

	}
	#endregion

}