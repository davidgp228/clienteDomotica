using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace clienteDomotica.Vistas
{
    public partial class seleccionarInmueble : ContentPage
    {
        public seleccionarInmueble()
        {
            InitializeComponent();
            //NavigationPage.SetHasNavigationBar(this, false);

            cargarInmuebles();
        }

        Image imagenFondo;
        AbsoluteLayout absolutePrincipal, absoluteControles;
        StackLayout stackControles;
        Button btnPerzonalizar;
        Label nombre;
        Grid gridControles;

        TapGestureRecognizer tapEvent;

        public void cargarInmuebles(){
            using (var accesoBD = new BD.Acceso_BD())
            {
                var inmuebles = accesoBD.obtenerInmuebles();

                for (int i = 0; i < inmuebles.Count; i++)
                {

                    //**Contenedor principal de cada inmueble
                    absolutePrincipal = new AbsoluteLayout();

                    imagenFondo = new Image
                    {
                        Source = "empresa.jpg",
                        Aspect = Aspect.AspectFill,
                        HeightRequest = 160
                    };

                    AbsoluteLayout.SetLayoutBounds(imagenFondo, new Rectangle(1, 1, 1, 1));
                    AbsoluteLayout.SetLayoutFlags(imagenFondo, AbsoluteLayoutFlags.All);

                    //**Degradado de la interfaz
                    absoluteControles = new AbsoluteLayout
                    {
                        BackgroundColor = Color.FromHex("#50000000"),
                        // Margin = new Thickness(10, 10, 10, 10)
                    };
                    AbsoluteLayout.SetLayoutBounds(absoluteControles, new Rectangle(1, 1, 1, 1));
                    AbsoluteLayout.SetLayoutFlags(absoluteControles, AbsoluteLayoutFlags.All);

                    stackControles = new Interfaces.Inmuebles.stackLayoutInmueble
                    {
                        idInmueble= inmuebles[i].id_inmueble,
                        nombreInmueble= inmuebles[i].nombre_inmueble,
                        dnsInmueble= inmuebles[i].dns_inmueble,
                        Orientation = StackOrientation.Vertical,
                        BackgroundColor = Color.Transparent
                    };

                    AbsoluteLayout.SetLayoutBounds(stackControles, new Rectangle(1, 1, 1, 1));
                    AbsoluteLayout.SetLayoutFlags(stackControles, AbsoluteLayoutFlags.All);
                    tapEvent = new TapGestureRecognizer();
                    tapEvent.Tapped += seleccionarInmueble1;
                    stackControles.GestureRecognizers.Add(tapEvent);

                    gridControles = new Grid();
                    gridControles.Margin = new Thickness(5, 5, 5, 5);
                    gridControles.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
                    gridControles.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });

                    nombre = new Label
                    {
                        Text = inmuebles[i].nombre_inmueble,
                        TextColor = Color.White,
                        FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label))
                    };

                    btnPerzonalizar = new Button
                    {
                        
                        BackgroundColor = Color.Transparent,
                        Image = "perzonalizar.png",
                        WidthRequest = 40,
                        HeightRequest = 40
                    };
                   
                    btnPerzonalizar.Clicked += editarInmueble;

                    gridControles.Children.Add(nombre, 0, 0);
                    gridControles.Children.Add(btnPerzonalizar, 1, 0);

                    stackControles.Children.Add(gridControles);

                    absoluteControles.Children.Add(stackControles);

                    absolutePrincipal.Children.Add(imagenFondo);
                    absolutePrincipal.Children.Add(absoluteControles);

                    contenedor.Children.Add(absolutePrincipal);
                }
            }
        }

        public async void seleccionarInmueble1(object sender, EventArgs e)
        {
            Interfaces.Inmuebles.stackLayoutInmueble item = (Interfaces.Inmuebles.stackLayoutInmueble)sender;


            // var respuesta = await DisplayAlert("Exit", "¿Desea establecer este inmueble como predeterminado?", "Yes", "No");

            /*  if(respuesta){
                  System.Diagnostics.Debug.WriteLine("IdInmueble " + item.idInmueble);
              }else{
                  System.Diagnostics.Debug.WriteLine("" );
              }*/

             MainPage.idInmueble = item.idInmueble;
             MainPage.dnsInmueble = item.dnsInmueble;
             await ((NavigationPage)this.Parent).PushAsync(new MainPage());
             Navigation.RemovePage(this);

             NavigationPage.SetHasNavigationBar(this, false);
        }

        public async void editarInmueble(object sender, EventArgs e)
        {
            await ((NavigationPage)this.Parent).PushAsync(new InmuebleEditar());
            Navigation.RemovePage(this);
        }
    }
}
