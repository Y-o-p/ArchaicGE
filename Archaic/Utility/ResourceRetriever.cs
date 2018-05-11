using Archaic.Renderables;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using static Archaic.Maths;

namespace Archaic
{
	class ResourceRetriever
	{
		private Dictionary<string, MeshData> mesh_table;

		public ResourceRetriever()
		{
			mesh_table = new Dictionary<string, MeshData>();
		}

		private MeshData parse_mesh(IEnumerable<string> data)
		{
			var vertices = new List<Vertex3D>();

			var positions = new List<Vec3>();
			var normals = new List<Vec3>();
			var indices = new List<ushort>();

			var vertex_table = new Dictionary<string, ushort>();

			foreach (var line in data)
			{
				int pos = 0;
				switch (line[pos])
				{
					case 'v':
						{
							pos++;

							bool normal;
							if (line[pos] == 'n')
							{
								normal = true;
								pos += 2;
							}
							else if (line[pos] == ' ')
							{
								normal = false;
								pos++;
							}
							else
							{
								break;
							}

							var float_strings = new string[] {
								"", "", ""
							};
							var floats = new float[3];

							var curr_string = 0;

							// Loop to get all three floats
							for (int i = pos; i < line.Length; i++)
							{
								if (line[i] == ' ')
								{
									curr_string++;
								}
								else
								{
									float_strings[curr_string] += line[i];
								}
							}

							floats[0] = float.Parse(float_strings[0], CultureInfo.InvariantCulture.NumberFormat);
							floats[1] = float.Parse(float_strings[1], CultureInfo.InvariantCulture.NumberFormat);
							floats[2] = float.Parse(float_strings[2], CultureInfo.InvariantCulture.NumberFormat);

							// In case it is a normal
							if (normal)
							{
								normals.Add(new Vec3(floats[0], floats[1], floats[2]));
							}
							else
							{
								positions.Add(new Vec3(floats[0], floats[1], floats[2]));
							}
						}
						break;
					case 'f':
						{
							pos++;

							if (line[pos] != ' ')
							{
								break;
							}

							pos++;

							var int_strings = new string[]
							{
								"", "", "",
								"", "", "",
								"", "", ""
							};

							var curr_string = 0;

							for (int i = pos; i < line.Length; i++)
							{
								if (line[i] == ' ' || line[i] == '/')
								{
									curr_string++;
								}
								else
								{
									int_strings[curr_string] += line[i]; 
								}
							}

							for (int i = 0; i < 9; i += 3)
							{
								string key = int_strings[i] + int_strings[i + 1] + int_strings[i + 2];
								if (!vertex_table.ContainsKey(key))
								{
									vertices.Add(new Vertex3D(positions[int.Parse(int_strings[i]) - 1], normals[int.Parse(int_strings[i + 2]) - 1]));
									ushort index = (ushort)(vertices.Count - 1);
									vertex_table.Add(key, index);

									indices.Add(index);
								}
								else
								{
									indices.Add(vertex_table[key]);
								}
							}
						}
						break;
					default:
						break;
				}
			}

			return new MeshData(vertices.ToArray(), indices.ToArray());
		}

		public MeshData read_mesh(string file_path)
		{
			if (!mesh_table.ContainsKey(file_path))
			{
				var file_data = System.IO.File.ReadLines(file_path);
				mesh_table[file_path] = parse_mesh(file_data);
			}

			return mesh_table[file_path].copy();
		}
	}
}
