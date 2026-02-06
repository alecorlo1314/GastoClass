using GastoClass.Aplicacion.DTOs;
using GastoClass.Dominio.Interfaces;

namespace GastoClass.Aplicacion.UseCase.TarjetaCreditoCasoUso;

public class ObtenerTarjetaCreditoCasoUso 
{
    #region Inyeccion Dependencias
    /// <summary>
    /// Inyeccion Dependencias hacia respositorio de Tarjeta de Credito
    /// </summary>
    private readonly  IRepositorioTarjetaCredito _repositorioTarjetaCredito;

    #endregion

    #region Constructor
    public ObtenerTarjetaCreditoCasoUso(IRepositorioTarjetaCredito tarjetaCreditoRepositorio)
    {
        _tarjetaCreditoRepositorio = tarjetaCreditoRepositorio;
    }

    #endregion

    #region Obtener Tarjetas de Credito
    public async Task<List<TarjetaCreditoDto>?> Ejecutar()
    {
        //devuelve una lista de *entidades* de Tarjeta de Credito
        var resultados = await _repositorioTarjetaCredito.ObtenerTodosAsync();

        //Se realiza un mapeo de las entidades a DTO
        return resultados?.Select(tarjeta => new TarjetaCreditoDto
        {
            Id = tarjeta.Id,
            Tipo = tarjeta.Tipo.ToString(),
            Nombre = tarjeta.Nombre?.Valor,
            UltimosCuatroDigitos = tarjeta.UltimosCuatro?.Valor,
            Vencimiento = tarjeta.Vencimiento?.ToString()
        }).ToList();
    }
    #endregion
}
