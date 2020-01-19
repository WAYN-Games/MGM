public sealed class InputProcessorInjector
{

    
    public static InputProcessorInjector Instance { get; } = new InputProcessorInjector();
    
    // Explicit static constructor to tell C# compiler
    // not to mark type as beforefieldinit
    static InputProcessorInjector()
    {
    }

    private InputProcessorInjector()
    {
    }

}
