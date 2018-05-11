using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Archaic.Maths;

namespace Archaic
{
	class Camera3D
	{
		private Matrix m_camera;
		private Matrix m_perspective;

		private Vec3 m_position;
		private Vec3 m_rotation;
		private Vec3 m_scale;

		private bool m_update;

		public Camera3D(float screen_width, float screen_height, float view_distance, float fov)
		{
			m_perspective = Matrix.perspective(0.1f, view_distance, screen_width, screen_height, radians(fov));
			m_position = new Vec3(0.0f, 0.0f, 0.0f);
			m_rotation = new Vec3(0.0f, 0.0f, 0.0f);
			m_scale = new Vec3(1.0f, 1.0f, 1.0f);
			m_update = true;
		}

		public void update()
		{
			if (m_update)
			{
				Matrix translation = Matrix.translate(-m_position.x, -m_position.y, -m_position.z);
				Matrix rotation = Matrix.rotate_x(-m_rotation.x) * Matrix.rotate_y(-m_rotation.y) * Matrix.rotate_z(-m_rotation.z);
				Matrix scale = Matrix.scale(m_scale.x, m_scale.y, m_scale.z);

				m_camera = rotation * translation * scale;

				m_update = false;
			}
		}

		// Setters

		public void set_position(Vec3 position)
		{
			m_position = position;
			m_update = true;
		}

		public void set_rotation(Vec3 rotation)
		{
			m_rotation = rotation;
			m_update = true;
		}

		public void set_scale(Vec3 scale)
		{
			m_scale = scale;
			m_update = true;
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

		public Matrix get_camera_mat()
		{
			return m_camera;
		}

		public Matrix get_perspective_mat()
		{
			return m_perspective;
		}
	}
}
