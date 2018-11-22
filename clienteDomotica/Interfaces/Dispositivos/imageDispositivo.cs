using System;
using Xamarin.Forms;

namespace clienteDomotica.Interfaces.Dispositivos
{
    public class imageDispositivo : Image
    {
     
            public int idDispositivo { get; set; }
            public String status { get; set; }
            public String topico { get; set; }
    
    }
}
