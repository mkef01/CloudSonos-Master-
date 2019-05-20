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
        private readonly NubeDataContext _context;

        public LoginAPIController()
        {
            _context = new NubeDataContext();
        }

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

        [EnableCors("*", "*", "*")]
        [HttpPost]
        [Route("api/login/clase")]
        public Array clase()
        {
            //Entrada a = new Entrada();
            //a.Contraseña = "aloha";
            //a.Usuario = "elmer";
            //a.estado = true;
            //return a;
            Entrada[] array = new Entrada[5];
            for (int i=0; i<5; i++){
                Entrada obj1 = new Entrada();
                obj1.Usuario = "Elmer" + Convert.ToString(i);
                obj1.Contraseña = "Aloha" + Convert.ToString(i);
                obj1.estado = false;
                array[i] = obj1;
            };
            return array;
        }

        [EnableCors("*", "*", "*")]
        [HttpGet]
        [Route("api/login/canciones")]
        public List<lista> Origen(string playlist,int ID)
        {
            List<lista> datos = new List<lista>();
            lista f = new lista();
            f.nombre = "Ben Jadusable - Ben Drowned";
            f.url = "https://mkef01.000webhostapp.com/Song%20of%20healing%20%20Extended%20%20Reversed.mp3";
            datos.Add(f);
            lista d = new lista();
            d.nombre = "A Day in the Life - The Beatles";
            d.url = "https://mkef01.000webhostapp.com/13%20-%20A%20Day%20In%20The%20Life.flac";
            datos.Add(d);
            lista s = new lista();
            s.nombre = "Saxon - Crusader Prelude";
            s.url = "https://s3-us-west-2.amazonaws.com/allmetalmixtapes/Saxon%20-%201984%20-%20Crusader/01%20-%20Crusader%20Prelude.mp3";
            datos.Add(s);
            return datos;
        }

        [EnableCors("*", "*", "*")]
        [HttpPost]
        [Route("api/reproductor/album")]
        public List<apiAlbum> Album([FromBody] Pedido1 pedidito)
        {
            List<apiAlbum> detalleAlbum = new List<apiAlbum>();
            var query = from alb in _context.album
                        join g in _context.generos on alb.ID_Genero equals g.ID_Genero
                        join art in _context.artistabanda on alb.ID_Album equals art.ID_Album
                        where art.Nombre == pedidito.artista && alb.nombre == pedidito.album
                        select new
                        {
                            imgAlbum = alb.Imagen,
                            genNombre = g.GeneroNombre,
                            year = alb.año,
                            descrip = alb.Descripcion,
                            imgBanda = art.imagen,
                            albNombre = alb.nombre,
                            discografia = alb.Discografica
                        };
            var detalles = query.ToList();
            foreach (var detalleData in detalles)
            {
                detalleAlbum.Add(new apiAlbum()
                {
                    UrlBanda = detalleData.imgBanda,
                    UrlAlbum = detalleData.imgAlbum,
                    Genero = detalleData.genNombre,
                    Año = detalleData.year,
                    Discografia = detalleData.discografia,
                    Descripcion = detalleData.descrip
                });
            }
            return detalleAlbum;
        }

        [EnableCors("*", "*", "*")]
        [HttpPost]
        [Route("api/reproductor/playlist")]
        public List<apiPlaylist> Playlist([FromBody] Pedido1 pedidito)
        {
            List<apiPlaylist> detallePlaylist = new List<apiPlaylist>();
            var query = from alb in _context.album
                        join c in _context.cancionalbum on alb.ID_Album equals c.ID_Album
                        join can in _context.canciones on c.ID_Cancion equals can.ID_Cancion
                        join art in _context.artistabanda on alb.ID_Album equals art.ID_Album
                        where alb.nombre == pedidito.album && art.Nombre == pedidito.artista
                        select new
                        {
                            nombre = can.Nombre,
                            link = can.URL
                        };
            var detalles = query.ToList();
            foreach (var detalleData in detalles)
            {
                detallePlaylist.Add(new apiPlaylist()
                {
                    Nombre = detalleData.nombre,
                    URL = detalleData.link
                });
            }
            return detallePlaylist;
        }
    }
}
