using System.Diagnostics;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL;
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
    ClientSize = new Vector2i(640, 360),
    Flags = ContextFlags.ForwardCompatible,
    Title = "RetroForge",
    SrgbCapable = true,
    Vsync = VSyncMode.Adaptive,
    Profile = ContextProfile.Core,
};


var gameWindow = new GameWindow(settings, nativeWindowSettings);
var renderForge = new RenderForge();

Debug.Assert(gameWindow != null, nameof(gameWindow) + " != null");
Debug.Assert(renderForge != null, nameof(RenderForge) + " != null");



gameWindow.Load += () =>
{
    renderForge.RegisterWindow(ref gameWindow);
    GL.ClearColor(Color4.Aliceblue);
    gameWindow.RenderFrame += _ => { GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit); };
    gameWindow.RenderFrame += renderForge.Render; 
    gameWindow.RenderFrame += _ => { gameWindow.SwapBuffers(); };
    gameWindow.UpdateFrame += renderForge.Update;
};
gameWindow.Resize += eventArgs =>
{
    GL.Viewport(0, 0, eventArgs.Width, eventArgs.Height);
};
gameWindow.Move += eventArgs =>
{
    GL.Viewport(eventArgs.X, eventArgs.Y, gameWindow.ClientSize.X, gameWindow.ClientSize.Y);
};
gameWindow.Unload += () =>
{
    gameWindow.Close();
    gameWindow.Dispose();
};


Console.WriteLine("Hello World!");

gameWindow.Run();
Console.WriteLine("Goodbye World!");