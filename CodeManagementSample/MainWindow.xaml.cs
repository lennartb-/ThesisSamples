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

    private void OnTextChanged(object? sender, EventArgs e)
    {
        CorrectCaretOffset(ref OutputEditor);
        CorrectCaretOffset(ref Editor);
    }

    private static void CorrectCaretOffset(ref CodeTextEditor editor)
    {
        if (editor?.Document == null) return;
        editor.CaretOffset = editor.Document.TextLength < editor.CaretOffset ? editor.Document.TextLength : editor.CaretOffset;
    }
}
