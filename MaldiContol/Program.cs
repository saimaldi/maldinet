using System;
using System.Windows.Forms;
using MaldiRabbit;

namespace MaldiContol
{

    /// <summary>
    /// An example application which displays the maldi messages in a single line text
    /// field, the lock state, pumping state and stage position
    /// </summary>
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            MainWindow mymainwindow = new MainWindow();

            //Create a maldi reader to receive messages from the Maldi instrument
            MaldiReader reader = new MaldiReader("elimaldidev");

            //Create a maldi reader to send control messages to the Maldi instrument
            MaldiController controller = new MaldiController("elimaldidev");

            // install the controller into the main window to enable ui object events to 
            // send messages to the instrument
            mymainwindow.SetMaldiControlInterface(controller);

            //install the messaging interface into the reader, the reader will use this 
            // interface to provide the ui with updates
            //
            // in this instance we create an instance of a WindowsFormMessageInterface which
            // understands how to update the ui components of the application main window
            reader.SetMessageInterface(new WindowsFormMessageInterface(mymainwindow));

            // run the application
            Application.Run(mymainwindow);
        }
    }
}
