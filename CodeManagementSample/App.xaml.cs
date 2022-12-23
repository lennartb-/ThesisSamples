using System.Windows;
using Serilog;

namespace CodeManagementSample;

/// <summary>
///     Interaction logic for App.xaml.
/// </summary>
public partial class App
{
    /// <inheritdoc />
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Seq("http://localhost:5341")
            .CreateLogger();
    }
}