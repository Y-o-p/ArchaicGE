using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Archaic.Maths;

namespace Archaic
{
	namespace Renderables
	{
		class Vertex3D
		{
			public Vec3 position;
			public Vec3 normal;

			public Vertex3D(Vec3 position, Vec3 normal)
			{
				this.position = position;
				this.normal = normal;
			}

			public Vertex3D()
			{
				position = new Vec3(0.0f, 0.0f, 0.0f);
				normal = new Vec3(0.0f, 0.0f, 0.0f);
			}
		}

		struct Sprite
		{
			public Vec2 position;
			public Vec2 size;

			public Sprite(float x, float y, float width, float height)
			{
				position = new Vec2(x, y);
				size = new Vec2(width, height);
			}
		}

		struct MeshData
		{
			public Vertex3D[] vertices;
			public ushort[] indices;

			// New constructor
			public MeshData(Vertex3D[] vertex_data, ushort[] indices)
			{
				vertices = vertex_data;
				this.indices = indices;
			}

			public MeshData copy()
			{
				var copy_vertices = new Vertex3D[vertices.Length];
				for (int i = 0; i < vertices.Length; i++)
				{
					copy_vertices[i] = new Vertex3D();
					copy_vertices[i].position.x = vertices[i].position.x;
					copy_vertices[i].position.y = vertices[i].position.y;
					copy_vertices[i].position.z = vertices[i].position.z;

					copy_vertices[i].normal.x = vertices[i].normal.x;
					copy_vertices[i].normal.y = vertices[i].normal.y;
					copy_vertices[i].normal.z = vertices[i].normal.z;
				}

				var copy_indices = new ushort[indices.Length];
				for (int i = 0; i < indices.Length; i++)
				{
					copy_indices[i] = indices[i];
				}

				return new MeshData(copy_vertices, copy_indices);
			}
		}

		class Mesh
		{
			public MeshData data;

			private Vec3 m_position;
			private Vec3 m_rotation;
			private Vec3 m_scale;

			public Mesh(MeshData d)
			{
				data = d;

				m_position = new Vec3(0.0f, 0.0f, 0.0f);
				m_rotation = new Vec3(0.0f, 0.0f, 0.0f);
				m_scale = new Vec3(1.0f, 1.0f, 1.0f);
			}

			// Setters

			public void set_position(Vec3 position)
			{
				m_position = position;
			}

			public void set_rotation(Vec3 rotation)
			{
				m_rotation = rotation;
			}

			public void set_scale(Vec3 scale)
			{
				m_scale = scale;
			}

			// Getters

			public Vec3 get_position()
			{
				return m_position;
			}

			public Vec3 get_rotation()
			{
				return m_rotation;
			}

			public Vec3 get_scale()
			{
				return m_scale;
			}

			public Matrix get_model_mat()
			{
				Matrix translation = Matrix.translate(m_position.x, m_position.y, m_position.z);
				Matrix rotation = Matrix.rotate_x(m_rotation.x) * Matrix.rotate_y(m_rotation.y) * Matrix.rotate_z(m_rotation.z);
				Matrix scale = Matrix.scale(m_scale.x, m_scale.y, m_scale.z);

				return (translation * rotation * scale);
			}
		}
	}
}
