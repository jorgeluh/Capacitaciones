<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

void Main()
{
    // Como el proxy implementa la misma interfaz que el objeto original, son intercambiables sin problema para el código cliente.
	Console.WriteLine("Iniciando afiliación de teléfonos...");
	ISistemaNotificaciones sistemaNotificaciones = new SistemaNotificacionesAsincrono();
	for (int i = 55000000; i < 55000100; i++)
	{
		sistemaNotificaciones.Afiliar(i.ToString());
	}
	
	Console.WriteLine("La afiliación de teléfonos ha finalizado. Continuando con otras tareas...");
}

// Esta es la implementación original. Por desgracia tiene una alta latencia para responder.
public class SistemaNotificaciones : ISistemaNotificaciones
{
	public void Afiliar(string numeroTelefono)
	{
		Thread.Sleep(500);
		Console.WriteLine($"Afiliando el teléfono {numeroTelefono}...");
	}
}

// Se crea una interfaz basada en la implementación original.
public interface ISistemaNotificaciones
{
	void Afiliar(string numeroTelefono);
}

// Este es el proxy. La funcionalidad que agrega es la ejecución asíncrona de las operaciones.
public class SistemaNotificacionesAsincrono : ISistemaNotificaciones
{
    // Proxy es muy similar a decorator, pero en este caso el proxy administra el ciclo de vida del objeto que "envuelve".
	private static SistemaNotificaciones sistemaReal;
	
	public void Afiliar(string numeroTelefono)
	{
		if (sistemaReal == null)
		{
			sistemaReal = new SistemaNotificaciones();
		}
		
        // Si se ejecutan solicitudes a un servicio externo, tiene mucho sentido hacerlo de manera asíncrona para seguir aprovechando los
        // recursos de la máquina local.
		_ = Task.Run(() => sistemaReal.Afiliar(numeroTelefono));
	}
}