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
                    int query = context.Database.ExecuteSqlRaw($"AlumnoAdd '{alumno.Nombre}', '{alumno.ApellidoPaterno}', '{alumno.ApellidoMaterno}', {alumno.Semestre.IdSemestre}, '{alumno.FechaNacimiento}','{alumno.Imagen}'");

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
                    string query = "SELECT * FROM [Sheet1$]";
                    using (OleDbCommand cmd = new OleDbCommand())
                    {
                        cmd.CommandText = query;
                        cmd.Connection = context;


                        OleDbDataAdapter da = new OleDbDataAdapter();
                        da.SelectCommand = cmd;

                        DataTable tableMateria = new DataTable();

                        da.Fill(tableMateria);

                        if (tableMateria.Rows.Count > 0)
                        {
                            result.Objects = new List<object>();

                            foreach (DataRow row in tableMateria.Rows)
                            {
                                ML.Alumno materia = new ML.Alumno();
                                materia.Nombre = row[0].ToString();
                                //row[1] = (row[1] == null) ? 0 : materia.Creditos;
                                //materia.Creditos = (materia.Creditos == null) ? 0 : materia.Creditos;
                                materia.ApellidoPaterno = byte.Parse(row[1].ToString());
                                materia.Costo = decimal.Parse(row[2].ToString());

                                materia.Semestre = new ML.Semestre();
                                materia.Semestre.IdSemestre = int.Parse(row[3].ToString());

                                materia.Grupo = new ML.Grupo();
                                materia.Grupo.Horario = row[4].ToString();

                                materia.Grupo.Plantel = new ML.Plantel();
                                materia.Grupo.Plantel.IdPlantel = int.Parse(row[5].ToString());

                                materia.Status = bool.Parse(row[6].ToString());

                                result.Objects.Add(materia);
                            }

                            result.Correct = true;

                        }

                        result.Object = tableMateria;

                        if (tableMateria.Rows.Count > 1)
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
                foreach (ML.Materia materia in Object)
                {
                    ML.ErrorExcel error = new ML.ErrorExcel();
                    error.IdRegistro = i++;

                    if (materia.Nombre == "")
                    {
                        error.Mensaje += "Ingresar el nombre  ";
                    }
                    if (materia.Creditos.ToString() == "")
                    {
                        error.Mensaje += "Ingresar los creditos ";
                    }
                    if (materia.Costo.ToString() == "")
                    {
                        error.Mensaje += "Ingresar el Costo ";
                    }
                    if (materia.Semestre.IdSemestre.ToString() == "")
                    {
                        error.Mensaje += "Ingresar el semestre ";
                    }
                    if (materia.Grupo.Horario == "")
                    {
                        error.Mensaje += "Ingresar el horario ";
                    }
                    if (materia.Grupo.Plantel.IdPlantel.ToString() == "")
                    {
                        error.Mensaje += "Ingresar el plantel ";
                    }
                    if (materia.Status.ToString() == "")
                    {
                        error.Mensaje += "Ingresar el status ";
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