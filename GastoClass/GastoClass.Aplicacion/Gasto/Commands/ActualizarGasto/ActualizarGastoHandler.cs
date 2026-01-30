using GastoClass.Aplicacion.Common;
using GastoClass.Dominio.Entidades;
using GastoClass.Dominio.Excepciones;
using GastoClass.GastoClass.Dominio.Interfaces;
using MediatR;

namespace GastoClass.GastoClass.Aplicacion.Gasto.Commands.ActualizarGasto;

public class ActualizarGastoHandler(IRepositorioGasto repositorioGasto)
    : IRequestHandler<ActualizarGastoCommand, ResultadosValidacion>
{
    public async Task<ResultadosValidacion> Handle(ActualizarGastoCommand request, CancellationToken cancellationToken)
    {
        //Para las validaciones
        var resultados = new ResultadosValidacion();
        try
        {
            //Se valida el negocio
            var gastoDominio = GastoDominio.Actualizar(
                id: request.IdCommand,
                monto: request.MontoCommand!.Value,
                fecha: request.FechaCommand!.Value,
                tarjetaId: request.TarjetaIdCommand!.Value,
                descripcion: request.DescripcionCommand,
                categoria: request.CategoriaCommand,
                comercio: request.ComercioCommand,
                estado: request.EstadoCommand,
                nombreImagen: request.NombreImagenCommand
            );

            //Agregar al repositorio
            await repositorioGasto.ActualizarAsync(gastoDominio);
        }
        catch (ExcepcionDominio ex)
        {
            //Convertir errores de dominio a errores de aplicación
            resultados.Errores.Add(ex.Campo, ex.Message);
        }
        return resultados;
    }
}
