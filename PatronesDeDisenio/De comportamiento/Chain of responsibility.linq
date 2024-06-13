<Query Kind="Program" />

void Main()
{
	// Se crean los diferentes manejadores y se "encadenan".
	IManejadorTransferencia validadorRol = new ValidadorRol();
	IManejadorTransferencia validadorAutorizaciones = new ValidadorAutorizaciones();
	IManejadorTransferencia validadorCuenta = new ValidadorCuenta();
	IManejadorTransferencia operadorTransferencia = new OperadorTransferencia();
	
	validadorRol.FijarSiguiente(validadorAutorizaciones).FijarSiguiente(validadorCuenta).FijarSiguiente(operadorTransferencia);
	
	// Se usa el primer manejador para iniciar la ejecución.
	validadorRol.Procesar(new Transferencia(12345, "bgarcia", "01234", "56789", 678.90M, 2));
	validadorRol.Procesar(new Transferencia(12346, "jmorales", "00123", "00567", 9999.99M, 5));
	validadorRol.Procesar(new Transferencia(12347, "wramirez", "000123", "00012", 100M, 2));
	validadorRol.Procesar(new Transferencia(12348, "cmarroquin", "00012", "23409", 200M, 0));
	
	// Mientras se mantenga una referencia, se puede iniciar la cadena desde cualquier manejador.
	validadorCuenta.Procesar(new Transferencia(12349, "mfernandez", "00123", "23409", 25M, 0));
}

// Esta es la interfaz de los manejadores que se pueden usar con una transferencia.
public interface IManejadorTransferencia
{
	IManejadorTransferencia FijarSiguiente(IManejadorTransferencia manejador);
	
	bool Procesar(Transferencia transferencia);
}

// El manejador base declara la referencia al siguiente manejador en la cadena e implementa el método para asignarlo.
public abstract class ManejadorBase : IManejadorTransferencia
{
	private IManejadorTransferencia siguienteManejador;
	
	// Aquí se implementa el método para asignar el siguiente manejador.
	public IManejadorTransferencia FijarSiguiente(IManejadorTransferencia manejador) => this.siguienteManejador = manejador;
	
	// La implementación de Procesar predeterminada llama al siguiente manejador si no es nulo.
	public virtual bool Procesar(Transferencia transferencia) => this.siguienteManejador?.Procesar(transferencia) ?? false;
}

// Este manejador comprueba si el usuario tiene el rol necesario para poder autorizar la transferencia.
public class ValidadorRol : ManejadorBase
{	
	public override bool Procesar(Transferencia transferencia)
	{
		if (this.ValidarRolUsuario(transferencia.Usuario))
		{
			Console.WriteLine("El usuario tiene el rol necesario para autorizar la transferencia...");
			return base.Procesar(transferencia);
		}
		else
		{
			Console.WriteLine("El usuario no tiene el rol necesario para autorizar la transferencia...");
			return false;
		}
	}
	
	private bool ValidarRolUsuario(string usuario)
	{
		Console.WriteLine($"Validando el rol del usuario {usuario}...");
		return usuario.Length % 2 == 0;
	}
}

// Este manejador graba y consulta el total de autorizaciones que tiene la transferencia.
public class ValidadorAutorizaciones : ManejadorBase
{
	public override bool Procesar(Transferencia transferencia)
	{
		// Un manejador puede decidir no sólo terminar la cadena generando su propia respuesta, también pueden decidir traladar la petición
        // sin hacer nada.
		if (transferencia.AutorizacionesRequeridas == 0)
		{
			Console.WriteLine("La transferencia no requiere autorizaciones...");
			return base.Procesar(transferencia);
		}
		
		if (this.RegistrarAutorizacion(transferencia.Identificador) >= transferencia.AutorizacionesRequeridas)
		{
			Console.WriteLine("Autorizaciones suficientes...");
			return base.Procesar(transferencia);
		}
		else
		{
			Console.WriteLine("Cantidad de autorizaciones insuficiente...");
			return false;
		}
	}
	
	private byte RegistrarAutorizacion(long identificadorTransferencia)
	{
		Console.WriteLine($"Registrando autorización para la transferencia {identificadorTransferencia}...");
		return 2;
	}
}

// Este manejador valida si el usuario tiene permisos de transferencia sobre la cuenta.
public class ValidadorCuenta : ManejadorBase
{
	public override bool Procesar(Transferencia transferencia)
	{
		
		if (this.ValidarPermisosCuenta(transferencia.Usuario, transferencia.CuentaDebito))
		{
			Console.WriteLine("El usuario tiene permisos de transferencia sobre la cuenta.");
			return base.Procesar(transferencia);
		}
		else
		{
			Console.WriteLine("El usuario no tiene permisos de transferencia sobre la cuenta.");
			return false;
		}
	}
	
	private bool ValidarPermisosCuenta(string usuario, string cuenta)
	{
		Console.WriteLine($"Validando permisos del usuario {usuario} sobre la cuenta {cuenta}...");
		return usuario.Length % 2 == 0 && cuenta.Length % 2 != 0;
	}
}

// Este es el último manejador, se encarga de ejecutar la transferencia.
public class OperadorTransferencia : ManejadorBase
{
	public override bool Procesar(Transferencia transferencia)
	{
		Console.WriteLine($"Ejecutando la transferencia {transferencia.Identificador}...");
		return true;
	}
}

public record Transferencia(
    long Identificador, string Usuario, string CuentaDebito, string CuentaCredito, decimal Monto, byte AutorizacionesRequeridas) { }