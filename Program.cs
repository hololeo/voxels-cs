using System;
using System.IO;
using System.Diagnostics;
using System.Threading.Tasks;
using OpenGL;
using OpenGL.Objects;

namespace Voxels {
    public class Program : IDisposable {
        private IntPtr _window;
        private Shader _shader;
        private ShaderProgram _program;

        private void Init() {
            Glfw.SetErrorCallback((_, desc) => throw new Exception($"GLFW error: {desc}"));
            Glfw.Init();

            Glfw.WindowHint(WindowHint.Resizable, 0);
            Glfw.WindowHint(WindowHint.Visible, 0);
            Glfw.WindowHint(WindowHint.OpenGLProfile,
                (int) OpenGLProfile.Core);
            Glfw.WindowHint(WindowHint.ContextVersionMajor, 4);
            Glfw.WindowHint(WindowHint.ContextVersionMinor, 3);
            _window = Glfw.CreateWindow(800, 450, "My Window", IntPtr.Zero, IntPtr.Zero);
            Gl.Initialize();
            Glfw.MakeContextCurrent(_window);
            Glfw.ShowWindow(_window);

            var ctx = new GraphicsContext(DeviceContext.Create());
            var lines = File.ReadAllLines("Assets/Vertex.glsl");

            _shader = new Shader(ShaderType.VertexShader);
            _shader.LoadSource(lines);
            _shader.Create(ctx);
            _program = new ShaderProgram("program");
            _program.AttachShader(_shader);
            _program.Create(ctx);
        }

        public void Dispose() {
            _shader?.Dispose();
            _program?.Dispose();
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