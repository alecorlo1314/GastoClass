using GastoClass.Aplicacion.Common;
using GastoClass.Dominio.Entidades;
using GastoClass.Dominio.Excepciones;
using GastoClass.GastoClass.Dominio.Interfaces;
using MediatR;

namespace GastoClass.GastoClass.Aplicacion.Gasto.Commands.AgregarGasto;

public class AgregarGastoHandler(IRepositorioGasto repositorioGasto)
    : IRequestHandler<AgregarGastoCommand, ResultadosValidacion>
{
    public async Task<ResultadosValidacion> Handle(AgregarGastoCommand request, CancellationToken cancellationToken)
    {
        //Para las validaciones
        var resultados = new ResultadosValidacion();
        try
        {
            // 1️⃣ Crear dominio (aquí se valida el negocio)
            var gastoDominio = GastoDominio.Crear(
                monto: request.Monto,
                fecha: request.Fecha,
                tarjetaId: request.TarjetaId,
                descripcion: request.Descripcion,
                categoria: request.Categoria,
                comercio: request.Comercio,
                estado: request.Estado,
                nombreImagen: request.NombreImagen
            );

            //Agregar al repositorio
            await repositorioGasto.AgregarAsync(gastoDominio);
        }
        catch (ExcepcionDominio ex)
        {
            // 3️⃣ Convertir errores de dominio a errores de aplicación
            resultados.Errores.Add(ex.Campo, ex.Message);
        }

        return resultados;

    }
}
