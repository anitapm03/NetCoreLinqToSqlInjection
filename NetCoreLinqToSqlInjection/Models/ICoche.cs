namespace NetCoreLinqToSqlInjection.Models
{
    public interface ICoche
    {

        //las interfaces no tienen ambito, solamente los
        //metodos y propiedades qy¡ue debe tener un objeto
        string Marca { get; set; }
        string Modelo { get; set; }
        string Imagen { get; set; }
        int Velocidad { get; set; }
        int VelocidadMaxima { get; set; }

        int Acelerar();
        int Frenar();
    }
}
