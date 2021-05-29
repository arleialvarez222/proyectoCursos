using System;
using System.Net;
using System.Threading.Tasks;
using Aplicacion.ManejadorError;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace WebAPI.Middleware
{
    public class ManejadorErrorMiddelware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ManejadorErrorMiddelware> _logger;
        public ManejadorErrorMiddelware(RequestDelegate next, ILogger<ManejadorErrorMiddelware> logger){
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context){
           try{
                await _next(context);
           }catch(Exception ex){
               await ManejadorExcepcionAsincrono(context, ex, _logger);
           }
        }

        private async Task ManejadorExcepcionAsincrono(HttpContext context, Exception ex, ILogger<ManejadorErrorMiddelware> logger)
        {
            object errores = null;
                switch (ex)
                {
                    case ManejadorExcepcion me:
                        logger.LogError(ex, "Manejador error");
                        errores = me.Errores;
                        context.Response.StatusCode = (int)me.Codigo;
                        break;
                    case Exception e :
                        logger.LogError(ex, "Error de servidor");
                        errores = string.IsNullOrWhiteSpace(e.Message) ? "Error" : e.Message;
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;    
                }
                context.Response.ContentType = "application/json";
                if(errores != null){
                    var resultados = JsonConvert.SerializeObject(new {errores});
                    await context.Response.WriteAsync(resultados);
                }   
        }
    }
}