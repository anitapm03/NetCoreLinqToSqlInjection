namespace NetCoreLinqToSqlInjection.Models
{
    public interface IRepositoryDoctores
    {
        List<Doctor> GetDoctores();
        void InsertarDoctor
            (int id, string apellido, string especialidad,
            int salario, int idHospital);
    }
}
