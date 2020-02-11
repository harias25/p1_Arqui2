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

        [HttpGet("/getData")]
        public string getData()
        {
            DBManager db = new DBManager();
            String query = "SELECT * FROM request_ascii_morse WHERE status = 0 LIMIT 1";
            DataTable table = db.getTableByQuery(query, "9KWIfwbSFw");
            if (table == null) return "";
            if(table.Rows.Count ==0) return "";
            query = "UPDATE request_ascii_morse SET status = 1 WHERE id = " + table.Rows[0][0].ToString()+";";
            db.execQuery(query, "9KWIfwbSFw");

            return table.Rows[0][2].ToString();

        }


        [HttpGet("/getHistory")]
        public string getHistory()
        {
            DBManager db = new DBManager();
            String query = "SELECT text_morse, text_convert, DATE_FORMAT(date_request, \" %d /%m /%Y %h:%i:%S\") as date FROM request_morse_ascii;";
            DataTable table = db.getTableByQuery(query, "9KWIfwbSFw");
            if (table == null) return "<table style='with:100%'> </table>";

            String tablehtml = "<table with:'100%'> ";
            tablehtml += "<tr> "+
                         "  <th width='50%'>Morse Ingresado</th> " +
                         "  <th width='35%'>Morse - Ascii</th> " +
                         "  <th width='15%'>Fecha Ingresado</th> " +
                         "</tr>";

            foreach (DataRow row in table.Rows)
            {
                tablehtml += "<tr> " +
                         "  <td>"+row[0].ToString()+"</td> " +
                         "  <td>"+ row[1].ToString() + "</td> " +
                         "  <td>"+ row[2].ToString() + "</td> " +
                         "</tr>";
            }
            tablehtml += "</table>";


            return tablehtml;

        }


        [HttpPost, Route("/convert_ascii")]
        public ActionResult convertAscii([FromBody] Request request)
        {

            if (!ModelState.IsValid)
            {
                var message = JsonConvert.SerializeObject(BadRequest(ModelState).Value);
                return Content("{\"codigoResultado\":-1,\"mensajeResultado\":[" + JsonConvert.SerializeObject(BadRequest(ModelState).Value) + "]}", "application/json");
            }

            return Content(request.ConvertAscii(), "application/json");
        }

        [HttpPost, Route("/convert_morse")]
        public ActionResult convertMorse([FromBody] Request request)
        {

            if (!ModelState.IsValid)
            {
                var message = JsonConvert.SerializeObject(BadRequest(ModelState).Value);
                return Content("{\"codigoResultado\":-1,\"mensajeResultado\":[" + JsonConvert.SerializeObject(BadRequest(ModelState).Value) + "]}", "application/json");
            }

            return Content(request.ConvertMorse(), "application/json");
        }

    }
}
