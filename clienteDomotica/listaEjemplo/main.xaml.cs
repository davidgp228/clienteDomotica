using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace clienteDomotica.listaEjemplo
{
    public partial class main : ContentPage
    {
        public main()
        {
            InitializeComponent();
            this.BindingContext = new UserViewModel();
        }
    }
}
