using Archaic.Renderables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Archaic.Maths;

namespace Archaic
{
	class Renderer3D
	{
		private List<Matrix> transformation_stack;
		private List<Lighting.Diffuse> lights;

		public Renderer3D()
		{
			transformation_stack = new List<Matrix>();
			transformation_stack.Add(Matrix.identity());

			lights = new List<Lighting.Diffuse>();
		}

		public void push(Matrix mat)
		{
			transformation_stack.Add(mat * transformation_stack[transformation_stack.Count - 1]);
		}

		public void pop()
		{
			int stack_count = transformation_stack.Count;
			if (stack_count > 1)
				transformation_stack.RemoveAt(stack_count - 1);
		}

		public void submit_light(Lighting.Diffuse diffuse)
		{
			lights.Add(diffuse);
		}

		public void start()
		{
			Rasterizer.bind_light_sources(lights.ToArray());
		}

		/// <summary>
		/// Takes in a mesh and renders it
		/// </summary>
		/// <param name="mesh"></param>
		public void render(Mesh mesh)
		{
			Rasterizer.Vertex[] position_buffer = new Rasterizer.Vertex[mesh.data.vertices.Length];
			Matrix changed = transformation_stack[transformation_stack.Count - 1];
			Matrix model = mesh.get_model_mat();

			for (int i = 0; i < mesh.data.vertices.Length; i++)
			{
				ref Vertex3D current_vertex = ref mesh.data.vertices[i];

				position_buffer[i] = new Rasterizer.Vertex(current_vertex.position, current_vertex.normal);
			}

			Rasterizer.bind_model_matrix(model);
			Rasterizer.bind_vertex_buffer(position_buffer);
			Rasterizer.draw_data(mesh.data.vertices.Length);
		}

		public void flush()
		{
			Rasterizer.bind_light_sources(null);
			Rasterizer.bind_model_matrix(null);
			Rasterizer.bind_vertex_buffer(null);

			Rasterizer.flush();
			lights.Clear();
		}
	}
}
