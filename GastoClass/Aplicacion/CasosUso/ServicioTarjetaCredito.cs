using GastoClass.Aplicacion.DTOs;
using GastoClass.Aplicacion.Excepciones;
using GastoClass.Dominio.Interfacez;
using GastoClass.Dominio.Model;
using System.Globalization;

namespace GastoClass.Aplicacion.CasosUso;
public class ServicioTarjetaCredito
{
    #region Inyeccion de dependencias
    private readonly IServicioTarjetaCredito _servicioTarjetaCredito;
    private readonly IServicioGastos _servicioGastos;
    #endregion

    #region Constructor
    public ServicioTarjetaCredito(IServicioTarjetaCredito servicioTarjetaCredito, IServicioGastos servicioGastos)
    {
        //Inyeccion de dependencias
        _servicioTarjetaCredito = servicioTarjetaCredito;
        _servicioGastos = servicioGastos;
    }
    #endregion

    #region Metodos
    /// <summary>
    /// Guarda una nueva tarjeta de credito
    /// </summary>
    /// <param name="tarjetaCredito"></param>
    /// <returns></returns>
    public async Task<int> AgregarTarjetaCreditoAsync(TarjetaCredito tarjetaCredito)
    {
        //Guarda una nueva tarjeta de credito
        return await _servicioTarjetaCredito.AgregarTarjetaCreditoAsync(tarjetaCredito);
    }

    /// <summary>
    /// Obtiene todas las tarjetas de credito
    /// </summary>
    /// <returns></returns>
    public async Task<List<TarjetaCredito>?> ObtenerTarjetasCreditoAsync() 
    {
       return await _servicioTarjetaCredito.ObtenerTarjetasCreditoAsync();
    } 

    /// <summary>
    /// Elimina todas las tarjetas de credito
    /// </summary>
    /// <returns></returns>
    public async Task<int> EliminarTarjetasCreditoAsync()
    {
        return await _servicioTarjetaCredito.EliminarTarjetasCreditoAsync();
    }

    /// <summary>
    /// Obtiene los gastos por tarjeta
    /// </summary>
    /// <returns></returns>
    public async Task<List<TotalGastoPorTarjeta>?> ObtenerGastosPorTarjetasCreditoAsync()
    {
        return await _servicioTarjetaCredito.ObtenerGastosPorTarjetasCreditoAsync();
    }

    public async Task<List<UltimoTresMovimientoDTOs>?> ObtenerUltimosTresGastosPorTarjetaCreditoAsync(int? idTarjetaCredito)
    {
        var gastos = await _servicioGastos.ObtenerGastosAsync();
        if (!gastos!.Any()) throw new ServiciosExcepciones("No se encontro el gastos");
        var tarjeta = await _servicioTarjetaCredito.TarjetaPorIdAsync(idTarjetaCredito);
        if (tarjeta == null) throw new ServiciosExcepciones("No se encontro la tarjeta");

        //Mapear el gasto y tarjeta
        var resultados = ( from gasto in gastos
                           where gasto.TarjetaId == idTarjetaCredito
                           select new UltimoTresMovimientoDTOs
                           {
                               Imagen = gasto.NombreImagen,
                               Descripcion = gasto.Descripcion,
                               Fecha = gasto.Fecha,
                               Categoria = gasto.Categoria,
                               UltimosCuatroDigitos = tarjeta.UltimosCuatroDigitos,
                               Estado = gasto.Estado,
                               Monto = gasto.Monto
                           }).Take(3).ToList();
        return resultados;
    }

    public async Task<List<GastoCategoriaUltimosSieteDiasTarjetaDTO>?> GastosPorCategoriaSemanaAsync(int? idTarjetaCredito)
    {
        var gastos = await _servicioGastos.ObtenerGastosAsync();

        var fechaLimite = DateTime.Now.AddDays(-7);

        var query = from g in gastos
                    where g.TarjetaId == idTarjetaCredito
                    where g.Fecha >= fechaLimite
                    group g by new 
                    { 
                        Dia = g.Fecha.DayOfWeek, 
                        g.Categoria 
                    }
                    into grupo
                    select new GastoCategoriaUltimosSieteDiasTarjetaDTO
                    {
                        Dia = grupo.Key.Dia.ToString(),
                        Categoria = grupo.Key.Categoria,
                        TotalMonto = grupo.Sum(x => x.Monto)
                    };

        return query.ToList();
    }
    #endregion
}
