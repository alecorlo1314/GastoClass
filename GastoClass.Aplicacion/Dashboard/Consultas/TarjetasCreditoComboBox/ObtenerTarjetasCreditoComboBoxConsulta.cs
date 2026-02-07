using GastoClass.Aplicacion.Dashboard.DTOs;
using MediatR;

namespace GastoClass.Aplicacion.Dashboard.Consultas.TarjetasCreditoComboBox;

public record ObtenerTarjetasCreditoComboBoxConsulta
    : IRequest<IEnumerable<TarjetaCreditoComboBoxDto>> { }
