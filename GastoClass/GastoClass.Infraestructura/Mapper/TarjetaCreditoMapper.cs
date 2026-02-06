using GastoClass.Dominio.Entidades;
using Infraestructura.Mapper;
using Infraestructura.Persistencia.Entidades;

namespace GastoClass.GastoClass.Infraestructura.Mapper;

public static class TarjetaCreditoMapper
{
    public static TarjetaCreditoEntidad ToEntidad(TarjetaCreditoDominio dominio)
    {
        if (dominio == null)
            throw new ArgumentNullException(nameof(dominio));

        return new TarjetaCreditoEntidad
        {
            Id = dominio.Id,
            TipoTarjeta = dominio.Tipo.Valor,
            NombreTarjeta = dominio.NombreTarjeta.Valor,
            UltimosCuatroDigitos = dominio.UltimosCuatroDigitos.Valor,
            MesVencimiento = dominio.MesVencimiento.Mes,
            AnioVencimiento = dominio.AnioVencimiento.Anio,
            LimiteCredito = dominio.LimiteCredito.Valor,
            Balance = dominio.Balance,
            CreditoDisponible = dominio.CreditoDisponible,
            Moneda = dominio.TipoMoneda.Valor,
            DiaCorte = dominio.DiaCorte.Dia,
            DiaPago = dominio.DiaPago.Dia,
            NombreBanco = dominio.NombreBanco.Valor,
            IdPreferenciaTarjeta = dominio.Preferencia.Id
        };
    }
    public static TarjetaCreditoEntidad ToEntidadConPreferencia(TarjetaCreditoDominio dominio, PreferenciaTarjetaDominio preferenciaTarjetaDominio)
    {
        if (dominio == null)
            throw new ArgumentNullException(nameof(dominio));
        if (preferenciaTarjetaDominio == null)
            throw new ArgumentNullException(nameof(preferenciaTarjetaDominio));
        return new TarjetaCreditoEntidad
        {
            Id = dominio.Id,
            TipoTarjeta = dominio.Tipo.Valor,
            NombreTarjeta = dominio.NombreTarjeta.Valor,
            UltimosCuatroDigitos = dominio.UltimosCuatroDigitos.Valor,
            MesVencimiento = dominio.MesVencimiento.Mes,
            AnioVencimiento = dominio.AnioVencimiento.Anio,
            LimiteCredito = dominio.LimiteCredito.Valor,
            Balance = dominio.Balance,
            CreditoDisponible = dominio.CreditoDisponible,
            Moneda = dominio.TipoMoneda.Valor,
            DiaCorte = dominio.DiaCorte.Dia,
            DiaPago = dominio.DiaPago.Dia,
            NombreBanco = dominio.NombreBanco.Valor,
            IdPreferenciaTarjeta = preferenciaTarjetaDominio.Id,
            PreferenciaTarjeta = preferenciaTarjetaDominio
        };
    }
    public static TarjetaCreditoDominio ToDominio(this TarjetaCreditoEntidad entidad)
    {
        if (entidad == null)
            throw new ArgumentNullException(nameof(entidad));

        var tarjeta = TarjetaCreditoDominio.Crear(
            tipo: entidad.TipoTarjeta!,
            nombre: entidad.NombreTarjeta!,
            ultimosCuatroDigitos: entidad.UltimosCuatroDigitos!.Value,
            mesVencimiento: entidad.MesVencimiento!.Value,
            anioVencimiento: entidad.AnioVencimiento!.Value,
            tipoMoneda: entidad.Moneda!,
            limiteCredito: entidad.LimiteCredito!.Value,
            diaCorte: entidad.DiaCorte!.Value,
            diaPago: entidad.DiaPago!.Value,
            nombreBanco: entidad.NombreBanco!,
            preferencia: entidad.PreferenciaTarjeta!
        );

        tarjeta.SetId(entidad.Id);
        tarjeta.SetBalance(entidad.Balance!.Value);
        tarjeta.SetCreditoDisponible(entidad.CreditoDisponible!.Value);

        return tarjeta;
    }
    public static TarjetaCreditoDominio ToDominio(this TarjetaCreditoEntidad entidad,PreferenciaTarjetaDominio preferencia)
    {
        if (entidad == null)
            throw new ArgumentNullException(nameof(entidad));
        if (preferencia == null)
            throw new ArgumentNullException(nameof(preferencia));

        var tarjeta = TarjetaCreditoDominio.Crear(
            tipo: entidad.TipoTarjeta!,
            nombre: entidad.NombreTarjeta!,
            ultimosCuatroDigitos: entidad.UltimosCuatroDigitos!.Value,
            mesVencimiento: entidad.MesVencimiento!.Value,
            anioVencimiento: entidad.AnioVencimiento!.Value,
            tipoMoneda: entidad.Moneda!,
            limiteCredito: entidad.LimiteCredito!.Value,
            diaCorte: entidad.DiaCorte!.Value,
            diaPago: entidad.DiaPago!.Value,
            nombreBanco: entidad.NombreBanco!,
            preferencia: preferencia
        );

        tarjeta.SetId(entidad.Id);
        tarjeta.SetBalance(entidad.Balance!.Value);
        tarjeta.SetCreditoDisponible(entidad.CreditoDisponible!.Value);

        return tarjeta;
    }
    public static TarjetaCreditoDominio ToDominioConPreferencia(this TarjetaCreditoEntidad entidad)
    {
        if (entidad == null)
            throw new ArgumentNullException(nameof(entidad));
        if (entidad.PreferenciaTarjeta == null)
            throw new InvalidOperationException("La preferencia debe estar cargada antes de convertir a dominio");

        var preferenciaDominio = entidad.PreferenciaTarjeta.ToEntidad().ToDominio();

        return entidad.ToDominio(preferenciaDominio);
    }
    public static List<TarjetaCreditoDominio> ToDominio(this IEnumerable<TarjetaCreditoEntidad> entidades,
        Func<int, PreferenciaTarjetaDominio> obtenerPreferencia)
    {
        if (entidades == null)
            throw new ArgumentNullException(nameof(entidades));
        if (obtenerPreferencia == null)
            throw new ArgumentNullException(nameof(obtenerPreferencia));

        return entidades
            .Select(e => e.ToDominio(obtenerPreferencia(e.IdPreferenciaTarjeta!.Value)))
            .ToList();
    }
    public static List<TarjetaCreditoEntidad> ToEntidad(this IEnumerable<TarjetaCreditoDominio> dominios)
    {
        if (dominios == null)
            throw new ArgumentNullException(nameof(dominios));
        List<TarjetaCreditoEntidad> entidades = new();
        foreach (var dominio in dominios)
        {
            entidades.Add(TarjetaCreditoMapper.ToEntidad(dominio));
        }
        return entidades;
    }

}
