using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace InnodisProject5._2
{
    public class hwndutils : IDisposable
    {
        private bool _mIsStartButtonDisabled = false;
        private bool _mIsCloseButtonDisabled = false;
        private IntPtr _mHwnd = IntPtr.Zero;
        public bool StartButtonDisabled
        {
            set
            {
                this._mIsStartButtonDisabled = value;
                if (this._mIsStartButtonDisabled)
                {
                    if (this.oldWndProc == IntPtr.Zero) //already hooked?
                    {
                        this.hookWindow();
                    }
                }
                else
                {
                    if (this._mIsCloseButtonDisabled == false)//we will not unhook if hook is installed for StartButton
                    {
                        this.unhookWindow();
                    }
                }
            }
            get { return this._mIsStartButtonDisabled; }
        }

        public bool CloseButtonDisabled
        {
            set
            {
                this._mIsCloseButtonDisabled = value;
                if (this._mIsCloseButtonDisabled)
                {
                    if (this.oldWndProc == IntPtr.Zero) //already hooked?
                    {
                        this.hookWindow();
                    }
                }
                else
                {
                    if (this._mIsStartButtonDisabled == false)//we will not unhook if hook is installed for StartButton
                    {
                        this.unhookWindow();
                    }
                }
            }
            get { return this._mIsCloseButtonDisabled; }
        }
        public hwndutils()
        {
        }

        //dispose
        public void Dispose()
        {

            unhookWindow();
        }

        private void unhookWindow()
        {
            if (this.oldWndProc != IntPtr.Zero && this._mHwnd != IntPtr.Zero)
            {
                SetWindowLong(this._mHwnd, GWL_WNDPROC, this.oldWndProc); //restore old wndproc!
                this._mIsStartButtonDisabled = false;
                this.oldWndProc = IntPtr.Zero;
                this._mHwnd = IntPtr.Zero;
            }
        }
        /// <summary>
        /// SubClassing: Install the wndproc hook
        /// </summary>
        /// <returns></returns>
        private bool hookWindow()
        {
            //find taskbar
            IntPtr hWndTaskbar = FindWindow("HHTaskbar", IntPtr.Zero);
            if (hWndTaskbar == IntPtr.Zero)
                return false;
            //enable the taskbar, not realy necessary
            EnableWindow(hWndTaskbar, true);
            //already installed?
            if (oldWndProc == IntPtr.Zero)
            {
                //find the menu_worker window
                IntPtr hwndMenu_Worker = FindWindow("menu_worker", IntPtr.Zero);
                if (hwndMenu_Worker != IntPtr.Zero)
                {
                    //get the child window which has the buttons on it
                    IntPtr hwndToolbar = GetWindow(hwndMenu_Worker, GetWindow_Cmd.GW_CHILD);
                    if (hwndToolbar != IntPtr.Zero)
                    {
                        _mHwnd = hwndToolbar;       //store to remember
                        SubclassHWnd(hwndToolbar);  //subclass the wndproc
                    }
                }
            }
            return true;
        }
        #region PINVOKES
        // Win32 API needed
        [DllImport("coredll.dll", SetLastError = true)]
        public static extern System.UInt32 GetWindowLong(IntPtr hWnd, int nIndex);
        public enum GWL
        {
            GWL_WNDPROC = (-4),
            GWL_HINSTANCE = (-6),
            GWL_HWNDPARENT = (-8),
            GWL_STYLE = (-16),
            GWL_EXSTYLE = (-20),
            GWL_USERDATA = (-21),
            GWL_ID = (-12)
        }
        [DllImport("coredll.dll", SetLastError = true)]
        static extern IntPtr GetWindow(IntPtr hWnd, GetWindow_Cmd uCmd);

        enum GetWindow_Cmd : uint
        {
            GW_HWNDFIRST = 0,
            GW_HWNDLAST = 1,
            GW_HWNDNEXT = 2,
            GW_HWNDPREV = 3,
            GW_OWNER = 4,
            GW_CHILD = 5,
            GW_ENABLEDPOPUP = 6
        }
        [DllImport("coredll.dll", EntryPoint = "FindWindow", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, IntPtr lpWindowName);
        [DllImport("coredll.dll")]
        public static extern bool EnableWindow(IntPtr hWnd, bool bEnable);

        [DllImport("coredll")]
        private static extern IntPtr SetWindowLong(IntPtr hWnd, int nIndex, Win32WndProc newProc);
        [DllImport("coredll")]
        private static extern IntPtr SetWindowLong(IntPtr hWnd, int nIndex, IntPtr newProc);
        [DllImport("coredll")]
        private static extern IntPtr CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImport("coredll.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowRect(IntPtr hwnd, out RECT lpRect);
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;        // x position of upper-left corner
            public int Top;         // y position of upper-left corner
            public int Right;       // x position of lower-right corner
            public int Bottom;      // y position of lower-right corner
        }
        #endregion

        // A delegate that matches Win32 WNDPROC:
        private delegate IntPtr Win32WndProc(IntPtr hWnd, int Msg, int wParam, int lParam);

        // from winuser.h:
        private const int GWL_WNDPROC = -4;
        private const int WM_LBUTTONDOWN = 0x0201;
        private const int WM_LBUTTONUP = 0x0202;

        // program variables
        private IntPtr oldWndProc = IntPtr.Zero;
        private Win32WndProc newWndProc;// = IntPtr.Zero; //null;

        private void SubclassHWnd(IntPtr hWnd)
        {
            // hWnd is the window you want to subclass..., create a new 
            // delegate for the new wndproc
            newWndProc = new Win32WndProc(MyWndProc);
            // subclass
            oldWndProc = SetWindowLong(hWnd, GWL_WNDPROC, newWndProc);
        }

        // this is the new wndproc, just show a messagebox on left button down:
        private IntPtr MyWndProc(IntPtr hWnd, int msg, int wParam, int lParam)
        {
            //is this a message for us?
            if (((msg == (int)WM_LBUTTONDOWN) || (msg == (int)WM_LBUTTONUP)) && (this._mIsStartButtonDisabled || this._mIsCloseButtonDisabled))
            {
                int x = ((int)lParam) & 0xFFFF;
                int y = ((int)lParam) >> 16;

                bool isVGA;
                bool isQVGA;
                using (System.Windows.Forms.Control detector = new System.Windows.Forms.Control())
                {
                    using (System.Drawing.Graphics gr = detector.CreateGraphics())
                    {
                        isVGA = gr.DpiY == 192;
                        isQVGA = gr.DpiY == 96;
                    }
                }

                RECT rect;
                GetWindowRect(hWnd, out rect); //get the rectangle of the menu_bar

                int width = Math.Max(rect.Left, rect.Right) - Math.Min(rect.Left, rect.Right);
                int height = Math.Max(rect.Bottom, rect.Top) - Math.Min(rect.Bottom, rect.Top);

                //width values are assumed
                int buttonWidth = (isQVGA | isVGA) ? 92 : 46;
                int buttonHeight = height; //(isQVGA | isVGA) ? 72 : 36;

                System.Drawing.Rectangle rectStartButton = new System.Drawing.Rectangle(0, 0, buttonWidth, buttonHeight);
                System.Drawing.Rectangle rectCloseButton = new System.Drawing.Rectangle(width - buttonWidth, 0, buttonWidth, buttonHeight);

                //check if enabled and click is inside the start or close button rectangle
                if (this._mIsStartButtonDisabled && rectStartButton.Contains(x, y))
                    return IntPtr.Zero;
                if (this._mIsCloseButtonDisabled && rectCloseButton.Contains(x, y))
                    return IntPtr.Zero;

                //if both are false, we have to provide the click to windows
                return CallWindowProc(oldWndProc, hWnd, msg, wParam, lParam);
            }
            else
                return CallWindowProc(oldWndProc, hWnd, msg, wParam, lParam);
        }
    }
}
