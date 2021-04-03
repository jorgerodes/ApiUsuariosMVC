using ApiUsuariosMVC.Datos;
using ApiUsuariosMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;

namespace ApiUsuariosMVC.Controllers
{
    /// <summary>
    /// login controller para autentificar usuarios
    /// </summary>
    [AllowAnonymous]
    [RoutePrefix("api/login")]
    public class LoginController : ApiController
    {
        private UsuariosEntities dbContext = new UsuariosEntities();
        /// <summary>
        /// Loguea con un usuario y devuelve el token JWT con el Claim de su login y su rol Administador o Usuario
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("authenticate")]
        public IHttpActionResult Authenticate(LoginRequest login)
        {
            if (login == null)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            // Validar credenciales 

            List<Usuario> users = dbContext.Usuarios.ToList();
            Usuario existe = users.FirstOrDefault(u => u.Login == login.Username && u.Pwd == login.Password);
            bool credencialesValidas = existe != null ? true : false;
            if (credencialesValidas)
            {
                var token = TokenGenerator.GenerateTokenJwt(existe);
                return Ok(token);
            }
            else
            {
                return Unauthorized();
            }
        }
    }
}
