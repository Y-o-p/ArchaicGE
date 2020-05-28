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

		private List<float> get_line_data(string data, int start)
		{
			var vals = new List<float>();
			String float_str = "";
			for (int i = start; i < data.Length; i++)
			{
				char curr_char = data[i];
				if (curr_char != ' ')
				{
					float_str += data[i];
				}
				if ((curr_char == ' ' && float_str != "") || i == data.Length - 1)
				{
					vals.Add(float.Parse(float_str, CultureInfo.InvariantCulture.NumberFormat));
					float_str = "";
				}
			}

			return vals;
		}

		private float string_to_float(String str)
		{
			return float.Parse(str, CultureInfo.InvariantCulture.NumberFormat);
		}

		private List<String> parse_line(String line)
		{
			var string_list = new List<String>();
			var curr_string = "";
			for (int i = 0; i < line.Length; i++)
			{
				var curr_char = line[i];
				// If we're not on a space then add the current char to the string
				if (curr_char != ' ')
				{
					curr_string += line[i];
				}
				// If we are on a space add the string to the list and reset the string
				else
				{
					string_list.Add(curr_string);
					curr_string = "";
				}
			}
			// Adds the last string parsed
			string_list.Add(curr_string);

			return string_list;
		}

		private Tuple<int, int, int> parse_face(String face)
		{
			List<String> strings = new List<String>();
			String curr_string = "";
			for (int i = 0; i < face.Length; i++)
			{
				var curr_char = face[i];
				if (curr_char != '/')
				{
					curr_string += curr_char;
				}
				else
				{
					strings.Add(curr_string);
					curr_string = "";
				}
			}
			strings.Add(curr_string);

			return new Tuple<int, int, int>(int.Parse(strings[0]), int.Parse(strings[1]), int.Parse(strings[2]));
		}

		private MeshData parse_mesh(IEnumerable<string> data)
		{
			var vertices = new List<Vertex3D>();

			var positions = new List<Vec3>();
			var uvs = new List<Vec2>();
			var normals = new List<Vec3>();

			var vertex_table = new Dictionary<string, ushort>();

			foreach (var line in data)
			{
				var parsed_line = parse_line(line);
				if (parsed_line.Count > 0) {
					switch (parsed_line[0])
					{
						case "v":
							positions.Add(new Vec3(string_to_float(parsed_line[1]), string_to_float(parsed_line[2]), string_to_float(parsed_line[3])));
							break;
						case "vt":
							uvs.Add(new Vec2(string_to_float(parsed_line[1]), string_to_float(parsed_line[2])));
							break;
						case "vn":
							normals.Add(new Vec3(string_to_float(parsed_line[1]), string_to_float(parsed_line[2]), string_to_float(parsed_line[3])));
							break;
						case "f":
							for (int i = 1; i < parsed_line.Count; i++)
							{
								String curr_face = parsed_line[i];
								var face_data = parse_face(curr_face);
								vertices.Add(new Vertex3D(positions[face_data.Item1 - 1], uvs[face_data.Item2 - 1], normals[face_data.Item3 - 1]));
							}
							break;
						default:
							break;
					}
				}
			}

			return new MeshData(vertices.ToArray());
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
