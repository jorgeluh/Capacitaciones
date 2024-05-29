<Query Kind="Program" />

void Main()
{
	// Esta prueba está pensada para que múltiples hilos traten de acceder la instancia al mismo tiempo y comprobar si realmente se está
    // creando una sola vez.
	Random generador = new Random();
	Thread hilo = null;
    for (int i = 0; i < 10000; i++)
	{
		hilo = new Thread(() => EjecutarProceso(generador.Next(1000, 1010)));
		hilo.Start();
	}
}

public void EjecutarProceso(int milisegundosEspera)
{
	// Esta espera aleatoria hace que los hilos se "amontonen" para intentar acceder al mismo tiempo a la instancia.
	Thread.Sleep(milisegundosEspera);
	
	// Como la función ObtenerValor() no es estática, se requiere una instancia de ConfiguracionesAplicacion pero sólo se puede obtener por
    // medio de la función ObtenerInstancia(), es imposible usar su constructor desde otra clase.
    ConfiguracionesAplicacion configuraciones = ConfiguracionesAplicacion.ObtenerInstancia();
    string direccionServicio = configuraciones.ObtenerValor<string>("DireccionServicio");
    Console.WriteLine(direccionServicio);
}

// La clase singleton cumple con sus propias funciones, su característica es que sólo se necesita una instancia por proceso por algún
// motivo.
public class ConfiguracionesAplicacion
{
    // Este objeto sirve para bloquear todos los hilos que puedan tratar de crear una nueva instancia mientras se crea la primera (y única).
    private static readonly object Candado = new object();
    
    // La única instancia de la clase se mantiene como un atributo privado estático de la misma clase.
    private static ConfiguracionesAplicacion instancia;
    
    private readonly Dictionary<string, object> repositorioConfiguraciones;
    
    // El constructor debe ser privado para que sea imposible crear una instancia que no sea por medio de ObtenerInstancia(). Por lo demás
    // se trata de un constructor normal con todo el código de inicialización necesario.
    private ConfiguracionesAplicacion()
    {
		// El código dentro del constructor no debe considerar la concurrencia, de eso se encarga ObtenerInstancia().
        Console.WriteLine("Construyendo la instancia usando el constructor privado...");
        this.repositorioConfiguraciones = new Dictionary<string, object>();
        this.ConsultarValores();
    }
    
    // Este método es la única manera de obtener una instancia de la clase.
    public static ConfiguracionesAplicacion ObtenerInstancia()
    {
        // Se hace una validación inicial para saber si la instancia aún no se ha creado.
        if (instancia == null)
        {
            // El bloqueo es costoso pues obliga a todos los hilos del proceso a ejecutarse secuencialmente en el bloque siguiente.
            Console.WriteLine("La instancia es nula, creando bloqueo...");
            lock (Candado)
            {
                // Varios hilos pudieron haber pasado la primera validación mientras la instancia era nula. El primero que obtuvo el bloqueo
                // se encarga de crear la instancia y los siguientes no lo harán debido a esta segunda validación.
                if (instancia == null)
                {
                    Console.WriteLine("La instancia no existe, creando una...");
                    instancia = new ConfiguracionesAplicacion();
                }
            }
        }
        
        Console.WriteLine("La instancia ya existe.");
        return instancia;
    }
    
    public T ObtenerValor<T>(string nombreConfiguracion)
    {
        Console.WriteLine($"Consultando la configuración {nombreConfiguracion}...");
        return (T)this.repositorioConfiguraciones[nombreConfiguracion];
    }
    
    private void ConsultarValores()
    {
        Console.WriteLine("Cargando las configuraciones...");
        this.repositorioConfiguraciones.Add("CantidadHilos", 10 );
        this.repositorioConfiguraciones.Add("DireccionServicio", "http://servicio.com/api/consultas");
        this.repositorioConfiguraciones.Add("FechaInicial", new DateTime(2024, 1, 1));
    }
}