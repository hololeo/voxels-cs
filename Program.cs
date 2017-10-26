using System;
using System.Diagnostics;
using System.Numerics;
using OpenGL;

namespace Voxels {
    public struct Vertex {
        [VertexAttrib(0, 2, typeof(float))] public Vector2 Position;
    }

    public class Program : IDisposable {
        public static readonly Resources Resources = new Resources();

        private IntPtr _window;
        private VertexArray _vao = new VertexArray();

        private void Init() {
            Glfw.SetErrorCallback((_, desc) => throw new Exception($"GLFW error: {desc}"));
            Glfw.Init();

            Glfw.WindowHint(WindowHint.Resizable, 0);
            Glfw.WindowHint(WindowHint.Visible, 0);
            Glfw.WindowHint(WindowHint.OpenGLProfile, (int) OpenGLProfile.Core);
            Glfw.WindowHint(WindowHint.ContextVersionMajor, 4);
            Glfw.WindowHint(WindowHint.ContextVersionMinor, 3);
            _window = Glfw.CreateWindow(800, 450, "My Window", IntPtr.Zero, IntPtr.Zero);
            Gl.Initialize();
            Glfw.MakeContextCurrent(_window);
            Glfw.ShowWindow(_window);

            Resources.Load();

            var positions = new[] {
                new Vertex { Position = new Vector2(-1f, -1f) },
                new Vertex { Position = new Vector2(0f, 1f) },
                new Vertex { Position = new Vector2(1f, -1f) }
            };

            _vao.Create(() => new[] {
                 new ArrayBuffer().CreateAsVertices(positions, BufferUsage.StaticDraw)
            });
        }

        public void Dispose() {
            _vao?.Dispose();
            Resources?.Dispose();
            Glfw.DestroyWindow(_window);
            Glfw.Terminate();
        }

        private void Run() {
            Console.WriteLine("OpenGL Version: " + Gl.CurrentVersion);
            Console.WriteLine("OpenGL Vendor: " + Gl.CurrentVendor);
            Console.WriteLine("OpenGL Renderer: " + Gl.CurrentRenderer);

            var timewatch = new Stopwatch();
            var framewatch = new Stopwatch();
            timewatch.Start();
            framewatch.Start();
            var lastTime = timewatch.Elapsed;
            var fps = 0;

            while (!Glfw.WindowShouldClose(_window)) {
                var delta = (float) ((timewatch.Elapsed - lastTime).Ticks / (double) TimeSpan.TicksPerSecond);
                lastTime = timewatch.Elapsed;

                Gl.ClearColor(1.0f, 0.5f, 0.2f, 1.0f);
                Gl.Clear(ClearBufferMask.ColorBufferBit);

                Gl.UseProgram(Resources.VoxelProgram);
                Gl.BindVertexArray(_vao.Vao);
                Gl.DrawArrays(PrimitiveType.Triangles, 0, 3);
                Gl.BindVertexArray(0);
                Gl.UseProgram(0);

                Glfw.SwapInterval(1);
                Glfw.SwapBuffers(_window);
                Glfw.PollEvents();
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