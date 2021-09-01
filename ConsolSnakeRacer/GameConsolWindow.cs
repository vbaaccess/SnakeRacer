using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Text;

namespace ConsolSnakeRacer
{
    public static class GameConsolWindow
    {

        public static void Size(int consolSize_X, int consolSize_y)
        {
            Console.SetWindowSize(consolSize_X, consolSize_y);
            Console.BufferHeight = consolSize_X;
            Console.BufferWidth = consolSize_y;
        }

        public static void Center()
        {
            var consoleWnd = System.Diagnostics.Process.GetCurrentProcess().MainWindowHandle;
            Imports.SetWindowPos(consoleWnd, 0, 300, 300, 0, 0, Imports.SWP_NOSIZE | Imports.SWP_NOZORDER);
            //TO DO - optymaliz center consol window
        }
    }

    static class Imports
    {
        public static IntPtr HWND_BOTTOM = (IntPtr)1;
        // public static IntPtr HWND_NOTOPMOST = (IntPtr)-2;
        public static IntPtr HWND_TOP = (IntPtr)0;
        // public static IntPtr HWND_TOPMOST = (IntPtr)-1;

        public static uint SWP_NOSIZE = 1;
        public static uint SWP_NOZORDER = 4;

        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        public static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int Y, int cx, int cy, uint wFlags);
    }

}
