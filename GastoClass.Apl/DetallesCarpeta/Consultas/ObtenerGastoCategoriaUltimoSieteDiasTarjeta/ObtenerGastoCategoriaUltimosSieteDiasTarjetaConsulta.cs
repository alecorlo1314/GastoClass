using MediatR;

namespace GastoClass.Aplicacion.DetallesCarpeta.Consultas.ObtenerGastoCategoriaUltimoSieteDiasTarjeta;

public record ObtenerGastoCategoriaUltimosSieteDiasTarjetaConsulta(int? IdTarjeta) :
    IRequest<List<GastoCategoriaUltimosSieteDiasTarjetaDto>>
{

}
