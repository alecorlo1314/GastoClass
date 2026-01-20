
using SQLite;

namespace GastoClass.Dominio.Model;

public class TarjetaCredito
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string? TipoTarjeta { get; set; }
    public string? NombreTarjeta { get; set; }
    public int? UltimosCuatroDigitos { get; set; }
    public int? MesVencimiento { get; set; }
    public int? AnioVencimiento { get; set; }
    public decimal? LimiteCredito { get; set; }
    public decimal? Balance { get; set; }
    public decimal? CreditoDisponible { get; set; }
    public string? Moneda { get; set; }
    public int? DiaCorte { get; set; }
    public int? DiaPago { get; set; }
    public string? NombreBanco { get; set; }

    //Clave Foranea
    public int? IdPreferenciaTarjeta { get; set; }
    [Ignore]
    public PreferenciaTarjeta? PreferenciaTarjeta { get; set; }

    //metodo calculo credito disponible
    public decimal? CalcularCreditoDisponible()
    {
        return CreditoDisponible = LimiteCredito - Balance;
    }
}
