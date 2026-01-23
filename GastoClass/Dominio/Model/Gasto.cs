using SQLite;
using System.ComponentModel.DataAnnotations.Schema;

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
        public string? Descripcion { get; set; }
        public decimal Monto { get; set; }
        public string? Categoria { get; set; }
        public string? Comercio { get; set; }
        public DateTime Fecha { get; set; }
        [Ignore]
        public string? Estado { get; set; } = "Completado";
        public string? NombreImagen { get; set; }

        //Foreign key
        public int TarjetaId { get; set; }
        [Ignore]
        public TarjetaCredito? Tarjeta { get; set; }
    }
}
