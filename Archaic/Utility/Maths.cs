using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archaic
{
	class Maths
	{
		public struct Vec4
		{
			public float x;
			public float y;
			public float z;
			public float w;

			public Vec4(float x, float y, float z, float w)
			{
				this.x = x;
				this.y = y;
				this.z = z;
				this.w = w;
			}

			public Vec3 normalize()
			{
				return new Vec3(x / w, y / w, z / w);
			}

			public void add(Vec4 vec)
			{
				x += vec.x;
				y += vec.y;
				z += vec.z;
				w += vec.w;
			}

			public void subtract(Vec4 vec)
			{
				x -= vec.x;
				y -= vec.y;
				z -= vec.z;
				w -= vec.w;
			}

			public void multiply(Vec4 vec)
			{
				x *= vec.x;
				y *= vec.y;
				z *= vec.z;
				w *= vec.w;
			}

			public void multiply(float f)
			{
				x *= f;
				y *= f;
				z *= f;
				w *= f;
			}

			public void multiply(Matrix mat)
			{
				Vec2 mat_size = mat.get_size();

				if (mat_size.x == 4)
				{

					float[] vec =
					{
						this.x, this.y, z, w
					};

					float[] vec2 =
					{
						this.x, this.y, z, w
					};

					for (int y = 0; y < mat_size.y; y++)
					{
						float temp = 0.0f;
						for (int x = 0; x < mat_size.x; x++)
						{
							temp += mat.data[y, x] * vec2[x];
						}
						vec[y] = temp;
					}

					this.x = vec[0];
					this.y = vec[1];
					z = vec[2];
					w = vec[3];
				}
			}

			public void divide(Vec4 vec)
			{
				x /= vec.x;
				y /= vec.y;
				z /= vec.z;
				w /= vec.w;
			}

			public static bool operator ==(Vec4 vec_a, Vec4 vec_b)
			{
				if (vec_a.x == vec_b.x && vec_a.y == vec_b.y && vec_a.z == vec_b.z && vec_a.w == vec_b.w)
				{
					return true;
				}

				return false;
			}

			public static bool operator !=(Vec4 vec_a, Vec4 vec_b)
			{
				if (vec_a.x != vec_b.x || vec_a.y != vec_b.y || vec_a.z != vec_b.z || vec_a.w != vec_b.w)
				{
					return true;
				}

				return false;
			}

			public static Vec4 operator +(Vec4 vec_a, Vec4 vec_b)
			{
				vec_a.add(vec_b);
				return vec_a;
			}

			public static Vec4 operator -(Vec4 vec_a, Vec4 vec_b)
			{
				vec_a.subtract(vec_b);
				return vec_a;
			}

			public static Vec4 operator *(Vec4 vec_a, Vec4 vec_b)
			{
				vec_a.multiply(vec_b);
				return vec_a;
			}

			public static Vec4 operator *(Vec4 vec_a, float f)
			{
				vec_a.multiply(f);
				return vec_a;
			}

			public static Vec4 operator *(Vec4 vec_a, Matrix matrix)
			{
				vec_a.multiply(matrix);
				return vec_a;
			}

			public static Vec4 operator /(Vec4 vec_a, Vec4 vec_b)
			{
				vec_a.divide(vec_b);
				return vec_a;
			}
		};

		public struct Vec3
		{
			public float x;
			public float y;
			public float z;

			public Vec3(float x, float y, float z)
			{
				this.x = x;
				this.y = y;
				this.z = z;
			}

			public static float dot(Vec3 vec_a, Vec3 vec_b)
			{
				return vec_a.x * vec_b.x + vec_a.y * vec_b.y + vec_a.z * vec_b.z;
			}

			public static float length(Vec3 vec)
			{
				return (float)Math.Sqrt(vec.x * vec.x + vec.y * vec.y + vec.z * vec.z);
			}

			public static Vec3 normalize(Vec3 vec)
			{
				float magnitude = length(vec);
				return new Vec3(vec.x / magnitude, vec.y / magnitude, vec.z / magnitude);
			}

			public void add(Vec3 vec)
			{
				x += vec.x;
				y += vec.y;
				z += vec.z;
			}

			public void subtract(Vec3 vec)
			{
				x -= vec.x;
				y -= vec.y;
				z -= vec.z;
			}

			public void multiply(Vec3 vec)
			{
				x *= vec.x;
				y *= vec.y;
				z *= vec.z;
			}

			public void multiply(float f)
			{
				x *= f;
				y *= f;
				z *= f;
			}

			public void multiply(Matrix mat)
			{
				Vec2 mat_size = mat.get_size();

				float[] vec =
				{
					this.x, this.y, z, 1.0f
				};

				float[] vec2 =
				{
					this.x, this.y, z, 1.0f
				};

				for (int y = 0; y < mat_size.y; y++)
				{
					float temp = 0.0f;
					for (int x = 0; x < mat_size.x; x++)
					{
						temp += mat.data[y, x] * vec2[x];
					}
					vec[y] = temp;
				}

				this.x = vec[0];
				this.y = vec[1];
				z = vec[2];
			}

			public void divide(Vec3 vec)
			{
				x /= vec.x;
				y /= vec.y;
				z /= vec.z;
			}

			public static bool operator ==(Vec3 vec_a, Vec3 vec_b)
			{
				if (vec_a.x == vec_b.x && vec_a.y == vec_b.y && vec_a.z == vec_b.z)
				{
					return true;
				}

				return false;
			}

			public static bool operator !=(Vec3 vec_a, Vec3 vec_b)
			{
				if (vec_a.x != vec_b.x || vec_a.y != vec_b.y || vec_a.z != vec_b.z)
				{
					return true;
				}

				return false;
			}

			public static Vec3 operator +(Vec3 vec_a, Vec3 vec_b)
			{
				vec_a.add(vec_b);
				return vec_a;
			}

			public static Vec3 operator -(Vec3 vec_a, Vec3 vec_b)
			{
				vec_a.subtract(vec_b);
				return vec_a;
			}

			public static Vec3 operator *(Vec3 vec_a, Vec3 vec_b)
			{
				vec_a.multiply(vec_b);
				return vec_a;
			}

			public static Vec3 operator *(Vec3 vec_a, float f)
			{
				vec_a.multiply(f);
				return vec_a;
			}

			public static Vec3 operator *(Vec3 vec_a, Matrix matrix)
			{
				vec_a.multiply(matrix);
				return vec_a;
			}

			public static Vec3 operator /(Vec3 vec_a, Vec3 vec_b)
			{
				vec_a.divide(vec_b);
				return vec_a;
			}
		};

		public struct Vec2
		{
			public float x;
			public float y;

			public Vec2(float x, float y)
			{
				this.x = x;
				this.y = y;
			}

			public float distance(Vec2 vec)
			{
				return (float)(Math.Sqrt(Math.Pow((vec.x - x), 2) + Math.Pow((vec.y - y), 2)));
			}

			public static float dot(Vec2 vec_a, Vec2 vec_b)
			{
				return vec_a.x * vec_b.x + vec_a.y * vec_b.y;
			}

			public void add(Vec2 vec)
			{
				x += vec.x;
				y += vec.y;
			}

			public void subtract(Vec2 vec)
			{
				x -= vec.x;
				y -= vec.y;
			}

			public void multiply(Vec2 vec)
			{
				x *= vec.x;
				y *= vec.y;
			}

			public void multiply(float f)
			{
				x *= f;
				y *= f;
			}

			public void divide(Vec2 vec)
			{
				x /= vec.x;
				y /= vec.y;
			}

			public static bool operator ==(Vec2 vec_a, Vec2 vec_b)
			{
				if (vec_a.x == vec_b.x && vec_a.y == vec_b.y)
				{
					return true;
				}

				return false;
			}

			public static bool operator !=(Vec2 vec_a, Vec2 vec_b)
			{
				if (vec_a.x != vec_b.x || vec_a.y != vec_b.y)
				{
					return true;
				}

				return false;
			}

			public static Vec2 operator +(Vec2 vec_a, Vec2 vec_b)
			{
				vec_a.add(vec_b);
				return vec_a;
			}

			public static Vec2 operator -(Vec2 vec_a, Vec2 vec_b)
			{
				vec_a.subtract(vec_b);
				return vec_a;
			}

			public static Vec2 operator *(Vec2 vec_a, Vec2 vec_b)
			{
				vec_a.multiply(vec_b);
				return vec_a;
			}

			public static Vec2 operator *(Vec2 vec_a, float f)
			{
				vec_a.multiply(f);
				return vec_a;
			}

			public static Vec2 operator /(Vec2 vec_a, Vec2 vec_b)
			{
				vec_a.divide(vec_b);
				return vec_a;
			}
		};

		public class Matrix
		{
			public float[,] data;

			public Matrix()
			{
				data = new float[,] { };
			}

			public Matrix(float[,] data)
			{
				this.data = data;
			}

			public Matrix copy()
			{
				Vec2 array_size = get_size();
				float[,] copied_data = new float[(uint)array_size.x, (uint)array_size.y];
				Array.Copy(data, copied_data, data.Length);
				return new Matrix(copied_data);
			}

			public static Matrix identity()
			{
				float[,] data =
				{
					{ 1.0f, 0, 0, 0 },
					{ 0, 1.0f, 0, 0 },
					{ 0, 0, 1.0f, 0 },
					{ 0, 0, 0, 1.0f },
				};

				return new Matrix(data);
			}

			public static Matrix perspective_right(float near, float far, float window_width, float window_height, float fov)
			{
				float height = (float)(Math.Cos(0.5f * fov) / Math.Sin(0.5f * fov));
				float width = height * window_height / window_width;

				float[,] data = new float[4, 4] {
					{ width, 0.0f, 0.0f, 0.0f },
					{ 0.0f, height, 0.0f, 0.0f },
					{ 0.0f, 0.0f, -(far + near) / (far - near), -1.0f },
					{ 0.0f, 0.0f, -(2 * far * near) / (far - near), 0.0f },
				};

				return new Matrix(data);
			}

			public static Matrix perspective(float near, float far, float window_width, float window_height, float fov)
			{
				float aspect = window_width / window_height;
				float tan_fov = (float)(Math.Tan(fov / 2.0f));

				float[,] data = new float[4, 4] {
					{ 1.0f / (aspect * tan_fov), 0.0f, 0.0f, 0.0f },
					{ 0.0f, 1.0f / tan_fov, 0.0f, 0.0f },
					{ 0.0f, 0.0f, -(far + near) / (far - near), -(2.0f * far * near) / (far - near) },
					{ 0.0f, 0.0f, -1.0f, 0.0f },
				};

				return new Matrix(data);
			}

			public static Matrix perspective_left(float near, float far, float window_width, float window_height, float fov)
			{
				float height = (float)(Math.Cos(0.5f * fov) / Math.Sin(0.5f * fov));
				float width = height * window_height / window_width;

				float[,] data = new float[4, 4] {
					{ width, 0.0f, 0.0f, 0.0f },
					{ 0.0f, height, 0.0f, 0.0f },
					{ 0.0f, 0.0f, (far + near) / (far - near), 1.0f },
					{ 0.0f, 0.0f, -(2.0f * far * near) / (far - near), 0.0f },
				};

				/*float aspect = window_width / window_height;
				float tan_fov = (float)(Math.Tan(fov / 2.0f));

				float[,] data = new float[4, 4] {
					{ 1.0f / (aspect * tan_fov), 0.0f, 0.0f, 0.0f },
					{ 0.0f, 1.0f / tan_fov, 0.0f, 0.0f },
					{ 0.0f, 0.0f, -(far + near) / (far - near), -(2 * far * near) / (far - near) },
					{ 0.0f, 0.0f, -1.0f, 0.0f },
				};*/

				return new Matrix(data);
			}

			public static Matrix orthographic(float window_width, float window_height)
			{
				float[,] data = new float[4, 4]
				{
					{ 1.0f / window_width, 0.0f, 0.0f, 0.0f },
					{ 0.0f, 1.0f / window_height, 0.0f, 0.0f },
					{ 0.0f, 0.0f, 1.0f, 0.0f },
					{ 0.0f, 0.0f, 0.0f, 1.0f },
				};

				return new Matrix(data);
			}

			public Vec2 get_size()
			{
				return new Vec2(data.GetLength(1), data.GetLength(0));
			}

			/// <summary>
			/// Returns a matrix with translation values
			/// </summary>
			/// <param name="x"></param>
			/// <param name="y"></param>
			/// <param name="z"></param>
			/// <returns></returns>
			public static Matrix translate(float x, float y, float z)
			{
				Matrix translation = identity();
				translation.data[0, 3] = x;
				translation.data[1, 3] = y;
				translation.data[2, 3] = z;

				return translation;
			}

			public static Matrix rotate_x(float value)
			{
				Matrix rotation = identity();
				rotation.data[1, 1] = (float)Math.Cos(value);
				rotation.data[1, 2] = (float)-Math.Sin(value);
				rotation.data[2, 1] = (float)Math.Sin(value);
				rotation.data[2, 2] = (float)Math.Cos(value);

				return rotation;
			}

			public static Matrix rotate_y(float value)
			{
				Matrix rotation = identity();
				rotation.data[0, 0] = (float)Math.Cos(value);
				rotation.data[0, 2] = (float)Math.Sin(value);
				rotation.data[2, 0] = (float)-Math.Sin(value);
				rotation.data[2, 2] = (float)Math.Cos(value);

				return rotation;
			}

			public static Matrix rotate_z(float value)
			{
				Matrix rotation = identity();
				rotation.data[0, 0] = (float)Math.Cos(value);
				rotation.data[0, 1] = (float)-Math.Sin(value);
				rotation.data[1, 0] = (float)Math.Sin(value);
				rotation.data[1, 1] = (float)Math.Cos(value);

				return rotation;
			}

			public static Matrix scale(float x, float y, float z)
			{
				Matrix scale = identity();
				scale.data[0, 0] = x;
				scale.data[1, 1] = y;
				scale.data[2, 2] = z;

				return scale;
			}

			public void add(Matrix mat)
			{
				if (get_size() == mat.get_size())
				{
					for (int y = 0; y < data.GetLength(0); y++)
					{
						for (int x = 0; x < data.GetLength(1); x++)
						{
							data[x, y] += mat.data[x, y];
						}
					}
				}
			}

			public void multiply(Matrix mat)
			{
				Vec2 mat_a_size = get_size();
				Vec2 mat_b_size = mat.get_size();

				Matrix temp_mat = new Matrix(new float[(int)mat_a_size.x, (int)mat_a_size.y]);
				Array.Copy(data, temp_mat.data, data.Length);

				if (mat_a_size == mat_b_size)
				{
					for (int y = 0; y < mat_a_size.y; y++)
					{
						for (int xb = 0; xb < mat_b_size.x; xb++)
						{
							float temp = 0.0f;
							for (int yb = 0; yb < mat_b_size.y; yb++)
							{
								temp += temp_mat.data[y, yb] * mat.data[yb, xb];
							}

							data[y, xb] = temp;
						}
					}
				}
			}

			public static Matrix operator +(Matrix mat_a, Matrix mat_b)
			{
				mat_a.add(mat_b);
				return mat_a;
			}

			public static Matrix operator *(Matrix mat_a, Matrix mat_b)
			{
				Matrix new_matrix = mat_a.copy();
				new_matrix.multiply(mat_b);
				return new_matrix;
			}

			public static Matrix matrix3x3(Matrix mat)
			{
				Matrix copy = mat.copy();
				var data = new float[3, 3]
				{
					{ copy.data[0, 0], copy.data[0, 1], copy.data[0, 2] },
					{ copy.data[1, 0], copy.data[1, 1], copy.data[1, 2] },
					{ copy.data[2, 0], copy.data[2, 1], copy.data[2, 2] }
				};
				return new Matrix(data);
			}
		};

		public static float clamp(float value, float min, float max)
		{
			return Math.Max(Math.Min(value, max), min);
		}

		public static float radians(float degrees)
		{
			return (float)(degrees * Math.PI) / 180.0f;
		}

		public static float safe_divide(float numerator, float denominator)
		{
			return denominator != 0.0f ? numerator / denominator : 0.0f;
		}

		public static float interpolate(float alpha, float a, float b)
		{
			return a * (1 - alpha) + b * alpha;
		}

		public static Vec3 interpolate(float alpha, Vec3 a, Vec3 b)
		{
			Vec3 c = new Vec3();
			c.x = interpolate(alpha, a.x, b.x);
			c.y = interpolate(alpha, a.y, b.y);
			c.z = interpolate(alpha, a.z, b.z);
			return c;
		}

		public static Vec3 barycentric(Vec2 point, Vec2 a, Vec2 b, Vec2 c)
		{
			Vec2 vec_a = b - a, vec_b = c - a, vec_c = point - a;
			float d00 = Vec2.dot(vec_a, vec_a);
			float d01 = Vec2.dot(vec_a, vec_b);
			float d11 = Vec2.dot(vec_b, vec_b);
			float d20 = Vec2.dot(vec_c, vec_a);
			float d21 = Vec2.dot(vec_c, vec_b);
			float denominator = d00 * d11 - d01 * d01;

			float v = (d11 * d20 - d01 * d21) / denominator;
			float w = (d00 * d21 - d01 * d20) / denominator;
			float u = 1.0f - v - w;
			return new Vec3(u, v, w);
		}
	}
}