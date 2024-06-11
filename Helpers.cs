using System.Runtime.InteropServices;
using Windows.Win32.UI.Input.KeyboardAndMouse;
using Windows.Win32;

namespace PebcakCity.MouseJiggler
{
    internal static class Helpers
    {
        internal const uint AttachParentProcess = unchecked((uint)-1);
        internal static void Jiggle( int delta )
        {
            var inp = new INPUT
            {
                type = INPUT_TYPE.INPUT_MOUSE,
                Anonymous = new INPUT._Anonymous_e__Union
                {
                    mi = new MOUSEINPUT
                    {
                        dx = delta,
                        dy = delta,
                        mouseData = 0,
                        dwFlags = MOUSE_EVENT_FLAGS.MOUSEEVENTF_MOVE,
                        time = 0,
                        dwExtraInfo = 0
                    },
                },
            };
            _ = PInvoke.SendInput([inp,], Marshal.SizeOf<INPUT>());
        }
    }
}
