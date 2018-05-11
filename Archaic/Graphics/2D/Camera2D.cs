using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Archaic.Maths;

namespace Archaic.Graphics._2D
{
	class Camera2D
	{
		private Matrix m_camera;
		private Matrix m_orthographic;

		private Vec2 m_position;
		private float m_rotation;
		private Vec2 m_scale;

		private bool m_update;

		public Camera2D(float screen_width, float screen_height)
		{
			m_orthographic = Matrix.orthographic(screen_width, screen_height);
			m_position = new Vec2(0.0f, 0.0f);
			m_rotation = 0.0f;
			m_scale = new Vec2(1.0f, 1.0f);
			m_update = true;
		}

		public void update()
		{
			if (m_update)
			{
				Matrix translation = Matrix.translate(-m_position.x, -m_position.y, 0.0f);
				Matrix rotation = Matrix.rotate_z(-m_rotation);
				Matrix scale = Matrix.scale(m_scale.x, m_scale.y, 0.0f);

				m_camera = rotation * translation * scale;

				m_update = false;
			}
		}

		// Setters

		public void set_position(Vec2 position)
		{
			m_position = position;
			m_update = true;
		}

		public void set_rotation(float rotation)
		{
			m_rotation = rotation;
			m_update = true;
		}

		public void set_scale(Vec2 scale)
		{
			m_scale = scale;
			m_update = true;
		}

		// Getters

		public Vec2 get_position()
		{
			return m_position;
		}

		public float get_rotation()
		{
			return m_rotation;
		}

		public Vec2 get_scale()
		{
			return m_scale;
		}

		public Matrix get_camera_mat()
		{
			return m_camera;
		}

		public Matrix get_orthographic_mat()
		{
			return m_orthographic;
		}
	}
}
