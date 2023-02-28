using Microsoft.EntityFrameworkCore;

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


    }
} 