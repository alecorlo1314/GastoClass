using GastoClass.Dominio.Interfaces;
using GastoClass.Aplicacion.Tarjeta.Commands;
using MediatR;

namespace GastoClass.GastoClass.Aplicacion.Tarjeta.Consultas;

public class EliminarTodasHandler(
    IRepositorioTarjetaCredito repositorioTarjetaCredito, 
    IRepositorioPreferenciaTarjeta repositorioPreferenciaTarjeta)
    : IRequestHandler<EliminarTodasTarjetasCommand, int>
{
    public async Task<int> Handle(EliminarTodasTarjetasCommand request, CancellationToken cancellationToken)
    {
        //Mas adentro realizamos la logica para ver si hay gastos asociados


        //Primero eliminamos las preferencias de tarjetas
        var preferenciasEliminadas = await repositorioPreferenciaTarjeta.EliminarTodasAsync();
        //Segun eliminamos las tarjetas de credito
        var tarjetasEliminadas = await repositorioTarjetaCredito.EliminarTodasAsync();

        return tarjetasEliminadas;
    }
}
