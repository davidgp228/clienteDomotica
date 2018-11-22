using System;
using System.Collections.Generic;
using System.IO;
using Xamarin.Forms;
using clienteDomotica.Clases;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace clienteDomotica.Vistas
{
    public partial class login : ContentPage
    {
        private string url;
        private HttpClient _Client = new HttpClient();

        public login()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);

            //** Init comprobar actualizaciones y validar si la base esta vacia
            lblcaptcha.Text = captcha.generarCaptcha();

            //**Validaciones
            etUsuario.MaxLength = 10;

            etContrasena.MaxLength = 10;
            etContrasena.IsPassword = true;

            etcaptcha.MaxLength = 4;
            
        }

        /// <summary>
        /// El usuario ingresa por primera vez a esta aplicacion, se valida mediante la conexion de un REST que el usaurio y contraseña
        /// existan en la bse de datos del administrador, el metodo valida la insercion de caracteres correctos, si el usario existe 
        /// se consulta un REST para descargar toda la informacion que se dio de alta en el servidor.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        async void  Ingresar(object sender, System.EventArgs e)
        {
            //Validacion de la informacion
            String mensajeUsuario =   validacionesEntry.validarCadena(etUsuario.Text, 0);
            String mensajeContrasena= validacionesEntry.validarCadena(etContrasena.Text, 0);
            String mensajeCaptcha=    validacionesEntry.validarCadena(etcaptcha.Text, 4);

            if (!mensajeUsuario.Equals("OK")){
                await DisplayAlert(variblesGlobales.tituloAlertaNotificacion, "Usuario " + mensajeUsuario, "Aceptar");
            }
            else if(!mensajeContrasena.Equals("OK")){
                await DisplayAlert(variblesGlobales.tituloAlertaNotificacion, "Contraseña " + mensajeContrasena, "Aceptar");
            }
            else if (!mensajeCaptcha.Equals("OK"))
            {
                await DisplayAlert(variblesGlobales.tituloAlertaNotificacion, "Captcha " + mensajeCaptcha, "Aceptar");
            }
            else if(!etcaptcha.Text.Equals(lblcaptcha.Text.ToString()))
            {
                lblcaptcha.Text = captcha.generarCaptcha();
                etcaptcha.Text = "";
                await DisplayAlert(variblesGlobales.tituloAlertaNotificacion, "Captcha texto incorrecto", "Aceptar");
            }
            else
            {
                //**Descargamos informacion del usuario
                validarUsuario(etUsuario.Text, etContrasena.Text);

            }

        }

        /// <summary>
        /// Validars the usuario. compara que en la base de datos exita el usario y contraseña ingresados, si existe retorna
        /// un valor true si no existe se retorna un false
        /// </summary>
        /// <returns>void</returns>
        /// <param name="usuario">Usuario.</param>
        /// <param name="contrasena">Contrasena.</param>
        public async void validarUsuario(String usuario, String contrasena){

            url = "http://albadti2018.ddns.net/domoticaRest/domoticaRest/restapi/login?usuario="+usuario+"&contrasena="+contrasena;

            //**Va consulta a REST
            // _Client.Timeout = TimeSpan.FromMilliseconds(3000);
            var content = await _Client.GetStringAsync(url);
            var resttLogin = JsonConvert.DeserializeObject<Interfaces.objLogin>(content);
            // _post = new ObservableCollection<BD.Tabla_Habitaciones>(post);
            
            if(resttLogin.login.Equals("-1"))
            {

                await DisplayAlert(variblesGlobales.tituloAlertaError, "Error al iniciar sesion", "Aceptar");
                limpiarCampos();

            }
            else if(resttLogin.login.Equals("0"))
            {

                await DisplayAlert(variblesGlobales.tituloAlertaError, "Usuario o contraseña incorrectos", "Aceptar");
                limpiarCampos();

            }
            else
            {
                //**Insertar usuario en la base de datos
                using (var accesoBD = new BD.Acceso_BD())
                {
                    BD.Tabla_Usuario usuarioInsertar = new BD.Tabla_Usuario(){
                        idUsuario= int.Parse(resttLogin.login),
                        ultimoInmueble=0,
                        contratoFk="",
                        fechaRegistro=""
                    };

                    accesoBD.insertarUsuario(usuarioInsertar);

                    System.Diagnostics.Debug.WriteLine("User insert OK");

                    descargarInformacion(usuario, contrasena);
                }
              
            }


        }

        /// <summary>
        /// Descargars the informacion. 
        /// </summary>
        public async void descargarInformacion(String token1, String token2){

            //**Se descarga la informacion mediante REST y se almacena en la base de datos local
            using (var accesoBD = new BD.Acceso_BD())
            {
                //**Descargamos la lista de inmuebles he insertamos en la base de datos
                url = "http://albadti2018.ddns.net/domoticaRest/domoticaRest/restapi/inmuebles?token1=" + token1 + "&token2=" + token2;
                var content2 = await _Client.GetStringAsync(url);
                var resttInmuebles = JsonConvert.DeserializeObject<List<BD.Tabla_Inmuebles>>(content2);

                if (resttInmuebles.Count > 0)
                {
                    //**Insertar informacion en la base de datos

                    foreach (BD.Tabla_Inmuebles inmueble in resttInmuebles)
                    {
                        accesoBD.insertarInmueble(inmueble);
                    }
                    System.Diagnostics.Debug.WriteLine("Inmuebles insert OK");

                }
                else
                {
                    await DisplayAlert(variblesGlobales.tituloAlertaError, "No se detectaron inmuebles \n Comunicarse con el administrador", "Aceptar"); ;
                    return;
                }
                //**Finaliza consulta inmuebles

                //***Consultar todas las habitaciones he insertar en la base de datos
                url = "http://albadti2018.ddns.net/domoticaRest/domoticaRest/restapi/habitaciones?token1=" + token1 + "&token2=" + token2;
                var contentHabitaciones = await _Client.GetStringAsync(url);
                var restHabitacion = JsonConvert.DeserializeObject<List<BD.Tabla_Habitaciones>>(contentHabitaciones);
                System.Diagnostics.Debug.WriteLine("Content habitacion "+ contentHabitaciones);
                if (restHabitacion.Count > 0)
                {
                    foreach (BD.Tabla_Habitaciones habitacion in restHabitacion)
                        accesoBD.insertarHabitacion(habitacion);

                    System.Diagnostics.Debug.WriteLine("Habitacion insert OK ");
                }
                //**Finaliza consulta de habitaciones

                //***Consultar todas los dispositivos he insertar en la base de datos
                url = "http://albadti2018.ddns.net/domoticaRest/domoticaRest/restapi/dispositivos?token1=" + token1 + "&token2=" + token2;
                var contentDispositivos = await _Client.GetStringAsync(url);
                var restDispositivo = JsonConvert.DeserializeObject<List<BD.Tabla_Dispositivos>>(contentDispositivos);

                if (restDispositivo.Count > 0)
                {
                    foreach (BD.Tabla_Dispositivos dispositivo in restDispositivo)
                        accesoBD.insertarDispositivo(dispositivo);

                    System.Diagnostics.Debug.WriteLine("Dispositivo insert OK ");
                }
                //**Finaliza consulta de dispositivos

                //***Consultar todas los topicos he insertar en la base de datos
                url = "http://albadti2018.ddns.net/domoticaRest/domoticaRest/restapi/topicos?token1=" + token1 + "&token2=" + token2;
                var contentTopicos = await _Client.GetStringAsync(url);
                var restTopicos = JsonConvert.DeserializeObject<List<BD.Tabla_Topicos>>(contentTopicos);

                if (restTopicos.Count > 0)
                {
                    foreach (BD.Tabla_Topicos topico in restTopicos)
                        accesoBD.insertarTopico(topico);

                    System.Diagnostics.Debug.WriteLine("Topico insert OK ");
                }
                //**Finaliza consulta de topicos

            }

            await ((NavigationPage)this.Parent).PushAsync(new seleccionarInmueble());
            Navigation.RemovePage(this);
        }


        public void limpiarCampos(){
            lblcaptcha.Text = captcha.generarCaptcha();
            etcaptcha.Text = "";
            etUsuario.Text = "";
            etContrasena.Text = "";
        }

    }
}
