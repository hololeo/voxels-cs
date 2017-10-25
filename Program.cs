using System;
using OpenGL;

namespace Voxels {
    public static class Program {
        public static void Main(string[] args) {
            Glfw.SetErrorCallback((_, desc) => throw new Exception($"GLFW error: {desc}"));
            Glfw.Init();

            Glfw.WindowHint(WindowHint.Resizable, 0);
            Glfw.WindowHint(WindowHint.Visible, 0);
            Glfw.WindowHint(WindowHint.OpenGLProfile, (int)OpenGLProfile.Core);
            Glfw.WindowHint(WindowHint.ContextVersionMajor, 4);
            Glfw.WindowHint(WindowHint.ContextVersionMinor, 3);
            var window = Glfw.CreateWindow(800, 450, "My Window", IntPtr.Zero, IntPtr.Zero);
            Gl.Initialize();
            Glfw.MakeContextCurrent(window);
            Glfw.ShowWindow(window);

            Console.WriteLine("OpenGL Version: " + Gl.CurrentVersion);
            Console.WriteLine("OpenGL Vendor: " + Gl.CurrentVendor);
            Console.WriteLine("OpenGL Renderer: " + Gl.CurrentRenderer);

            while (!Glfw.WindowShouldClose(window)) {
                Gl.ClearColor(1.0f, 0.5f, 0.2f, 1.0f);
                Gl.Clear(ClearBufferMask.ColorBufferBit);
                Glfw.SwapInterval(1);
                Glfw.SwapBuffers(window);
                Glfw.PollEvents();
            }

            Glfw.DestroyWindow(window);
            Glfw.Terminate();
        }
    }
}