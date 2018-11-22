using System;
using System.Linq;

namespace clienteDomotica.Clases
{
    public class captcha
    {
        static char[] az = Enumerable.Range('A', 'Z' - 'A' + 1).Select(i => (Char)i).ToArray();

        static Random random = new Random();

        static Boolean lleno;

        static int vuelta;

        public captcha()
        {
        }

        public static String generarCaptcha()
        {
            //** Reiniciar variables
            String request = "";
            lleno = false;
            vuelta = 0;
            String[] array = { "", "", "", "" };

            //**Generar letras aleatorias
            int letra1 = random.Next(0, az.Length - 1);
            int letra2 = random.Next(0, az.Length - 1);

            //**Generar numeros aleatorios
            int numero1 = random.Next(0, 9);
            int numero2 = random.Next(0, 9);

            //**Colocar los datos en alguna posicion
            while (!lleno)
            {

                //** Si el arreglo no tiene valor asignar uno nuevo
                int posicioninsertar = random.Next(0, array.Length);
                if (array[posicioninsertar].Equals(""))
                {
                    //System.Diagnostics.Debug.WriteLine("Insertando informacion " + posicioninsertar);

                    switch (vuelta)
                    {
                        case 0:
                            array[posicioninsertar] = "" + az[letra1];
                            vuelta++;
                            break;
                        case 1:
                            array[posicioninsertar] = "" + az[letra2];
                            vuelta++;
                            break;
                        case 2:
                            array[posicioninsertar] = "" + numero1;
                            vuelta++;
                            break;
                        case 3:
                            array[posicioninsertar] = "" + numero2;
                            vuelta++;
                            break;
                    }
                }

                //** Validar si el arreglo ya esta poblado en su totalidad
                int contar = 0;
                foreach (var c in array)
                {
                    if (!c.Equals(""))
                    {
                        contar++;
                    }
                }

                //System.Diagnostics.Debug.WriteLine("Contar " + contar);

                //**finalizar el recorrido en caso de que este lleno
                if (contar == 4)
                {
                    lleno = true;

                    foreach (var r in array)
                    {
                       // System.Diagnostics.Debug.WriteLine("Datos " + r);
                        request += r;
                    }
                }

            }


            return request;
        }
    }
}
