using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LibreriaGenericos.Clases
{
    public class ZigZag : Cifrado
    {
        int Nivel;
        //Key -> A,B,C
        //Value -> X,A,D
        FileStream ArchivoDestino;
        FileStream ArchivoOriginal;
        public void Encriptar(string RutaOriginal, string RutaDestino, int nivel)
        {
            Nivel = nivel;
            Cifrar(RutaOriginal, RutaDestino);
        }
        public void Desencriptar(string RutaOriginal, string RutaDestino, int nivel)
        {
            Nivel = nivel;
            Decifrar(RutaOriginal, RutaDestino);
        }
        protected override void Cifrar(string RutaOriginal, string RutaDestino)
        {
            ArchivoDestino = new FileStream(RutaDestino, FileMode.OpenOrCreate);
            ArchivoOriginal = new FileStream(RutaOriginal, FileMode.OpenOrCreate);
            BinaryReader reader = new BinaryReader(ArchivoOriginal);
            int Pos = 0;
            bool Subida = true;
            int Llenar = 0;
            int CantOlas = 0;
            string[] Texto = new string[Nivel];
            while (ArchivoOriginal.Position < ArchivoOriginal.Length)
            {
                var buffer = reader.ReadBytes(25000);
                foreach (var Item in buffer)
                {
                    if (Pos < Nivel - 1 && Subida)
                    {
                        Texto[Pos] += Item + ",";
                        Pos++;
                        if (Pos == Nivel - 1)
                            Subida = false;
                    }
                    else if (Pos > 0)
                    {
                        Texto[Pos] += Item + ",";
                        Pos--;
                        if (Pos == 0)
                            Subida = true;
                    }
                    Llenar++;
                    if (Llenar == Nivel + (Nivel - 2))
                    {
                        Llenar = 0;
                        CantOlas++;
                    }
                    if (CantOlas == 20)
                    {
                        EscribirOlas(Texto);
                        Texto = new string[Nivel];
                        CantOlas = 0;
                    }
                }
            }
            while (Llenar != 0 || CantOlas != 20)
            {
                if (Pos < Nivel - 1 && Subida)
                {
                    Texto[Pos] += 158 + ",";
                    Pos++;
                    if (Pos == Nivel - 1)
                        Subida = false;
                }
                else if (Pos > 0)
                {
                    Texto[Pos] += 158 + ",";
                    Pos--;
                    if (Pos == 0)
                        Subida = true;
                }
                Llenar++;
                if (Llenar == Nivel + (Nivel - 2))
                {
                    Llenar = 0;
                    CantOlas++;
                }
            }
            EscribirOlas(Texto);
            ArchivoDestino.Close();
            reader.Close();
            ArchivoOriginal.Close();
        }
        void EscribirOlas(string[] Texto)
        {
            foreach (var Item in Texto)
            {
                int PosBuffer = 0;
                byte[] BufferEscritura = new byte[25000];
                string Linea = Item.Substring(0, Item.Length - 1);
                foreach (var Valor in Linea.Split(','))
                {
                    BufferEscritura[PosBuffer++] = Convert.ToByte(Valor);
                }
                ArchivoDestino.Write(BufferEscritura, 0, PosBuffer);
            }
            ArchivoDestino.Flush();
        }
        protected override void Decifrar(string RutaOriginal, string RutaDestino)
        {
            ArchivoDestino = new FileStream(RutaDestino, FileMode.OpenOrCreate);
            ArchivoOriginal = new FileStream(RutaOriginal, FileMode.OpenOrCreate);
            BinaryReader reader = new BinaryReader(ArchivoOriginal);
            byte[] buffer;
            byte[] bufferEscitura = new byte[25000];
            int TamArchivo = 0;
            while (ArchivoOriginal.Position < ArchivoOriginal.Length)
            {
                buffer = reader.ReadBytes(25000);
                foreach (var Item in buffer)
                    TamArchivo++;
            }
            int SizeOla = Nivel + (Nivel - 2);
            string[] Texto = new string[Nivel];
            int NivelActual = 0;
            int Contador = 0;
            ArchivoOriginal.Seek(0, SeekOrigin.Begin);
            while (ArchivoOriginal.Position < ArchivoOriginal.Length)
            {
                buffer = reader.ReadBytes(25000);
                foreach (var Item in buffer)
                {
                    if (NivelActual == 0)
                    {
                        Texto[NivelActual] += Item + ",";
                        if (Contador == 19)
                        {
                            NivelActual++;
                            Contador = 0;
                        }
                    }
                    else if (NivelActual == Nivel - 1)
                    {
                        Texto[NivelActual] += Item + ",";
                        if (Contador == 20)
                        {
                            NivelActual++;
                            Contador = 0;
                        }
                    }
                    else
                    {
                        Texto[NivelActual] += Item + ",";
                        if (Contador == 40)
                        {
                            NivelActual++;
                            Contador = 0;
                        }
                    }
                    Contador++;
                    if (Nivel == NivelActual)
                    {
                        DesEncriptarOlas(Texto);
                        Texto = new string[Nivel];
                        NivelActual = 0;
                        Contador = 0;
                    }

                }
            }
            ArchivoDestino.Close();
            reader.Close();
            ArchivoOriginal.Close();
        }
        void DesEncriptarOlas(string[] Texto)
        {
            int Length = (2 * 20) + (2 * 20 * (Nivel - 2));
            int LengthOla = (Nivel - 2) + Nivel;
            byte[] bufferEscritura = new byte[Length];
            int NivelActual = 0;
            int PosBuffer = 0;
            int Ola = 0;
            foreach (var Item in Texto)
            {
                string[] Linea = Item.Split(',');
                if (NivelActual == 0)
                {
                    for (int i = 0; i < Linea.Length - 1; i++)
                    {
                        if (Linea[i] != "158")
                        {
                            bufferEscritura[i * LengthOla] = Convert.ToByte(Linea[i]);
                            PosBuffer++;
                        }
                    }
                }
                else if (NivelActual == Nivel - 1)
                {
                    for (int i = 0; i < Linea.Length - 1; i++)
                    {
                        if (Linea[i] != "158")
                        {
                            bufferEscritura[((i*LengthOla) + (Nivel - 1))] = Convert.ToByte(Linea[i]);
                            PosBuffer++;
                        }
                    }
                }
                else
                {
                    bool PrimeaPosicion = true;
                    for (int i = 0; i < Linea.Length - 1; i++)
                    {
                        if (Linea[i] != "158")
                        {
                            if (PrimeaPosicion)
                            {
                                bufferEscritura[(LengthOla * Ola) + NivelActual] = Convert.ToByte(Linea[i]);
                                PrimeaPosicion = !PrimeaPosicion;
                            }
                            else
                            {
                                bufferEscritura[(LengthOla * Ola) + (LengthOla-NivelActual) ] = Convert.ToByte(Linea[i]);
                                PrimeaPosicion = !PrimeaPosicion;
                                Ola++;
                            }
                            PosBuffer++;

                        }
                    }
                }
                NivelActual++;
                Ola = 0;
            }
            ArchivoDestino.Write(bufferEscritura, 0, PosBuffer);
            ArchivoDestino.Flush();
        }
    }
}