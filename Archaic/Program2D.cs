/*using Archaic.Renderables;
using static Archaic.Maths;
using static Archaic.Lighting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Archaic.Graphics._2D;

namespace Archaic
{
	class Program2D
	{
		static public Window window;

		static public Camera2D camera_2D;

		static void init(int window_width, int window_height)
		{
			window = new Window(window_width, window_height);

			camera_2D = new Camera2D(window_width, window_height);

			Console.CursorVisible = false;

			Console.ForegroundColor = ConsoleColor.DarkGreen;
			Console.BackgroundColor = ConsoleColor.Black;
		}

		static void Main(string[] args)
		{
			Time time = new Time();
			Input input = new Input();

			Vec2 window_size = new Vec2(600, 180);
			init((int)window_size.x, (int)window_size.y);

			camera_2D.set_position(new Vec2(0.0f, 0.0f));
			camera_2D.update();
			Rasterizer.bind_projection_matrix(camera_2D.get_orthographic_mat());

			Renderer2D renderer = new Renderer2D((int)window_size.x, (int)window_size.y);
			Sprite square = new Sprite(0.0f, 0.0f, 32.0f, 32.0f);
			
			while (true)
			{
				float delta_time = time.get_delta_time();

				camera_2D.update();
				Rasterizer.bind_camera_matrix(camera_2D.get_camera_mat());

				renderer.start();
				renderer.submit(square);

				input.update();

				float speed = 6.0f * delta_time;

				if (input.key_down(ConsoleKey.W))
				{
					camera_2D.set_position(camera_2D.get_position() + new Vec2(0.0f, speed));
				}

				if (input.key_down(ConsoleKey.A))
				{
					camera_2D.set_position(camera_2D.get_position() + new Vec2(-speed, 0.0f));
				}

				if (input.key_down(ConsoleKey.S))
				{
					camera_2D.set_position(camera_2D.get_position() + new Vec2(0.0f, -speed));
				}

				if (input.key_down(ConsoleKey.D))
				{
					camera_2D.set_position(camera_2D.get_position() + new Vec2(speed, 0.0f));
				}

				float fps = time.get_fps();
				Rasterizer.text(new Vec2(0.0f, window_size.y - 1.0f), fps.ToString());

				renderer.flush();
				time.cycle();
			}
		}
	}
}*/