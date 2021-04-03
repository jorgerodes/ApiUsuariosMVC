using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiUsuariosMVC.Models.Respuesta
{
    public class DatosRetorno
    {
        public string Mensaje { get; set; }
        public object Datos { get; set; }
        public DatosRetorno(string mensaje = "", object datos = null)
        {
            Mensaje = mensaje;
            Datos = datos;
        }
    }
}