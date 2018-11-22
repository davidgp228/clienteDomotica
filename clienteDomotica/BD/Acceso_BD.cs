using System;
using System.Collections.Generic;
using SQLite.Net;
using Xamarin.Forms;

namespace clienteDomotica.BD
{
    public class Acceso_BD : IDisposable
    {
        private SQLiteConnection connection;

        public Acceso_BD()
        {
            var config = DependencyService.Get<Interfaces.IconfigSQL>();
            connection = new SQLiteConnection(config.Plataforma, System.IO.Path.Combine(config.DirectorioDB, "domoticabd.db3"));
            connection.CreateTable<Tabla_Usuario>();
            connection.CreateTable<Tabla_Inmuebles>();
            connection.CreateTable<Tabla_Habitaciones>();
            connection.CreateTable<Tabla_Dispositivos>();
            connection.CreateTable<Tabla_Topicos>();
        }
        //**Usuario>>>>>
        public List<Tabla_Usuario> obtenerUsuarios()
        {
            return connection.Query<Tabla_Usuario>("SELECT * FROM Tabla_Usuario ");
        }

        public void insertarUsuario(Tabla_Usuario usuario)
        {
            connection.Insert(usuario);
        }

        //**Inmueble>>>>
        public void insertarInmueble(Tabla_Inmuebles inmuebles)
        {
            connection.Insert(inmuebles);
        }

        public List<Tabla_Inmuebles> obtenerInmuebles()
        {
            return connection.Query<Tabla_Inmuebles>("SELECT * FROM Tabla_Inmuebles ");
        }


        public void actualizarInmueble(Tabla_Inmuebles inmuebles)
        {
            connection.Update(inmuebles);
        }

        public void eliminarInmuebles()
        {
            connection.Execute("DELETE FROM Tabla_Inmuebles" );
        }

        //***Habitacion>>>>>>
        public List<Tabla_Habitaciones> obtenerHabitacion(int fkinmueble)
        {
            return connection.Query<Tabla_Habitaciones>("SELECT * FROM Tabla_Habitaciones where inmueble_fk=" + fkinmueble);
        }

        public void insertarHabitacion(Tabla_Habitaciones habitacion)
        {
            connection.Insert(habitacion);
        }

        //**Dispositivo>>>>>>
        public List<Tabla_Dispositivos> obtenerDispositivos(int fkhabitacion)
        {
            return connection.Query<Tabla_Dispositivos>("SELECT * FROM Tabla_Dispositivos where area_fk=" + fkhabitacion);
        }

        public void insertarDispositivo(Tabla_Dispositivos dispositivo)
        {
            connection.Insert(dispositivo);
        }

        //**Topico>>>>>>
        public List<Tabla_Topicos> obtenerTopicos(int fkdispositivo)
        {
            return connection.Query<Tabla_Topicos>("SELECT * FROM Tabla_Topicos where dispositivos_fk=" + fkdispositivo);
        }

        public void insertarTopico(Tabla_Topicos topico)
        {
            connection.Insert(topico);
        }


        public void Dispose()
        {
            connection.Dispose();
        }
    }
}
