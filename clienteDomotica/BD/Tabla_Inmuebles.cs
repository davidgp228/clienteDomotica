using System;
using SQLite.Net.Attributes;

namespace clienteDomotica.BD
{
    public class Tabla_Inmuebles
    {
        [PrimaryKey, NotNull, Unique]
        public int id_inmueble { get; set; }
        public String nombre_inmueble { get; set; }
        public String domicilio { get; set; }
        public String num_ext { get; set; }
        public String num_int { get; set; }
        public String colonia { get; set; }
        public String municipio { get; set; }
        public String ciudad { get; set; }
        public String cp { get; set; }
        public String telefono { get; set; }
        public String comp_domicilio { get; set; }
        public String fecha_comp { get; set; }
        public String coordenadas { get; set; }
        public String referencias { get; set; }
        public String fecha_instalacion { get; set; }
        public String fecha_registro { get; set; }
        public String dns_inmueble { get; set; }
        public String foto { get; set; }
        public int contrato_fk { get; set; }
        public int usuarios_fk { get; set; }
    }
}
