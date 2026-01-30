using GastoClass.Dominio.Entidades;

namespace GastoClass.Aplicacion.DTOs;
/// <summary>
/// Se encarga de la entrada / salida de la aplicacion
/// No recibe ViewModels, Entidades de forma directa desde la capa de Presentacion
/// </summary>
public record TarjetaCreditoDto
{
    public int? Id { get; set; }
    public string? TipoTarjeta { get; set; }
    public string? Nombre { get; set; }
    public int? UltimosCuatroDigitos { get; set; }
    public int MesVencimiento { get; set; }
    public int AnioVencimiento { get; set; }
    public decimal? LimiteCredito { get; set; }//cuanto dinero se tiene al incio de cada mes
    public decimal? Balance { get; set; } // total que se debe pagar a la tarjeta por cada compra sumada
    public decimal? CreditoDisponible { get; set; } //es el limite de credito menos el balance
    public string? Moneda { get; set; }
    public int? DiaCorte { get; set; }
    public int? DiaPago { get; set; }
    public string? NombreBanco { get; set; }
    public int idPreferencia { get; set; }  
    public PreferenciasTarjetaCreditoDto? PreferenciaTarjeta { get; set; }
}
