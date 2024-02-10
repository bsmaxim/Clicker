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

    // get default value from radio button
    private bool needKeyRecord = false;
    private bool needMouseRecord = false;

    public MainWindow()
    {
        InitializeComponent();
        needMouseRecord = NeedMouseRecordButton.IsChecked ?? false;
        needKeyRecord = NeedKeyRecordButton.IsChecked ?? false;
    }

    private void OnStartRecord(object sender, RoutedEventArgs e)
    {
        if (needMouseRecord)
        {
            mouseRecorder.Start();
        }
        if (needKeyRecord)
        {
            keyboardRecorder.Start();
        }
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

    private void ToggleNeedMouseRecord(object sender, RoutedEventArgs e)
    {
        needMouseRecord = !needMouseRecord;
    }

    private void ToggleNeedKeyRecord(object sender, RoutedEventArgs e)
    {
        needKeyRecord = !needKeyRecord;
    }
}
