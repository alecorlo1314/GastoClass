using GastoClass.Dominio.ValueObjects.ValueObjectsTarjetaCredito;

namespace GastoClass.Dominio.Entidades;
/// <summary>
/// Esta Clase es el nucleo de la entidad TarjetaCredito
/// Aqui se define las propiedades de la entidad
/// </summary>
public class TarjetaCreditoDominio
{
    public int Id { get; private set; }
    public TipoTarjeta Tipo { get; private set; }
    public NombreTarjeta NombreTarjeta { get; private set; }
    public UltimosCuatroDigitosTarjeta UltimosCuatroDigitos { get; private set; }
    public MesVencimiento MesVencimiento { get; private set; }
    public AnioVencimiento AnioVencimiento { get; private set; }
    public LimiteCredito LimiteCredito { get; private set; }
    public decimal Balance { get; private set; }
    public decimal CreditoDisponible { get; private set; }
    public TipoMoneda TipoMoneda { get; private set; }
    public DiaCorte DiaCorte { get; private set; }
    public DiaPago DiaPago { get; private set; }
    public NombreBanco NombreBanco { get; private set; }
    public PreferenciaTarjetaDominio Preferencia { get; private set; }

    public void SetId(int id) => Id = id;
    public void SetBalance(decimal balance) => Balance = balance;

    private TarjetaCreditoDominio(
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
        NombreBanco nombreBanco,
        PreferenciaTarjetaDominio preferencia)
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
        Preferencia = preferencia;

        // Inicializar balance y crédito disponible
        Balance = 0;
        CreditoDisponible = limiteCredito.Valor;
    }

    #region Crear 
    public static TarjetaCreditoDominio Crear(
           string tipo,
           string nombre,
           int ultimosCuatroDigitos,
           int mesVencimiento,
           int anioVencimiento,
           string tipoMoneda,
           decimal limiteCredito,
           int diaCorte,
           int diaPago,
           string nombreBanco,
           PreferenciaTarjetaDominio preferencia)
    {
        return new TarjetaCreditoDominio(
            id: 0,
            tipo: new TipoTarjeta(tipo!),
            nombre: new NombreTarjeta(nombre!),
            ultimosCuatro: new UltimosCuatroDigitosTarjeta(ultimosCuatroDigitos),
            mesVencimiento: new MesVencimiento(mesVencimiento),
            anioVencimiento: new AnioVencimiento(anioVencimiento),
            limiteCredito: new LimiteCredito(limiteCredito),
            tipoMoneda: new TipoMoneda(tipoMoneda!),
            diaCorte: new DiaCorte(diaCorte),
            diaPago: new DiaPago(diaPago),
            nombreBanco: new NombreBanco(nombreBanco!),
            preferencia: preferencia);
    }

    #endregion

    public void ActualizarDetalles(
    string TipoTarjeta,
    string NombreTarjeta,
    string TipoMoneda,
    string NombreBanco)
    {
        new TipoTarjeta(TipoTarjeta);
        new NombreTarjeta(NombreTarjeta);
        new TipoMoneda(TipoMoneda);
        new NombreBanco(NombreBanco);
    }

    public void RevertirGasto(decimal montoGasto)
    {
        if(montoGasto <= 0)
            throw new ExcepcionDominio(nameof(montoGasto), "El monto del gasto debe ser mayor a cero");

        //Aumentar creadito disponible
        CreditoDisponible += montoGasto;

            //Reducir balance
            Balance -= montoGasto;

            // Validaciones de consistencia
            if (CreditoDisponible > LimiteCredito.Valor) 
                CreditoDisponible = LimiteCredito.Valor; // nunca debe superar el límite

            if (Balance < 0) Balance = 0; // nunca debe quedar negativo
    }
    public void RestarBalance(decimal gastoAnterior, decimal gastoActual)
    {
        Balance -= gastoAnterior - gastoActual;
    }
    public void SumarBalance(decimal gastosAnterior, decimal gastoActual)
    {
        Balance += gastoActual - gastosAnterior;
    }
    public void ActualizacionCreditoDisponible(decimal limiteCredito, decimal balance)
    {
        CreditoDisponible = limiteCredito - balance;
    }
    public void AumentarBalance(decimal gastoTotal)
    {
        Balance += gastoTotal;
    }
    public void AplicarGasto(decimal monto)
    {
        if (monto <= 0)
            throw new ExcepcionDominio(nameof(monto), "El monto debe ser mayor a cero");

        if (CreditoDisponible < monto)
            throw new ExcepcionDominio(nameof(monto), "Crédito insuficiente");

        Balance += monto;
        CreditoDisponible -= monto;
    }

}
