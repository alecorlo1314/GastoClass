
using GastoClass.Dominio.Enums;
using GastoClass.Dominio.ValueObjects;

namespace GastoClass.Dominio.Entidades;
/// <summary>
/// Esta Clase es el nucleo de la entidad TarjetaCredito
/// Aqui se define las propiedades de la entidad
/// </summary>
public class TarjetaCredito
{
    public Guid Id { get; }
    public TipoTarjeta Tipo { get; }
    public NombreTarjeta? Nombre { get; private set; }
    public UltimosCuatroDigitosTarjeta? UltimosCuatro { get; }
    public FechaVencimiento? Vencimiento { get; }

    public TarjetaCredito(
        Guid id, 
        TipoTarjeta tipo, 
        NombreTarjeta? nombre, 
        UltimosCuatroDigitosTarjeta? ultimosCuatro, 
        FechaVencimiento? vencimiento)
    {
        Id = id;
        Tipo = tipo;
        Nombre = nombre;
        UltimosCuatro = ultimosCuatro;
        Vencimiento = vencimiento;
    }

    public void ActualizarNombre(NombreTarjeta? nombre)
    {
        Nombre = nombre;
    }
}
