using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    public class AlumnoController : Controller
    {
        //Inyeccion de dependencias-- patron de diseño
        private readonly IConfiguration _configuration;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;

        public AlumnoController(IConfiguration configuration, Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment)
        {
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
        }
        //exponer la logica de negocio a internet 
        //HTTP 
        //URL 
        [HttpGet]
        public ActionResult GetAll()
        {

            ML.Alumno alumno = new ML.Alumno();

            //alumno.Nombre = (alumno.Nombre == null) ? "" : alumno.Nombre;
            //alumno.ApellidoPaterno = (alumno.ApellidoPaterno == null) ? "" : alumno.ApellidoPaterno;
            //alumno.ApellidoMaterno = (alumno.ApellidoMaterno == null) ? "" : alumno.ApellidoMaterno;
            //ML.Result result = BL.Alumno.GetAll(alumno);

            //alumno.Alumnos = result.Objects;
            //return View(alumno);
            ML.Result result = new ML.Result();
            result.Objects = new List<object>();

            try
            {
                //SOAP 
                //REST (JSON) Serializacion y deserializacion Postman swagger(probar el endpoint, validarlo)
                //Probar(Consumo) web services : postman-swagger
                //Consumirlo en el proyecto : el controlador de MVC o AJAX 
                using (var client = new HttpClient())
                {
                    string urlApi = _configuration["urlApi"];
                    client.BaseAddress = new Uri(urlApi);

                    var responseTask = client.GetAsync("Alumno/GetAll");
                    responseTask.Wait();

                    var resultServicio = responseTask.Result;

                    if (resultServicio.IsSuccessStatusCode)
                    {
                        var readTask = resultServicio.Content.ReadAsAsync<ML.Result>();
                        readTask.Wait();

                        foreach (var resultItem in readTask.Result.Objects)
                        {
                            ML.Alumno resultItemList = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Alumno>(resultItem.ToString());
                            result.Objects.Add(resultItemList);
                        }
                    }
                    alumno.Alumnos = result.Objects;
                }

            }
            catch (Exception ex)
            {
            }

            return View(alumno);
        }

        [HttpGet]
        public ActionResult Form(int? idAlumno) //2
        {
            ML.Result resultSemestre = BL.Semestre.GetAll();
            ML.Result resultPlantel = BL.Plantel.GetAll();

            ML.Alumno alumno = new ML.Alumno();
            alumno.Semestre = new ML.Semestre();

            alumno.Horario = new ML.Horarios();
            alumno.Horario.Grupo = new ML.Grupo();
            alumno.Horario.Grupo.Plantel = new ML.Plantel();


            if (resultSemestre.Correct && resultPlantel.Correct)
            {
                alumno.Horario.Grupo.Plantel.Planteles = resultPlantel.Objects;
                alumno.Semestre.Semestres = resultSemestre.Objects;
            }
            if (idAlumno == null)
            {
                //add //formulario vacio
                return View(alumno);
            }
            else
            {
                //getById
                ML.Result result = BL.Alumno.GetById(idAlumno.Value); //2

                if (result.Correct)
                {
                    alumno = (ML.Alumno)result.Object;//unboxing
                    alumno.Semestre.Semestres = resultSemestre.Objects;

                    alumno.Horario = new ML.Horarios();
                    alumno.Horario.Grupo = new ML.Grupo();
                    alumno.Horario.Grupo.Plantel = new ML.Plantel();
                    alumno.Horario.Grupo.Plantel.Planteles = resultPlantel.Objects;
                    //update
                    return View(alumno);
                }
                else
                {
                    ViewBag.Message = "Ocurrio al consultar la información del alumno";
                    return View("Modal");
                }


            }


        }

        [HttpPost]
        public ActionResult Form(ML.Alumno alumno)
        {
            IFormFile file = Request.Form.Files["fuImage"];

            if (file != null)
            {
                byte[] imagen = ConvertToBytes(file);

                alumno.Imagen = Convert.ToBase64String(imagen);
            }
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_configuration["urlApi"]);

                //HTTP POST
                var postTask = client.PostAsJsonAsync<ML.Alumno>("Alumno/Add",alumno );
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    ViewBag.Mensaje = "Se ha registrado el alumno";
                    return PartialView("Modal");
                }
                else
                {
                    ViewBag.Mensaje = "No se ha registrado el alumno";
                    return PartialView("Modal");
                }
            }
            //ML.Result result = new ML.Result();
            //if (ModelState.IsValid == true)
            //{
            //    if (alumno.IdAlumno == 0)
            //    {
            //        //add 
            //        result = BL.Alumno.Add(alumno);

            //        if (result.Correct)
            //        {
            //            ViewBag.Message = "Se completo el registro satisfactoriamente";
            //        }
            //        else
            //        {
            //            ViewBag.Message = "Ocurrio un error al insertar el registro";
            //        }

            //        return View("Modal");
            //    }
            //    else
            //    {

            //        //update
            //        //result = BL.Alumno.Add(alumno);

            //        //if (result.Correct)
            //        //{
            //        //    ViewBag.Message = "Se actualizo la información satisfactoriamente";
            //        //}
            //        //else
            //        //{
            //        //    ViewBag.Message = "Ocurrio un error al actualizar el registro";
            //        //}
            //        return View("Modal");
            //    }
            //}
            //else
            //{
            //    alumno.Semestre = new ML.Semestre();
            //    alumno.Horario = new ML.Horarios();
            //    alumno.Horario.Grupo = new ML.Grupo();
            //    alumno.Horario.Grupo.Plantel = new ML.Plantel();

            //    ML.Result resultSemestre = BL.Semestre.GetAll();
            //    ML.Result resultPlantel = BL.Plantel.GetAll();

            //    alumno.Semestre.Semestres = resultSemestre.Objects;
            //    alumno.Horario.Grupo.Plantel.Planteles = resultPlantel.Objects;
            //    return View(alumno);
            //}

        }

        [HttpGet]
        public ActionResult Delete(int idAlumno)
        {
            //ML.Result result = BL.Alumno.Delete(idAlumno);
            return View();
        }

        [HttpPost]
        public JsonResult GrupoGetByIdPlantel(int idPlantel)
        {
            ML.Result result = new ML.Result();
            result = BL.Grupo.GetByIdPlantel(idPlantel);

            return Json(result.Objects);
        }

        public static byte[] ConvertToBytes(IFormFile imagen)
        {

            using var fileStream = imagen.OpenReadStream();

            byte[] bytes = new byte[fileStream.Length];
            fileStream.Read(bytes, 0, (int)fileStream.Length);

            return bytes;
        }
        [HttpPost]
        public JsonResult CambiarStatus(int idAlumno, bool status)
        {
            ML.Result result = BL.Alumno.ChangeStatus(idAlumno, status);

            return Json(result);
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string nombre, string apellido)
        {
            ML.Result result = BL.Alumno.GetByNombre(nombre);
            if (result.Correct)
            {
                ML.Alumno alumno = (ML.Alumno)result.Object;
                if (apellido == alumno.ApellidoPaterno)
                {
                    return View();
                }
                else
                {
                    ViewBag.Message = "La contraseña no coincide";
                    return PartialView("Modal");
                }
            }
            else
            {
                ViewBag.Messge = "El usuario no existe";
                return PartialView("Modal");
            }
        }
    }
}
