using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using apiArqui2.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace apiArqui2.Controllers
{
    [ApiController]
    public class MorseController : ControllerBase
    {


        [HttpGet("/status")]
        public string status()
        {
            return "Conversor Morse-Ascii " + DateTime.Now.ToString();
        }

        [HttpGet("/statusDB")]
        public string statusDB()
        {
            DBManager db = new DBManager();
            return db.status();
        }

        [HttpGet("/getWord")]
        public string getData()
        {
            DBManager db = new DBManager();
            String query = "SELECT * FROM request_ascii_morse WHERE status = 0 LIMIT 1";
            DataTable table = db.getTableByQuery(query, "arqui2");

            Response response = new Response();
            response.codigoResultado = -1;
            response.mensajeResultado = "No hay palabras en cola.";


            if (table == null) return "";//return JsonConvert.SerializeObject(response);
            if (table.Rows.Count == 0) return "";//return JsonConvert.SerializeObject(response);
            query = "UPDATE request_ascii_morse SET status = 1 WHERE id = " + table.Rows[0][0].ToString() + ";";
            db.execQuery(query, "arqui2");

            response.codigoResultado = 0;
            response.mensajeResultado = "Palabra en Cola.";
            response.palabra = table.Rows[0][1].ToString();
            return table.Rows[0][1].ToString();//JsonConvert.SerializeObject(response);

        }


        [HttpGet("/getHistory")]
        public string getHistory()
        {
            DBManager db = new DBManager();
            String query = "SELECT text_morse, text_convert, DATE_FORMAT(date_request, \" %d /%m /%Y %h:%i:%S\") as date FROM request_morse_ascii;";
            DataTable table = db.getTableByQuery(query, "arqui2");
            if (table == null) return "<table style='with:100%'> </table>";

            String tablehtml = "<table style = 'width: 100%;'> ";
            tablehtml += "<tr> "+
                         "  <th width='45%' align='center'>Morse Ingresado</th> " +
                         "  <th width='35%' align='center'>Morse Convertido</th> " +
                         "  <th width='20%' align='center'>Fecha Ingresado</th> " +
                         "</tr>";

            foreach (DataRow row in table.Rows)
            {
                tablehtml += "<tr> " +
                         "  <td align='center'>" + row[1].ToString()+"</td> " +
                         "  <td align='center'>" + row[0].ToString() + "</td> " +
                         "  <td align='center'>" + row[2].ToString() + "</td> " +
                         "</tr>";
            }
            tablehtml += "</table>";


            return tablehtml;

        }

        [HttpGet("/newGame")]
        public ActionResult game(String word, String player)
        {
            if (word == null || player == null)
                return Content("{\"codigoResultado\":-1,\"mensajeResultado\":\"Se requieren los parametros para ingresar la información\"", "application/json");

            Request request = new Request();
            request.value = word;
            request.player = player;

            return Content(request.ingresarJuego(), "application/json");
        }


        [HttpGet("/getGame")]
        public string getGame()
        {
            DBManager db = new DBManager();
            String query = "SELECT * FROM game WHERE status = 0 LIMIT 1";
            DataTable table = db.getTableByQuery(query, "arqui2");

            Response response = new Response();
            response.codigoResultado = -1;
            response.mensajeResultado = "No hay juegos en cola.";


            if (table == null) return "";//return JsonConvert.SerializeObject(response);
            if (table.Rows.Count == 0) return "";//return JsonConvert.SerializeObject(response);
            query = "UPDATE game SET status = 1 WHERE id = " + table.Rows[0][0].ToString() + ";";
          //  db.execQuery(query, "arqui2");

            response.codigoResultado = 0;
            response.mensajeResultado = "Palabra en Cola.";
            response.palabra = table.Rows[0][1].ToString();
            return table.Rows[0][0].ToString()+"|"+table.Rows[0][1].ToString();//JsonConvert.SerializeObject(response);

        }

        [HttpGet("/setScore")]
        public string score(String id, String score)
        {
            if (id == null || score == null)
                return "Valores no validos";

            String query = "UPDATE game SET score = "+score+", status=1 WHERE id = " +id + ";";
            DBManager db = new DBManager();
            bool result = db.execQuery(query, "arqui2");
            if(result)
                return "Se actualizo correctamente";
            else
                return "No se actualizo ningun registro";

        }

        [HttpGet("/getGames")]
        public string getGames()
        {
            DBManager db = new DBManager();
            String query = "SELECT text_play, player, DATE_FORMAT(date_request, \" %d /%m /%Y %h:%i:%S\") as date, score FROM game WHERE status = 1;";
            DataTable table = db.getTableByQuery(query, "arqui2");
            if (table == null) return "<table style='with:100%'> </table>";
            
            String tablehtml = "<table style = 'width: 100%;'> ";
            tablehtml += "<tr> " +
                         "  <th width='25%' align='center'>Jugador</th> " +
                         "  <th width='30%' align='center'>Texto Jugado</th> " +
                         "  <th width='25%' align='center'>Fecha Ingresado</th> " +
                         "  <th width='20%' align='center'>Punteo</th> " +
                         "</tr>";

            foreach (DataRow row in table.Rows)
            {
                tablehtml += "<tr> " +
                         "  <td align='center'>" + row[1].ToString() + "</td> " +
                         "  <td align='center'>" + row[0].ToString() + "</td> " +
                         "  <td align='center'>" + row[2].ToString() + "</td> " +
                         "  <td align='center'>" + row[3].ToString() + "</td> " +
                         "</tr>";
            }
            tablehtml += "</table>";


            return tablehtml;

        }

        [HttpGet("/init")]
        public String init()
        {
            DBManager db = new DBManager();
            db.execQuery(queryInit, "arqui2");
            return "";
        }


        [HttpGet("/addWord")]
        public ActionResult ingresarPalabra(String word)
        {
            if(word == null)
                return Content("{\"codigoResultado\":-1,\"mensajeResultado\":\"Se requiere el parametro de la palabra para poder ingresarla\"", "application/json");

            Request request = new Request();
            request.value = word;

            return Content(request.InsertarPalabraCola(), "application/json");

        }

        [HttpGet("/addMorse")]
        public ActionResult ingresarMorse(String word)
        {
            if (word == null)
                return Content("{\"codigoResultado\":-1,\"mensajeResultado\":\"Se requieren los parametros para ingresar la información\"", "application/json");

            Request request = new Request();
            request.value = word;

            return Content(request.RecibirMorse(), "application/json");

        }


        String queryInit = "create table request_ascii_morse( " +
                          " id INT             NOT NULL AUTO_INCREMENT, " +
                          " text_ascii VARCHAR(60)     NOT NULL, " +
                          " text_convert    VARCHAR(256)    NULL, " +
                          " date_request DATETIME        NOT NULL DEFAULT NOW(), " +
                          " status BIT             NOT NULL DEFAULT 0, " +
                          " CONSTRAINT request_ascii_morse_pk PRIMARY KEY(id) " +
                          " ); " +

                           " create table request_morse_ascii( " +
                           "    id INT             NOT NULL AUTO_INCREMENT, " +
                           "    text_morse VARCHAR(256)    NOT NULL, " +
                           " text_convert    VARCHAR(60) NOT NULL, " +
                           " date_request    DATETIME NOT NULL DEFAULT NOW(), " +
                           "    status BIT             NOT NULL DEFAULT 0, " +
                           "    CONSTRAINT request_morse_ascii_pk PRIMARY KEY(id) " +
                           "); " +

                           " create table game( " +
                           "    id INT             NOT NULL AUTO_INCREMENT, " +
                           "    text_play VARCHAR(10)     NOT NULL, " +
                           " player          VARCHAR(60)     NOT NULL, " +
                           " date_request    DATETIME NOT NULL DEFAULT NOW(), " +
                           "    status BIT             NOT NULL DEFAULT 0, " +
                           "    score int NOT NULL DEFAULT 0, " +
                           " CONSTRAINT game_pk PRIMARY KEY(id) " +
                           " );";



    }
}
