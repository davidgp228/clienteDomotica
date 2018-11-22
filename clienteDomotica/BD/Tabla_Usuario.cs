using System;
using SQLite.Net.Attributes;

namespace clienteDomotica.BD
{
    public class Tabla_Usuario
    {
        [PrimaryKey, AutoIncrement,NotNull, Unique]
        public int idUsuario { get; set; }
        public int ultimoInmueble { get; set; }
        public String contratoFk { get; set; }
        public String fechaRegistro { get; set; }
    }
}
