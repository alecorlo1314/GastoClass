using GastoClass.Dominio.Interfaces;
using GastoClass.Aplicacion.Dashboard.DTOs;
using MediatR;

namespace GastoClass.Aplicacion.Dashboard.Consultas.TarjetasCreditoComboBox;

public class ObtenerTarjetasCreditoComboBoxHandler(IRepositorioTarjetaCredito repositorioTarjetaCredito)
    : IRequestHandler<ObtenerTarjetasCreditoComboBoxConsulta, IEnumerable<TarjetaCreditoComboBoxDto>>
{
    public async Task<IEnumerable<TarjetaCreditoComboBoxDto>> Handle(ObtenerTarjetasCreditoComboBoxConsulta request, CancellationToken cancellationToken)
    {
        //Consulta a la base de datos
        var resultado = await repositorioTarjetaCredito.ObtenerTodosAsync();

        //MapeoMapeo de Dominio a DTOs
        return resultado!.Select(tarjetaCredito => new TarjetaCreditoComboBoxDto
        {
            Id = tarjetaCredito.Id,
            //El icono por el momento no se necesitara
            NombreTarjeta = tarjetaCredito.NombreTarjeta.Valor,
            NumeroTarjeta = tarjetaCredito.UltimosCuatroDigitos.Valor
        });
    }
}
