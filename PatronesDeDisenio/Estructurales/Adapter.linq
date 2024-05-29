<Query Kind="Program">
  <Namespace>System.Text.Json</Namespace>
</Query>

void Main()
{
    // El código cliente nunca se encarga de la consulta o la conversión de los datos.
	// Usando inyección de dependencias sólo es necesaria la referencia a IConsultaCuenta.
	ServicioSoapCuenta servicioSoap = new ServicioSoapCuenta();
	IConsultaCuenta consulta = new AdaptadorSoap(servicioSoap);
	DatosCuenta datos = consulta.ConsultarCuenta("01234");
	datos.Dump();
	
	ServicioRestCuenta servicioRest = new ServicioRestCuenta();
	consulta = new AdaptadorRest(servicioRest);
	datos = consulta.ConsultarCuenta("01234");
	datos.Dump();
}

// Esta es la interfaz compatible con el código cliente. Encapsula la lógica necesaria para obtener la información de la cuenta, sólo nos
// interesan los datos. No cómo se obtienen.
public interface IConsultaCuenta
{
	DatosCuenta ConsultarCuenta(string numero);
}

// Implementación de la interfaz de consulta para llamar a un servcicio SOAP.
public class AdaptadorSoap : IConsultaCuenta
{
	private readonly ServicioSoapCuenta servicio;
	
	public AdaptadorSoap(ServicioSoapCuenta servicio) => this.servicio = servicio;
	
    // La función de la interfaz se encarga de la conversión de formatos entre el código cliente y el destino.
	public DatosCuenta ConsultarCuenta(string numero)
	{
		string respuesta = this.servicio.ObtenerDatosCuenta(int.Parse(numero));
		Console.WriteLine($"Analizando respuesta SOAP \"{respuesta}\"...");
		XElement raiz = XDocument.Parse(respuesta).Root;
		return new DatosCuenta(raiz.Element("numero").Value, raiz.Element("nombre").Value, (decimal)raiz.Element("saldo"));
	}
}

// Implementación de la interfaz de consulta para llamar a un servcicio REST.
public class AdaptadorRest : IConsultaCuenta
{
	private readonly ServicioRestCuenta servicio;
	
	public AdaptadorRest(ServicioRestCuenta servicio) => this.servicio = servicio;
	
	public DatosCuenta ConsultarCuenta(string numero)
	{
		string respuesta = this.servicio.ObtenerInformacionCuenta(int.Parse(numero));
		Console.WriteLine($"Analizando respuesta REST \"{respuesta}\"...");
		Dictionary<string, JsonElement> valores = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(respuesta);
		return new DatosCuenta(valores["numero"].GetInt32().ToString(), valores["nombre"].GetString(), valores["saldo"].GetDecimal());
	}
}

// Simulador de servicio REST.
public class ServicioSoapCuenta
{
	public string ObtenerDatosCuenta(int numero)
	{
		return $"<cuenta><numero>{numero}</numero><nombre>Nombre Cuenta {numero}</nombre><saldo>123.45</saldo></cuenta>";
	}
}

// Simulador de servicio SOAP.
public class ServicioRestCuenta
{
	public string ObtenerInformacionCuenta(int numero)
	{
		return $"{{ \"numero\": {numero}, \"nombre\": \"Nombre Cuenta {numero}\", \"saldo\": 123.45 }}";
	}
}

public record DatosCuenta(string Numero, string Nombre, decimal Saldo)
{
}