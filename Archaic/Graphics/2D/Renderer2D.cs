using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archaic
{
	class Renderer2D
	{
		private ushort[] index_data;
		private Rasterizer.Vertex[] buffer_data;
		private int tracker;
		private int vertex_count;

		private const int MAX_SPRITES = 10000;

		public Renderer2D(int window_width, int window_height)
		{
			index_data = new ushort[MAX_SPRITES * 6];
			ushort offset = 0;
			for (int i = 0; i + 6 < index_data.Length; i += 6)
			{
				index_data[i] = (ushort)(offset + 0);
				index_data[i + 1] = (ushort)(offset + 1);
				index_data[i + 2] = (ushort)(offset + 2);

				index_data[i + 3] = (ushort)(offset + 2);
				index_data[i + 4] = (ushort)(offset + 3);
				index_data[i + 5] = (ushort)(offset + 0);

				offset += 4;
			}
		}

		public void start()
		{
			buffer_data = new Rasterizer.Vertex[MAX_SPRITES * 4];

			tracker = 0;
			vertex_count = 0;
		}

		public void submit(Renderables.Sprite sprite)
		{
			buffer_data[tracker].position = new Maths.Vec3(sprite.position.x, sprite.position.y, 0.0f);
			tracker++;
			buffer_data[tracker].position = new Maths.Vec3(sprite.position.x, sprite.position.y, 0.0f) + new Maths.Vec3(0.0f, sprite.size.y, 0.0f);
			tracker++;
			buffer_data[tracker].position = new Maths.Vec3(sprite.position.x, sprite.position.y, 0.0f) + new Maths.Vec3(sprite.size.x, sprite.size.y, 0.0f);
			tracker++;
			buffer_data[tracker].position = new Maths.Vec3(sprite.position.x, sprite.position.y, 0.0f) + new Maths.Vec3(sprite.size.x, 0.0f, 0.0f);
			tracker++;

			vertex_count += 6;
		}

		public void flush()
		{
			Rasterizer.bind_model_matrix(Maths.Matrix.identity());
			Rasterizer.bind_vertex_buffer(buffer_data);
			Rasterizer.bind_index_buffer(index_data);
			Rasterizer.draw_data(vertex_count);

			Rasterizer.flush();
		}
	}
}