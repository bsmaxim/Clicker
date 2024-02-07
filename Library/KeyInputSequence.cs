using static AutoClicker.Library.InputStructs;

namespace AutoClicker.Library
{
    /// <summary>
    /// Represents a sequence of key inputs.
    /// </summary>
    public class KeyInputSequence
    {
        /// <summary>
        /// Gets or sets the timestamp of the frame.
        /// </summary>
        public long FrameTimestamp { get; set; }

        /// <summary>
        /// Gets or sets the array of input objects representing the key inputs.
        /// </summary>
        public INPUT[]? InputSequence { get; set; }
    }
}
