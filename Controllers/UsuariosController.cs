using ApiUsuariosMVC.Datos;
using ApiUsuariosMVC.Helpers;
using ApiUsuariosMVC.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ApiUsuariosMVC.Controllers
{
    [Authorize]
    [RoutePrefix("api/user")]
    public class UsuariosController : ApiController
    {
        private UsuariosEntities dbContext = new UsuariosEntities();
        /// <summary>
        /// Listado de usuarios
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("list")]
        public IEnumerable<User> Listado()
        {
            using (UsuariosEntities ue = new UsuariosEntities())
            {
                var result = from u in dbContext.Usuarios
                             select new User
                             {
                                 Id = u.Id,
                                 Login = u.Login,
                                 Balance = (decimal)u.Balance,
                                 Administrador = u.Administrador
                             };
                return result;
            }
        }
        /// <summary>
        /// Información del usuario pasado por parámetro en la URI
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("i/{id}")]
        public HttpResponseMessage Get(int id)
        {
            using (UsuariosEntities ue = new UsuariosEntities())
            {

                var result = from u in dbContext.Usuarios
                             where u.Id == id
                             select new User
                             {
                                 Id = u.Id,
                                 Login = u.Login,
                                 Balance = (decimal)u.Balance,
                                 Administrador = u.Administrador
                             };

                if (result.Any())
                {
                    return RespuestaHelper.GestionaOKHttpResponse("Ok", result);
                }
                else
                {
                    return RespuestaHelper.GestionaErrorHttpResponse("Error", "Usuario no encontrado", httpStatusCode: HttpStatusCode.BadRequest);
                }
            }
        }

        /// <summary>
        /// Transfiere balance de un usuario a otro 
        /// </summary>
        /// <remarks>Se devuelve una respuesta json con el resultado de la operación </remarks>
        /// <param name="idUsuario actual"></param>
        /// <param name="LoginUsuarioDestino"></param>
        /// <param name="Cantidad"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("transfer")]
        public HttpResponseMessage Transfiere(EntradaDatosTx data)
        {
            string mensaje;
            bool err = false;

            var usuario = dbContext.Usuarios.FirstOrDefault(u => u.Id == data.IdUsuario);
            var usuarioDestino = dbContext.Usuarios.FirstOrDefault(u => u.Login == data.LoginUsuarioDestino);
            if (data.Cantidad <= 0)
            {
                err = true;
                mensaje = "La cantidad a transferir debe ser superior a 0";
            }
            else
            {
                if (usuario != null && usuarioDestino != null)
                {
                    string errs = "";
                    if (!transfiereBalance(usuario, usuarioDestino, data.Cantidad, ref errs))
                    {
                        err = true;
                        mensaje = errs;
                    }
                    else
                    {
                        mensaje = data.Cantidad + " € transferidos al usuario " + usuarioDestino.Login + ".Su nuevo balance es de " + usuario.Balance + "€.";
                    }
                }
                else
                {
                    err = true;
                    mensaje = "Usuario de origen o destino no encontrados";
                }
            }

            if (err)
            {
                return RespuestaHelper.GestionaErrorHttpResponse("Error", mensaje, httpStatusCode: HttpStatusCode.BadRequest);
            }
            else
            {
                User u = new User { Id = usuarioDestino.Id, Login = usuarioDestino.Login, Balance = usuarioDestino.Balance, Administrador = usuarioDestino.Administrador };
                return RespuestaHelper.GestionaOKHttpResponse(mensaje,u);
            }
            
 
        }
        #region métodos privados
        private bool transfiereBalance(Usuario usuarioActual, Usuario usuarioDestino, decimal cantidad, ref string err)
        {
            //compruebo que el balance del usuario es superior o igual a lo que quiere transferir
            if (usuarioActual.Balance < cantidad)
            {
                err = "Balance insuficiente para transferencia";
                return false;
            }
            usuarioActual.Balance -= cantidad;
            usuarioDestino.Balance += cantidad;
            dbContext.Entry(usuarioActual).State = EntityState.Modified;
            dbContext.Entry(usuarioDestino).State = EntityState.Modified;
            dbContext.SaveChanges();
            return true;
        }
        #endregion

    }


}
