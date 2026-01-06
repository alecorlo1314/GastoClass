using GastoClass.Dominio.Interfacez;
using GastoClass.Model;

namespace GastoClass.Infraestructura.Repositorios
{
    public class DatosGastos : IServicioGastos
    {
        //Inyeccion de dependecias
        private readonly RepositorioBaseDatos _repositorioBaseDatos;
        public DatosGastos(RepositorioBaseDatos repositorioBaseDatos)
        {
            _repositorioBaseDatos = repositorioBaseDatos;
        }

        public async Task<decimal> ObtenerGastosTotalesDelMesAsync(int mes, int anio)
        {
            try
            {
                //obtener la conexion a la base de datos
                var conexion = await _repositorioBaseDatos.ObtenerConexion();

                //var total = await conexion.ExecuteScalarAsync<float>("SELECT SUM(Monto) FROM Gasto");

                //Consulta para obtener los gastos del mes y año especificados
                var consulta = conexion.Table<Gasto>()
                    .Where(g => g.Fecha.Month == mes && g.Fecha.Year == anio);
                //lo convertimos en lista asyncrona
                var gastosDelMes = await consulta.ToListAsync();
                //inicializamos el total de gastos
                decimal totalGastos = 0;
                //iteramos sobre los gastos y sumamos los montos
                foreach (var gasto in gastosDelMes)
                {
                    totalGastos += gasto.Monto;
                }
                //retornamos el total de gastos
                return totalGastos;
            }
            catch (Exception ex)
            {
                // Manejo de errores
                throw new Exception("Error al obtener los gastos totales del mes.", ex);
            }
        }
        public Task<List<int>> ObtenerTransaccionesDelMesAsync(int mes, int anio)
        {
            throw new NotImplementedException();
        }
        public Task<List<string>> ObtenerCategoriasMayorGastoDelMess(int mes, int anio)
        {
            throw new NotImplementedException();
        }
        public Task<List<GastoClass.Model.Gasto>> ObtenerUltimos5GastosAsync()
        {
            throw new NotImplementedException();
        }
    }
}
