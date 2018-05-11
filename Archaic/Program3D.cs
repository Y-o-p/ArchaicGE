using Archaic.Renderables;
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
    class Program3D
    {
		static public Window window;

		static public Camera3D camera_3D;

		static void init(int window_width, int window_height)
        {
			window = new Window(window_width, window_height);

			camera_3D = new Camera3D(window_width, window_height, 50.0f, 70.0f);

			Console.CursorVisible = false;

            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.BackgroundColor = ConsoleColor.Black;
        }

        static void Main(string[] args)
        {
			Time time = new Time();
			Input input = new Input();
			ResourceRetriever resources = new ResourceRetriever();

			Vec2 window_size = new Vec2(600, 180);
            init((int)window_size.x, (int)window_size.y);

			camera_3D.set_position(new Vec3(0.0f, 0.0f, 8.0f));
			camera_3D.update();
			Rasterizer.bind_projection_matrix(camera_3D.get_perspective_mat());

			Renderer3D renderer = new Renderer3D();
			Mesh cube = new Mesh(resources.read_mesh("cube.obj"));
			Mesh monkey = new Mesh(resources.read_mesh("monkeyboi.obj"));
			Mesh triangle = new Mesh(resources.read_mesh("triangle.obj"));

			cube.set_position(new Vec3(-5.0f, 0.0f, 0.0f));
			monkey.set_position(new Vec3(0.0f, 0.0f, 0.0f));
			triangle.set_position(new Vec3(5.0f, 0.0f, 0.0f));

			Diffuse light1 = new Diffuse(new Vec3(0.0f, 1.0f, 1.0f), 1.0f);

			bool pause = true;

			while (true)
            {
				float delta_time = time.get_delta_time();

				if (!pause)
				{
					cube.set_rotation(cube.get_rotation() + new Vec3(delta_time * 0.3f, 0.0f, 0.0f));
					monkey.set_rotation(monkey.get_rotation() + new Vec3(0.0f, delta_time * 0.3f, 0.0f));
					triangle.set_rotation(triangle.get_rotation() + new Vec3(0.0f, 0.0f, delta_time * 0.3f));
				}

				camera_3D.update();
				Rasterizer.bind_camera_matrix(camera_3D.get_camera_mat());
				renderer.submit_light(light1);

				renderer.start();

				renderer.render(cube);
				renderer.render(monkey);
				renderer.render(triangle);

				input.update();

				float speed = 2.0f * delta_time;

				if (input.key_down(ConsoleKey.W))
				{
					camera_3D.set_position(camera_3D.get_position() + new Vec3(0.0f, speed, 0.0f));
					//light1.position.y += speed;
				}

				if (input.key_down(ConsoleKey.A))
				{
					camera_3D.set_position(camera_3D.get_position() + new Vec3(-speed, 0.0f, 0.0f));
					//light1.position.x -= speed;
				}

				if (input.key_down(ConsoleKey.S))
				{
					camera_3D.set_position(camera_3D.get_position() + new Vec3(0.0f, -speed, 0.0f));
					//light1.position.y -= speed;
				}

				if (input.key_down(ConsoleKey.D))
				{
					camera_3D.set_position(camera_3D.get_position() + new Vec3(speed, 0.0f, 0.0f));
					//light1.position.x += speed;
				}

				if (input.key_down(ConsoleKey.Q))
				{
					camera_3D.set_position(camera_3D.get_position() + new Vec3(0.0f, 0.0f, -speed));
				}

				if (input.key_down(ConsoleKey.E))
				{
					camera_3D.set_position(camera_3D.get_position() + new Vec3(0.0f, 0.0f, speed));
				}

				if (input.key_down(ConsoleKey.UpArrow))
				{
					camera_3D.set_rotation(camera_3D.get_rotation() + new Vec3(speed, 0.0f, 0.0f));
				}

				if (input.key_down(ConsoleKey.LeftArrow))
				{
					camera_3D.set_rotation(camera_3D.get_rotation() + new Vec3(0.0f, speed, 0.0f));
				}

				if (input.key_down(ConsoleKey.DownArrow))
				{
					camera_3D.set_rotation(camera_3D.get_rotation() + new Vec3(-speed, 0.0f, 0.0f));
				}

				if (input.key_down(ConsoleKey.RightArrow))
				{
					camera_3D.set_rotation(camera_3D.get_rotation() + new Vec3(0.0f, -speed, 0.0f));
				}

				if (input.key_down(ConsoleKey.T))
				{
					pause = true;
				}

				if (input.key_down(ConsoleKey.Y))
				{
					pause = false;
				}

				float fps = time.get_fps();
				Rasterizer.text(new Vec2(0.0f, window_size.y - 1.0f), fps.ToString());

				renderer.flush();
				time.cycle();
			}
        }
    }
}