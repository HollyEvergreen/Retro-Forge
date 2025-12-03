using OpenTK.Graphics.OpenGL;

public class VertexBuffer
{
    private float[] verts = [];
    public VertexBuffer()
    {
        ID = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, ID);
    }
    public int ID { get; private set; }

    public void AddVerts(float[] buf)
    {
        verts = Concatenate(verts, buf);
        GL.BufferData(BufferTarget.ArrayBuffer, buf.Length * sizeof(float), buf, BufferUsage.StaticDraw );
    }

    private static float[] Concatenate(float[] floats, float[] buf)
    {
        var verts = new float[floats.Length + buf.Length];
        floats.CopyTo(verts, 0);
        buf.CopyTo(verts, floats.Length);
        return verts;
    }

    ~VertexBuffer()
    {
        GL.BindBuffer(BufferTarget.ArrayBuffer, ID);
        GL.DeleteBuffer(ID);
    }
}