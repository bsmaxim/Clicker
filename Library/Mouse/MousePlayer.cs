using System.Diagnostics;
using System.Runtime.InteropServices;
using AutoClicker.Library.Input;
using static AutoClicker.Library.Input.WinInputStructs;

namespace AutoClicker.Library
{


  public class MousePlayer : InputPlayer
  {
    public override List<InputUnit> GetInputSequences(Dictionary<long, List<IInputEvent>> keyPlaybackBuffer)
    {
      Dictionary<long, List<MouseEvent>> mouseEventDict = keyPlaybackBuffer.ToDictionary(
        pair => pair.Key,
        pair => pair.Value.Cast<MouseEvent>().ToList()
        );
      return InputSequencer.BuildSequence(mouseEventDict);
    }

    protected override uint MySendInput(INPUT[] inputs)
    {
      for (int i = 0; i < inputs.Length; i++)
      {
        ChangeToScreenCoords(ref inputs[i].data.mi.x, ref inputs[i].data.mi.y);
      }

      return WinApi.SendInput(
        (uint)inputs.Length,
        inputs,
        Marshal.SizeOf(typeof(INPUT))
      );
    }

    private static void ChangeToScreenCoords(ref int x, ref int y)
    {
      x = x * 65536 / WinApi.GetSystemMetrics(SystemMetric.SM_CXSCREEN);
      y = y * 65536 / WinApi.GetSystemMetrics(SystemMetric.SM_CYSCREEN);
    }
  }
}
