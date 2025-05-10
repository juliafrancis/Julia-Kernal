using Presentation.Plugins;
using Wasmtime;

namespace Presentation;

public class WasmRunner
{
    private readonly string wasmPath;
    private readonly Manifest manifest;

    public WasmRunner(string wasmPath, Manifest manifest)
    {
        this.wasmPath = wasmPath;
        this.manifest = manifest;
    }

    public void Run()
    {
        using var engine = new Engine();
        using var module = Module.FromFile(engine, wasmPath);
        using var linker = new Linker(engine);
        using var store = new Store(engine);

        // Add plugin functions (e.g., tx_execute)
        if (manifest.Permissions != null && manifest.Permissions.Contains("tx_execute"))
        {
            linker.Define("env", "tx_execute", Function.FromCallback(store, () =>
            {
                Console.WriteLine("🚀 Executing transaction via plugin...");
                TxExecutor.Execute();
            }));
        }

        var instance = linker.Instantiate(store, module);
        var start = instance.GetAction("run");
        start();
    }
}