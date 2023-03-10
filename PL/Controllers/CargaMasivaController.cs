using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    public class CargaMasivaController : Controller
    {
        //Inyeccion de dependencias-- patron de diseño
        private readonly IConfiguration _configuration;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;

        public CargaMasivaController(IConfiguration configuration, Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment)
        {
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
        }

        public ActionResult CargaMasiva()
        {
            ML.Alumno alumno = new ML.Alumno();

            return View();
        }
        [HttpPost]
        public ActionResult AlumnoCargaMasiva(ML.Alumno alumno)
        {
            IFormFile archivo = Request.Form.Files["FileExcel"];
            //Session
            //GetString = Obtener la session
            //SetString = Crear una session
            if (HttpContext.Session.GetString("PathArchivo") == null)
            {
                //Si el archivo trae información
                if (archivo.Length > 0)
                {
                    //obtener el nombre de nuestro archivo
                    string fileName = Path.GetFileName(archivo.FileName);

                    string folderPath = _configuration["PathFolder:value"];
                    string extensionArchivo = Path.GetExtension(archivo.FileName).ToLower();
                    string extensionModulo = _configuration["TipoExcel"];

                    if (extensionArchivo == extensionModulo) //validando que sea un archivo excel
                    {
                        string filePath = Path.Combine(_hostingEnvironment.ContentRootPath, folderPath, Path.GetFileNameWithoutExtension(fileName)) + '-' + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";

                        if (!System.IO.File.Exists(filePath))
                        {
                            using (FileStream stream = new FileStream(filePath, FileMode.Create))
                            {
                                archivo.CopyTo(stream);
                            }

                            string connectionString = _configuration["ConnectionStringExcel:value"] + filePath;
                            ML.Result resultAlumnos = BL.Alumno.ConvertXSLXtoDataTable(connectionString);

                            if (resultAlumnos.Correct)
                            {
                                ML.Result resultValidacion = BL.Alumno.ValidarExcel(resultAlumnos.Objects);
                                if (resultValidacion.Objects.Count == 0)
                                {
                                    resultValidacion.Correct = true;
                                    HttpContext.Session.SetString("PathArchivo", filePath);
                                }

                                return View(resultValidacion);
                            }
                            else
                            {
                                ViewBag.Message = "El excel no contiene registros";
                            }
                        }
                    }
                    else
                    {
                        ViewBag.Message = "Favor de seleccionar un archivo .xlsx, Verifique su archivo";
                    }
                }
                else
                {
                    ViewBag.Message = "No selecciono ningun archivo, Seleccione uno correctamente";
                }
            }
            else
            {
                string rutaArchivoExcel = HttpContext.Session.GetString("PathArchivo");
                string connectionString = _configuration["ConnectionStringExcel:value"] + rutaArchivoExcel;

                ML.Result resultData = BL.Alumno.ConvertXSLXtoDataTable(connectionString);
                if (resultData.Correct)
                {
                    ML.Result resultErrores = new ML.Result();
                    resultErrores.Objects = new List<object>();

                    foreach (ML.Alumno alumnoItem in resultData.Objects)
                    {

                        ML.Result resultAdd = BL.Alumno.Add(alumnoItem);
                        if (!resultAdd.Correct)
                        {
                            resultErrores.Objects.Add("No se insertó el Alumno con nombre: " + alumnoItem.Nombre + " Creditos:" + alumnoItem.ApellidoPaterno + "Costo:" + alumnoItem.ApellidoMaterno + " Error: " + resultAdd.ErrorMessage);
                        }
                    }
                    if (resultErrores.Objects.Count > 0)
                    {

                        string fileError = Path.Combine(_hostingEnvironment.WebRootPath, @"~\Files\logErrores.txt");
                        using (StreamWriter writer = new StreamWriter(fileError))
                        {
                            foreach (string ln in resultErrores.Objects)
                            {
                                writer.WriteLine(ln);
                            }
                        }
                        ViewBag.Message = "Las Alumnos No han sido registrados correctamente";
                    }
                    else
                    {
                        ViewBag.Message = "Las Alumnos han sido registrados correctamente";
                    }

                }

            }
            return PartialView("Modal");
        }
    }
}
