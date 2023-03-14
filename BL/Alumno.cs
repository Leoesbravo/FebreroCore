using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.OleDb;

namespace BL
{
    public class Alumno
    {
        public static ML.Result Add(ML.Alumno alumno)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.IespinozaProgramacionNcapasGf2023Context context = new DL.IespinozaProgramacionNcapasGf2023Context())
                {
                    // query = context.AlumnoAdd(alumno.Nombre, alumno.ApellidoPaterno, alumno.ApellidoMaterno, alumno.Semestre.IdSemestre, alumno.Imagen);
                    int query = context.Database.ExecuteSqlRaw($"AlumnoAdd '{alumno.Nombre}', '{alumno.ApellidoPaterno}', '{alumno.ApellidoMaterno}', {alumno.Semestre.IdSemestre},'{alumno.Imagen}'");

                    if (query >= 1)
                    {
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No se insertó el registro";
                    }

                    result.Correct = true;

                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
            }
            return result;
        }
        public static ML.Result GetAll()
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.IespinozaProgramacionNcapasGf2023Context context = new DL.IespinozaProgramacionNcapasGf2023Context())
                {
                    //var query = context.AlumnoGetAll().ToList();
                    var query = context.Alumnos.FromSqlRaw("AlumnoGetAll").ToList();

                    if (query != null)
                    {
                        result.Objects = new List<object>();

                        foreach (var obj in query)
                        {
                            ML.Alumno alumno = new ML.Alumno();

                            alumno.IdAlumno = obj.IdAlumno;
                            alumno.Nombre = obj.Nombre;
                            alumno.ApellidoPaterno = obj.ApellidoPaterno;
                            alumno.ApellidoMaterno = obj.ApellidoMaterno;
                            alumno.FechaNacimiento = obj.FechaNacimiento.Value.ToString("dd/MM/yyyy");
                            alumno.Imagen = obj.Imagen;


                            alumno.Semestre = new ML.Semestre();
                            alumno.Semestre.IdSemestre = obj.IdSemestre.Value;
                            alumno.Semestre.Nombre = obj.SemestreNombre;

                            alumno.Status = obj.Status.Value;

                            result.Objects.Add(alumno);
                        }
                    }
                }



                result.Correct = true;


            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
                result.Correct = false;

            }
            return result;
        }

        public static ML.Result GetById(int IdAlumno)
        {
            ML.Result result = new ML.Result();

            try
            {

                using (DL.IespinozaProgramacionNcapasGf2023Context context = new DL.IespinozaProgramacionNcapasGf2023Context())//conexion
                {
                    var query = context.Alumnos.FromSqlRaw($"AlumnoGetById {IdAlumno}").AsEnumerable().FirstOrDefault();

                    if (query != null)
                    {

                        ML.Alumno alumno = new ML.Alumno();

                        alumno.IdAlumno = query.IdAlumno;
                        alumno.Nombre = query.Nombre;
                        alumno.ApellidoPaterno = query.ApellidoPaterno;
                        alumno.ApellidoMaterno = query.ApellidoMaterno;
                        alumno.FechaNacimiento = query.FechaNacimiento.Value.ToString("dd-MM-yyyy");
                        alumno.Imagen = query.Imagen;



                        alumno.Semestre = new ML.Semestre();
                        alumno.Semestre.IdSemestre = query.IdSemestre.Value;



                        result.Object = alumno; //boxing

                    }

                    result.Correct = true;

                }

            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
                result.Correct = false;

            }
            return result;
        }
        public static ML.Result ChangeStatus(int idAlumno, bool status)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.IespinozaProgramacionNcapasGf2023Context context = new DL.IespinozaProgramacionNcapasGf2023Context())
                {
                    int query = context.Database.ExecuteSqlRaw($"AlumnoChangeStatus '{idAlumno}', {status}");

                    if (query >= 1)
                    {
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No se insertó el registro";
                    }

                    result.Correct = true;

                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
            }
            return result;
        }
        public static ML.Result GetByNombre(string nombre)
        {
            ML.Result result = new ML.Result();

            try
            {

                using (DL.IespinozaProgramacionNcapasGf2023Context context = new DL.IespinozaProgramacionNcapasGf2023Context())//conexion
                {
                    var query = context.Alumnos.FromSqlRaw($"AlumnoGetByNombre {nombre}").AsEnumerable().FirstOrDefault();

                    if (query != null)
                    {

                        ML.Alumno alumno = new ML.Alumno();

                        //alumno.IdAlumno = query.IdAlumno;
                        alumno.Nombre = query.Nombre;
                        alumno.ApellidoPaterno = query.ApellidoPaterno;
                        //alumno.ApellidoMaterno = query.ApellidoMaterno;
                        //alumno.FechaNacimiento = query.FechaNacimiento.Value.ToString("dd-MM-yyyy");
                        //alumno.Imagen = query.Imagen;



                        //alumno.Semestre = new ML.Semestre();
                        //alumno.Semestre.IdSemestre = query.IdSemestre.Value;



                        result.Object = alumno; //boxing

                    }

                    result.Correct = true;

                }

            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
                result.Correct = false;

            }
            return result;
        }
        public static ML.Result ConvertXSLXtoDataTable(string connString)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (OleDbConnection context = new OleDbConnection(connString))
                {
                    //string query = "SELECT * FROM [Hoja1$]";
                    string query = "SELECT * FROM [Sheet1$]";
                    using (OleDbCommand cmd = new OleDbCommand())
                    {
                        cmd.CommandText = query;
                        cmd.Connection = context;


                        OleDbDataAdapter da = new OleDbDataAdapter();
                        da.SelectCommand = cmd;

                        DataTable tableAlumno = new DataTable();

                        da.Fill(tableAlumno);

                        if (tableAlumno.Rows.Count > 0)
                        {
                            result.Objects = new List<object>();

                            foreach (DataRow row in tableAlumno.Rows)
                            {
                                ML.Alumno alumno = new ML.Alumno();
                                alumno.Nombre = row[0].ToString();
                                alumno.ApellidoPaterno = row[1].ToString();
                                alumno.ApellidoMaterno = row[2].ToString();

                                result.Objects.Add(alumno);
                            }

                            result.Correct = true;

                        }

                        result.Object = tableAlumno;

                        if (tableAlumno.Rows.Count > 1)
                        {
                            result.Correct = true;
                        }
                        else
                        {
                            result.Correct = false;
                            result.ErrorMessage = "No existen registros en el excel";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;

            }

            return result;


        }
        public static ML.Result ValidarExcel(List<object> Object)
        {
            ML.Result result = new ML.Result();

            try
            {
                result.Objects = new List<object>();
                //DataTable  //Rows //Columns
                int i = 1;
                foreach (ML.Alumno alumno in Object)
                {
                    ML.ErrorExcel error = new ML.ErrorExcel();
                    error.IdRegistro = i++;

                    if (alumno.Nombre == "")
                    {
                        error.Mensaje += "Ingresar el nombre  ";
                    }
                    else
                    {

                    }

                    alumno.Nombre = (alumno.Nombre == "") ? error.Mensaje += "Ingresar el nombre  " : alumno.Nombre;



                    if (alumno.ApellidoPaterno == "")
                    {
                        error.Mensaje += "Ingresar los creditos ";
                    }
                    if (alumno.ApellidoMaterno.ToString() == "")
                    {
                        error.Mensaje += "Ingresar el Costo ";
                    }
                    if (error.Mensaje != null)
                    {
                        result.Objects.Add(error);
                    }


                }
                result.Correct = true;
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;

            }

            return result;
        }
    }
}