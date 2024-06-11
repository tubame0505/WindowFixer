using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Windowfixer
{
    internal class WindowInfo
    {
        internal struct INFO
        {
            public IntPtr hWnd;
            public int top;
            public int left;
            public int width;
            public int height;
        }
        List<INFO> window_info;

        internal List<INFO> GetWindowInfo()
        {
            window_info = new List<INFO>();
            EnumWindows(new EnumWindowCallBack(GetCallBack), IntPtr.Zero);
            return window_info;
        }

        internal void SetWindowInfo(List<INFO> _window_info)
        {
            window_info = _window_info;
            EnumWindows(new EnumWindowCallBack(SetCallBack), IntPtr.Zero);
        }



        [DllImport("User32.dll")]
        static extern int MoveWindow(
        IntPtr hWnd,
        int x,
        int y,
        int nWidth,
        int nHeight,
        int bRepaint
        );
        [DllImport("User32.Dll", CharSet = CharSet.Unicode)]
        public static extern int GetClassName(
            IntPtr hWnd,
            StringBuilder s,
            int nMaxCount
            );
        [DllImport("User32.Dll", CharSet = CharSet.Unicode)]
        static extern int GetWindowText(
            IntPtr hWnd,
            StringBuilder s,
            int nMaxCount
            );
        [DllImport("User32.Dll")]
        static extern int GetParent(
            IntPtr hWnd
            );
        [DllImport("User32.Dll")]
        static extern int IsWindowVisible(
            IntPtr hWnd
            );
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        private struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        [DllImport("User32.Dll")]
        static extern int GetWindowRect(
            IntPtr hWnd,      // ウィンドウのハンドル
            out RECT rect   // ウィンドウの座標値
            );
        [DllImport("user32.Dll")]
        static extern int EnumWindows(EnumWindowCallBack x, IntPtr y);

        public delegate bool EnumWindowCallBack(IntPtr hwnd, IntPtr lParam);

        public bool GetCallBack(IntPtr hwnd, IntPtr lParam)
        {
            StringBuilder sbWindowText = new StringBuilder(256);
            StringBuilder sbClassName = new StringBuilder(256);
            GetWindowText(hwnd, sbWindowText, sbWindowText.Capacity);
            GetClassName(hwnd, sbClassName, sbClassName.Capacity);
            //System.Console.WriteLine(hwnd.ToString() + ":" + sbWindowText.ToString() + ":" + IsWindowVisible(hwnd).ToString());
            if ((sbClassName.Length > 0) && (sbWindowText.Length > 0) && IsWindowVisible(hwnd) > 0 && GetParent(hwnd) == 0)
            {
                RECT rect;
                var r = GetWindowRect(hwnd, out rect);
                if (r == 0)
                {
                    var errorcode = Marshal.GetLastWin32Error();
                }
                else
                {
                    INFO info = new INFO();
                    info.hWnd = hwnd;
                    info.top = rect.top;
                    info.left = rect.left;
                    info.width = rect.right - rect.left;
                    info.height = rect.bottom - rect.top;
                    window_info.Add(info);
                }
            }
            return true;
        }
        public bool SetCallBack(IntPtr hwnd, IntPtr lParam)
        {
            StringBuilder sbWindowText = new StringBuilder(256);
            StringBuilder sbClassName = new StringBuilder(256);
            GetWindowText(hwnd, sbWindowText, sbWindowText.Capacity);
            GetClassName(hwnd, sbClassName, sbClassName.Capacity);
            //System.Console.WriteLine(hwnd.ToString() + ":" + sbWindowText.ToString() + ":" + IsWindowVisible(hwnd).ToString());
            if ((sbClassName.Length > 0) && (sbWindowText.Length > 0) && IsWindowVisible(hwnd) > 0 && GetParent(hwnd) == 0)
            {
                foreach (var w in window_info)
                {
                    if (w.hWnd == hwnd)
                    {
                        MoveWindow(hwnd, w.left, w.top, w.width, w.height, 1);
                    }
                }
            }
            return true;
        }
    }
}
