using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL;
using System.Diagnostics;
using RetroForge.NET;

public class Program
{
	private static bool DEV_MODE;
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
                    case "-checks":
                        Logger.Log(Engine.AppData("shaders"));
                        dirInfo = new DirectoryInfo(Engine.AppData("shaders"));
                        Console.WriteLine($"{dirInfo.Exists}");
                        if (!dirInfo.Exists)
                            break;
                        var shaders = dirInfo.EnumerateFiles("*.spv");
                        Console.WriteLine($"Shader Count: {shaders.Count()}");
                        foreach (var shader in shaders)
                        {
                            Console.WriteLine($"\t{shader.Name}");
                        }
                        break;
                    case "-regen":
						dirInfo = new DirectoryInfo(Engine.AppData("shaders"));
						if (!dirInfo.Exists)
						{
							dirInfo = Directory.CreateDirectory(dirInfo.FullName);
						}

						dirInfo = new DirectoryInfo($"C://Users/{Environment.UserName}/AppData/RetroForgeEngine/Plugins");
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
			UpdateFrequency = 144000,
			Win32SuspendTimerOnDrag = false,
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
			Title = "RetroForgeEngine",
			SrgbCapable = true,
			Vsync = VSyncMode.Adaptive,
			Profile = ContextProfile.Core,
		};


		var gameWindow = new Window(settings, nativeWindowSettings);
		Debug.Assert(gameWindow != null, nameof(gameWindow) + " != null");
		Logger.Log($"Dev mode -> {DEV_MODE}");


		gameWindow.Load += () =>
		{
            var RetroForgeEngine = new Engine(ref gameWindow);
            Debug.Assert(RetroForgeEngine != null, nameof(Engine) + " != null");
			GL.ClearColor(Color4.Aliceblue);
			DirectoryInfo pluginFolder = new(Engine.AppData("plugins"));
			Logger.Log("Load Plugins");
			RetroForgeEngine.LoadPlugins(pluginFolder);
			RetroForgeEngine.LogPlugins();
			Logger.Log("Set Plugin");
			RetroForgeEngine.SetPlugin(0);
			Logger.Log("Load Plugin");
			RetroForgeEngine.OnLoad();
            gameWindow.RenderFrame += RetroForgeEngine.Render;
			gameWindow.UpdateFrame += RetroForgeEngine.Update;
		};
		gameWindow.Resize += eventArgs =>
		{
			GL.Viewport(0, 0, eventArgs.Width, eventArgs.Height);
		};
		gameWindow.Run();
		gameWindow.Dispose();
		Logger.ResetConsole();
	}
}