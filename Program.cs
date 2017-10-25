﻿using System;
using System.Diagnostics;
using System.Threading.Tasks;
using OpenGL;

namespace Voxels {
    public class Program : IDisposable {
        public static readonly Resources Resources = new Resources();

        private IntPtr _window;
        private uint _buffer;

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
                -1f, -1f,
                0f, 1f,
                1f, -1f
            };

            _buffer = Gl.GenBuffer();
            Gl.BindBuffer(BufferTarget.ArrayBuffer, _buffer);
            Gl.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * 6, positions, BufferUsage.StaticDraw);
            Gl.EnableVertexAttribArray(0);
            Gl.VertexAttribPointer(0, 2, VertexAttribType.Float, false, sizeof(float) * 2, IntPtr.Zero);
            Gl.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        public void Dispose() {
            Gl.DeleteBuffers(_buffer);
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
                Gl.BindBuffer(BufferTarget.ArrayBuffer, _buffer);
                Gl.DrawArrays(PrimitiveType.Triangles, 0, 3);
                Gl.BindBuffer(BufferTarget.ArrayBuffer, 0);
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