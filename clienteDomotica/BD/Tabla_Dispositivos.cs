using System;
using SQLite.Net.Attributes;

namespace clienteDomotica.BD
{
    public class Tabla_Dispositivos
    {
        [PrimaryKey, NotNull, Unique]
        public int id_dispositivo { get; set; }
        public int area_fk { get; set; }
        public String nombre { get; set; }
        public String mac { get; set; }
        public String nserie { get; set; }
        public String alias { get; set; }
        public String estado { get; set; }
        public String t_uso { get; set; }
        public String vida { get; set; }
        public String fecha_creacion { get; set; }
    }
}
