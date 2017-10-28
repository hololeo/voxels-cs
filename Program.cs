using System;
using System.Diagnostics;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;

namespace Voxels {
    public class Program : IDisposable {
        public static Resources Resources { get; private set; } = new Resources();
        public static float AspectRatio { get; private set; }

        private GameWindow _window;
        private World _world;

        private Program() {
            _window = new GameWindow(800, 450, GraphicsMode.Default, "Voxels", GameWindowFlags.FixedWindow,
                DisplayDevice.Default, 4, 6, GraphicsContextFlags.ForwardCompatible) {
                VSync = VSyncMode.Adaptive, Visible = true
            };
            _window.MakeCurrent();

            Resources.Load();
            AspectRatio = 800f / 450f;

            _world = new World();
            _world.GenerateChunk(1, 1, 1);
        }

        public void Dispose() {
            _world?.Dispose();
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

                GL.Clear(ClearBufferMask.ColorBufferBit);

                _world.Render();

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
                program.Run();
            }
        }
    }
}