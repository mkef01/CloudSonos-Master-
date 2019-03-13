using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Mvc;
using CloudSonos.Models;
using Newtonsoft.Json;

namespace CloudSonos.Controllers
{
    public class LoginController : Controller
    {
		// GET: Login
	    private string baseURL = "http://localhost:56131/api/login/";
		public ActionResult Index()
        {
            return View();
        }

	    [System.Web.Mvc.HttpPost]
	    public ActionResult Index(Entrada inEntrada)
	    {
			Entrada entradita = new Entrada();
			//Entrada modeloEntrada = new Entrada();
		 //   modeloEntrada.Usuario = inEntrada.Usuario;
		 //   modeloEntrada.Contraseña = inEntrada.Contraseña;
		 //   var json = Newtonsoft.Json.JsonConvert.SerializeObject(modeloEntrada);
			using (var cliente = new HttpClient())
		    {
			    cliente.BaseAddress = new Uri(baseURL);

			    HttpResponseMessage respuesta = cliente.PostAsJsonAsync<Entrada>("acceso",inEntrada).Result;
			    var json = respuesta.Content.ReadAsStringAsync().Result;

			    ViewBag.Message = json;

		    }
			
		    return View();
	    }
    }
}