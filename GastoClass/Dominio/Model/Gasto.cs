using SQLite;

namespace GastoClass.Dominio.Model
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
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public decimal Monto { get; set; }
        public DateTime Fecha { get; set; }
        public string? Descripcion { get; set; }
        public string? Categoria { get; set; }
        [Ignore]
        public string? NombreImagen { get; set; }
    }
}
