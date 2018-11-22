using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace clienteDomotica.Vistas
{
    public partial class habitacionEditar : ContentPage
    {
    
        public habitacionEditar()
        {
            InitializeComponent();

            var settings = new ToolbarItem
            {
                Icon = "back.png",
                Text = "Settting",
                Command = new Command(this.atras)
            };
            this.ToolbarItems.Add(settings);

            TapGestureRecognizer tapEvent1 = new TapGestureRecognizer();
            tapEvent1.Tapped += CameraButton_Clicked;
            imgCamara.GestureRecognizers.Add(tapEvent1);

            TapGestureRecognizer tapEvent2 = new TapGestureRecognizer();
            tapEvent2.Tapped += GalleryButton_Clicked;
            imgGaleria.GestureRecognizers.Add(tapEvent2);
        }


        private async void GalleryButton_Clicked(object sender, EventArgs e)
        {

            if (!Plugin.Media.CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("Galeria no soportado", ":( No hay permiso para acceder a la galeria.", "ACEPTAR");
                return;
            }

            var photo = await Plugin.Media.CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
            {
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Custom,
                CustomPhotoSize = 50 //Resize to 20% of original
            });

            if (photo != null)
            {
                PhotoImage.Source = ImageSource.FromStream(() => { return photo.GetStream(); });
              //  imagenbd = Convert.ToBase64String(ReadFully(photo.GetStream()));
            }



            //***Decodificar base64 a String y almacenar en byte para mostrar
            //   byte[] newBytes = Convert.FromBase64String(imagenbd);

            // PhotoImage.Source = ImageSource.FromStream(() => new MemoryStream(newBytes));

        }

        private async void CameraButton_Clicked(object sender, EventArgs e)
        {

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Camera", ":( No camera available.", "OK");
                return;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                Directory = "Test",
                SaveToAlbum = true,
                CompressionQuality = 75,
                CustomPhotoSize = 50,
                PhotoSize = PhotoSize.MaxWidthHeight,
                MaxWidthHeight = 2000,
                DefaultCamera = CameraDevice.Front
            });

            if (file == null)
                return;

           // await  DisplayAlert("File Location", file.Path, "OK");

            PhotoImage.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                file.Dispose();
                return stream;
            });
        }

        private async void atras(object obj)
        {
            await ((NavigationPage)this.Parent).PushAsync(new habitaciones());
            Navigation.RemovePage(this);
        }
    }
}
