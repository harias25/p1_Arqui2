using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apiArqui2.Models
{
    public class Response
    {
        public int codigoResultado { get; set; }
        public String mensajeResultado { get; set; }

        public String palabra { get; set; } = null;
    }
}
