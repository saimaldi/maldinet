using MaldiRabbit;

namespace MaldiContol
{
    /// <summary>
    /// Provides and interface for applications to set commands to the MALDI Instrument
    /// </summary>
    public class MaldiController
    {
        private readonly RabbitMQConnection connection;
        public MaldiController(string hostname = "10.1.234.1")
        {

            RabbitMQConnectionDetails connectionParameters = new RabbitMQConnectionDetails();
            connectionParameters.Host = hostname;
            connectionParameters.SetControlExchange();
            connection = new RabbitMQConnection();
            connection.Connect(connectionParameters, false);
        }

        /// <summary>
        /// Request the instrument moves the sample to the sample
        /// inlet allows the user to change the sample plate
        /// </summary>
        public void SampleOut()
        {
            connection.SendMessage("ADVISE FRONTPANEL BUTTON_PRESSED");
        }

        /// <summary>
        /// Request the instrument tries to pump the sample inlet
        /// and take the sample into the instrument if sucessful
        /// </summary>
        public void SampleIn()
        {
            connection.SendMessage("SET PUMPINGSTATE LOCK_PUMPDOWN");
        }
    }
}
