using GastoClass.Dominio.ValueObjects.ValueObjectsGasto;

namespace GastoClass.Dominio.Entidades;

public class Gasto
{
    public int Id { get; private set; }
    public Descripcion Descripcion { get; }
    public Monto Monto { get; }
    public Categoria Categoria { get; }
    public Comercio? Comercio { get; }
    public Fecha Fecha { get; }
    public Estado Estado { get; }
    public NombreImagen? NombreImagen { get; }
    public int TarjetaId { get; }
    public Gasto(
        Descripcion descripcion,
        Monto monto, 
        Categoria categoria, 
        Comercio? comercio, 
        Fecha fecha, 
        Estado estado, 
        NombreImagen nombreImagen, 
        int tarjetaId)
    {
        Descripcion = descripcion;
        Monto = monto;
        Categoria = categoria;
        Comercio = comercio;
        Fecha = fecha;
        Estado = estado;
        NombreImagen = nombreImagen;
        TarjetaId = tarjetaId;
    }
    public void SetId(int id) => Id = id;

    public static Gasto Crear(
        string? descripcion,
        decimal monto,
        string? categoria,
        string? comercio,
        DateTime fecha,
        string? estado,
        int tarjetaId,
        string? nombreImagen)
    {
        return new Gasto(
            descripcion: new Descripcion(descripcion!),
            monto: new Monto(monto),
            categoria: new Categoria(categoria),
            comercio: new Comercio(comercio!),
            fecha: new Fecha(fecha),
            estado: new Estado(estado!),
            nombreImagen: new NombreImagen(nombreImagen),
            tarjetaId: tarjetaId
        );
    }
}
