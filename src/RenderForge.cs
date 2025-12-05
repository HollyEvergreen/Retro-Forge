using System.Diagnostics;
using System.Reflection;
using System.Runtime.Loader;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Graphics.OpenGL;
using RetroForge.NET;

public class RenderForge
{
    private GameWindow gameWindow;
    private VertexBuffer vertexBuffer;
    private List<IRetroForgePlugin> plugins = [];

    public void RegisterWindow(ref GameWindow gameWindow)
    {
        this.gameWindow = gameWindow;
        RegisterKeyDownEvent((args =>
        {
            if (args is { Key: Keys.Escape, Alt: true })
            {
                this.gameWindow.Close();
            }
        }));
        vertexBuffer = new VertexBuffer();

        // TODO: Create Base Shader and Register verts


    }

    public void Render(FrameEventArgs frameEventArgs)
    {

    }

    public void Update(FrameEventArgs frameEventArgs)
    {

    }

    void RegisterVertexBuffer(float[] _vertexBuffer)
    {
        vertexBuffer.AddVerts(_vertexBuffer);
    }

    public void RegisterKeyEvent(Action<KeyboardKeyEventArgs> KeyDown, Action<KeyboardKeyEventArgs> KeyUp)
    {
        Debug.Assert(gameWindow is not null);
        gameWindow.KeyDown += KeyDown;
        gameWindow.KeyUp += KeyUp;
    }
    public void RegisterKeyDownEvent(Action<KeyboardKeyEventArgs> KeyDown)
    {
        gameWindow.KeyDown += KeyDown;
    }
    public void RegisterKeyUpEvent(Action<KeyboardKeyEventArgs> KeyUp)
    {
        gameWindow.KeyUp += KeyUp;
    }

    public void LoadPlugins(DirectoryInfo pluginPath)
    {
        if (!Directory.Exists(pluginPath.FullName))
        {
            throw new DirectoryNotFoundException(pluginPath.FullName);
        }
        var PluginLoader = new AssemblyLoadContext(null, true);
        var pluginAssemblies = pluginPath.EnumerateFiles("*-Game.dll");
        foreach (var assembly in pluginAssemblies)
        {
            var types = PluginLoader.LoadFromAssemblyPath(assembly.FullName).GetExportedTypes();
            plugins.AddRange(
                from type in types
                where type.IsAssignableTo(typeof(IRetroForgePlugin))
                select (IRetroForgePlugin)Activator.CreateInstance(type)!
                );
        }
    }

    public void LogPlugins()
    {
        Console.WriteLine($"{plugins.Count} plugins loaded.");
        if (plugins.Count == 0)
            return;
        foreach (var plugin in plugins)
        {
            plugin.ToString();
        }
    }

    internal void LoadPluginAt(FileInfo dll)
    {
        if (!dll.Exists) throw new FileNotFoundException($"{dll.FullName} does not exits at the location specified");
        var PluginLoader = new AssemblyLoadContext(null, true);
        FileInfo pluginAssembly = dll;

        var types = PluginLoader.LoadFromAssemblyPath(pluginAssembly.FullName).GetExportedTypes();
        plugins.AddRange(
            from type in types
            where type.IsAssignableTo(typeof(IRetroForgePlugin))
            select (IRetroForgePlugin)Activator.CreateInstance(type)!
            );

    }
}
