using System.Windows.Forms;

namespace MaldiContol
{
    /// <summary>
    /// A class to deal with the marshalling to gui updates from non gui thread
    /// updates.  It just hides the windows complexity
    /// </summary>
    class AsyncTextDisplay
    {
        private TextBox TextElement;

        public AsyncTextDisplay(TextBox thisText)
        {
            TextElement = thisText;
        }
        private delegate void InvokeDelegate(string message);

        public void SetText(string val)
        {
            if (TextElement.InvokeRequired)
            {
                TextElement.BeginInvoke(new InvokeDelegate(SetText), val);
            }
            else
            {
                TextElement.Text = val;
            }
        }
    }
}
