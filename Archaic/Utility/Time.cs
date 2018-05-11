using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Archaic
{
	class Time
	{
		private Stopwatch m_timer;

		private float m_fps = 0.0f;
		private float m_delta_time = 0.0f;

		private double m_current_ns = 0.0, m_previous_ns = 0.0;

		public Time()
		{
			m_timer = new Stopwatch();
			m_timer.Start();
		}

		private double to_milliseconds(double ticks)
		{
			// Returns the number of nanoseconds passed
			return 1000.0 * (ticks / Stopwatch.Frequency);
		}

		public void cycle()
		{
			//m_timer.Stop();
			m_current_ns = m_timer.ElapsedMilliseconds;
			//m_timer.Start();
			double difference = m_current_ns - m_previous_ns;
			m_previous_ns = m_timer.ElapsedMilliseconds;

			if (difference > 0.0)
			{
				m_fps = (float)(1000.0 / difference);
			}

			m_delta_time = (float)(difference / 1000.0);

		}

		public float get_fps()
		{
			return m_fps;
		}

		public float get_delta_time()
		{
			return m_delta_time;
		}
	}
}
