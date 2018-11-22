using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace clienteDomotica.Vistas
{
    public partial class cargarDispositivos : ContentPage
    {

        public static int fkHabitacion { get; set; }
        public static String nombreHabitacion{ get; set; }

        public cargarDispositivos()
        {
            InitializeComponent();
            var settings = new ToolbarItem
            {
                Icon = "back.png",
                Text = "Atras",
                Command = new Command(this.atras)
            };
            this.ToolbarItems.Add(settings);

            //***Adinar nombe a la habitacion
            lblNombreHabitacion.Text = nombreHabitacion;

            cargarDispositivos1();
        }

        private async void atras(object obj)
        {
            await((NavigationPage)this.Parent).PushAsync(new habitaciones());
            Navigation.RemovePage(this);
        }

        int contador = 0, indice=0;
        StackLayout contenedor;
        Frame frame;
        StackLayout opciones;
        Image imagen;
        Label texto;

        private void cargarDispositivos1()
        {
            using (var accesoBD = new BD.Acceso_BD())
            {
                //Asignamos nombre a la habitacion

                var dispositivos = accesoBD.obtenerDispositivos(fkHabitacion);

                System.Diagnostics.Debug.WriteLine("numero de dispositivo "+ dispositivos.Count);

                //**Si es mayor a 1 dividir entre dos el recorrido
                if (dispositivos.Count > 1)
                    contador = dispositivos.Count / 2;
                else
                    contador = dispositivos.Count;

                for (int i = 0; i < contador; i++)
                {

                    contenedor = new StackLayout
                    {
                        Orientation = StackOrientation.Horizontal,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        HeightRequest = 120
                    };

                    for (int j = indice; j < dispositivos.Count; j++)
                    {

                        frame = new Frame();
                        frame.CornerRadius = 10;
                        frame.BackgroundColor = Color.FromRgba(255, 255, 255, 0.05);
                        frame.HorizontalOptions = LayoutOptions.FillAndExpand;

                        opciones = new StackLayout();

                        //**Consultar topico de cada dispositivo
                        var topico = accesoBD.obtenerTopicos(dispositivos[j].id_dispositivo);

                        imagen = new Interfaces.Dispositivos.imageDispositivo
                        {
                            status = "0",
                            topico = topico[0].topico,
                            Source = "focob.png",
                            WidthRequest = 60,
                            HeightRequest = 60,
                            Margin = new Thickness(0, 5, 0, 0),
                        };
                        TapGestureRecognizer tapEvent = new TapGestureRecognizer();
                        tapEvent.Tapped += foco;
                        imagen.GestureRecognizers.Add(tapEvent);
                       
                        texto = new Label
                        {
                            Text = dispositivos[j].nombre,
                            TextColor = Color.White,
                            HorizontalTextAlignment = TextAlignment.Center,
                            FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label))
                        };
                        opciones.Children.Add(imagen);
                        opciones.Children.Add(texto);

                        frame.Content = opciones;

                        contenedor.Children.Add(frame);

                        indice++;
                    }

                    System.Diagnostics.Debug.WriteLine("Linea +" + i);
                    contenedorDispositivos.Children.Add(contenedor);
                }
            }
        }

        public void foco(object sender, EventArgs e)
        {
            Interfaces.Dispositivos.imageDispositivo item = (Interfaces.Dispositivos.imageDispositivo)sender;

            System.Diagnostics.Debug.WriteLine("DNS "+ MainPage.dnsInmueble);

            if (item.status.Equals("0"))
            {
                item.status = "1";
                item.Source = "focoencendido.png";
				DependencyService.Get<sendmqtt>().enviarmqtt("192.168.1.68", item.topico, "1");
                System.Diagnostics.Debug.WriteLine("ON ");
            }
            else
            {
                item.status = "0";
                item.Source = "focob.png";
                DependencyService.Get<sendmqtt>().enviarmqtt("192.168.1.68", item.topico, "0");
                System.Diagnostics.Debug.WriteLine("OFF ");
            }

        }
    }
}
