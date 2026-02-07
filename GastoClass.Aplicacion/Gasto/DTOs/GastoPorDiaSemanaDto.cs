namespace GastoClass.Aplicacion.Gasto.DTOs;

public class GastoPorDiaSemanaDto
{
    public string? DiaSemana { get; set; }
    public decimal Total { get; set; }
    public int NumeroDia { get; set; } // 0=Domingo, 1=Lunes, 2=Martes ...
    public int CantidadTransacciones { get; set; }
}
