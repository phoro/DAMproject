using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace cliente
{
    class Client : IDisposable
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());

            String missatge = "peticio del client";
            String servidor = "127.0.0.1";
            Connecta(servidor, missatge);
        }

        private static void Connecta(String servidor, String missatge)
        {
            try
            {
                //Crea un client
                int port = 8118;
                TcpClient client = new TcpClient(servidor, port);

                //String --> Byte
                Byte[] dades = System.Text.Encoding.ASCII.GetBytes(missatge);

                //Fluxe de dades per comunicar-se amb servidor
                NetworkStream stream = client.GetStream();

                // Envia missatge
                stream.Write(dades, 0, dades.Length);
                Console.WriteLine("Enviat: {0}", missatge);

                // Receive the TcpServer.response.

                // Buffer to store the response bytes.
                dades = new Byte[256];

                // String to store the response ASCII representation.
                String responseData = String.Empty;

                // Read the first batch of the TcpServer response bytes.
                Int32 bytes = stream.Read(dades, 0, dades.Length);
                responseData = System.Text.Encoding.ASCII.GetString(dades, 0, bytes);
                Console.WriteLine("Received: {0}", responseData);

                // Close everything.
                stream.Close();
                client.Close();

            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);

            }






        }
    }
}
