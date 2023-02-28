using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    public class AlumnoController : Controller
    {
        [HttpGet]
        public ActionResult GetAll()
        {
            ML.Result result = BL.Alumno.GetAll();
            ML.Alumno alumno = new ML.Alumno();

            alumno.Alumnos = result.Objects;
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
            //HttpPostedFileBase file = Request.Files["fuImage"];
            IFormFile file = Request.Form.Files["fuImage"];

            if (file != null)
            {
                byte[] imagen = ConvertToBytes(file);

               alumno.Imagen = Convert.ToBase64String(imagen);
            }


            ML.Result result = new ML.Result();

            if (alumno.IdAlumno == 0)
            {
                //add 
                result = BL.Alumno.Add(alumno);

                if (result.Correct)
                {
                    ViewBag.Message = "Se completo el registro satisfactoriamente";
                }
                else
                {
                    ViewBag.Message = "Ocurrio un error al insertar el registro";
                }

                return View("Modal");
            }
            else
            {

                //update
                //result = BL.Alumno.Add(alumno);

                //if (result.Correct)
                //{
                //    ViewBag.Message = "Se actualizo la información satisfactoriamente";
                //}
                //else
                //{
                //    ViewBag.Message = "Ocurrio un error al actualizar el registro";
                //}
                return View("Modal");
            }


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

    }
}
