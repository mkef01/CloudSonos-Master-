using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CloudSonos.Models
{
    public class NuevoUsuario
    {
        public string Usuario { get; set; }
        public string Coreo { get; set; }
        public string Password1 { get; set; }
        public string Password2 { get; set; }
    }
}