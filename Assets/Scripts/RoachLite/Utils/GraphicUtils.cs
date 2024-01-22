// File create date:2022/4/3
using System;
using System.Collections.Generic;
using UnityEngine;
// Created By Yu.Liu

namespace RoachLite.Utils {
	/// <summary>
	/// 图像绘制辅助工具
	/// </summary>
	public static class GraphicUtils {

		#region OpenGL Part

		/// <summary>
		/// 开始一次绘制过程，必须调用后才能使用Draw功能
		/// </summary>
		/// <param name="mat">绘制所用材质</param>
		/// <param name="pass">指定材质Shader内的Pass</param>
		/// <param name="is2D">是否2D模式</param>
		public static void Start(Material mat, int pass = 0, bool is2D = true) {
			if (!mat) {
				Debug.LogError("Please Assign a material on the inspector");
				return;
			}

			mat.SetPass(pass);
			GL.PushMatrix();
			if (is2D) {
				GL.LoadOrtho();
			}
		}

		/// <summary>
		/// 结束此次绘制过程，必须与Start搭配使用
		/// </summary>
		public static void End() {
			GL.PopMatrix();
			
		}

		/// <summary>
		/// 进行矩阵变换
		/// </summary>
		/// <param name="matrix">变换矩阵</param>
		public static void MultiplyMatrix(Matrix4x4 matrix) {
			GL.MultMatrix(matrix);
		}

		/// <summary>
		/// 绘制三角形
		/// </summary>
		/// <param name="a">顶点1</param>
		/// <param name="b">顶点2</param>
		/// <param name="c">顶点3</param>
		public static void DrawTriangle(VertexInfo a, VertexInfo b, VertexInfo c) {
			GL.Begin(GL.TRIANGLES);
			GL.Vertex(a.position);
			GL.TexCoord(a.uv);
			GL.Vertex(b.position);
			GL.TexCoord(a.uv);
			GL.Vertex(c.position);
			GL.TexCoord(a.uv);
			GL.End();
		}

		/// <summary>
		/// 绘制线段
		/// </summary>
		/// <param name="a">线段起点</param>
		/// <param name="b">线段终点</param>
		public static void DrawLineSegment(VertexInfo a, VertexInfo b) {
			GL.Begin(GL.LINES);
			GL.Vertex(a.position);
			GL.TexCoord(a.uv);
			GL.Vertex(b.position);
			GL.TexCoord(b.uv);
			GL.End();
		}

		/// <summary>
		/// 绘制折线
		/// </summary>
		/// <param name="list">折线点位列表</param>
		public static void DrawBrokenLine(List<VertexInfo> list) {
			GL.Begin(GL.LINES);
			foreach (var v in list) {
				GL.Vertex(v.position);
				GL.TexCoord(v.uv);
			}
			GL.End();
		}

		#endregion
	}

	/// <summary>
	/// 顶点信息
	/// </summary>
	public struct VertexInfo {
		
		public Vector3 position;
		public Vector3 uv;
		public float x => position.x;
		public float y => position.y;
		public float z => position.z;
		
		public static VertexInfo Create(Vector3 position) {
			return new VertexInfo {position = position};
		}
	}
}
