using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archaic
{
	class Window
	{
		// Window size
		private int window_width;
		private int window_height;

		// Window name
		private string window_name;

		public Window(int window_width, int window_height, string window_name = "Archaic")
		{
			set_window_res(window_width, window_height);
			set_window_name(window_name);
		}

		public void set_window_res(int window_width, int window_height)
		{
			this.window_width = window_width;
			this.window_height = window_height;
			Rasterizer.set_viewport(window_width, window_height);

			change_window:
			try
			{
				Console.SetWindowSize(window_width, window_height);
				Console.SetBufferSize(window_width, window_height + 1);
			}
			catch
			{
				Console.WriteLine("Error: Font size too large.");
				Console.WriteLine("Solution: Right click on the console. Click on properties. Go to the Font tab. Change the font to your liking. Change the font size to the lowest possible value. Press \"Enter\" when ready.");
				Console.ReadLine();
				goto change_window;
			}
		}

		public void set_window_name(string window_name)
		{
			this.window_name = window_name;
			Console.Title = window_name;
		}
	}
}
