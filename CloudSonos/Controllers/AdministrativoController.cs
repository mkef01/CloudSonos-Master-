using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CloudSonos.Models;

namespace CloudSonos.Controllers
{
    public class AdministrativoController : Controller
    {
        private readonly NubeDataContext _context;

        
        // Agregar Genero
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index (generos datos)
        {
            using (NubeDataContext nubeDataContext = new NubeDataContext())
            {
                try
                {
                    nubeDataContext.generos.InsertOnSubmit(datos);
                    nubeDataContext.SubmitChanges();
                }catch (Exception e){
                    Console.WriteLine(e);
                    throw;

                }
            }
            return View();
        }
    }
}