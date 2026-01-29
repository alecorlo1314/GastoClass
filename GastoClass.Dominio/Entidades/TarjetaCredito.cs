using GastoClass.Dominio.ValueObjects.ValueObjectsTarjetaCredito;

namespace GastoClass.Dominio.Entidades;
/// <summary>
/// Esta Clase es el nucleo de la entidad TarjetaCredito
/// Aqui se define las propiedades de la entidad
/// </summary>
public class TarjetaCredito
{
    public int Id { get; }
    public TipoTarjeta Tipo { get; private set; }
    public NombreTarjeta NombreTarjeta { get; private set; }
    public UltimosCuatroDigitosTarjeta UltimosCuatroDigitos { get; }
    public MesVencimiento MesVencimiento { get; }
    public AnioVencimiento AnioVencimiento { get; }
    public decimal Balance { get; }
    public LimiteCredito LimiteCredito { get; }
    public TipoMoneda TipoMoneda { get; private set; }
    public DiaCorte DiaCorte { get; }
    public DiaPago DiaPago { get; }
    public NombreBanco NombreBanco { get; private set; }

    public TarjetaCredito(
        int id, 
        TipoTarjeta tipo, 
        NombreTarjeta nombre, 
        UltimosCuatroDigitosTarjeta ultimosCuatro, 
        MesVencimiento mesVencimiento,
        AnioVencimiento anioVencimiento,
        LimiteCredito limiteCredito,
        TipoMoneda tipoMoneda,
        DiaCorte diaCorte,
        DiaPago diaPago,
        NombreBanco nombreBanco)
    {
        Id = id;
        Tipo = tipo;
        NombreTarjeta = nombre;
        UltimosCuatroDigitos = ultimosCuatro;
        MesVencimiento = mesVencimiento;
        AnioVencimiento = anioVencimiento;
        LimiteCredito = limiteCredito;
        TipoMoneda = tipoMoneda;
        DiaCorte = diaCorte;
        DiaPago = diaPago;
        NombreBanco = nombreBanco;
    }
    public void ActualizarDatos(
        TipoTarjeta tipoTarjeta,
        NombreTarjeta nombreTarjeta,
        TipoMoneda tipoMoneda,
        NombreBanco nombreBanco)
    {
        Tipo = tipoTarjeta;
        NombreTarjeta = nombreTarjeta;
        TipoMoneda = tipoMoneda;
        NombreBanco = nombreBanco;
    }
}
