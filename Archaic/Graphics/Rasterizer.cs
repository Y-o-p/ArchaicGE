/*	
 *	This is the most important file in this solution made in an OpenGL style.
 *	
 *	This class will take in vertex data and then render it to the console according to the viewport size.
 *	Of course it does have other uses; like being able to render text, points, lines, and, most importantly, triangles.
 *	
 *	Resources:
 *		For understanding the basic pipeline:
 *			https://www.gamedev.net/articles/programming/graphics/the-total-beginners-guide-to-3d-graphics-theory-r3402/?tab=comments
 *	
 *		For rasterizing triangles:
 *			http://www.sunshine2k.de/coding/java/TriangleRasterization/TriangleRasterization.html
 *	
 *		YouTube accounts that detailed certain aspects of the pipeline:
 *			JamieKing
 *			TheBennyBox
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static Archaic.Maths;

namespace Archaic
{
	class Rasterizer
	{
		public struct Vertex
		{
			public Vec3 position;
			public Vec3 normal;

			public Vertex(Vec3 position, Vec3 normal)
			{
				this.position = position;
				this.normal = normal;
			}
		}

		// Every vertex being rendered will use this struct
		private struct VertexShaderData
		{
			public Vec3 screen_pos;
			public Vertex world_vertex;
            public float brightness;

			public VertexShaderData(Vec3 screen_pos, Vertex world_vertex, float brightness = 13)
			{
				this.screen_pos = screen_pos;
				this.world_vertex = world_vertex;
                this.brightness = brightness;
			}
		}

		// Every character will need this data to get the interpolated values
		private struct FragmentShaderData
		{
			public Vertex vert_a, vert_b, vert_c;
			public Vec3 screen_a, screen_b, screen_c;

			public FragmentShaderData(Vertex vert_a, Vertex vert_b, Vertex vert_c, Vec3 screen_a, Vec3 screen_b, Vec3 screen_c)
			{
				this.vert_a = vert_a;
				this.vert_b = vert_b;
				this.vert_c = vert_c;

				this.screen_a = screen_a;
				this.screen_b = screen_b;
				this.screen_c = screen_c;
			}
		}

		// Viewport
		private static int viewport_width = 0, viewport_height = 0;
		private static float viewport_transformation_x = 0.0f, viewport_transformation_y = 0.0f;

		// Buffers and output
		private static byte[] image;
		private static float[] z_buffer;
		private static Stream stream = Console.OpenStandardOutput();

		/// <summary>
		/// Resets all the buffers related to the screen to their default values
		/// </summary>
		private static void reset_screen_buffers()
		{
			for (int i = 0; i < image.Length; i++)
			{
				// Reset every character to the 'space' character
				image[i] = 32;
				// Reset every value in the z_buffer to a very large number to test against
				z_buffer[i] = 9999999999.0f;
			}
		}

		/// <summary>
		/// Sets the size of the viewport
		/// </summary>
		/// <param name="width"></param>
		/// <param name="height"></param>
		public static void set_viewport(int width, int height)
		{
			viewport_width = width;
			viewport_height = height;

			viewport_transformation_x = width / 2;
			viewport_transformation_y = height / 2;

			image = new byte[viewport_width * viewport_height];
			z_buffer = new float[viewport_width * viewport_height];
			reset_screen_buffers();
		}

		private static float ambient_brightness = 0.05f;

		public static void set_ambient_brightness(float brightness)
		{
			ambient_brightness = brightness;
		}

		// Buffers
		private static Vertex[] vertex_buffer;

		private static Lighting.Diffuse[] light_sources;

		private static Matrix model_matrix;
		private static Matrix camera_matrix;
		private static Matrix projection_matrix;

		/// <summary>
		/// Binds the vertex positions to be drawn onto the screen
		/// </summary>
		/// <param name="buffer"></param>
		public static void bind_vertex_buffer(Vertex[] buffer)
		{
			vertex_buffer = buffer;
		}

		/// <summary>
		/// Binds diffuse lights to be used in rasterization later
		/// </summary>
		/// <param name="sources"></param>
		public static void bind_light_sources(Lighting.Diffuse[] sources)
		{
			light_sources = sources;
		}

		/// <summary>
		/// Binds the view matrix
		/// </summary>
		/// <param name="matrix"></param>
		public static void bind_camera_matrix(Matrix matrix)
		{
			camera_matrix = matrix;
		}

		/// <summary>
		/// Binds the model matrix
		/// </summary>
		/// <param name="matrix"></param>
		public static void bind_model_matrix(Matrix matrix)
		{
			model_matrix = matrix;
		}

		/// <summary>
		/// Binds the projection matrix
		/// </summary>
		/// <param name="matrix"></param>
		public static void bind_projection_matrix(Matrix matrix)
		{
			projection_matrix = matrix;
		}

		/// <summary>
		/// Clears the screen then outputs everything to the console
		/// </summary>
		public static void flush()
		{
			// Clear the console of characters
			Console.Clear();
			// Write the image data to the screen
			stream.Write(image, 0, image.Length);

			// Reset the screen buffers for the next frame's use
			reset_screen_buffers();
		}

		/// <summary>
		/// Will write a character at a specific point. (0, 0) is the bottom left of the screen.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="value"></param>
		public static void point(int x, int y, byte value)
		{
			// Flip the y
			y = viewport_height - y;

			// Check if the point is inside the console
			if (y < viewport_height && y >= 0 && x < viewport_width && x >= 0)
				// Set that position to the value given
				image[x + y * viewport_width] = value;
		}

		/// <summary>
		/// Draw a line between two points using the Bresenham's Line Algorithm
		/// </summary>
		/// <param name="vert_a"></param>
		/// <param name="vert_b"></param>
		public static void line(Vec2 vert_a, Vec2 vert_b)
		{
			bool changed = false;
			int x = (int)vert_a.x;
			int y = (int)vert_a.y;

			int sign_x = Math.Sign(vert_b.x - vert_a.x);
			int sign_y = Math.Sign(vert_b.y - vert_a.y);

			int delta_x = (int)Math.Abs(vert_b.x - vert_a.x);
			int delta_y = (int)Math.Abs(vert_b.y - vert_a.y);

			if (delta_y > delta_x)
			{
				int temp = delta_x;
				delta_x = delta_y;
				delta_y = temp;

				changed = true;
			}

			byte brightness = Lighting.get_brightness(19);

			float error = 2 * delta_y - delta_x;
			for (int i = 1; i <= delta_x; i++)
			{
				point(x, y, brightness);

				while (error >= 0)
				{
					if (changed)
						x += sign_x;
					else
						y += sign_y;
					error -= 2 * delta_x;
				}

				if (changed)
					y += sign_y;
				else
					x += sign_x;
				error += 2 * delta_y;
			}
		}

		/// <summary>
		/// Draws text at the desired coordinates
		/// </summary>
		/// <param name="position"></param>
		/// <param name="text"></param>
		public static void text(Vec2 position, string text)
		{
			for (int x = 0; x < text.Length; x++)
			{
				point(x + (int)position.x, (int)position.y, (byte)text[x]);
			}
		}

		/// <summary>
		/// Translates the coordinate according to the viewport size
		/// </summary>
		/// <param name="point"></param>
		/// <returns></returns>
		private static Vec3 viewport_transformation(Vec3 point)
		{
			point.x = point.x * viewport_width + viewport_transformation_x;
			point.y = point.y * viewport_height + viewport_transformation_y;
			return point;
		}

		/// <summary>
		/// Takes the data from the bound data buffer and draws triangles
		/// </summary>
		/// <param name="image"></param>
		/// <param name="count"></param>
		public static void draw_data(int count)
		{
			if (vertex_buffer != null)
			{
				var final_matrix = projection_matrix * camera_matrix * model_matrix;
				for (int i = 0; i + 3 <= count; i += 3)
				{
					Vec3[] curr_triangle = new Vec3[3];

					float[] brights = new float[3];

					Matrix normal_matrix = Matrix.matrix3x3(model_matrix);

					ref Vertex o_vert_a = ref vertex_buffer[i];
					Vertex vert_a = new Vertex(o_vert_a.position * model_matrix, o_vert_a.normal * normal_matrix);
					ref Vertex o_vert_b = ref vertex_buffer[i + 1];
					Vertex vert_b = new Vertex(o_vert_b.position * model_matrix, o_vert_b.normal * normal_matrix);
					ref Vertex o_vert_c = ref vertex_buffer[i + 2];
					Vertex vert_c = new Vertex(o_vert_c.position * model_matrix, o_vert_c.normal * normal_matrix);

					Vec3 normal_model = o_vert_a.normal * normal_matrix;

					for (int v = 0; v < 3; v++)
					{
						ref Vertex vertex = ref vertex_buffer[i + v];
						Vec4 world_pos = new Vec4(vertex.position.x, vertex.position.y, vertex.position.z, 1.0f) * model_matrix;
						Vec4 position = new Vec4(vertex.position.x, vertex.position.y, vertex.position.z, 1.0f) * final_matrix;

						if ((-position.w > position.x || position.w < position.x) &&
							(-position.w > position.y || position.w < position.y) &&
							(-position.w > position.z || position.w < position.z))
						{
							goto dont_render;
						}

						Vec3 position3v = new Vec3(world_pos.x, world_pos.y, world_pos.z);
						float final_brightness = 0.0f;
						foreach (var light in light_sources)
						{
							Vec3 light_distance = Vec3.normalize(light.position - position3v);

							final_brightness += Math.Max(Vec3.dot(Vec3.normalize(normal_model), light_distance), 0);
						}

						brights[v] = final_brightness;
						curr_triangle[v] = position.normalize();
					}


					triangle(
						new VertexShaderData(viewport_transformation(curr_triangle[0]), vert_a, brights[0]),
						new VertexShaderData(viewport_transformation(curr_triangle[1]), vert_b, brights[1]),
						new VertexShaderData(viewport_transformation(curr_triangle[2]), vert_c, brights[2]));

				dont_render:;
				}
			}
		}

		/// <summary>
		/// Rasterizes a triangle (defined by the vertices) onto the image buffer
		/// </summary>
		/// <param name="image"></param>
		/// <param name="vert_a"></param>
		/// <param name="vert_b"></param>
		/// <param name="vert_c"></param>
		private static void triangle(VertexShaderData vert_a, VertexShaderData vert_b, VertexShaderData vert_c)
		{
			VertexShaderData[] data =
			{
				vert_a, vert_b, vert_c
			};
			sort_vertices(ref data);

            if (data[1].screen_pos.y == data[2].screen_pos.y)
			{
				flat_bottom(data[0], data[1], data[2]);
			}
			else if (data[0].screen_pos.y == data[1].screen_pos.y)
			{
				flat_top(data[0], data[1], data[2]);
			}
			else
			{
				VertexShaderData vert_d = new VertexShaderData(new Vec3(data[0].screen_pos.x + ((data[1].screen_pos.y - data[0].screen_pos.y)
					/ (data[2].screen_pos.y - data[0].screen_pos.y)) * (data[2].screen_pos.x - data[0].screen_pos.x), data[1].screen_pos.y, 0.0f),
					new Vertex());

				Vec3 b_coords = barycentric(
					new Vec2(vert_d.screen_pos.x, vert_d.screen_pos.y),
					new Vec2(data[0].screen_pos.x, data[0].screen_pos.y),
					new Vec2(data[1].screen_pos.x, data[1].screen_pos.y),
					new Vec2(data[2].screen_pos.x, data[2].screen_pos.y)
				);

				vert_d.screen_pos.z = b_coords.x * data[0].screen_pos.z + b_coords.y * data[1].screen_pos.z + b_coords.z * data[2].screen_pos.z;

				vert_d.world_vertex.position = new Vec3(
					b_coords.x * data[0].world_vertex.position.x + b_coords.y * data[1].world_vertex.position.x + b_coords.z * data[2].world_vertex.position.x,
					b_coords.x * data[0].world_vertex.position.y + b_coords.y * data[1].world_vertex.position.y + b_coords.z * data[2].world_vertex.position.y,
					b_coords.x * data[0].world_vertex.position.z + b_coords.y * data[1].world_vertex.position.z + b_coords.z * data[2].world_vertex.position.z
				);

                vert_d.world_vertex.normal = data[0].world_vertex.normal;

                vert_d.brightness = b_coords.x * data[0].brightness + b_coords.y * data[1].brightness + b_coords.z * data[2].brightness;

                flat_bottom(data[0], data[1], vert_d);
				flat_top(data[1], vert_d, data[2]);
			}
		}

		/*public static void triangle_wireframe(Vec2 vert_a, Vec2 vert_b, Vec2 vert_c)
		{
			Vec3[] data =
			{
				vert_a, vert_b, vert_c
			};
			sort_vertices(ref data);

			line(data[0], data[1]);
			line(data[1], data[2]);
			line(data[2], data[0]);
		}*/

		/// <summary>
		/// Assumes that vert_a is left to x_b
		/// </summary>
		/// <param name="vert_a"></param>
		/// <param name="x_b"></param>
		private static void horizontal_line(int x_a, int x_b, int y, FragmentShaderData data, float[] brights)
		{
			Vec2 s_a = new Vec2(data.screen_a.x, data.screen_a.y);
			Vec2 s_b = new Vec2(data.screen_b.x, data.screen_b.y);
			Vec2 s_c = new Vec2(data.screen_c.x, data.screen_c.y);

			for (int x = x_a; x < x_b; x++)
			{
				//Vec3 b_coords = barycentric(new Vec2(x, y), s_a, s_b, s_c);

				//float z = b_coords.x * data.screen_a.z + b_coords.y * data.screen_b.z + b_coords.z * data.screen_c.z;

				/*Vec3 position = new Vec3(
					b_coords.x * data.vert_a.position.x + b_coords.y * data.vert_b.position.x + b_coords.z * data.vert_c.position.x,
					b_coords.x * data.vert_a.position.y + b_coords.y * data.vert_b.position.y + b_coords.z * data.vert_c.position.y,
					b_coords.x * data.vert_a.position.z + b_coords.y * data.vert_b.position.z + b_coords.z * data.vert_c.position.z
				);*/

                /*Vec3 normal = new Vec3(
					b_coords.x * data.vert_a.normal.x + b_coords.y * data.vert_b.normal.x + b_coords.z * data.vert_c.normal.x,
					b_coords.x * data.vert_a.normal.y + b_coords.y * data.vert_b.normal.y + b_coords.z * data.vert_c.normal.y,
					b_coords.x * data.vert_a.normal.z + b_coords.y * data.vert_b.normal.z + b_coords.z * data.vert_c.normal.z
				);*/

                int z_buffer_pos = x + y * viewport_width;
				if (x >= 0 && y >= 0 && x < viewport_width && y < viewport_height)
				{
                    /*if (z < z_buffer[z_buffer_pos])
					{
                        float brightness = b_coords.x * brights[0] + b_coords.y * brights[1] + b_coords.z * brights[2];

						z_buffer[z_buffer_pos] = z;
						point(x, y, Lighting.get_brightness((byte)(Math.Max(brightness * 10.0f, 0.0f))));
					}*/
                    point(x, y, Lighting.get_brightness((byte)(Math.Max(10.0f * 10.0f, 0.0f))));
                }
			}
		}

		/// <summary>
		/// Draws a triangle (with the given vertices) that has a flat bottom
		/// </summary>
		/// <param name="image"></param>
		/// <param name="vert_a"></param>
		/// <param name="vert_b"></param>
		/// <param name="vert_c"></param>
		private static void flat_bottom(VertexShaderData vert_a, VertexShaderData vert_b, VertexShaderData vert_c)
		{
			// Swap the legs if vert_c is more left than vert_b
			if (vert_c.screen_pos.x < vert_b.screen_pos.x)
			{
				VertexShaderData temp = vert_b;
				vert_b = vert_c;
				vert_c = temp;
			}

            // Calculate Y
            int min_y = (int)Math.Ceiling(vert_a.screen_pos.y - 0.5f);
			int max_y = (int)Math.Ceiling(vert_c.screen_pos.y - 0.5f);

            // Calculate inverse slope
            float invslope_a = safe_divide(vert_b.screen_pos.x - vert_a.screen_pos.x, vert_b.screen_pos.y - vert_a.screen_pos.y);
			float invslope_b = safe_divide(vert_c.screen_pos.x - vert_a.screen_pos.x, vert_c.screen_pos.y - vert_a.screen_pos.y);

			for (int y = min_y; y < max_y; y++)
			{
			    float x_a = invslope_a * ((float)y + 0.5f - vert_a.screen_pos.y) + vert_a.screen_pos.x;
			    float x_b = invslope_b * ((float)y + 0.5f - vert_a.screen_pos.y) + vert_a.screen_pos.x;

                int min_x = (int)Math.Ceiling(x_a - 0.5f);
                int max_x = (int)Math.Ceiling(x_b - 0.5f);

                horizontal_line(min_x, max_x, y, new FragmentShaderData(vert_a.world_vertex, vert_b.world_vertex, vert_c.world_vertex,
																	vert_a.screen_pos, vert_b.screen_pos, vert_c.screen_pos),
                                                                    new float[3] { vert_a.brightness, vert_b.brightness, vert_c.brightness });
			}
		}

		/// <summary>
		/// Draws a triangle, with the given vertices, that has a flat top
		/// </summary>
		/// <param name="image"></param>
		/// <param name="vert_a"></param>
		/// <param name="vert_b"></param>
		/// <param name="vert_c"></param>
		private static void flat_top(VertexShaderData vert_a, VertexShaderData vert_b, VertexShaderData vert_c)
		{
			// Swap the legs if vert_b is more left than vert_a
			if (vert_b.screen_pos.x < vert_a.screen_pos.x)
			{
				VertexShaderData temp = vert_a;
				vert_a = vert_b;
				vert_b = temp;
			}

			// Calculate Y
			int min_y = (int)Math.Ceiling(vert_a.screen_pos.y - 0.5f);
			int max_y = (int)Math.Ceiling(vert_c.screen_pos.y - 0.5f);

			// Calculate inverse slope
			float invslope_a = safe_divide(vert_c.screen_pos.x - vert_a.screen_pos.x, vert_c.screen_pos.y - vert_a.screen_pos.y);
			float invslope_b = safe_divide(vert_c.screen_pos.x - vert_b.screen_pos.x, vert_c.screen_pos.y - vert_b.screen_pos.y);

			for (int y = min_y; y < max_y; y++)
			{
                float x_a = invslope_a * ((float)y + 0.5f - vert_a.screen_pos.y) + vert_a.screen_pos.x;
                float x_b = invslope_b * ((float)y + 0.5f - vert_b.screen_pos.y) + vert_b.screen_pos.x;

                int min_x = (int)Math.Ceiling(x_a - 0.5f);
                int max_x = (int)Math.Ceiling(x_b - 0.5f);

                horizontal_line(min_x, max_x, y, new FragmentShaderData(vert_a.world_vertex, vert_b.world_vertex, vert_c.world_vertex,
																	vert_a.screen_pos, vert_b.screen_pos, vert_c.screen_pos),
                                                                    new float[3] { vert_a.brightness, vert_b.brightness, vert_c.brightness });
			}
		}

		/// <summary>
		/// Sorts the given vertices from the smallest y to the largest y
		/// </summary>
		/// <param name="vertices"></param>
		private static void sort_vertices(ref VertexShaderData[] vertices)
		{
			IEnumerable<VertexShaderData> sorted_vertices = vertices.OrderBy(vertex => vertex.screen_pos.y);
			int i = 0;
			foreach (VertexShaderData vert in sorted_vertices)
			{
				vertices[i++] = vert;
			}
		}
	}
}
