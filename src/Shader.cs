
public class Shader
{
    private int ID;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="paths">
    /// 0 => vertex
    /// 1 => fragment
    /// 2 => compute
    /// </param>
    public Shader(string?[] paths)
    {
        string? vertexShader = paths[0];
        string? fragmentShader = paths[1];

        if (vertexShader is not null)
        {
            // TODO: implement vertexShader compilation
        }
        if (fragmentShader is not null)
        {
            // TODO: implement fragmentShader compilation
        }

    }
}