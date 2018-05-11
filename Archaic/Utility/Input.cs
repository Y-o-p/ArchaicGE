using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Archaic
{
	class Input
	{
		private Dictionary<ConsoleKey, bool> key_map;

		public Input()
		{
			key_map = new Dictionary<ConsoleKey, bool>();
		}

		public void update()
		{
			foreach (ConsoleKey key in key_map.Keys.ToList())
			{
				key_map[key] = false;
			}

			while (Console.KeyAvailable)
			{
				ConsoleKeyInfo info = Console.ReadKey(true);
				key_map[info.Key] = true;
			}
		}

		public bool key_down(ConsoleKey key)
		{
			if (key_map.ContainsKey(key))
			{
				return key_map[key];
			}

			return false;
		}
	}
}
