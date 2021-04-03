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
    [Authorize(Roles = "Administrador")]
    [RoutePrefix("api/admin")]
    public class AdminController : ApiController
    {
        private UsuariosEntities dbContext = new UsuariosEntities();
        /// <summary>
        /// Añade un usuario. 
        /// </summary>
        /// <param name="usu"> Objeto de usuario a crear en la BD.El id es autoíncremental, así que aunque se pase, , ignorará </param>
        /// <returns></returns>
        [HttpPost]
        [Route("add")]
        public HttpResponseMessage AddUsuario([FromBody] Usuario usu)
        {

            if (ModelState.IsValid)
            {
                dbContext.Usuarios.Add(usu);
                dbContext.SaveChanges();
                return RespuestaHelper.GestionaOKHttpResponse("Usuario creado correctamente", usu);
            }
            else
            {
                return RespuestaHelper.GestionaErrorHttpResponse("Error", "Error en la petición Add", httpStatusCode: HttpStatusCode.BadRequest);
            }
        }
        /// <summary>
        /// Actualiza datos de usuario, salvo el balance
        /// </summary>
        /// <param name="usu"> Objeto de usuario a actualizar a crear en la BD. Ignorará el balance aunque se pase en la petición</param>
        /// <returns></returns>
        /// <summary>
        /// Modifica el balance del id de usuario pasado por parámetro, con la cantidad, positiva o negativa, indicadafd
        /// </summary>
        /// <param name="id">Id de usuario cuyo balance se quiere modificar</param>
        /// <param name="cantidad">cantidad, positiva o negativa, a añadir</param>
        /// <returns></returns>
        [HttpPost]
        [Route("updateBalance")]
        public HttpResponseMessage ModificaBalance(int id, decimal cantidad)
        {
            string mensaje;
            bool err = false;
            var usr = dbContext.Usuarios.FirstOrDefault(u => u.Id == id);
            //modifico balance
            if (usr != null)
            {
                //compruebo que balance resultante de operación sea mayor o igual a 0  
                if (usr.Balance + cantidad < 0)
                {
                    err = true;
                    mensaje = "El balance resultante del usuario no puede resultar negativo";
                }
                else
                {
                    usr.Balance += cantidad;
                    //indicamos que se modifica el  balance
                    dbContext.Entry(usr).Property(u => u.Balance).IsModified = true;
                    dbContext.SaveChanges();
                    mensaje = "Balance del usuario modificado correctamente";
                }
            }
            else
            {
                err = true;
                mensaje = "Usuario no encontrado";
            }
            if (err)
            {
                return RespuestaHelper.GestionaErrorHttpResponse("Error", mensaje, httpStatusCode: HttpStatusCode.BadRequest);
            }
            else
            {
                User u = new User { Id = usr.Id, Login = usr.Login, Balance = usr.Balance, Administrador = usr.Administrador };
                return RespuestaHelper.GestionaOKHttpResponse(mensaje, u);
            }
        }
        /// <summary>
        /// Actualiza un usuario
        /// </summary>
        /// <param name="usu"></param>
        /// <returns>Objeto que queremos actualizar en la BD. No actualiza el balance</returns>
        [HttpPut]
        [Route("update")]
        public HttpResponseMessage UpdateUsuario([FromBody]Usuario usu)
        {
            string mensaje;
            bool err = false;

            if (ModelState.IsValid)
            {
                //compruebo que login no exista, excepto para usuario actual
                var usuarioExiste = dbContext.Usuarios.Count(u => u.Id == usu.Id) > 0;
                var loginrepetido = dbContext.Usuarios.Count(u => u.Id != usu.Id && u.Login==usu.Login) > 0;
                if (usuarioExiste && !loginrepetido)
                {
                    dbContext.Entry(usu).State = EntityState.Modified;
                    //indicamos que no modifica balance
                    dbContext.Entry(usu).Property(u => u.Balance).IsModified = false;
                    dbContext.SaveChanges();
                   
                    mensaje = "Usuario con Id " + usu.Id+ " modificado correctamente";
                }
                else if(loginrepetido)
                {
                    err = true;
                    mensaje = "Login existente, elija otro nombre";
                }else
                {
                    err = true;
                    mensaje = "Usuario no encontrado";
                }
            }
            else
            {
                mensaje = "Error en la petición Add";
            }
            if (err)
            {
                return RespuestaHelper.GestionaErrorHttpResponse("Error", mensaje, httpStatusCode: HttpStatusCode.BadRequest);
            }
            else
            {
                var balanceUsuario = dbContext.Usuarios.Where(us => us.Id == usu.Id).Select(v=>v.Balance).First();
                User u = new User { Id = usu.Id, Login = usu.Login, Balance = (decimal)balanceUsuario, Administrador = usu.Administrador };
                return RespuestaHelper.GestionaOKHttpResponse(mensaje, u);
            }
        }


        /// <summary>
        /// Borra un usuario
        /// </summary>
        /// <param name="id">Id de usuario a borrar en la BD</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete/{Id}")]
        public HttpResponseMessage DeleteUsuario(int id)
        {
            var usu = dbContext.Usuarios.Find(id);
            if(usu!=null)
            {
                dbContext.Usuarios.Remove(usu);
                dbContext.SaveChanges();
                return RespuestaHelper.GestionaOKHttpResponse("Usuario con Id " + usu.Id + " borrado correctamente", null);
            }
            else
            {
                return RespuestaHelper.GestionaErrorHttpResponse("Error", "Usuario no encontrado", httpStatusCode: HttpStatusCode.BadRequest);
            }
        }

    }
}
