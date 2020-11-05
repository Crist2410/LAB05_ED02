using System;
using LibreriaGenericos;
using LibreriaGenericos.Clases;
using System.Collections.Generic;
using System.IO;


namespace PruebaConsola
{
    class Program
    {
        static void Main(string[] args)
        {
            //string Ruta = Path.GetFullPath("..\\..\\..\\..\\API_Proyecto3\\Archivos");
            string Path1 = @"C:\Users\Crist\Desktop\LAB05_ED02\API\Archivos\Archivo1.txt";
            string Path2 = @"C:\Users\Crist\Desktop\LAB05_ED02\API\Archivos\Archivo2.txt";
            string RutaTexto = @"C:\Users\Crist\Desktop\Cifrado\API_Proyecto3\ArchivosPrueba\BIBLIA COMPLETA.txt";
            // string RutaTexto = @"C:\Users\Crist\Desktop\Cifrado\API_Proyecto3\ArchivosPrueba\la odisea.txt";
            //string RutaTexto = @"C:\Users\Crist\Desktop\Cifrado\API_Proyecto3\ArchivosPrueba\easy test.txt";
            //string RutaTexto = @"C:\Users\Crist\Desktop\Cifrado\API_Proyecto3\ArchivosPrueba\Prueba1.txt";
            //Cesar CifradoCesar = new Cesar();
            //CifradoCesar.Encriptar(RutaTexto, Path1, "murcielago");
            //CifradoCesar.Desencriptar(Path1, Path2, "murcielago");
            ZigZag Cifradozigzag = new ZigZag();
            Cesar cesar = new Cesar();
            AuxClase auxClase = new AuxClase();
            //auxClase.Encode(RutaTexto, Path1, 6);
            //auxClase.Decode(Path1, Path2, 6);
            cesar.Encriptar(RutaTexto, Path1, "Parangaracutirimicuaro");
            cesar.Desencriptar(Path1, Path2, "parangaracutirimicuaro");


        }


    }
}
