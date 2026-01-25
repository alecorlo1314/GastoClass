
using GastoClass.Aplicacion.Excepciones;
using GastoClass.Dominio.Interfaces;

namespace GastoClass.Aplicacion.UseCase;

/// <summary>
/// Se encarga de eliminar una Tarjeta de Credito
/// </summary>
public class EliminarTarjetaCasoUso
{
    #region Inyeccion de Dependencias
    /// <summary>
    /// Utiliza los servicios del repositorio de Tarjeta de Credito
    /// </summary>
    private readonly IRepositorioTarjetaCredito _repositorioTarjetaCredito;

    #endregion

    #region Constructor
    public EliminarTarjetaCasoUso(IRepositorioTarjetaCredito repositorioTarjetaCredito)
    {
        _repositorioTarjetaCredito = repositorioTarjetaCredito;
    }

    #endregion

    public async Task<int>? EliminarTarjetaAsyn(Guid? idTarjetaCredito)
    {
        if (idTarjetaCredito == Guid.Empty)
            throw new ExcepcionValidacionCasoUso("Id de tarjeta inválido");

        return await _repositorioTarjetaCredito.EliminarAsync(idTarjetaCredito);
    }
}
