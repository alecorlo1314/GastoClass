using GastoClass.Dominio.ValueObjects.ValueObjectsTarjetaCredito;

namespace GastoClass.Dominio.Entidades;
/// <summary>
/// Esta Clase es el nucleo de la entidad TarjetaCredito
/// Aqui se define las propiedades de la entidad
/// </summary>
public class TarjetaCreditoDominio
{
    public int Id { get; }
    public TipoTarjeta Tipo { get; private set; }
    public NombreTarjeta NombreTarjeta { get; private set; }
    public UltimosCuatroDigitosTarjeta UltimosCuatroDigitos { get; }
    public MesVencimiento MesVencimiento { get; }
    public AnioVencimiento AnioVencimiento { get; }
    public LimiteCredito LimiteCredito { get; }
    public decimal Balance { get; private set; }
    public decimal CreditoDisponible { get; private set; }
    public TipoMoneda TipoMoneda { get; private set; }
    public DiaCorte DiaCorte { get; }
    public DiaPago DiaPago { get; }
    public NombreBanco NombreBanco { get; private set; }

    public TarjetaCreditoDominio(
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

    public void RevertirGasto(decimal montoGasto)
    {
        if(montoGasto <= 0)
            throw new ArgumentException("El monto del gasto debe ser mayor a cero.");

            //Aumentar creadito disponible
            CreditoDisponible += montoGasto;

            //Reducir balance
            Balance -= montoGasto;

            // Validaciones de consistencia
            if (CreditoDisponible > LimiteCredito.Valor) 
                CreditoDisponible = LimiteCredito.Valor.Value; // nunca debe superar el límite

            if (Balance < 0) Balance = 0; // nunca debe quedar negativo
    }
}
