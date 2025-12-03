using System.Diagnostics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Graphics.OpenGL;
public class RenderForge
{
    private GameWindow gameWindow;
    private VertexBuffer vertexBuffer;

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
}