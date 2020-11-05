using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace LibreriaGenericos.Clases
{
   public class Ruta : Cifrado
    {
        int Altura, Ancho;
        public void Encriptar(string RutaOriginal, string RutaDestino, int altura, int ancho)
        {
            Altura = altura;
            Ancho = ancho;
            Cifrar(RutaOriginal, RutaDestino);
        }
        public void Desencriptar(string RutaOriginal, string RutaDestino, int altura, int ancho)
        {
            Altura = altura;
            Ancho = ancho;
            Decifrar(RutaOriginal, RutaDestino);
        }

        protected override void Cifrar(string RutaOriginal, string RutaDestino)
        {
            //establecer rutas de archivos y buffer
            FileStream LecturaOriginal = new FileStream(RutaOriginal, FileMode.OpenOrCreate);
            using var Reader = new BinaryReader(LecturaOriginal);
            var buffer = new byte[2000000];

            //creacion lista de arreglos, variables
            List<byte[,]> ListaArreglos = new List<byte[,]>();
            List<byte> ListaBytes = new List<byte>();
            int ContadorBuffer = 0;

            //creacion writer
            using var Writer = new FileStream(RutaDestino, FileMode.OpenOrCreate);

            //obtencion lista arreglos
            while (Reader.BaseStream.Position != Reader.BaseStream.Length)
            {
                buffer = Reader.ReadBytes(Altura * Ancho);
                byte[,] ArregloTemporal = new byte[Altura, Ancho];
                for (int i = 0; i < Altura; i++)
                {
                    for (int j = 0; j < Ancho; j++)
                    {
                        if (ContadorBuffer < buffer.Length)
                        {
                            ArregloTemporal[j, i] = buffer[ContadorBuffer];
                            ++ContadorBuffer;
                        }
                        else
                        {
                            ArregloTemporal[j, i] = 158;
                        }
                    }
                }
                ListaArreglos.Add(ArregloTemporal);
                ContadorBuffer = 0;
            }

            //impresion arreglos en lista
            foreach (var item in ListaArreglos)
            {
                for (int i = 0; i < Altura; i++)
                {
                    for (int j = 0; j < Ancho; j++)
                    {
                        ListaBytes.Add(item[i, j]);
                    }
                }
                Writer.Write(ListaBytes.ToArray(), 0, ListaBytes.ToArray().Length);
                ListaBytes.Clear();
            }
            Writer.Close();
            Reader.Close();
        }

        protected override void Decifrar(string RutaOriginal, string RutaDestino)
        {
            //establecer rutas de archivos y buffer
            FileStream LecturaOriginal = new FileStream(RutaOriginal, FileMode.OpenOrCreate);
            using var Reader = new BinaryReader(LecturaOriginal);
            var buffer = new byte[2000000];

            //creacion lista de arreglos, variables
            List<byte[,]> ListaArreglos = new List<byte[,]>();
            List<byte> ListaBytes = new List<byte>();
            int ContadorBuffer = 0;

            //creacion writer
            using var Writer = new FileStream(RutaDestino, FileMode.OpenOrCreate);

            //obtencion lista arreglos
            while (Reader.BaseStream.Position != Reader.BaseStream.Length)
            {
                buffer = Reader.ReadBytes(Altura * Ancho);
                byte[,] ArregloTemporal = new byte[Altura, Ancho];
                for (int i = 0; i < Altura; i++)
                {
                    for (int j = 0; j < Ancho; j++)
                    {
                        ArregloTemporal[i, j] = buffer[ContadorBuffer];
                        ++ContadorBuffer;
                    }
                }
                ListaArreglos.Add(ArregloTemporal);
                ContadorBuffer = 0;
            }

            //impresion arreglos en lista
            foreach (var item in ListaArreglos)
            {
                for (int i = 0; i < Altura; i++)
                {
                    for (int j = 0; j < Ancho; j++)
                    {
                        if (item[j, i] != 158)
                        {
                            ListaBytes.Add(item[j, i]);
                        }
                    }
                }
                Writer.Write(ListaBytes.ToArray(), 0, ListaBytes.ToArray().Length);
                ListaBytes.Clear();
            }
            Writer.Close();
            Reader.Close();
        }
    }
}
