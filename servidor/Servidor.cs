using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace servidor
{
    public static class Servidor
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());

            Escolta();
        }

        private static void Escolta()
        {
            // És necessari instanciar-lo fora del 'try' per tenir accés al 'finally'
            TcpListener tcplisten = null;

            try
            {
                // Defineix listener que escoltarà qualsevol ip pel port indicat
                IPAddress ip = IPAddress.Any;
                int port = 8118;
                tcplisten = new TcpListener(ip, port);
                tcplisten.Start();
                Console.WriteLine("servidor iniciat, escoltant port " + port);

                //Buffer per la lectura de dades
                Byte[] bytespack = new Byte[256];
                String dades = null;

                // Escolta de manera indefinida
                while (true)
                {

                    Console.WriteLine("esperant connexions... ");

                    // client local usat per rebre dades
                    TcpClient client = tcplisten.AcceptTcpClient();
                    Console.WriteLine("connexió establida ");

                    //buida les possibles dades anteriors
                    dades = null;

                    // Objecte Stream pel flux de lectura escriptura
                    NetworkStream stream = client.GetStream();

                    // variable auxiliar per anar obtenint paquets de bytes
                    int i = stream.Read(bytespack, 0, bytespack.Length);

                    //bucle per obtenir totes les dades enviades
                    while ((i != 0))
                    {
                        // byte --> string
                        dades = System.Text.Encoding.ASCII.GetString(bytespack, 0, i);
                        Console.WriteLine("Rebut: {0}", dades);

                        // transforma el text a majúscules
                        dades = dades.ToUpper();

                        // string --> byte
                        byte[] missatge = System.Text.Encoding.ASCII.GetBytes(dades);

                        // Envia resposta
                        stream.Write(missatge, 0, missatge.Length);
                        Console.WriteLine("Enviat: {0}", dades);

                        //Tanca client. Allibera els recursos
                        client.Close();

                    }
                   

                }

            }
            catch (Exception e)
            {
                Console.WriteLine("\nError" + e.StackTrace);
            }
            finally
            {
                //Tanca servidor. Allibera els recursos
                tcplisten.Stop();
            }





        }

    }
}
