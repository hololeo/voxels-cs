﻿using System;
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
        private World _world = new World();
        private int _ppo;

        private void Init() {
            _window = new GameWindow(800, 450, GraphicsMode.Default, "Voxels", GameWindowFlags.FixedWindow,
                DisplayDevice.Default, 4, 6, GraphicsContextFlags.ForwardCompatible) {
                VSync = VSyncMode.Adaptive, Visible = true
            };
            _window.MakeCurrent();

            Resources.Load();

            var positions = new[] {
                new Vertex { Position = new Vector2(-0.5f, -0.5f) },
                new Vertex { Position = new Vector2(0f, 0.5f) },
                new Vertex { Position = new Vector2(0.5f, -0.5f) }
            };

            _vao.Create(() => new[] {
                 new ArrayBuffer().CreateAsVertices(positions, BufferUsageHint.StaticDraw)
            });

            _ppo = GL.GenProgramPipeline();
            GL.UseProgramStages(_ppo, ProgramStageMask.VertexShaderBit, Resources.VoxelVS.ProgramID);
            GL.UseProgramStages(_ppo, ProgramStageMask.FragmentShaderBit, Resources.VoxelFS.ProgramID);
            GL.UseProgramStages(_ppo, ProgramStageMask.GeometryShaderBit, Resources.SolidBlockGS.ProgramID);

            var color = GL.GetUniformLocation(Resources.VoxelFS.ProgramID, "u_color");
            GL.ProgramUniform3(Resources.VoxelFS.ProgramID, color, 0.2f, 0.5f, 1.0f);

            _world.GenerateChunk(1, 1, 1);
        }

        public void Dispose() {
            if (GL.IsProgramPipeline(_ppo)) GL.DeleteProgramPipeline(_ppo);
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

                GL.Clear(ClearBufferMask.ColorBufferBit);

                GL.BindProgramPipeline(_ppo);
                GL.BindVertexArray(_vao.Vao);
                GL.DrawArrays(PrimitiveType.Points, 0, 3);
                GL.BindVertexArray(0);
                GL.BindProgramPipeline(0);

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