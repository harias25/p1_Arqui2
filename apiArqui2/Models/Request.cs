using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace apiArqui2.Models
{
    public class Request
    {
        [Required()]
        public String value
        {
            get; set;
        }

        public string player { get; set; }

        public string RecibirMorse()
        {
            String morse = "";
            for (int i = 0; i < value.Length; i++)
            {
                String val = MorseCode.GetofValue(value.Substring(i, 1));
                morse = morse + " " + val;
            }


            String query = "INSERT INTO request_morse_ascii(text_morse,text_convert) VALUES('" + value.Trim() + "','" + morse + "');";
            DBManager db = new DBManager();
            db.execQuery(query, "arqui2");

            Response r = new Response();
            r.codigoResultado = 0;
            r.mensajeResultado = "Palabra ingresada correctamente en el sistema";
            r.palabra = value;

            return value;
        }

        public string ingresarJuego()
        {
            value = value.Trim().ToUpper();

            String temp = value;
            value = "";

            //validacion de letras
            foreach (char letra in temp)
            {
                String val = MorseCode.GetofValue(letra.ToString());
                if (val != "")
                {
                    if(val!=null)
                        value += letra;
                }
            }


            if (value.Length > 10)
            {
               value = value.Substring(0, 10);
            }else if(value.Length < 10)
            {
               for(int i=value.Length; i<10; i++)
               {
                    Random rnd = new Random();
                    char randomChar = (char)rnd.Next('A', 'Z');
                    value +=randomChar;
               }
            }




            String query = "INSERT INTO game(text_play,player) VALUES('" + value + "','" + player + "');";
            DBManager db = new DBManager();
            db.execQuery(query, "arqui2");


            Response r = new Response();
            r.codigoResultado = 0;
            r.mensajeResultado = "Jugador "+player +" ingresado correctamente con la palabra "+value;
            r.palabra = value;

            return JsonConvert.SerializeObject(r);

        }



        public string InsertarPalabraCola()
        {
            var decode = "";
            value = value.ToUpper();

            /*if(value.Length > 10)
            {
                value = value.Substring(0, 10);
            }else if(value.Length < 10)
            {
                for(int i=value.Length; i<10; i++)
                {
                    value += "0";
                }
            }*/

            for (int i = 0; i < value.Length; i++)
            {
                String val = MorseCode.GetofValue(value.Substring(i, 1));
                decode = decode + " " + val;
            }


            String query = "INSERT INTO request_ascii_morse(text_ascii,text_convert) VALUES('"+value+"','"+decode+"');";
            DBManager db = new DBManager();
            db.execQuery(query, "arqui2");


            Response r = new Response();
            r.codigoResultado = 0;
            r.mensajeResultado = "Palabra agregada correctamente en la cola";
            r.palabra = decode;

            return JsonConvert.SerializeObject(r);

        }


    }

    public class MorseCode
    {
        private static Dictionary<string, string> _dic = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            {"._", "A"},{"_...", "B"},{"_._.", "C"}, {"_..", "D"},{".", "E"},{".._.", "F"},{"__.", "G"},{"....", "H"},
            {"..", "I"},{".___", "J"},{"_._", "K"},{"._..", "L"},{"__", "M"},{"_.", "N"},{"___", "O"},{".__.", "P"},
            {"__._", "Q"},{"._.","R"},{"...", "S"},{"_", "T"},{".._", "U"},{"..._", "V"},{"_.._", "X"},{".__", "W"},{"_.__", "Y"},{"__..", "Z"},

            {".____", "1"},{"..___", "2"},{"...__", "3"},{"...._", "4"},{".....", "5"},{"_....", "6"},{"__...", "7"},
            {"___..", "8"},{"____.", "9"},{"_____", "0"},

          
        };

        public static string Get(string code)
        {
            try
            {
                return _dic[code];
            }
            catch
            {
                return "";
            }
            
        }

        public static string GetofValue(String code)
        {
            try
            {
                return _dic.FirstOrDefault(x => x.Value == code).Key;
            }
            catch
            {
                return "";
            }
            
        }
    }
}
