using System;
using System.Threading.Tasks;
using Aplicacion.Documentos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
     [ApiController]
    [Route("api/[controller]")]
    public class DocumentoController : MiControllerBase
    {
        [HttpPost]
        public  async Task<ActionResult<Unit>> GuardarArchivos(SubirArchivo.Ejecuta parametros){
            return await Mediator.Send(parametros);
        }

        [HttpGet("{id}")]
        public  async Task<ActionResult<ArchivoGenerico>> ObtenerDocumento(Guid id){
            return await Mediator.Send(new ObtenerArchivo.Ejecuta { Id = id });
        }

    }
}