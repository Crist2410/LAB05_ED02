using System;
using System.Collections.Generic;
using System.Text;

namespace LibreriaGenericos
{
    public abstract class Cifrado
    {
        protected abstract void Cifrar(string RutaOriginal, string RutaDestino);
        protected abstract void Decifrar(string RutaOriginal, string RutaDestino);
    }
}
