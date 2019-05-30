using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.UI.WebControls;
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
            var query = from usuario in _context.usuario
                        where usuario.Usuario1.Equals(inEntrada.Usuario) && usuario.Contraseña.Equals(inEntrada.Contraseña)
                        select new
                        {
                            idUsuario = usuario.ID_Usuario
                        };
            int total = query.Count();
            if (total == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
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

        [EnableCors("*", "*", "*")]
        [HttpPost]
        [Route("api/reproductor/index")]
        public List<IndexReproductor> West()
        {
            List<IndexReproductor> indexReproductors = new List<IndexReproductor>();
            var query = from alb in _context.album
                        join art in _context.artistabanda on alb.ID_Album equals art.ID_Album
                        orderby alb.nombre ascending 
                        select new
                        {
                            albumnombre = alb.nombre,
                            albumimagen = alb.Imagen,
                            artista = art.Nombre,
                            duracion = alb.Duracion
                        };
            var detalles = query.ToList();
            foreach(var detalleData in detalles)
            {
                indexReproductors.Add(new IndexReproductor()
                {
                    Album = detalleData.albumnombre,
                    UrlCaratula = detalleData.albumimagen,
                    NombreArtista = detalleData.artista,
                    Duracion = detalleData.duracion
                });
            }
            return indexReproductors;
        }

        [EnableCors("*", "*", "*")]
        [HttpGet]
        [Route("api/reproductor/random")]
        public List<Models.Random> locke()
        {
            List<Models.Random> lockerandom = new List<Models.Random>();
            var query = from canci in _context.canciones
                        select new
                        {
                            cantidad = canci.ID_Cancion
                        };
            int conteo = query.Count();
            var seed = Environment.TickCount;
            var random = new System.Random(seed);
            var value = random.Next(0, conteo);
            var query2 = from canci in _context.canciones
                         where canci.ID_Cancion == value
                         select new
                         {
                             link = canci.URL,
                             nom = canci.Nombre,
                         };
            var detalle = query2.ToList();
            foreach (var detalleData in detalle)
            {
                lockerandom.Add(new Models.Random
                {
                    nombre = detalleData.nom,
                    URl = detalleData.link
                });
            }
            return lockerandom;
        }

        [EnableCors("*", "*", "*")]
        [HttpPost]
        [Route("api/reproductor/bookmark")]
        public Boolean simonsimon([FromBody] Bookmark book)
        {
            if (book.album.Equals("Some girls"))
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        [EnableCors("*", "*", "*")]
        [HttpPost]
        [Route("api/reproductor/artistas")]
        public List<Models.Artistas> lupita()
        {
            List<Models.Artistas> detaList = new List<Artistas>();
            var query = from artis in _context.artistabanda
                join integrabanda in _context.integrabanda on artis.ID_Artista equals integrabanda.ID_Artista
                group new {artis} by new { artis.Nombre, artis.imagen, artis.ID_Artista } into grop
                select new
                {
                    nombre = grop.Key.Nombre,
                    banner = grop.Key.imagen,
                    idbanda = grop.Key.ID_Artista
                };
            var lista = query.ToList();
            foreach (var detalleLista in lista)
            {
                string integrantes = "";
                int iteracion = 0;
                var query2 = from integra in _context.integrabanda
                    where integra.ID_Artista == detalleLista.idbanda
                    select new
                    {
                        personal = integra.Persona
                    };
                var detalle = query2.ToList();
                int final = query2.Count();
                foreach (var detalle2 in detalle)
                {
                    if (iteracion < final-1)
                    {
                        integrantes = integrantes + detalle2.personal + ", ";
                    }
                    else
                    {
                        integrantes = integrantes + detalle2.personal;
                    }
                    iteracion++;
                }
                detaList.Add(new Artistas()
                {
                    NombreArtista = detalleLista.nombre,
                    UrlArtista = detalleLista.banner,
                    IntegrantesBanda = integrantes
                    
                });
            }

            return detaList;
        }

        [EnableCors("*", "*", "*")]
        [HttpPost]
        [Route("api/reproductor/nuevo")]
        public int aloha([FromBody] NuevoUsuario nuevoUsuario)
        {
            try
            {
                string pass1 = nuevoUsuario.Password1;
                string pass2 = nuevoUsuario.Password2;
                if (pass2.Equals(pass1))
                {
                    var query = from usu in _context.usuario
                        select new
                        {
                            co = usu.Correo,
                            nombre = usu.Usuario1
                        };
                    var lista = query.ToList();
                    foreach (var listadetalle in lista)
                    {
                        if (listadetalle.nombre.Equals(nuevoUsuario.Usuario))
                        {
                            return 40;
                        }

                        if (listadetalle.co.Equals(nuevoUsuario.Coreo))
                        {
                            return 50;
                        }
                    }

                }
                else
                {
                    return 60;
                }
            }
            catch (Exception e)
            {
                return 100;
            }

            return 0;
        }

        [EnableCors("*", "*", "*")]
        [HttpPost]
        [Route("api/reproductor/insertar")]
        public int NuevoUsuario([FromBody] usuario nuevoUsuario)
        {
            using (NubeDataContext objDataContext = new NubeDataContext())
            {
                try
                {
                    objDataContext.usuario.InsertOnSubmit(nuevoUsuario);
                    objDataContext.SubmitChanges();
                }
                catch (Exception e)
                {
                    return 0;
                }
            }

            return 5;
        }
    }
}
