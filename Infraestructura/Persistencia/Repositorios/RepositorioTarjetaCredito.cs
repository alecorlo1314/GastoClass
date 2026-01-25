using GastoClass.Dominio.Entidades;
using GastoClass.Dominio.Interfaces;
using Infraestructura.Persistencia.ContextoDB;
using Infraestructura.Persistencia.Entidades;

namespace Infraestructura.Persistencia.Repositorios;

public class RepositorioTarjetaCredito : IRepositorioTarjetaCredito
{
    #region Inyeccion de Dependencias
    /// <summary>
    /// Inyeccion de Dependencias para usar los servicios e inicializar el repositorio
    /// </summary>
    private readonly AppContextoDatos _conexion;

    #endregion

    #region Constructor
    public RepositorioTarjetaCredito(AppContextoDatos conexion)
    {
        _conexion = conexion;
    }

    #endregion

    public Task ActualizarAsync(TarjetaCredito tarjetaCredito)
    {
        throw new NotImplementedException();
    }

    public async Task AgregarAsync(TarjetaCredito tarjetaCredito)
    {
        var conexion = await _conexion.ObtenerConexionAsync();

        //Mapear el objeto Dominio a la Entidad
        var tarjetaCreditoEntidad = new TarjetaCreditoEntidad
        {
            Id = tarjetaCredito.Id,
            TipoTarjeta = tarjetaCredito.Tipo.Valor,
            NombreTarjeta = tarjetaCredito.Nombre.Valor,
            UltimosCuatroDigitos = tarjetaCredito.UltimosCuatro.Valor,
            MesVencimiento = tarjetaCredito.MesVencimiento.Mes,
            AnioVencimiento = tarjetaCredito.AnioVencimiento.Anio,
            LimiteCredito = tarjetaCredito.LimiteCredito.Valor,
            Balance = 0,
            CreditoDisponible = tarjetaCredito.LimiteCredito.Valor,
            Moneda = tarjetaCredito.TipoMoneda.Valor,
            DiaCorte = tarjetaCredito.DiaCorte.Dia,
            DiaPago = tarjetaCredito.DiaPago.Dia,
            NombreBanco = tarjetaCredito.NombreBanco.Valor,
            IdPreferenciaTarjeta = tarjetaCredito.Id,
        };

        if (tarjetaCredito.Id == 0)
        {
            await conexion.UpdateAsync(tarjetaCreditoEntidad);
        }
        else
        {
            await conexion.InsertAsync(tarjetaCreditoEntidad);
        }
    }

    public Task EliminarAsync(int idTarjetaCredito)
    {
        throw new NotImplementedException();
    }

    public Task<List<TarjetaCredito>?> ObtenerTodosAsync()
    {
        throw new NotImplementedException();
    }
}
