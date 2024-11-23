using System.Windows;
using System.Windows.Input;
using AutoClicker.Library;
using AutoClicker.Library.Keyboard;
using AutoClicker.Library.Mouse;
using NHotkey;
using NHotkey.Wpf;


namespace AutoClicker;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private bool isRecording = false;
    private readonly KeyRecorder _keyboardRecorder = new();
    private readonly KeyPlayer _keyboardPlayer = new();
    private readonly MouseRecorder _mouseRecorder = new();
    private readonly MousePlayer _mousePlayer = new();

    private bool needKeyRecord = false;
    private bool needMouseRecord = false;

    private static readonly KeyGesture IncrementGesture = new KeyGesture(Key.Insert, ModifierKeys.Control | ModifierKeys.Alt);


    public MainWindow()
    {
        InitializeComponent();
        needMouseRecord = NeedMouseRecordButton.IsChecked ?? false;
        needKeyRecord = NeedKeyRecordButton.IsChecked ?? false;

        HotkeyManager.Current.AddOrReplace("Increment", IncrementGesture, OnRecordToggle);
    }

    private void OnRecordToggle(object sender, HotkeyEventArgs e)
    {
        if (isRecording)
        {
            OnStopRecord(null, null);
        }
        else
        {
            OnStartRecord(null, null);
        }
    }


    private void OnStartRecord(object sender, RoutedEventArgs e)
    {
        if (isRecording)
        {
            return;
        }
        if (needMouseRecord)
        {
            _mouseRecorder.Start();
        }
        if (needKeyRecord)
        {
            _keyboardRecorder.Start();
        }
        isRecording = true;
    }

    private void OnStopRecord(object sender, RoutedEventArgs e)
    {
        if (!isRecording)
        {
            return;
        }
        _keyboardRecorder.Stop();
        _mouseRecorder.Stop();
        isRecording = false;
    }

    private void OnPlaySequence(object sender, RoutedEventArgs e)
    {
        _keyboardPlayer.Play(_keyboardRecorder.KeyPlaybackBuffer, 50);
        _mousePlayer.Play(_mouseRecorder.KeyPlaybackBuffer, 50);
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
