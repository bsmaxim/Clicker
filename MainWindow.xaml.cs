using System.Runtime.InteropServices;
using System.Windows;
using AutoClicker.Library;
using AutoClicker.Library.Keyboard;
using AutoClicker.Library.Mouse;
using static AutoClicker.Library.Input.WinInputStructs;

namespace AutoClicker;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly KeyRecorder keyboardRecorder = new();
    private readonly KeyPlayer keyboardPlayer = new();
    private readonly MouseRecorder mouseRecorder = new();
    private readonly MousePlayer mousePlayer = new();

    public MainWindow()
    {
        InitializeComponent();
    }

    private void OnStartRecord(object sender, RoutedEventArgs e)
    {
        keyboardRecorder.Start();
        mouseRecorder.Start();
    }

    private void OnStopRecord(object sender, RoutedEventArgs e)
    {
        keyboardRecorder.Stop();
        mouseRecorder.Stop();
    }

    private void OnPlaySequence(object sender, RoutedEventArgs e)
    {
        keyboardPlayer.Play(keyboardRecorder.KeyPlaybackBuffer, 50);
        mousePlayer.Play(mouseRecorder.KeyPlaybackBuffer, 50);
    }
}
