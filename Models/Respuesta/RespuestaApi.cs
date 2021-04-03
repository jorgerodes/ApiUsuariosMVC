using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiUsuariosMVC.Models.Respuesta
{
    public class RespuestaApi
    {
       
        public const string Resultado_ERROR = "ERROR";


        public const string Resultado_WARNING = "WARNING";

     
        public const string Resultado_OK = "OK";

   
        public string Resultado { get; set; }

        public DatosRetorno Retorno { get; set; }


        public RespuestaApi(string resultado, DatosRetorno retorno = null)
        {
            Resultado = resultado;
            Retorno = retorno;
        }
    }
}