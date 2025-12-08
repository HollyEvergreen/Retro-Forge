using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL;
using System.Diagnostics;
using RetroForge.NET;
using System.Runtime.CompilerServices;

internal class Program
{
	static bool DEV_MODE;
	private static void Main(string[] args)
	{
		DEV_MODE = args.ToList().Contains("dev");
		if (args.Length != 0)
		{
			if (args[0] == "hello")
				Logger.Log("hi there why u saying hi");
			if (args[0] == "utils")
			{
				DirectoryInfo? dirInfo;
				switch (args[1])
				{
					case "-shader":
						var shaderCompilationParams = args[2..].ToList();
						Shader.CompileShaders(shaderCompilationParams);
						break;
					case "-checks":
						dirInfo = new DirectoryInfo("./shaders");
						Console.WriteLine($"{dirInfo.Exists}");
						var shaders = dirInfo.EnumerateFiles("*.spv");
						Console.WriteLine($"Shader Count: {shaders.Count()}");
						foreach (var shader in shaders)
						{
							Console.WriteLine($"\t{shader.Name}");
						}
						break;
					case "-regen":
						dirInfo = new DirectoryInfo("./shaders");
						if (!dirInfo.Exists)
						{
							dirInfo = Directory.CreateDirectory(dirInfo.FullName);
						}

						dirInfo = new DirectoryInfo($"C://Users/{Environment.UserName}/AppData/RetroForge/Plugins");
						if (!dirInfo.Exists)
						{
							dirInfo.Create();
						}
						Console.WriteLine($"{dirInfo.Exists}");
						break;
				}
				return;
			}
		}

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


		var gameWindow = new Window(settings, nativeWindowSettings);
		var renderForge = new RetroForgeEngine();

		Debug.Assert(gameWindow != null, nameof(gameWindow) + " != null");
		Debug.Assert(renderForge != null, nameof(RetroForgeEngine) + " != null");
		Logger.Log($"Dev mode -> {DEV_MODE}");

        var plugin_type = renderForge.plugins[1];
        string[] _args = ["10000"];
        IRetroForgePlugin plugin = (plugin_type.GetConstructor([typeof(Window), typeof(string[])])?.Invoke([gameWindow, _args])) as IRetroForgePlugin ?? throw new Exception();
        gameWindow.Load += () =>
		{
			var pluginFolder = new DirectoryInfo($"C://Users/{Environment.UserName}/AppData/Roaming/RetroForge/Plugins");
			renderForge.LoadPlugins(pluginFolder);
			renderForge.RegisterWindow(ref gameWindow);
			GL.ClearColor(Color4.Aliceblue);
			gameWindow.RenderFrame += plugin.Render;
			gameWindow.UpdateFrame += plugin.Update;
			//gameWindow.RenderFrame += _ => { GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit); };
			//gameWindow.RenderFrame += renderForge.Render;
			//gameWindow.RenderFrame += _ => { gameWindow.SwapBuffers(); };
			//gameWindow.UpdateFrame += renderForge.Update;
		};
		gameWindow.Resize += eventArgs =>
		{
			GL.Viewport(0, 0, eventArgs.Width, eventArgs.Height);
		};
		gameWindow.Move += eventArgs =>
		{
			GL.Viewport(eventArgs.X, eventArgs.Y, gameWindow.ClientSize.X, gameWindow.ClientSize.Y);
		};

		//gameWindow.Run();
		gameWindow.Dispose();
		Logger.ResetConsole();
	}
}