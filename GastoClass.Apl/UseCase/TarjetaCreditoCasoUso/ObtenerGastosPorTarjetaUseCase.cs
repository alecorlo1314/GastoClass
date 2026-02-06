using GastoClass.Dominio.Interfaces;

namespace GastoClass.Aplicacion.UseCase.TarjetaCreditoCasoUso
{
    public class ObtenerGastosPorTarjetaUseCase
    {
        #region Inyeccion de dependencias
        private readonly IRepositorioTarjetaCredito _repositorioGasto;
        #endregion
        public ObtenerGastosPorTarjetaUseCase(IRepositorioGasto repositorioGasto)
        {
            _repositorioGasto = repositorioGasto;
        }
    }
}
