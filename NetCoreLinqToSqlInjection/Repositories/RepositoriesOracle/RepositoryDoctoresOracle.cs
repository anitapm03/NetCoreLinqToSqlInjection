using NetCoreLinqToSqlInjection.Models;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace NetCoreLinqToSqlInjection.Repositories.RepositoriesOracle
{
    /*--LENGUAJE PL/SQL
CREATE OR REPLACE PROCEDURE SP_DELETE_DOCTOR
(P_IDDOCTOR DOCTOR.DOCTOR_NO%TYPE)
AS
BEGIN
  DELETE FROM DOCTOR WHERE DOCTOR_NO=P_IDDOCTOR;
  COMMIT;
 END;

    MODIFICAR

    CREATE OR REPLACE PROCEDURE MODIFICAR_DOCTOR (
    P_ID IN DOCTOR.DOCTOR_NO%TYPE,
    P_APELLIDO IN DOCTOR.APELLIDO%TYPE,
    P_ESPECIALIDAD IN DOCTOR.ESPECIALIDAD%TYPE,
    P_SALARIO IN DOCTOR.SALARIO%TYPE,
    P_IDHOSPITAL IN DOCTOR.HOSPITAL_COD%TYPE
)
IS
BEGIN
    UPDATE DOCTOR
    SET
        APELLIDO = P_APELLIDO,
        ESPECIALIDAD = P_ESPECIALIDAD,
        SALARIO = P_SALARIO,
        HOSPITAL_COD = P_IDHOSPITAL
    WHERE
        DOCTOR_NO = P_ID;

    COMMIT;
END MODIFICAR_DOCTOR;

    */
    public class RepositoryDoctoresOracle : IRepositoryDoctores
    {
        private DataTable tablaDoctores;
        private OracleConnection cn;
        private OracleCommand com;

        public RepositoryDoctoresOracle()
        {
            string connectionString =
                @"Data Source=LOCALHOST:1521/XE; Persist Security Info=True;
                    User Id=SYSTEM; Password=oracle";
            this.cn = new OracleConnection(connectionString);
            this.com = new OracleCommand();
            this.com.Connection = this.cn;
            string sql = "SELECT * FROM DOCTOR";
            OracleDataAdapter adapter = new OracleDataAdapter(sql, this.cn);
            this.tablaDoctores = new DataTable();
            adapter.Fill(this.tablaDoctores);
        }

        public List<Doctor> GetDoctores()
        {
            //la ventaja de Linq es que se abstrae del origen de datos
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

        

        public void InsertarDoctor(int id, string apellido, string especialidad, 
            int salario, int idHospital)
        {
            string sql = "INSERT INTO DOCTOR VALUES (" +
                ":HOSPITAL, :ID, :APELLIDO, :ESPECIALIDAD, :SALARIO)";

            OracleParameter pamID = new OracleParameter(":ID",id);
            OracleParameter pamHospital = new OracleParameter(":HOSPITAL", idHospital);
            OracleParameter pamApellido = new OracleParameter(":APELLIDO", apellido);
            OracleParameter pamEspecialidad = new OracleParameter(":ESPECIALIDAD", especialidad);
            OracleParameter pamSalario = new OracleParameter(":SALARIO", salario);
            this.com.Parameters.Add(pamID);
            this.com.Parameters.Add(pamHospital);
            this.com.Parameters.Add(pamApellido);
            this.com.Parameters.Add(pamEspecialidad);
            this.com.Parameters.Add(pamSalario);

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
            if(consulta.Count() == 0)
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
            OracleParameter pamIdDoctor = new OracleParameter(":P_IDDOCTOR", idDoctor);
            this.com.Parameters.Add(pamIdDoctor);
            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = "SP_DELETE_DOCTOR";
            this.cn.Open();
            int af = this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();
        }

        public void ModificarDoctor(int idDoctor)
        {
            throw new NotImplementedException();
        }

        public Doctor FindDoctor(int idDoctor)
        {
            throw new NotImplementedException();
        }
    }
}
