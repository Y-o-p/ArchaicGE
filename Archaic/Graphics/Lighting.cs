using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Archaic.Maths;

namespace Archaic
{
	class Lighting
	{
		private static byte[] m_brightness =
		{
			(byte)' ',
			(byte)'.',
			(byte)'\'',
			(byte)'`',
			(byte)'-',
			(byte)'_',
			(byte)'^',
			(byte)'\"',
			(byte)',',
			(byte)'*',
			(byte)':',
			(byte)';',
			(byte)'I',
			(byte)'l',
			(byte)'!',
			(byte)'i',
			(byte)'>',
			(byte)'<',
			(byte)'~',
			(byte)'+',
			(byte)'?',
			(byte)']',
			(byte)'[',
			(byte)'}',
			(byte)'{',
			(byte)'1',
			(byte)')',
			(byte)'(',
			(byte)'|',
			(byte)'\\',
			(byte)'/',
			(byte)'t',
			(byte)'f',
			(byte)'j',
			(byte)'r',
			(byte)'x',
			(byte)'n',
			(byte)'u',
			(byte)'v',
			(byte)'c',
			(byte)'z',
			(byte)'X',
			(byte)'Y',
			(byte)'U',
			(byte)'J',
			(byte)'C',
			(byte)'L',
			(byte)'Q',
			(byte)'0',
			(byte)'O',
			(byte)'Z',
			(byte)'m',
			(byte)'w',
			(byte)'q',
			(byte)'p',
			(byte)'d',
			(byte)'b',
			(byte)'k',
			(byte)'h',
			(byte)'a',
			(byte)'o',
			(byte)'#',
			(byte)'M',
			(byte)'W',
			(byte)'&',
			(byte)'8',
			(byte)'%',
			(byte)'B',
			(byte)'@',
			(byte)'$',
			176,
			177,
			178,
			219
		};

        private static byte[] m_brightness_numbered =
        {
            (byte)'0',
            (byte)'1',
            (byte)'2',
            (byte)'3',
            (byte)'4',
            (byte)'5',
            (byte)'6',
            (byte)'7',
            (byte)'8',
            (byte)'9'
        };


        public static byte get_brightness(int index)
		{
            return m_brightness[(int)clamp(index, 0, m_brightness.Length - 1)];
            //return m_brightness_numbered[(int)clamp(index, 0, m_brightness_numbered.Length - 1)];
        }

		public static int get_max_brightness()
		{
			return m_brightness.Length;
		}

		public class Diffuse
		{
			public Vec3 position;
			public float intensity;

			public Diffuse(Vec3 position, float intensity)
			{
				this.position = position;
				this.intensity = intensity;
			}
		}
	}
}