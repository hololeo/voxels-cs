using System;

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
            Glfw.ShowWindow(window);

            while (!Glfw.WindowShouldClose(window)) {
                Glfw.SwapBuffers(window);
                Glfw.PollEvents();
            }

            Glfw.DestroyWindow(window);
            Glfw.Terminate();
        }
    }
}