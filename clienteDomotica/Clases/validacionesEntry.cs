using System;
using System.Text.RegularExpressions;

namespace clienteDomotica.Clases
{
    public class validacionesEntry
    {
        // static string caracteresEspeciales = @"^[^ ][a-zA-Z ]+[^ ]$";

        static string caracteresEspeciales = "^[A-Za-z0-9! _ - @ #]+$";

        public static String validarCadena(String cadena, int longitudCorrecta)
        {

            //Valida si el valor en el Entry se encuentra vacio o es igual a Null
            if (String.IsNullOrWhiteSpace(cadena))
            {
                return "campo vacio.";
            }

            //Valida que la cadena tenga la longitud correcta
            int longitud = cadena.Length;

            if (longitudCorrecta > 0) 
            { 
                if (longitud != longitudCorrecta)
                {
                    return "longitud invalida.";
                }
            }
            //Valida si se ingresan caracteres especiales
            bool resultado = Regex.IsMatch(cadena, caracteresEspeciales, RegexOptions.IgnoreCase);

            if (!resultado)
            {
                return "no se aceptan caracteres especiales, intente de nuevo.";
            }

            return "OK";
        }
    }
}
