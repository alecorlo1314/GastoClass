using GastoClass.Aplicacion.Excepciones;
using GastoClass.Dominio.Entidades;
using GastoClass.Dominio.Enums;
using GastoClass.Dominio.Interfaces;
using GastoClass.Dominio.ValueObjects;

namespace GastoClass.Aplicacion.CasosUso;

/// <summary>
/// Caso de Uso para Agregar Tarjeta de Credito
/// </summary>
public class AgregarTarjetaCreditoCasoUso
{
    #region Inyeccion Dependencias
    /// <summary>
    /// Inyeccion de Dependencias hacia Repositorio de Tarjeta de Credito
    /// </summary>
    private readonly IRepositorioTarjetaCredito _repositorioTarjetaCredito;

    #endregion

    #region Contructor
    public AgregarTarjetaCreditoCasoUso(IRepositorioTarjetaCredito repositorioTarjetaCredito)
    {
        _repositorioTarjetaCredito = repositorioTarjetaCredito;
    }
    #endregion

    #region Agregar Tarjeta de Credito
    public async Task<int>? AgregarTarjetaCreditoAsync(TarjetaCredito? tarjetaCredito)
    {
        //Relizar validaciones
        if (string.IsNullOrWhiteSpace(tarjetaCredito?.Tipo.ToString()))
            throw new ExcepcionValidacionCasoUso("El tipo de tarjeta es requerido");

        if (string.IsNullOrWhiteSpace(tarjetaCredito.Nombre?.Valor))
            throw new ExcepcionValidacionCasoUso("El nombre de la tarjeta es requerido");

        if (!Enum.TryParse<TipoTarjeta>(tarjetaCredito.Tipo.ToString(), out var tipoTarjeta))
            throw new ExcepcionValidacionCasoUso("Tipo de tarjeta inválido");

        //Se mapea el Dto tarjeta de credito
        var tarjeta = new TarjetaCredito(
                    Guid.NewGuid(),
                    tipoTarjeta,
                    new NombreTarjeta(tarjetaCredito.Nombre.Valor),
                    new UltimosCuatroDigitosTarjeta(tarjetaCredito.UltimosCuatro!.Valor),
                    new FechaVencimiento(tarjetaCredito.Vencimiento!.Mes, tarjetaCredito.Vencimiento.Anio)
                );

        //Se envial al repositorio
        //devuelve 1 si se agrego correctamente, 0 si no
        return await _repositorioTarjetaCredito.AgregarAsync(tarjeta);
    }

    #endregion
}
