using GastoClass.Dominio.Entidades;
using SQLite;

namespace GastoClass.Infraestructura.Persistencia.Entidades;

public class TarjetaCreditoEntidad
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string? TipoTarjeta { get; set; }
    public string? NombreTarjeta { get; set; }
    public int? UltimosCuatroDigitos { get; set; }
    public int? MesVencimiento { get; set; }
    public int? AnioVencimiento { get; set; }
    public decimal? LimiteCredito { get; set; }//cuanto dinero se tiene al incio de cada mes
    public decimal? Balance { get; set; } // total que se debe pagar a la tarjeta por cada compra sumada
    public decimal? CreditoDisponible { get; set; } //es el limite de credito menos el balance
    public string? Moneda { get; set; }
    public int? DiaCorte { get; set; }
    public int? DiaPago { get; set; }
    public string? NombreBanco { get; set; }

    //Clave Foranea
    public int? IdPreferenciaTarjeta { get; set; }
    [Ignore]
    public PreferenciaTarjetaDominio? PreferenciaTarjeta { get; set; }
}
