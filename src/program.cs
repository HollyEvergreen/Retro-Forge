using System.Numerics;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using number = double;

namespace Retro_Forge;
internal class Program
{
    public static void _Main(string[] args)
    {
        var settings = new GameWindowSettings()
        {
            UpdateFrequency = 144,
            Win32SuspendTimerOnDrag = false
        };
        var nativeWindowSettings = new NativeWindowSettings()
        {
            AlphaBits = 8,
            API = ContextAPI.OpenGL,
            APIVersion = new Version(4, 6, 0),
            AspectRatio = (16, 9),
            AutoIconify = false,
            AutoLoadBindings = true,
            RedBits = 8,
            BlueBits = 8,
            GreenBits = 8,
            DepthBits = 16,
            ClientSize = new Vector2i(1920, 1080),
            Flags = ContextFlags.ForwardCompatible,
            Title = "RetroForge",
            SrgbCapable = true,
            Vsync = VSyncMode.Adaptive,
        };
        
        
        
        var gameWindow = new GameWindow(settings, nativeWindowSettings);
        return;
    }
}