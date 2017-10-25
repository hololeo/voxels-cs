using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Voxels {
    public delegate void GlfwErrorFun(int error, string description);

    public enum WindowHint {
        Resizable = 0x00020003,
        Visible = 0x00020004,
        DoubleBuffer = 0x00021010,
        ContextVersionMajor = 0x00022002,
        ContextVersionMinor = 0x00022003,
        OpenGLProfile = 0x00022008
    }

    public enum OpenGLProfile {
        Core = 0x00032001
    }

    public static class Glfw {
        private const string _dlname = "glfw3";

        [DllImport(_dlname, EntryPoint = "glfwInit")]
        public static extern bool Init();

        [DllImport(_dlname, EntryPoint = "glfwTerminate")]
        public static extern void Terminate();

        [DllImport(_dlname, EntryPoint = "glfwSetErrorCallback")]
        public static extern GlfwErrorFun SetErrorCallback(GlfwErrorFun cbfun);

        [DllImport(_dlname, EntryPoint = "glfwWindowHint")]
        public static extern void WindowHint(WindowHint hint, int value);

        [DllImport(_dlname, EntryPoint = "glfwCreateWindow")]
        public static extern IntPtr CreateWindow(
            int width, int height, string title, IntPtr monitor, IntPtr share
        );

        [DllImport(_dlname, EntryPoint = "glfwDestroyWindow")]
        public static extern void DestroyWindow(IntPtr window);

        [DllImport(_dlname, EntryPoint = "glfwShowWindow")]
        public static extern void ShowWindow(IntPtr window);

        [DllImport(_dlname, EntryPoint = "glfwPollEvents")]
        public static extern void PollEvents();

        [DllImport(_dlname, EntryPoint = "glfwSwapBuffers")]
        public static extern void SwapBuffers(IntPtr window);

        [DllImport(_dlname, EntryPoint = "glfwWindowShouldClose")]
        public static extern bool WindowShouldClose(IntPtr window);
    }
}