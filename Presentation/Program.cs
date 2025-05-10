using Presentation;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("🔧 JuliaKernel Demo Start...");

        string agentPath = "Agents/wasm_agent.wasm";
        string manifestPath = "Agents/manifest.yaml";

        var manifest = Manifest.LoadFromFile(manifestPath);
        Console.WriteLine($"✅ Loaded manifest: {manifest.Name}");

        var runner = new WasmRunner(agentPath, manifest);
        runner.Run();

        Console.WriteLine("🏁 Agent execution finished.");
    }
}