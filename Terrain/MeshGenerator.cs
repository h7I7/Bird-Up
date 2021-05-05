//\===========================================================================================
//\ File: MeshGenerator.cs
//\ Author: Morgan James
//\ Brief: Create a mesh from a map. Heavily inspired from https://www.youtube.com/watch?v=v7yyZZjF1z4.
//\===========================================================================================

using UnityEngine;
using System.Collections.Generic;

public class MeshGenerator : MonoBehaviour
{
	private SquareGrid m_SquareGrid;
	private List<Vector3> m_Vertices;
	private List<int> m_Triangles;

	public void GenerateMesh(int[,] a_iaMap, float a_fSquareSize)
	{
		m_SquareGrid = new SquareGrid(a_iaMap, a_fSquareSize);
		m_Vertices = new List<Vector3>();
		m_Triangles = new List<int>();

		for (int iX = 0; iX < m_SquareGrid.squares.GetLength(0); iX++)
		{
			for (int iY = 0; iY < m_SquareGrid.squares.GetLength(1); iY++)
			{
				TriangulateSquare(m_SquareGrid.squares[iX, iY]);
			}
		}

		Mesh mesh = new Mesh();
		GetComponent<MeshFilter>().mesh = mesh;
		mesh.vertices = m_Vertices.ToArray();
		mesh.triangles = m_Triangles.ToArray();
		mesh.RecalculateNormals();
	}

	private void TriangulateSquare(Square a_sSquare)
	{
		switch (a_sSquare.iConfiguration)
		{
			case 0:
				break;

			// 1 points:
			case 1:
				MeshFromPoints(a_sSquare.nCentreBottom, a_sSquare.cnBottomLeft, a_sSquare.nCentreLeft);
				break;
			case 2:
				MeshFromPoints(a_sSquare.nCentreRight, a_sSquare.cnBottomRight, a_sSquare.nCentreBottom);
				break;
			case 4:
				MeshFromPoints(a_sSquare.nCentreTop, a_sSquare.cnTopRight, a_sSquare.nCentreRight);
				break;
			case 8:
				MeshFromPoints(a_sSquare.cnTopLeft, a_sSquare.nCentreTop, a_sSquare.nCentreLeft);
				break;

			// 2 points:
			case 3:
				MeshFromPoints(a_sSquare.nCentreRight, a_sSquare.cnBottomRight, a_sSquare.cnBottomLeft, a_sSquare.nCentreLeft);
				break;
			case 6:
				MeshFromPoints(a_sSquare.nCentreTop, a_sSquare.cnTopRight, a_sSquare.cnBottomRight, a_sSquare.nCentreBottom);
				break;
			case 9:
				MeshFromPoints(a_sSquare.cnTopLeft, a_sSquare.nCentreTop, a_sSquare.nCentreBottom, a_sSquare.cnBottomLeft);
				break;
			case 12:
				MeshFromPoints(a_sSquare.cnTopLeft, a_sSquare.cnTopRight, a_sSquare.nCentreRight, a_sSquare.nCentreLeft);
				break;
			case 5:
				MeshFromPoints(a_sSquare.nCentreTop, a_sSquare.cnTopRight, a_sSquare.nCentreRight, a_sSquare.nCentreBottom, a_sSquare.cnBottomLeft, a_sSquare.nCentreLeft);
				break;
			case 10:
				MeshFromPoints(a_sSquare.cnTopLeft, a_sSquare.nCentreTop, a_sSquare.nCentreRight, a_sSquare.cnBottomRight, a_sSquare.nCentreBottom, a_sSquare.nCentreLeft);
				break;

			// 3 point:
			case 7:
				MeshFromPoints(a_sSquare.nCentreTop, a_sSquare.cnTopRight, a_sSquare.cnBottomRight, a_sSquare.cnBottomLeft, a_sSquare.nCentreLeft);
				break;
			case 11:
				MeshFromPoints(a_sSquare.cnTopLeft, a_sSquare.nCentreTop, a_sSquare.nCentreRight, a_sSquare.cnBottomRight, a_sSquare.cnBottomLeft);
				break;
			case 13:
				MeshFromPoints(a_sSquare.cnTopLeft, a_sSquare.cnTopRight, a_sSquare.nCentreRight, a_sSquare.nCentreBottom, a_sSquare.cnBottomLeft);
				break;
			case 14:
				MeshFromPoints(a_sSquare.cnTopLeft, a_sSquare.cnTopRight, a_sSquare.cnBottomRight, a_sSquare.nCentreBottom, a_sSquare.nCentreLeft);
				break;

			// 4 point:
			case 15:
				MeshFromPoints(a_sSquare.cnTopLeft, a_sSquare.cnTopRight, a_sSquare.cnBottomRight, a_sSquare.cnBottomLeft);
				break;
		}
	}

	private void MeshFromPoints(params Node[] a_naPoints)
	{
		AssignVertices(a_naPoints);

		if (a_naPoints.Length >= 3)
			CreateTriangle(a_naPoints[0], a_naPoints[1], a_naPoints[2]);
		if (a_naPoints.Length >= 4)
			CreateTriangle(a_naPoints[0], a_naPoints[2], a_naPoints[3]);
		if (a_naPoints.Length >= 5)
			CreateTriangle(a_naPoints[0], a_naPoints[3], a_naPoints[4]);
		if (a_naPoints.Length >= 6)
			CreateTriangle(a_naPoints[0], a_naPoints[4], a_naPoints[5]);
	}

	private void AssignVertices(Node[] points)
	{
		for (int iI = 0; iI < points.Length; iI++)
		{
			if (points[iI].iVertexIndex == -1)
			{
				points[iI].iVertexIndex = m_Vertices.Count;
				m_Vertices.Add(points[iI].v3Pos);
			}
		}
	}

	private void CreateTriangle(Node nA, Node nB, Node nC)
	{
		m_Triangles.Add(nA.iVertexIndex);
		m_Triangles.Add(nB.iVertexIndex);
		m_Triangles.Add(nC.iVertexIndex);
	}

	public class SquareGrid
	{
		public Square[,] squares;

		public SquareGrid(int[,] a_iaMap, float a_fSquareSize)
		{
			int iNodeCountX = a_iaMap.GetLength(0);
			int iNodeCountY = a_iaMap.GetLength(1);
			float fMapWidth = iNodeCountX * a_fSquareSize;
			float fMapHeight = iNodeCountY * a_fSquareSize;

			ControlNode[,] controlNodes = new ControlNode[iNodeCountX, iNodeCountY];

			for (int iX = 0; iX < iNodeCountX; iX++)
			{
				for (int iY = 0; iY < iNodeCountY; iY++)
				{
					Vector3 v3Pos = new Vector3(-fMapWidth / 2 + iX * a_fSquareSize + a_fSquareSize / 2, -fMapHeight / 2 + iY * a_fSquareSize + a_fSquareSize / 2,0);
					controlNodes[iX, iY] = new ControlNode(v3Pos, a_iaMap[iX, iY] == 1, a_fSquareSize);
				}
			}

			squares = new Square[iNodeCountX - 1, iNodeCountY - 1];
			for (int iX = 0; iX < iNodeCountX - 1; iX++)
			{
				for (int iY = 0; iY < iNodeCountY - 1; iY++)
				{
					squares[iX, iY] = new Square(controlNodes[iX, iY + 1], controlNodes[iX + 1, iY + 1], controlNodes[iX + 1, iY], controlNodes[iX, iY]);
				}
			}

		}
	}

	public class Square
	{
		public ControlNode cnTopLeft, cnTopRight, cnBottomRight, cnBottomLeft;
		public Node nCentreTop, nCentreRight, nCentreBottom, nCentreLeft;
		public int iConfiguration;

		public Square(ControlNode a_cnTopLeft, ControlNode a_cnTopRight, ControlNode a_cnBottomRight, ControlNode a_cnBottomLeft)
		{
			cnTopLeft = a_cnTopLeft;
			cnTopRight = a_cnTopRight;
			cnBottomRight = a_cnBottomRight;
			cnBottomLeft = a_cnBottomLeft;

			nCentreTop = cnTopLeft.nRight;
			nCentreRight = cnBottomRight.nAbove;
			nCentreBottom = cnBottomLeft.nRight;
			nCentreLeft = cnBottomLeft.nAbove;

			if (cnTopLeft.bActive)
				iConfiguration += 8;
			if (cnTopRight.bActive)
				iConfiguration += 4;
			if (cnBottomRight.bActive)
				iConfiguration += 2;
			if (cnBottomLeft.bActive)
				iConfiguration += 1;
		}
	}

	public class Node
	{
		public Vector3 v3Pos;
		public int iVertexIndex = -1;

		public Node(Vector3 a_v3pos)
		{
			v3Pos = a_v3pos;
		}
	}

	public class ControlNode : Node
	{
		public bool bActive;
		public Node nAbove, nRight;

		public ControlNode(Vector3 a_v3Pos, bool a_bActive, float a_fSquareSize) : base(a_v3Pos)
		{
			bActive = !a_bActive;
			nAbove = new Node(v3Pos + Vector3.up * a_fSquareSize / 2f);
			nRight = new Node(v3Pos + Vector3.right * a_fSquareSize / 2f);
		}
	}

	
}