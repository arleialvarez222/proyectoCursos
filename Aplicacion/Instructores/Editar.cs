using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Persistencia.DapperConexion.Instructor;

namespace Aplicacion.Instructores
{
    public class Editar
    {
        public class  Ejecuta : IRequest {
            public Guid InstructorId {get;set;}
            public string Nombre {get;set;}
            public string Apellido {get;set;}
            public string Grado {get;set;}
        }
        public class EjecutaValida : AbstractValidator<Ejecuta>{
            public EjecutaValida(){
                RuleFor(x => x.Nombre).NotEmpty();
                RuleFor(x => x.Apellido).NotEmpty();
                RuleFor(x => x.Grado).NotEmpty();
            }
        }

        public class Manejador : IRequestHandler<Ejecuta>
        {
            private readonly IInstructor _instructorRepositorio;
            public Manejador(IInstructor instructorRepositorio){
                _instructorRepositorio = instructorRepositorio;
            }
            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
              var resultado = await _instructorRepositorio.Actualiza(request.InstructorId, request.Nombre, request.Apellido, request.Grado);
              if(resultado > 0){
                  return Unit.Value;
              }
              throw new Exception("No se pudo actualizar la data del instructor");
            }
        }
    }
}