namespace NetCoreLinqToSqlInjection.Models
{
    public interface IRepositoryDoctores
    {
        List<Doctor> GetDoctores();
        void InsertarDoctor
            (int id, string apellido, string especialidad,
            int salario, int idHospital);

        List<Doctor> GetDoctoresEspecialidad(string especialidad);

        void DeleteDoctor(int idDoctor);

        void ModificarDoctor(int idDoctor);

        Doctor FindDoctor(int idDoctor);
    }
}
