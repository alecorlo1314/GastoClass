using GastoClass.Aplicacion.Common;
using GastoClass.Aplicacion.DTOs;
using GastoClass.Dominio.Entidades;
using GastoClass.Dominio.Excepciones;
using GastoClass.Dominio.Excepciones.ExcepcionesGasto;
using GastoClass.Dominio.Interfaces;
using GastoClass.Dominio.ValueObjects.ValueObjectsGasto;

namespace GastoClass.Aplicacion.UseCase.GastoUseCase;

//public class CrearGastoUseCase
//{
//    #region Inyeccion de Dependencias
//    /// <summary>
//    /// Utiliza los servicios de la interfaz IGastoRepositorio
//    /// </summary>
//    private readonly IRepositorioGasto _repositorioGasto;
//    private readonly IRepositorioTarjetaCredito _repositorioTarjetaCredito;

//    #endregion

//    #region Constructor
//    public CrearGastoUseCase(IRepositorioGasto repositorioGasto, IRepositorioTarjetaCredito repositorioTarjetaCredito)
//    {
//        _repositorioGasto = repositorioGasto;
//        _repositorioTarjetaCredito = repositorioTarjetaCredito;
//    }

//    #endregion

//    #region Agregar Gasto
//    public async Task<ResultadosValidacion> Ejecutar(CreateGastoDto gastoDto)
//    {
//        //Intanciar el validador de errores
//        var validador = new ResultadosValidacion();
//        try
//        {
//            // se realiza el meapeo, esto verifica que los datos sean correctos
//            //si son incorrectos se lanza una exception la cual debera ser capturada en el catch
//            var gasto = new Gasto(
//                new Descripcion(gastoDto.Descripcion!),
//                new Monto(gastoDto.Monto),
//                new Categoria(gastoDto.Categoria!),
//                new Comercio(gastoDto.Comercio!),
//                new Fecha(gastoDto.Fecha),
//                new Estado(gastoDto.Estado!),
//                new NombreImagen(gastoDto.NombreImagen!),
//                gastoDto.TarjetaId
//                );
//            var listaGastos = await _repositorioGasto.ObtenerTodosAsync();
//            var listaTarjetas = await _repositorioTarjetaCredito.ObtenerTodosAsync();
//            //validar que la tarjeta exista
//            var tarjeta = listaTarjetas?.FirstOrDefault(t => t.Id == gasto.TarjetaId);
//            //Actualizar gasto si el id es diferente de 0
//            if (gasto.Id != 0)
//            {
//                //Validar que el gasto exista
//                var gastoExistente = listaGastos!.FirstOrDefault(g => g.Id == gasto.Id);
//                if (gastoExistente == null) throw new Exception("El gasto no existe.");

//                //Validar que el nuevo monto/categoría sean correctos.
//                if (gasto.Monto.Valor < 0 || gasto.Categoria!.Valor == null) throw new Exception("El monto y la categoría deben ser correctos.");
//                //antes de guardar el gasto, guardaremos el monto anterior
//                decimal montoAnterior = gastoExistente.Monto.Valor;
//                //Ajustar el balance actual si el monto actual es diferente al monto anterior
//                if (gasto.Monto.Valor < montoAnterior)
//                {
//                    //restamos la diferencia entre el monto anterior y el monto actual
//                    tarjeta.Balance -= montoAnterior - gasto.Monto.Valor;
//                }
//                else if (montoAnterior < gasto.Monto)
//                {
//                    //sumamos la diferencia entre el monto actual y el monto anterior
//                    tarjeta.Balance += gasto.Monto - montoAnterior;
//                }
//                // Validar límite
//                if (tarjeta.Balance > tarjeta.LimiteCredito.Valor)
//                    throw new RespositorioGastoExcepcion("El balance actual de la tarjeta es mayor al limite de credito.");
//                tarjeta.CreditoDisponible = tarjeta.LimiteCredito - tarjeta.Balance;
//                //Actualizar tarjeta y gasto
//                await conexion.UpdateAsync(gasto);
//                return await conexion.UpdateAsync(tarjeta);
//            }
//            await _repositorioGasto.AgregarAsync(gasto);
//        }
//        catch (ExcepcionDescripcionInvalida ex)
//        {
//            validador.Errores["Descripcion"] = ex.Message;
//        }
//        catch (ExcepcionMontoRequerido ex)
//        {
//            validador.Errores["Monto"] = ex.Message;
//        }
//        catch (ExcepcionMontoNegativo ex)
//        {
//            validador.Errores["Monto"] = ex.Message;
//        }
//        catch (ExcepcionCategoriaInvalida ex)
//        {
//            validador.Errores["Categoria"] = ex.Message;
//        }
//        catch (ExcepcionComercioRequerido ex)
//        {
//            validador.Errores["Comercio"] = ex.Message;
//        }
//        catch (ExcepcionFechaInvalida ex)
//        {
//            validador.Errores["Fecha"] = ex.Message;
//        }
//        catch (ExcepcionEstadoInvalido ex)
//        {
//            validador.Errores["Estado"] = ex.Message;
//        }
//        catch (ExcepcionNombreImagenRequerido ex)
//        {
//            validador.Errores["NombreImagen"] = ex.Message;
//        }
//        catch (ExcepcionDominio ex)
//        {
//            throw new Exception("Ocurrio un erro en CrearGastoCasoUso" + ex.Message);
//        }
//        return validador;
//    }
//    #endregion
//}
