using GastoClass.GastoClass.Aplicacion.Common;
using GastoClass.GastoClass.Aplicacion.Dashboard.DTOs;
using MediatR;

namespace GastoClass.GastoClass.Aplicacion.Dashboard.Consultas.TarjetasCreditoComboBox;

public record ObtenerTarjetasCreditoComboBoxConsulta
    : IRequest<ResultadoConsulta<List<TarjetaCreditoComboBoxDto>>>

{
}
