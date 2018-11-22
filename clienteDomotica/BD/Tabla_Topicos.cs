using System;
using SQLite.Net.Attributes;

namespace clienteDomotica.BD
{
    public class Tabla_Topicos
    {
        [PrimaryKey, NotNull, Unique]
        public int id_topicos { get; set; }
        public String topico { get; set; }
        public String fecha_creacion { get; set; }
        public int dispositivos_fk { get; set; }
    }
}
