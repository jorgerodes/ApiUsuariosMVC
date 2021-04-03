using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiUsuariosMVC.Models
{
    /// <summary>
    /// Clase para la formato de datos recibidos para transferir fondos
    /// </summary>
    public class EntradaDatosTx
    {
        public int IdUsuario { get; set; }
        public string LoginUsuarioDestino { get; set; }
        public decimal Cantidad { get; set; }
    }
}