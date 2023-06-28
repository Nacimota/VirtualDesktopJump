using System.Runtime.InteropServices;

namespace Nacimota.Win32;
internal static class Winuser
{
    const short CTRL = 0x11;
    const short LWIN = 0x5B;
    const short RIGHT = 0x27;
    const short LEFT = 0x25;

    private static INPUT[] leftChord = { new(CTRL), new(LWIN), new(LEFT), new(LEFT, true), new(LWIN, true), new(CTRL, true) };
    private static INPUT[] rightChord = { new(CTRL), new(LWIN), new(RIGHT), new(RIGHT, true), new(LWIN, true), new(CTRL, true) };

    [DllImport("user32.dll")]
    private static extern uint SendInput(int cInputs, INPUT[] pInputs, int cbSize);

    internal static void SendCtrlWinLeft() => SendInput(6, leftChord, INPUT.Size);
    internal static void SendCtrlWinRight() => SendInput(6, rightChord, INPUT.Size);
}

[StructLayout(LayoutKind.Sequential)]
internal struct KEYBDINPUT
{
    internal short wVk;
    internal short wScan;
    internal int dwFlags;
    internal int time;
    internal IntPtr dwExtraInfo;
}

[StructLayout(LayoutKind.Sequential)]
internal struct MOUSEINPUT
{
    internal int dx;
    internal int dy;
    internal int mouseData;
    internal int dwFlags;
    internal int time;
    internal IntPtr dwExtraInfo;
}

[StructLayout(LayoutKind.Sequential)]
internal struct HARDWAREINPUT
{
    internal int uMsg;
    internal short wParamL;
    internal short wParamH;
}

[StructLayout(LayoutKind.Explicit)]
internal struct InputUnion
{
    [FieldOffset(0)]
    internal MOUSEINPUT mi;
    [FieldOffset(0)]
    internal KEYBDINPUT ki;
    [FieldOffset(0)]
    internal HARDWAREINPUT hi;
}


[StructLayout(LayoutKind.Sequential)]
internal struct INPUT
{
    internal uint type = 1;

    internal InputUnion u;

    internal static int Size => Marshal.SizeOf(typeof(INPUT));

    internal INPUT(short virtualKey, bool keyup = false)
    {
        u.ki.wVk = virtualKey;
        u.ki.dwFlags = keyup ? 2 : 0;
    }
}
