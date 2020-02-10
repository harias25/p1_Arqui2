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
