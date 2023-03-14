using Microsoft.AspNetCore.Mvc;

namespace SL.Controllers
{
    public class UsuarioController : Controller
    {
        [HttpGet]
        [Route("api/Alumno/GetAll")]
        public ActionResult GetAll()
        {
            ML.Alumno alumno = new ML.Alumno();

            alumno.Nombre = (alumno.Nombre == null) ? "" : alumno.Nombre;
            alumno.ApellidoPaterno = (alumno.ApellidoPaterno == null) ? "" : alumno.ApellidoPaterno;
            alumno.ApellidoMaterno = (alumno.ApellidoMaterno == null) ? "" : alumno.ApellidoMaterno;
            ML.Result result = BL.Alumno.GetAll(alumno);
            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Route("api/Alumno/GetAll")]
        public ActionResult GetAll([FromBody] ML.Alumno alumno)
        {
            ML.Result result = BL.Alumno.GetAll(alumno);
            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return NotFound();
            }
        }
        [HttpPost]
        [Route("api/Usuario/Add")]
        public ActionResult Add([FromBody]ML.Alumno alumno)
        {
            ML.Result result = BL.Alumno.Add(alumno);
            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return NotFound(result);
            }
        }
    }
}
