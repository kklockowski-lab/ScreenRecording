using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

public class WindowHooker
{
    private const int WH_CALLWNDPROC = 4;
    private const int WM_LBUTTONDOWN = 0x0201;

    private delegate IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hMod, uint dwThreadId);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern bool UnhookWindowsHookEx(IntPtr hhk);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

    private static HookProc hookProc;
    private static IntPtr hookHandle = IntPtr.Zero;

    private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
    {
        if (nCode >= 0 && wParam == (IntPtr)WM_LBUTTONDOWN)
        {
            ;
            // Kliknięcie lewym przyciskiem myszy
            // Tutaj możesz wykonać dowolne operacje na klikniętym oknie
        }

        return CallNextHookEx(hookHandle, nCode, wParam, lParam);
    }

    public static void StartHook()
    {
        hookProc = new HookProc(HookCallback);
        IntPtr hModule = Process.GetCurrentProcess().MainModule.BaseAddress;

        hookHandle = SetWindowsHookEx(WH_CALLWNDPROC, hookProc, hModule, 0);
    }

    public static void StopHook()
    {
        UnhookWindowsHookEx(hookHandle);
    }
}
