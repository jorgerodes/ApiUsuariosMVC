using ApiUsuariosMVC.Models.Respuesta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http;

namespace ApiUsuariosMVC.Helpers
{
    public class RespuestaHelper
    {
        /// <summary>
        /// Gestiona respuesta de errores
        /// </summary>

        public static HttpResponseMessage GestionaErrorHttpResponse(string mensaje, string mensajeCompleto, bool esExcepcion = false,
            string resultadoMensaje = RespuestaApi.Resultado_ERROR, HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest)
        {
            var resp = GeneraRespuesta(mensaje, mensajeCompleto, resultadoMensaje, httpStatusCode);
            if (esExcepcion)
            {
                throw new HttpResponseException(resp);
            }
            else
            {
                return resp;
            }
        }
        /// <summary>
        /// gestiona respuesta ok
        /// </summary>

        public static HttpResponseMessage GestionaOKHttpResponse(string mensajeCompleto, object objetoDevolver, string resultadoMensaje = RespuestaApi.Resultado_OK, HttpStatusCode httpStatusCode = HttpStatusCode.OK)
        {
            return GeneraRespuesta(mensajeCompleto, objetoDevolver, resultadoMensaje, httpStatusCode);
        }

        #region Métodos Privados

        private static HttpResponseMessage GeneraRespuesta(string mensaje, object objetoDevolver, string resultadoMensaje, HttpStatusCode httpStatusCode)
        {

            var datosRetorno = new DatosRetorno(mensaje, objetoDevolver);
            var respuestaApi = new RespuestaApi(resultadoMensaje, retorno: datosRetorno);
            var resp = GeneraContent(respuestaApi, resultadoMensaje, httpStatusCode);
            return resp;
        }

        private static HttpResponseMessage GeneraContent(RespuestaApi miRespuesta, string resultadoMensaje, HttpStatusCode httpStatusCode)
        {
            var jsonFormatter = new JsonMediaTypeFormatter();
            jsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;

            var objRet = new ObjectContent<RespuestaApi>(miRespuesta, jsonFormatter);

            var resp = new HttpResponseMessage(httpStatusCode)
            {
                Content = objRet,
                ReasonPhrase = resultadoMensaje
            };

            return resp;
        }

        #endregion
    }
}