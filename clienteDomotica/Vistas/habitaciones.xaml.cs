using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace clienteDomotica.Vistas
{
    public partial class habitaciones : ContentPage
    {

        public habitaciones()
        {
            InitializeComponent();

            cargarHabitaciones();       

        }

        AbsoluteLayout absolutePrincipal, absoluteControles;
        StackLayout stackControles;
        Grid gridControles;
        Image imagenFondo;
        Label nombre;
        Button btnPerzonalizar;

        private void cargarHabitaciones(){

            using (var accesoBD = new BD.Acceso_BD())
            {

                var habitaciones = accesoBD.obtenerHabitacion(MainPage.idInmueble);
                System.Diagnostics.Debug.WriteLine("Id inmueble " + MainPage.idInmueble+" Cuenta "+ habitaciones.Count);
                if (habitaciones.Count > 0)
                {
                    for (int i = 0; i < habitaciones.Count; i++)
                    {

                        absolutePrincipal = new AbsoluteLayout();

                        imagenFondo = new Image
                        {
                            Source = "habitacionb.jpg",
                            Aspect = Aspect.AspectFill,
                            HeightRequest = 160
                        };

                        AbsoluteLayout.SetLayoutBounds(imagenFondo, new Rectangle(1, 1, 1, 1));
                        AbsoluteLayout.SetLayoutFlags(imagenFondo, AbsoluteLayoutFlags.All);

                        absoluteControles = new AbsoluteLayout
                        {
                            BackgroundColor = Color.FromHex("#50000000"),
                            // Margin = new Thickness(10, 10, 10, 10)
                        };
                        AbsoluteLayout.SetLayoutBounds(absoluteControles, new Rectangle(1, 1, 1, 1));
                        AbsoluteLayout.SetLayoutFlags(absoluteControles, AbsoluteLayoutFlags.All);

                        stackControles = new Interfaces.Habitaciones.stackLayouHabitacion
                        {
                            idHabitacion= habitaciones[i].id_area,
                            nombreHabitacion= habitaciones[i].area,
                            Orientation = StackOrientation.Vertical,
                            BackgroundColor = Color.Transparent
                        };
                        AbsoluteLayout.SetLayoutBounds(stackControles, new Rectangle(1, 1, 1, 1));
                        AbsoluteLayout.SetLayoutFlags(stackControles, AbsoluteLayoutFlags.All);
                        TapGestureRecognizer tapEvent = new TapGestureRecognizer();
                        tapEvent.Tapped += seleccionarHabitacion;
                        stackControles.GestureRecognizers.Add(tapEvent);

                        gridControles = new Grid();
                        gridControles.Margin = new Thickness(5, 5, 5, 5);
                        gridControles.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
                        gridControles.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
                        //gridControles.ColumnDefinitions.Add(new ColumnDefinition{Width= new GridLength(1, GridUnitType.Auto)});
                        //gridControles.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });

                        nombre = new Label
                        {
                            Text = habitaciones[i].area,
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
                        btnPerzonalizar.Clicked += editarHabitacion;

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
        }

        public async void seleccionarHabitacion(object sender, EventArgs e)
        {
            Interfaces.Habitaciones.stackLayouHabitacion item = (Interfaces.Habitaciones.stackLayouHabitacion)sender;

            System.Diagnostics.Debug.WriteLine("fkHabitacion "+ item.idHabitacion);

            cargarDispositivos.fkHabitacion = item.idHabitacion;
            cargarDispositivos.nombreHabitacion = item.nombreHabitacion;
            await ((NavigationPage)this.Parent).PushAsync(new cargarDispositivos());
            Navigation.RemovePage(this);
        }

        public async void editarHabitacion(object sender, EventArgs e)
        {
            await ((NavigationPage)this.Parent).PushAsync(new habitacionEditar());
            Navigation.RemovePage(this);
        }
    }
}
