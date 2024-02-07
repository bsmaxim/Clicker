using System.Windows;
using AutoClicker.Library;

namespace AutoClicker;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private KernelKeyRecorder recorder = new();
    private Thread? recordThread;
    private KernelKeyPlayer player = new();

    public MainWindow()
    {
        InitializeComponent();
    }

    private void StartRecord_Click(object sender, RoutedEventArgs e)
    {
        recorder.Start();
    }

    private void StopRecord_Click(object sender, RoutedEventArgs e)
    {
        recorder.Stop();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        player.Play(recorder.KeyPlaybackBuffer, 50);
    }
}
