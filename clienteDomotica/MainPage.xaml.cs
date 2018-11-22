using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace clienteDomotica
{
    public partial class MainPage : MasterDetailPage
    {
        public static int idInmueble { get; set; }
        public static String dnsInmueble { get; set; }

        public List<MasterPageItem> menuList
        {
            get;
            set;
        }

        public MainPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);

            System.Diagnostics.Debug.WriteLine("Id inmueble "+ idInmueble);

            menuList = new List<MasterPageItem>();
            menuList.Add(new MasterPageItem()
            {
                Title = "Habitaciones",
                Icon = "house.png",
                TargetType = typeof(Vistas.habitaciones)
            });
            menuList.Add(new MasterPageItem()
            {
                Title = "Cambiar inmueble",
                Icon = "change.png",
                TargetType = typeof(Vistas.seleccionarInmueble)
            });
            menuList.Add(new MasterPageItem()
            {
                Title = "Configuracion",
                Icon = "settings.png",
                TargetType = typeof(Vistas.configuracion)
            });
            navigationDrawerList.ItemsSource = menuList;
            Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(Vistas.habitaciones)));
        }

      
        private void OnMenuItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

            var item = e.SelectedItem as MasterPageItem;
            if (item != null)
            {
                Detail = new NavigationPage((Page)Activator.CreateInstance(item.TargetType));
                navigationDrawerList.SelectedItem = null;
                IsPresented = false;
            }

        }
    }
}
