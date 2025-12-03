using System.Diagnostics;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using Number = double;

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


const Number dt = 0;
Console.WriteLine(dt);
var gameWindow = new GameWindow(settings, nativeWindowSettings);
var renderForge = new RenderForge();


Debug.Assert(gameWindow != null, nameof(gameWindow) + " != null");
Debug.Assert(renderForge != null, nameof(RenderForge) + " != null");

renderForge.RegisterWindow(ref gameWindow);

gameWindow.Load += () => { };
gameWindow.RenderFrame += renderForge.Render;
gameWindow.UpdateFrame += renderForge.Update;


gameWindow.Close();