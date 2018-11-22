using System;
using clienteDomotica.Droid;
using SQLite.Net.Interop;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(ConfigSQLite))]
namespace clienteDomotica.Droid
{
    public class ConfigSQLite : Interfaces.IconfigSQL
    {
        private string directorioDB;
        private ISQLitePlatform plataforma;
        public ConfigSQLite()
        {
        }

        public string DirectorioDB
        {
            get
            {
                if (string.IsNullOrEmpty(directorioDB))
                {
                    directorioDB = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                }
                return directorioDB;
            }
        }

        public ISQLitePlatform Plataforma
        {
            get
            {
                if (plataforma == null)
                {
                    plataforma = new SQLite.Net.Platform.XamarinAndroid.SQLitePlatformAndroid();
                }
                return plataforma;
            }
        }
    }
}
