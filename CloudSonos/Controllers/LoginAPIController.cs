using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using CloudSonos.Models;

namespace CloudSonos.Controllers
{
    public class LoginAPIController : ApiController
    {
		[EnableCors("*","*","*")]
		[HttpPost]
	    [Route("api/login/acceso")]
	    public bool Login([FromBody] Entrada inEntrada)
	    {
		    int a = 0;
		    if (inEntrada.Usuario.Equals("mkef01"))
		    {
			    a++;
		    }
		    if (inEntrada.Contraseña.Equals("qazxsw123"))
		    {
			    a++;
		    }
		    if (a == 2)
		    {
			    return true;
		    }
		    return false;
	    }
    }
}
