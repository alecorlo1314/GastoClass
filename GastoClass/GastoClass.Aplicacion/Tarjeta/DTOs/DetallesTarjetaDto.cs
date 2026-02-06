using GastoClass.Dominio.Entidades;

namespace GastoClass.Aplicacion.DTOs
{
    public class DetallesTarjetaDto
    {
        // Datos financieros
        public int IdTarjeta { get; set; }
        public string? TipoTarjeta { get; set; }
        public string? NombreTarjeta { get; set; }
        public int UltimosCuatroDigitos { get; set; }
        public int MesVencimiento { get; set; }
        public int AnioVencimiento { get; set; }
        public decimal LimiteCredito { get; set; }
        public decimal Balance { get; set; }
        public decimal CreditoDisponible { get; set; }
        public string? Moneda { get; set; }
        public int DiaCorte { get; set; }
        public int DiaPago { get; set; }
        public string? NombreBanco { get; set; }

        // Preferencias visuales
        public string? ColorHex1 { get; set; }
        public string? ColorHex2 { get; set; }
        public string? ColorBorde { get; set; }
        public string? ColorTexto { get; set; }
        public string? IconoTipoTarjeta { get; set; }
        public string? IconoChip { get; set; }

        // Método de fábrica para mapear desde entidades de dominio
        public static DetallesTarjetaDto DeEntidad(TarjetaCreditoDominio tarjeta, PreferenciaTarjetaDominio preferencia)
        {
            return new DetallesTarjetaDto
            {
                IdTarjeta = tarjeta.Id,
                TipoTarjeta = tarjeta.Tipo.Valor,
                NombreTarjeta = tarjeta.NombreTarjeta.Valor,
                UltimosCuatroDigitos = tarjeta.UltimosCuatroDigitos.Valor,
                MesVencimiento = tarjeta.MesVencimiento.Mes,
                AnioVencimiento = tarjeta.AnioVencimiento.Anio,
                LimiteCredito = tarjeta.LimiteCredito.Valor!,
                Balance = tarjeta.Balance,
                CreditoDisponible = tarjeta.CreditoDisponible,
                Moneda = tarjeta.TipoMoneda.Valor,
                DiaCorte = tarjeta.DiaCorte.Dia,
                DiaPago = tarjeta.DiaPago.Dia,
                NombreBanco = tarjeta.NombreBanco.Valor,

                ColorHex1 = preferencia.ColorHex1.Valor,
                ColorHex2 = preferencia.ColorHex2.Valor,
                ColorBorde = preferencia.ColorBorde.Valor,
                ColorTexto = preferencia.ColorTexto.Valor,
                IconoTipoTarjeta = preferencia.IconoTipoTarjeta.Valor,
                IconoChip = preferencia.IconoChip.Valor
            };
        }
    }
}
