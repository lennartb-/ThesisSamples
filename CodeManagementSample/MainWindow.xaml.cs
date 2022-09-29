using System;
using System.Windows;
using Microsoft.CodeAnalysis.Differencing;
using RoslynPad.Editor;

namespace CodeManagementSample;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainWindowVm();
    }
}
