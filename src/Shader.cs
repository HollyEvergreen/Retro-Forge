using System.Diagnostics;
using shaderc;
public class Shader
{
    private static Compiler? compiler;
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

    /// <summary>
    /// compiles a list of shaders in headless mode
    /// </summary>
    /// <param name="shaderCompilationParams">
    /// a list of compilation parameters
    /// Retro-Forge -shader -language: [HLSL, GLSL] -n: int paths:string[] fmt{path->entryPoint->[Fragment, Vertex, Compute]}
    /// </param>
    /// <returns> returns a list of SPIR-V ShaderFiles ready for loading</returns>
    public static List<string> CompileShaders(List<string> shaderCompilationParams)
    {
        var lang = shaderCompilationParams[0] switch
        {
            ("-HLSL") => SourceLanguage.Hlsl,
            ("-GLSL") => SourceLanguage.Glsl,
            _ => throw new ArgumentException($"{shaderCompilationParams[0]} not recognized")
        };
        Console.WriteLine($"lang: {lang.ToString()}");
        compiler = new Compiler(new Options()
        {
            SourceLanguage = lang,
            NanClamp = true,
            Optimization = OptimizationLevel.Performance
        });
        int n = int.Parse(shaderCompilationParams[1]);
        Console.WriteLine($"number of shaders = {n}");
        var shader_paths = shaderCompilationParams[2..].ToList();
        Console.WriteLine($"shader_paths:\n\t{string.Join(";\n\t", shader_paths)}");
        List<string> shaders = [];
        List<Task> files = [];
        for (var i = 0; i < n; i++)
        {
            string[] path = shader_paths[i].Split("->");
            Console.Write($"Shader {path[0]}: \n\t");
            Console.Write($"Entrypoint: {path[1]} \n\t");
            Console.Write($"ShaderKind: {path[2]} \n");

            var shader_code = CompileShader(path);
            string outPath = ShaderPathGen(path[0]);
            Console.WriteLine("outputting shader file to -> " + $"shaders/{outPath}");
            File.Create($"./shaders/{outPath}");
            files.Add(File.WriteAllBytesAsync($"shaders/{outPath}", shader_code.ToArray()));
            shaders.Add(outPath);
        }
        while (!files.All(f => f.IsCompleted)) ;
        return shaders;
    }

    private static string ShaderPathGen(string path)
    {
        string[] _path = path.Split("\\").Last().Split(".");

        return $"{_path[0]}-{_path[1]}.spv";
    }

    private static Span<byte> CompileShader(string[] path)
    {
        var shaderKind = path[2] switch
        {
            ("Fragment") => ShaderKind.FragmentShader,
            ("Vertex") => ShaderKind.VertexShader,
            ("Compute") => ShaderKind.ComputeShader,
            _ => throw new ArgumentException($"{path[1]} not recognized")
        };
        var result = compiler.Compile(path[0], shaderKind, path[1]);
        unsafe
        {
            var bytecode = (byte*)(uint*)result.CodePointer;
            var bytecodeLength = (int)result.CodeLength;
            return new Span<byte>(bytecode, bytecodeLength);
        }
    }

    private struct CompilationParams(string compiler_path, string path, string[] options, Dictionary<string, string> kwargs)
    {
        public string[] Options = options;
        public Dictionary<string, string> Kwargs = kwargs;
    }
}
