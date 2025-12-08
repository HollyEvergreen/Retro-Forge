using System.Runtime.Loader;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using RetroForge.NET;
using Window = RetroForge.NET.Window;


public class RetroForgeEngine
{
    private Window? gameWindow = null;
    private VertexBuffer? vertexBuffer = null;
    public List<Type> plugins = [];

    public void RegisterWindow(ref Window gameWindow)
    {
        this.gameWindow = gameWindow;
        RegisterKeyDownEvent((args =>
        {
            if (args.Key == Keys.Escape)
            {
                Logger.Log("Closing");
                this.gameWindow.Close();
            }
        }));
        vertexBuffer = new VertexBuffer();


        // TODO: Create Base Shader and Register verts

        //Shader BaseVert = new(@"D:\RetroForge\Retro-Forge\shaders\base-vert.spv");
        //Shader BaseFrag = new(@"D:\RetroForge\Retro-Forge\shaders\base-frag.spv");
        //Logger.Log(ConsoleColor.Red, $"{BaseVert.id}\n{BaseFrag.id}");

    }

    public void Render(FrameEventArgs frameEventArgs)
    {

    }
    public void Update(FrameEventArgs frameEventArgs)
    {

    }

    void RegisterVertexBuffer(float[] _vertexBuffer)
    {
        vertexBuffer?.AddVerts(_vertexBuffer);
    }

    public void RegisterKeyEvent(Action<KeyboardKeyEventArgs> KeyDown, Action<KeyboardKeyEventArgs> KeyUp)
    {
        Logger.Assert(() => { return gameWindow is not null; });
        gameWindow?.KeyDown += KeyDown;
        gameWindow?.KeyUp += KeyUp;
    }
    public void RegisterKeyDownEvent(Action<KeyboardKeyEventArgs> KeyDown)
    {
        gameWindow?.KeyDown += KeyDown;
    }
    public void RegisterKeyUpEvent(Action<KeyboardKeyEventArgs> KeyUp)
    {
        gameWindow?.KeyUp += KeyUp;
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
                select type
                );
        }
    }

    public void LogPlugins()
    {
        Logger.Log($"{plugins.Count} plugins loaded.");
        if (plugins.Count == 0)
            return;
        foreach (var plugin in plugins)
        {
            Logger.Log(plugin.Name);
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
            select type!
            );

    }
}
