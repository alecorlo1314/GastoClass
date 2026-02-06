namespace GastoClass.GastoClass.Aplicacion.Gasto.DTOs;

public class GastoPorDiaSemanaDto
{
    public string DiaSemana { get; init; } = string.Empty;
    public decimal Total { get; init; }
    public int NumeroDia { get; init; }
    public int CantidadTransacciones { get; init; }
}
