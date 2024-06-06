<Query Kind="Program" />

void Main()
{
	// Se instancia el cliente del servicio y se decora según sea necesario.
	IServicio servicio = new ClienteServicio();
	servicio = new ValidadorEntrada(servicio);
	servicio = new Filtro(servicio);
	servicio = new ManejadorExcepciones(servicio);
	servicio = new Bitacora(servicio);
	
	servicio.EjecutarConsulta("<cuenta><numero>01234</numero></cuenta>");
	servicio.EjecutarConsulta("<cuenta></cuenta>");
}

// Interfaz donde se declara la misma operación de la implementación que se va a decorar.
public interface IServicio
{
	string EjecutarConsulta(string peticion);
}

// Esta es la implementación original, que se puede decorar a conveniencia para mejorarla.
public class ClienteServicio : IServicio
{
	public string EjecutarConsulta(string peticion)
	{
		XElement raiz = XDocument.Parse(peticion).Root;
		string numeroCuenta = raiz.Element("numero").Value;
		Console.WriteLine($"Consultando la cuenta {numeroCuenta}...");
		return $"<cuenta><numero>{numeroCuenta}</numero><nombre>Nombre Cuenta {numeroCuenta}</nombre><saldo>123.45</saldo></cuenta>";
	}
}

// Este es el decorador base. Es abstracto pero contiene la referencia a un IServicio para la composición.
public abstract class DecoradorBase : IServicio
{
	protected readonly IServicio servicio;
	
	public DecoradorBase(IServicio servicio) => this.servicio = servicio;
	
	public virtual string EjecutarConsulta(string peticion) => this.servicio.EjecutarConsulta(peticion);
}

// Decorador que valida el formato de la cadena de entrada.
public class ValidadorEntrada : DecoradorBase
{
	public ValidadorEntrada(IServicio servicio) : base(servicio) { }
	
	public override string EjecutarConsulta(string peticion)
	{
		if (!Regex.IsMatch(peticion, @"<cuenta><numero>\d+</numero></cuenta>", RegexOptions.IgnoreCase))
		{
			return "XML de petición no válido.";
		}
		
		return base.EjecutarConsulta(peticion);
	}
}

// Este decorador captura excepciones y asigna el texto de la excepción como respuesta.
public class ManejadorExcepciones : DecoradorBase
{
	public ManejadorExcepciones(IServicio servicio) : base(servicio) { }

	public override string EjecutarConsulta(string peticion)
	{
		string respuesta;
		try
		{
			respuesta = base.EjecutarConsulta(peticion);
		}
		catch (Exception ex)
		{
			Console.WriteLine("Capturando excepción...");
			respuesta = $"Error al llamar al servicio: {ex}";
		}
		
		return respuesta;
	}
}

// Este decorador modifica la respuesta para ocultar información que no debería quedar grabada en una bitácora.
public class Filtro : DecoradorBase
{
	public Filtro(IServicio servicio) : base(servicio) { }
	
	public override string EjecutarConsulta(string peticion)
	{
		string respuesta = base.EjecutarConsulta(peticion);
		Console.WriteLine("Ocultando el saldo de la cuenta...");
		respuesta = Regex.Replace(respuesta, @"<saldo>[\d.]+</saldo>", "<saldo>*****</saldo>");
		
		return respuesta;
	}
}

// Este decorador graba la petición y la respuesta en una bitácora.
public class Bitacora : DecoradorBase
{
	public Bitacora(IServicio servicio) : base(servicio) { }
	
	public override string EjecutarConsulta(string peticion)
	{
		Console.WriteLine($"Petición: {peticion}");
		string respuesta = base.EjecutarConsulta(peticion);
		Console.WriteLine($"Respuesta: {respuesta}");
		return respuesta;
	}
}