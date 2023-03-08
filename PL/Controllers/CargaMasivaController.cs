using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    public class CargaMasivaController : Controller
    {
        private readonly IConfiguration _configuration;

        public CargaMasivaController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public ActionResult CargaMasiva()
        {
            return View();
        }
        [HttpPost]
        public ActionResult MateriaCargaMasiva(ML.Alumno alumno)
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

                    if (extensionArchivo == extensionModulo)
                    {
                        string filePath = Path.Combine(_hostingEnvironment.ContentRootPath, folderPath, Path.GetFileNameWithoutExtension(fileName)) + '-' + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";

                        if (!System.IO.File.Exists(filePath))
                        {
                            using (FileStream stream = new FileStream(filePath, FileMode.Create))
                            {
                                archivo.CopyTo(stream);
                            }

                            string connectionString = _configuration["ConnectionStringExcel:value"] + filePath;
                            ML.Result resultMaterias = BL.Materia.ConvertXSLXtoDataTable(connectionString);

                            if (resultMaterias.Correct)
                            {
                                ML.Result resultValidacion = BL.Materia.ValidarExcel(resultMaterias.Objects);
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

                ML.Result resultData = BL.Materia.ConvertXSLXtoDataTable(connectionString);
                if (resultData.Correct)
                {
                    ML.Result resultErrores = new ML.Result();
                    resultErrores.Objects = new List<object>();

                    foreach (ML.Materia materiaItem in resultData.Objects)
                    {

                        ML.Result resultAdd = BL.Materia.Add(materiaItem);
                        if (!resultAdd.Correct)
                        {
                            resultErrores.Objects.Add("No se insertó el Alumno con nombre: " + materiaItem.Nombre + " Creditos:" + materiaItem.Creditos + "Costo:" + materiaItem.Costo + " Error: " + resultAdd.ErrorMessage);
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
                        ViewBag.Message = "Las Materias No han sido registrados correctamente";
                    }
                    else
                    {
                        ViewBag.Message = "Las Materias han sido registrados correctamente";
                    }

                }

            }
            return PartialView("Modal");
        }
    }
}
