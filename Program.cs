using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;

namespace Voxels {
    public class Program : IDisposable {
        public static Program Instance { get; private set; }
        public static Resources Resources { get; } = new Resources();
        public static float AspectRatio { get; private set; }
        public static GameWindow Window => Instance._window;

        private GameWindow _window;
        private World _world;

        private Program() {
            Instance = this;
            _window = new GameWindow(800, 450, GraphicsMode.Default, "Voxels", GameWindowFlags.FixedWindow,
                DisplayDevice.Default, 4, 6, GraphicsContextFlags.ForwardCompatible | GraphicsContextFlags.Debug) {
                VSync = VSyncMode.Adaptive, Visible = true
            };
            _window.MakeCurrent();

            ConfigureGLDebug();

            _window.CursorVisible = false;
            _window.KeyDown += (sender, args) => {
                if (args.Key != Key.Escape) return;
                _window.CursorVisible = !_window.CursorVisible;
            };

            Resources.Load();
            AspectRatio = 800f / 450f;

            var random = new Random(123456);
            _world = new World();
            _world.GenerateChunk(1, 1, 1, random);
            _world.GenerateChunk(0, 0, 0, random);
            _world.GenerateChunk(0, 2, 0, random);
            _world.GenerateChunk(0, 0, 3, random);
            _world.GenerateChunk(-7, 0, 0, random);
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

                _world.Update(delta);
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

        private static void ConfigureGLDebug() {
            var flags = (ContextFlagMask) GL.GetInteger(GetPName.ContextFlags);
            if ((flags & ContextFlagMask.ContextFlagDebugBit) != ContextFlagMask.ContextFlagDebugBit) return;
            GL.Enable(EnableCap.DebugOutput);
            GL.Enable(EnableCap.DebugOutputSynchronous);
            GL.DebugMessageControl(DebugSourceControl.DontCare, DebugTypeControl.DontCare,
                DebugSeverityControl.DontCare, 0, new int[0], true);
            GL.DebugMessageCallback((source, type, id, severity, length, message, param) => {
                Console.WriteLine($"(Debug) {type}, {id}, {severity}: {Marshal.PtrToStringAnsi(message)}");
            }, IntPtr.Zero);
        }

        public static void Main(string[] args) {
            Debug.WriteLine(Process.GetCurrentProcess().Id);
            using (var program = new Program()) {
                program.Run();
            }
        }
    }
}