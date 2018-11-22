using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace clienteDomotica
{
    public partial class App : Application
    {
        List<BD.Tabla_Usuario> usuario;


        public App()
        {
            InitializeComponent();

            try
            {
                using (var accesoBD = new BD.Acceso_BD())
                {
                    usuario = accesoBD.obtenerUsuarios();
                    
                    if (usuario.Count > 0)
                    {

                        //**Si ya se guardo un inmueble mandar hasta el menu principal
                        if (usuario[0].ultimoInmueble > 0)
                        {
                            MainPage = new NavigationPage(new MainPage());
                        }
                        //** Si no mandar a la pantalla para seleccionar el inmueble
                        else
                        {
                            MainPage = new NavigationPage(new Vistas.seleccionarInmueble());
                        }
                    }
                    else
                    {
                        MainPage = new NavigationPage(new Vistas.login());
                    }
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("validarInformacion error " + e + " Link " + e.HelpLink);
                e.GetBaseException();
            }

        }

      


        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
