namespace GastoClass.Infraestructura.Persistencia.ContextoDB;

public static class Constantes
{
    public const string NombreBaseDatos = "GastoClass.db3";

    public const SQLite.SQLiteOpenFlags Flags =
        //ABRIR LA BASE DE DATOS EN MODO LECTURA Y ESCRITURA
        SQLite.SQLiteOpenFlags.ReadWrite |
        //CREAR LA BASE DE DATOS SI NO EXISTE
        SQLite.SQLiteOpenFlags.Create |
        //PERMITIR EL USO DE LA BASE DE DATOS POR VARIOS PROCESOS
        SQLite.SQLiteOpenFlags.SharedCache;

    public static string RutaBaseDatos =>
        Path.Combine(FileSystem.AppDataDirectory, NombreBaseDatos);
}
