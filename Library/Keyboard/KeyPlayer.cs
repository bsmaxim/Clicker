using AutoClicker.Library.Input;


namespace AutoClicker.Library.Keyboard
{
  public class KeyPlayer : InputPlayer
  {
    public override List<InputUnit> GetInputSequences(Dictionary<long, List<IInputEvent>> keyPlaybackBuffer)
    {
      Dictionary<long, List<KeyEvent>> keyEventDict = keyPlaybackBuffer.ToDictionary(
        pair => pair.Key,
        pair => pair.Value.Cast<KeyEvent>().ToList()
        );
      return InputSequencer.BuildSequence(keyEventDict);
    }
  }
}
