using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace cliente
{
    public class Client 
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
                //Crea un client TCP
                int port = 8118;
                TcpClient client = new TcpClient(servidor, port);

                // Enmmagatzema el missatge en un array de byte
                // missatge String --> Byte[]
                Byte[] buffer = System.Text.Encoding.ASCII.GetBytes(missatge);

                //Fluxe de dades per comunicar-se amb servidor
                NetworkStream stream = client.GetStream();

                // Envia missatge
                stream.Write(buffer, 0, buffer.Length);
                Console.WriteLine("Enviat: {0}", missatge);

                // Resposta del servidor.

                // Reinicializem el buffer per desar la resposta.
                // màxim de bytes que es llegiran -->256 
                buffer = new Byte[256];

                // String per la resposta.
                String resposta = String.Empty;

                // Llegeix la resposta del servidor
                //bytes indica el nombre total que es llegeixen al buffer
                Int32 bytes = stream.Read(buffer, 0, buffer.Length);

                // resposta byte[] --> string
                resposta = System.Text.Encoding.ASCII.GetString(buffer, 0, bytes);
                Console.WriteLine("Received: {0}", resposta);

                // Tancar
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
