using System;
using System.Collections.Generic;
using System.IO;

namespace LibreriaGenericos.Clases
{
    public class Cesar : Cifrado
    {
        string Llave;
        //Key -> A,B,C
        //Value -> X,A,D
        Dictionary<char, char> DicOriginal = new Dictionary<char, char>();
        Dictionary<char, char> DicLlave = new Dictionary<char, char>();
        FileStream ArchivoDestino;
        FileStream ArchivoOriginal;

        public void Encriptar(string RutaOriginal, string RutaDestino, string Key)
        {
            ValidarLlave(Key);
            RealizarDiccionarios();
            Cifrar(RutaOriginal, RutaDestino);
        }
        public void Desencriptar(string RutaOriginal, string RutaDestino, string Key)
        {
            ValidarLlave(Key);
            RealizarDiccionarios();
            Decifrar(RutaOriginal, RutaDestino);
        }
        protected override void Cifrar(string RutaOriginal, string RutaDestino)
        {
            ArchivoDestino = new FileStream(RutaDestino, FileMode.OpenOrCreate);
            ArchivoOriginal = new FileStream(RutaOriginal, FileMode.OpenOrCreate);
            BinaryReader reader = new BinaryReader(ArchivoOriginal);
            byte[] buffer;
            byte[] bufferEscitura = new byte[20000];
            int Pos = 0;
            while (ArchivoOriginal.Position < ArchivoOriginal.Length)
            {
                buffer = reader.ReadBytes(20000);
                foreach (var Item in buffer)
                    if (DicOriginal.ContainsKey((char)Item))
                        bufferEscitura[Pos++] = Convert.ToByte(DicOriginal[(char)Item]);
                    else
                        bufferEscitura[Pos++] = Item;
                ArchivoDestino.Write(bufferEscitura, 0, Pos);
                ArchivoDestino.Flush();
                Pos = 0;
            }
            ArchivoDestino.Close();
            reader.Close();
            ArchivoOriginal.Close();
        }
        protected override void Decifrar(string RutaOriginal, string RutaDestino)
        {
            ArchivoDestino = new FileStream(RutaDestino, FileMode.OpenOrCreate);
            ArchivoOriginal = new FileStream(RutaOriginal, FileMode.OpenOrCreate);
            BinaryReader reader = new BinaryReader(ArchivoOriginal);
            byte[] buffer;
            byte[] bufferEscitura = new byte[20000];
            int Pos = 0;
            while (ArchivoOriginal.Position < ArchivoOriginal.Length)
            {
                buffer = reader.ReadBytes(20000);
                foreach (var Item in buffer)
                    if (DicLlave.ContainsKey((char)Item))
                        bufferEscitura[Pos++] = Convert.ToByte(DicLlave[(char)Item]);
                    else
                        bufferEscitura[Pos++] = Item;
                ArchivoDestino.Write(bufferEscitura, 0, Pos);
                ArchivoDestino.Flush();
                Pos = 0;
            }
            ArchivoDestino.Close();
            reader.Close();
            ArchivoOriginal.Close();
        }
        void ValidarLlave(string value)
        {
            Dictionary<char, int> DicAux = new Dictionary<char, int>();
            string LlaveFinal = "";
            string TextoAux = value;
            for (int i = 0; i < value.Length; i++)
            {
                char Caracter = Convert.ToChar(TextoAux.Substring(0, 1));
                TextoAux = TextoAux.Substring(1);
                if (!DicAux.ContainsKey(Caracter))
                {
                    LlaveFinal += Caracter;
                    DicAux.Add(Caracter, 1);
                }
            }
            Llave = LlaveFinal;
        }
        void RealizarDiccionarios()
        {
            DicLlave = new Dictionary<char, char>();
            DicOriginal = new Dictionary<char, char>();
            string Diccionario = Llave.ToUpper();
            for (int i = 65; i < 91; i++)
                if(!Diccionario.Contains((char)i))
                Diccionario += (char)i;
            string TextoAux = Diccionario;
            //Mayusculas
            for (int i = 65; i < 91; i++)
            {
                char value = Convert.ToChar(TextoAux.Substring(0, 1));
                TextoAux = TextoAux.Substring(1);
                DicOriginal.Add((char)i,value);
                DicLlave.Add(value,(char)i);
            }
            TextoAux = Diccionario.ToLower();
            //Minisculas
            for (int i = 97; i < 123; i++)
            {
                char value = Convert.ToChar(TextoAux.Substring(0, 1));
                TextoAux = TextoAux.Substring(1);
                DicOriginal.Add((char)i, value);
                DicLlave.Add(value, (char)i);
            }
        }
    }
}
