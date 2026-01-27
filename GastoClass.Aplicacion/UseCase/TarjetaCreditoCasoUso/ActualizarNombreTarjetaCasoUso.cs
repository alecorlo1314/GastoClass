using GastoClass.Aplicacion.Excepciones;
using GastoClass.Dominio.Interfaces;
using GastoClass.Dominio.ValueObjects;

namespace GastoClass.Aplicacion.UseCase.TarjetaCreditoCasoUso;

public class ActualizarNombreTarjetaCasoUso
{
    #region Inyeccion de Dependencias 
    /// <summary>
    /// Utiliza los servicio de repositorio de Tarjeta de Credito
    /// </summary>
    private readonly IRepositorioTarjetaCredito _repositorioTarjetaCredito;

    #endregion

    #region Constructor
    public ActualizarNombreTarjetaCasoUso(IRepositorioTarjetaCredito repositorioTarjetaCredito)
    {
        _repositorioTarjetaCredito = repositorioTarjetaCredito;
    }

    #endregion

    #region Actualizar Tarjeta
    public async Task<int>? Ejecutar(int idTarjetaCredito, string? nombreTarjetaCredito)
    {

        var tarjetaCredito = (await _repositorioTarjetaCredito.ObtenerTodosAsync())
            .FirstOrDefault(x => x.Id == idTarjetaCredito);

        if (tarjetaCredito is null)
            throw new ExcepcionDominio("Tarjeta no encontrada");

        tarjetaCredito.ActualizarNombre(new NombreTarjeta(nombreTarjetaCredito!));

        return await _repositorioTarjetaCredito.AgregarAsync(tarjetaCredito); // o actualizar si ya existe
    }

    #endregion
}
