using System.Data;
using System.Data.SqlClient;
using NetCoreLinqToSqlInjection.Models;

namespace NetCoreLinqToSqlInjection.Repositories

/*
 CREATE PROCEDURE SP_DELETE_DOCTOR
    (@IDDOCTOR INT)
    AS
        DELETE FROM DOCTOR 
        WHERE DOCTOR_NO=@IDDOCTOR 
    GO

CREATE PROCEDURE MODIFICAR_DOCTOR 
@P_DOCTOR_NO INT,
@P_APELLIDO NVARCHAR(MAX),
@P_ESPECIALIDAD NVARCHAR(MAX),
@P_SALARIO INT,
@P_HOSPITAL_COD INT
AS
BEGIN
UPDATE DOCTOR
SET
    APELLIDO = @P_APELLIDO,
    ESPECIALIDAD = @P_ESPECIALIDAD,
    SALARIO = @P_SALARIO,
    HOSPITAL_COD = @P_HOSPITAL_COD
WHERE
    DOCTOR_NO = @P_DOCTOR_NO;

PRINT 'Doctor modificado exitosamente.';
END;
 */
{
    public class RepositoryDoctoresSQLServer: IRepositoryDoctores
    {
        private DataTable tablaDoctores;
        private SqlConnection cn;
        private SqlCommand com;

        public RepositoryDoctoresSQLServer()
        {
            string connectionString = "Data Source=LOCALHOST\\SQLEXPRESS;Initial Catalog=HOSPITAL;Persist Security Info=True;User ID=SA;Password=MCSD2023";
            this.cn = new SqlConnection(connectionString);
            this.com = new SqlCommand();
            this.com.Connection = this.cn;
            this.tablaDoctores = new DataTable();
            string sql = "SELECT * FROM DOCTOR";
            SqlDataAdapter ad = new SqlDataAdapter(sql, this.cn);

            ad.Fill(this.tablaDoctores);
        
        }

        public List<Doctor> GetDoctores()
        {
            var consulta = from datos in this.tablaDoctores.AsEnumerable()
                           select datos;
            List<Doctor> doctores = new List<Doctor>();
            foreach (var row in consulta)
            {
                Doctor doc = new Doctor
                {
                    IdDoctor = row.Field<int>("DOCTOR_NO"),
                    Apellido = row.Field<string>("APELLIDO"),
                    Especialidad = row.Field<string>("ESPECIALIDAD"),
                    Salario = row.Field<int>("SALARIO"),
                    IdHospital = row.Field<int>("HOSPITAL_COD")
                };
                doctores.Add(doc);
            }
            return doctores;

        }

        public void InsertarDoctor
            (int id, string apellido, string especialidad, 
            int salario, int idHospital)
        {
            string sql = "INSERT INTO DOCTOR VALUES (" +
                "@HOSPITAL, @ID, @APELLIDO, @ESPECIALIDAD, @SALARIO)";
            this.com.Parameters.AddWithValue("@HOSPITAL", idHospital);
            this.com.Parameters.AddWithValue("@ID", id);
            this.com.Parameters.AddWithValue("@APELLIDO", apellido);
            this.com.Parameters.AddWithValue("@ESPECIALIDAD", especialidad);
            this.com.Parameters.AddWithValue("@SALARIO", salario);

            this.com.CommandType = CommandType.Text;
            this.com.CommandText = sql;
            this.cn.Open();
            int af = this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();
        }

        public List<Doctor> GetDoctoresEspecialidad(string especialidad)
        {
            var consulta = from datos in this.tablaDoctores.AsEnumerable()
                  where datos.Field<string>("ESPECIALIDAD").ToUpper() == especialidad.ToUpper()
                           select datos;
            if (consulta.Count() == 0)
            {
                return null;
            }
            else
            {
                List<Doctor> doctores = new List<Doctor>();
                foreach (var row in consulta)
                {
                    Doctor doc = new Doctor
                    {
                        IdDoctor = row.Field<int>("DOCTOR_NO"),
                        Apellido = row.Field<string>("APELLIDO"),
                        Especialidad = row.Field<string>("ESPECIALIDAD"),
                        Salario = row.Field<int>("SALARIO"),
                        IdHospital = row.Field<int>("HOSPITAL_COD")
                    };
                    doctores.Add(doc);
                }
                return doctores;
            }
        }

        public void DeleteDoctor(int idDoctor)
        {
            this.com.Parameters.AddWithValue("@IDDOCTOR", idDoctor);
            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = "SP_DELETE_DOCTOR";
            this.cn.Open();
            int af = this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear(); 
        }

        public void ModificarDoctor(int idDoctor)
        {
            this.com.Parameters.AddWithValue("@IDDOCTOR", idDoctor);



            this.cn.Open();
            int af = this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();
        }

        public Doctor FindDoctor(int idDoctor)
        {
            var consulta = from datos in this.tablaDoctores.AsEnumerable()
                           where datos.Field<int>("DOCTOR_NO") == idDoctor
                           select datos;
            var row = consulta.First();
            Doctor doc = new Doctor
            {
                    IdDoctor = row.Field<int>("DOCTOR_NO"),
                    Apellido = row.Field<string>("APELLIDO"),
                    Especialidad = row.Field<string>("ESPECIALIDAD"),
                    Salario = row.Field<int>("SALARIO"),
                    IdHospital = row.Field<int>("HOSPITAL_COD")
            };
            
            return doc;

        }
    }
}
