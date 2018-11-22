using System;
using SQLite.Net.Attributes;

namespace clienteDomotica.BD
{
    public class Tabla_Habitaciones
    {
        [PrimaryKey, NotNull, Unique]
        public int id_area { get; set; }
        public String area { get; set; }
        public String imagen { get; set; }
        public String num_focos { get; set; }
        public int inmueble_fk { get; set; }
    }
}