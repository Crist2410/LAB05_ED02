using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibreriaGenericos.Clases;
using Microsoft.AspNetCore.Http;
using System.IO;
using API.Models;

namespace API.Controllers
{
    [Route("api")]
    [ApiController]
    public class CifradoController : ControllerBase
    {
        public static Ruta CifradoRuta = new Ruta();
        public static ZigZag CifradoZigZag = new ZigZag();
        public static Cesar CifradoCesar = new Cesar();
        // GET: api/
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "Jose Daniel Giron", "Cristian Josue Barrientos" };
        }
        [HttpPost("cipher/{method}")]
        public IActionResult Cipher([FromRoute]string method, [FromForm] IFormFile file, [FromForm] Key key)
        {
            try
            {
                if (key != null && file != null)
                {
                    string RutaOriginal= Path.GetFullPath("Archivos Originales\\" + file.FileName);
                    string RutaCifrado;
                    FileStream ArchivoOriginal = new FileStream(RutaOriginal, FileMode.OpenOrCreate);
                    file.CopyTo(ArchivoOriginal);
                    ArchivoOriginal.Close();
                    if (method == "ruta")
                    {
                        RutaCifrado = Path.GetFullPath("Archivos Cifrado\\" + file.FileName.Split('.')[0] + ".rt");
                        CifradoRuta.Encriptar(RutaOriginal, RutaCifrado, key.columns, key.rows);
                        FileStream ArchivoFinal = new FileStream(RutaCifrado, FileMode.Open);
                        FileStreamResult FileFinal = new FileStreamResult(ArchivoFinal, "text/rt");
                        return FileFinal;
                    }
                    else if (method == "zigzag")
                    {
                        RutaCifrado = Path.GetFullPath("Archivos Cifrado\\" + file.FileName.Split('.')[0] + ".zz");
                        CifradoZigZag.Encriptar(RutaOriginal, RutaCifrado, key.levels);
                        FileStream ArchivoFinal = new FileStream(RutaCifrado, FileMode.Open);
                        FileStreamResult FileFinal = new FileStreamResult(ArchivoFinal, "text/zz");
                        return FileFinal;
                    }
                    else if (method == "cesar")
                    {
                        RutaCifrado = Path.GetFullPath("Archivos Cifrado\\" + file.FileName.Split('.')[0] + ".csr");
                        CifradoCesar.Encriptar(RutaOriginal, RutaCifrado, key.word);
                        FileStream ArchivoFinal = new FileStream(RutaCifrado, FileMode.Open);
                        FileStreamResult FileFinal = new FileStreamResult(ArchivoFinal, "text/csr");
                        return FileFinal;
                    }
                    else
                    {
                        return BadRequest();
                    }             
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("decipher")]
        public IActionResult Decipher([FromForm] IFormFile file, [FromForm] Key key)
        {
            try
            {
                if (key != null && file != null)
                {
                    string method = file.FileName.Split('.')[1];
                    string RutaOriginal = Path.GetFullPath("Archivos Originales\\" + file.FileName);
                    string RutaCifrado;
                    FileStream ArchivoOriginal = new FileStream(RutaOriginal, FileMode.OpenOrCreate);
                    file.CopyTo(ArchivoOriginal);
                    ArchivoOriginal.Close();
                    RutaCifrado = Path.GetFullPath("Archivos Decifrado\\" + file.FileName.Split('.')[0] + ".txt");
                    if (method == "rt")
                        CifradoRuta.Desencriptar(RutaOriginal, RutaCifrado, key.columns, key.rows);
                    else if (method == "zz")
                        CifradoZigZag.Desencriptar(RutaOriginal, RutaCifrado, key.levels);
                    else if (method == "csr")
                        CifradoCesar.Desencriptar(RutaOriginal, RutaCifrado, key.word);
                    else
                        return BadRequest();
                    FileStream ArchivoFinal = new FileStream(RutaCifrado, FileMode.Open);
                    FileStreamResult FileFinal = new FileStreamResult(ArchivoFinal, "text/rt");
                    return FileFinal;
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
