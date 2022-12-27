using Serilog;

namespace AugmentationFrameworkSampleApp;

/// <summary>
///     Interaction logic for App.xaml.
/// </summary>
public partial class App
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="App" /> class.
    /// </summary>
    public App()
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Seq("http://localhost:5341")
            .CreateLogger();
    }
}