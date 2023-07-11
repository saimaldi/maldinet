using System;

namespace MaldiContol
{
    /// <summary>
    /// An implementation of a MessageInterface which updates a
    /// windows text boxes.  This implementation uses the AsyncTextDisplay
    /// to hide the windows thread marshalling complexities
    /// </summary>
    public class WindowsFormMessageInterface : MessageInterface
    {
        AsyncTextDisplay TextDisplay;
        AsyncTextDisplay XMotorPositionDisplay;
        AsyncTextDisplay YMotorPositionDisplay;
        AsyncTextDisplay LockStatusDisplay;
        AsyncTextDisplay PumpingStateDisplay;

        /// <summary>
        /// The constructor connects the dialog components 
        /// to our internal asynctextdisplay objects.  This dialog must 
        /// be loaded and the components must exist before it is passed to this consturctor
        /// </summary>
        /// <param name="tellme">The windows dialog object which contains the ui elements
        /// </param>
        public WindowsFormMessageInterface(MainWindow tellme)
        {
            TextDisplay = new AsyncTextDisplay(tellme.LastMessageTextBox());
            XMotorPositionDisplay = new AsyncTextDisplay(tellme.XMotorPositionDisplay());
            YMotorPositionDisplay = new AsyncTextDisplay(tellme.YMotorPositionDisplay());
            LockStatusDisplay = new  AsyncTextDisplay(tellme.LockStatusDisplay());
            PumpingStateDisplay = new AsyncTextDisplay(tellme.PumpingStateDisplay());
        }

        /// <summary>
        /// Internal processing method for stage messages
        /// </summary>
        /// <param name="words">the array of words containing the message</param>
        private void ProcessStageMessage(string[] words)
        {
            if (words[3].Equals("POSITION"))
            {
                if (words[2].Equals("X"))
                {
                    XMotorPositionDisplay.SetText(words[4]);
                }
                else
                {
                    if (words[2].Equals("Y"))
                    {
                        YMotorPositionDisplay.SetText(words[4]);
                    }
                }


            }
        }

        /// <summary>
        /// Internal processiong of lock status messages
        /// </summary>
        /// <param name="words">the array of words containing the message</param>
        private void ProcessLockStatusMessage(String[] words)
        {
            LockStatusDisplay.SetText(words[2]);
        }

        /// <summary>
        /// Internal processiong of pumping state messages
        /// </summary>
        /// <param name="words">the array of words containing the message</param>
        private void ProcessPumpingStateMessage(String[] words)
        { 
            PumpingStateDisplay.SetText(words[2]);
        }

        /// <summary>
        /// The target of the subscription to incoming messages from the Maldi Instrument
        /// 
        /// The message is converted to a consistent case and then split into the individual words
        /// these words are then parsed and the relevant messages are passed onto methods which 
        /// update the ui
        /// </summary>
        /// <param name="Message"></param>
        public void ProcessMessage(String Message)
        {
            TextDisplay.SetText(Message);
            var words = Message.ToUpper().Split();
            // we look for messages of the form Stage Motor X Position interpreted_position actual_position
            if (words.Length >= 6)
            {
                if (words[0].Equals("STAGE"))
                {
                    ProcessStageMessage(words);
                }
            }
            if (words.Length > 2)
            {
                // LOCK STATUS STOPPED_CLOSED
                // LOCK STATUS STOPPED_OPEN
                if (words[0].Equals("LOCK") && words[1].Equals("STATUS"))
                {
                    ProcessLockStatusMessage(words);
                }
                //PUMPING STATE state
                if (words[0].Equals("PUMPING") && words[1].Equals("STATE"))
                {
                    ProcessPumpingStateMessage(words);
                }
            }
        }
    }
}
