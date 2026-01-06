namespace GastoClass.Model
{
    /// <summary>
    /// Clase contendra el gasto ejemplo
    /// 1
    /// 2000
    /// 05/01/2026
    /// Coca Cola
    /// Ocio
    /// </summary>
    public class Gasto
    {
        public int Id { get; set; }
        public decimal Mondo { get; set; }
        public DateTime Fecha { get; set; }
        public string? Descripcion { get; set; }
        public string? Categoria { get; set; }
        public string? NombreImagen { get; set; }
    }
}
