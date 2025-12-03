using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

public class RenderForge
{
    private GameWindow gameWindow;
    
    
    
    public void RegisterWindow(ref GameWindow gameWindow)
    {
        this.gameWindow = gameWindow;
    }

    public void Render(FrameEventArgs frameEventArgs)
    {
        
    }
    
    public void Update(FrameEventArgs frameEventArgs)
    {
        
    }
}