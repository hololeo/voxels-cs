using System;
using System.Diagnostics;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;

namespace Voxels {
    public struct Vertex {
        [VertexAttrib(0, 2, typeof(float))] public Vector2 Position;
    }

    public class Program : IDisposable {
        public static readonly Resources Resources = new Resources();

        private GameWindow _window;
        private VertexArray _vao = new VertexArray();

        private void Init() {
            _window = new GameWindow(800, 450, GraphicsMode.Default, "Voxels", GameWindowFlags.FixedWindow,
                DisplayDevice.Default, 4, 6, GraphicsContextFlags.ForwardCompatible) {
                VSync = VSyncMode.Adaptive, Visible = true
            };
            _window.MakeCurrent();

            Resources.Load();

            var positions = new[] {
                new Vertex { Position = new Vector2(-1f, -1f) },
                new Vertex { Position = new Vector2(0f, 1f) },
                new Vertex { Position = new Vector2(1f, -1f) }
            };

            _vao.Create(() => new[] {
                 new ArrayBuffer().CreateAsVertices(positions, BufferUsageHint.StaticDraw)
            });
        }

        public void Dispose() {
            _vao?.Dispose();
            Resources?.Dispose();
        }

        private void Run() {
            Console.WriteLine("OpenGL Version: " + GL.GetString(StringName.Version));
            Console.WriteLine("OpenGL Vendor: " + GL.GetString(StringName.Vendor));
            Console.WriteLine("OpenGL Renderer: " + GL.GetString(StringName.Renderer));

            var timewatch = new Stopwatch();
            var framewatch = new Stopwatch();
            timewatch.Start();
            framewatch.Start();
            var lastTime = timewatch.Elapsed;
            var fps = 0;

            while (!_window.IsExiting) {
                var delta = (float) ((timewatch.Elapsed - lastTime).Ticks / (double) TimeSpan.TicksPerSecond);
                lastTime = timewatch.Elapsed;

                GL.ClearColor(1.0f, 0.5f, 0.2f, 1.0f);
                GL.Clear(ClearBufferMask.ColorBufferBit);

                GL.UseProgram(Resources.VoxelProgram);
                GL.BindVertexArray(_vao.Vao);
                GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
                GL.BindVertexArray(0);
                GL.UseProgram(0);

                _window.SwapBuffers();
                _window.ProcessEvents();
                fps++;

                if (framewatch.ElapsedMilliseconds < 1000) continue;
                Console.WriteLine($"{fps} fps");
                framewatch.Restart();
                fps = 0;
            }
        }

        public static void Main(string[] args) {
            using (var program = new Program()) {
                program.Init();
                program.Run();
            }
        }
    }
}