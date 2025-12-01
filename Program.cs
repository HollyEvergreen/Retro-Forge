using Raylib_CSharp;
using Raylib_CSharp.Images;
using Raylib_CSharp.Windowing;

RetroForgeInfo Info = new(args);
GameLibrary Library = new(Info.GameLibraryPath);
Theme Theme = new(Info.ThemePath);
if (Theme.dim == (0, 0))
{
    Theme.dim = (Window.GetMonitorWidth(RetroForgeInfo.CurrMonitor), Window.GetMonitorHeight(RetroForgeInfo.CurrMonitor));
}
Window.Init(Theme.dim.Item1, Theme.dim.Item2, "Retro Forge");

while (!Window.ShouldClose())
{
    
}


internal struct RetroForgeInfo(string[] args)
{
    public string GameLibraryPath = args[0];
    public string ThemePath = args[1];
    public static int CurrMonitor => Window.GetCurrentMonitor();
}

internal class GameLibrary(string gameLibraryPath)
{
    Game[] Games = [];
}

struct Theme(string path)
{
    public (int, int) dim = (0, 0);
}

struct Game
{
    Image Thumbnail;
    string name;
    SaveData saveData;
    
}

struct SaveData;