using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiUsuariosMVC.Models
{
    /// <summary>
    /// Clase para la devolver al cliente 
    /// </summary>
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public decimal Balance { get; set; }
        public bool Administrador { get; set; }
    }
}