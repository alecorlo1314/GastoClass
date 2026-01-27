using GastoClass.Dominio.ValueObjects.ValueObjectsTarjetaCredito;

namespace GastoClass.Dominio.Entidades;
/// <summary>
/// Esta Clase es el nucleo de la entidad TarjetaCredito
/// Aqui se define las propiedades de la entidad
/// </summary>
public class TarjetaCredito
{
    public int Id { get; }
    public TipoTarjeta Tipo { get; }
    public NombreTarjeta Nombre { get; private set; }
    public UltimosCuatroDigitosTarjeta UltimosCuatro { get; }
    public MesVencimiento MesVencimiento { get; }
    public AnioVencimiento AnioVencimiento { get; }
    public LimiteCredito LimiteCredito { get; }
    public Moneda TipoMoneda { get; }
    public DiaCorte DiaCorte { get; }
    public DiaPago DiaPago { get; }
    public NombreBanco NombreBanco { get; }

    public TarjetaCredito(
        int id, 
        TipoTarjeta tipo, 
        NombreTarjeta nombre, 
        UltimosCuatroDigitosTarjeta ultimosCuatro, 
        MesVencimiento mesVencimiento,
        AnioVencimiento anioVencimiento,
        LimiteCredito limiteCredito,
        Moneda tipoMoneda,
        DiaCorte diaCorte,
        DiaPago diaPago,
        NombreBanco nombreBanco)
    {
        Id = id;
        Tipo = tipo;
        Nombre = nombre;
        UltimosCuatro = ultimosCuatro;
        MesVencimiento = mesVencimiento;
        AnioVencimiento = anioVencimiento;
        LimiteCredito = limiteCredito;
        TipoMoneda = tipoMoneda;
        DiaCorte = diaCorte;
        DiaPago = diaPago;
        NombreBanco = nombreBanco;
    }

    public void ActualizarNombre(NombreTarjeta nombre)
    {
        Nombre = nombre;
    }
}
