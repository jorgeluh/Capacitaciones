<Query Kind="Program" />

void Main()
{
    // El estado de todo el comprobante se mantiene entre el comprobante y la plantilla. Por eso se requieren objetos de ambos tipos.
	Comprobante comprobante;
	PlantillaComprobante plantilla;
	for (int i = 1; i < 100001; i++)
	{
        // Se usa el índice para generar una gran cantidad de comprobantes. Se crean plantillas adicionales.
		plantilla = FabricaPlantillaComprobante.ObtenerPlantilla((Banco)(i % 5));
		comprobante = new Comprobante(
			i.ToString().PadLeft(6, '0'),
			(1000000 - i).ToString().PadLeft(6, '0'),
			i / 100M,
			new DateTime(2024, i % 12 + 1, i % 28 + 1),
			plantilla);
		comprobante.Generar();
	}
}

// Esta clase contiene los datos específicos de cada comprobante.
public class Comprobante
{
	private readonly string cuentaDebito;
	
	private readonly string cuentaCredito;
	
	private readonly decimal monto;
	
	private readonly DateTime fecha;
	
	// Se mantiene una referencia al objeto con los datos comunes para este tipo de reporte.
	private readonly PlantillaComprobante plantilla;
	
    // En el constructor se recibe como parámetro el objeto con los datos comunes.
	public Comprobante(string cuentaDebito, string cuentaCredito, decimal monto, DateTime fecha, PlantillaComprobante plantilla)
	{
		this.cuentaDebito = cuentaDebito;
		this.cuentaCredito = cuentaCredito;
		this.monto = monto;
		this.fecha = fecha;
		this.plantilla = plantilla;
	}
	
    // Este método le traslada la información específica faltante al método de la plantilla.
	public void Generar() => this.plantilla.Generar(this.cuentaDebito, this.cuentaCredito, this.monto, this.fecha);
}

// La plantilla contiene datos generales como título, formato del texto y la imagen del logotipo.
public class PlantillaComprobante
{
	public readonly string titulo;

	public readonly string contenido;
	
    // El logotipo puede requerir mucha memoria, por lo que es ideal tenerlo en la plantilla. Incluso se pudo haber reutilizado entre
    // plantillas.
	public readonly byte[] imagenLogotipo;
	
	public PlantillaComprobante(string titulo, string contenido, byte[] imagenLogotipo)
	{
		this.titulo = titulo;
		this.contenido = contenido;
		this.imagenLogotipo = imagenLogotipo;
	}
	
    // El método de la plantilla no cuenta con la información específica, se debe solicitar como parámetro.
	public void Generar(string cuentaDebito, string cuentaCredito, decimal monto, DateTime fecha)
	{
		Console.WriteLine("\r\nGenerando comprobante de transferencia...");
		Console.WriteLine($"{this.MostrarLogotipo()} - {this.titulo} - {this.contenido}", cuentaDebito, cuentaCredito, monto, fecha);
	}
	
    // Método auxiliar para convertir el vector de bytes a "imagen".
	private string MostrarLogotipo() => Encoding.UTF8.GetString(this.imagenLogotipo);
}

// La fábrica de plantillas depende de las distintas variaciones de datos comunes.
public static class FabricaPlantillaComprobante
{
	private static Dictionary<Banco, PlantillaComprobante> Plantillas;
	
    // Se inicializa el "repositorio" de plantillas sólo con una local y una interbancaria. El resto se crea y almacena en tiempo de
    // ejecución.
	static FabricaPlantillaComprobante()
	{
		PlantillaComprobante comprobanteLocal = new PlantillaComprobante(
			"Transferencia local",
			"Transferencia de la cuenta {0} a {1} por {2:N2} el día {3:dd/MM/yyyy}",
			new byte[] { 126, 40, 61, 94, 226, 128, 165, 94, 41, 227, 131, 142 });
		PlantillaComprobante comprobanteBancoA = new PlantillaComprobante(
			"Transferencia hacia BancoA",
			"Transferencia interbancaria de la cuenta {0} a {1} por {2:N2} el día {3:dd/MM/yyyy}",
			new byte[] { 126, 40, 61, 94, 226, 128, 165, 94, 41, 227, 131, 142 });
		Plantillas = new Dictionary<Banco, PlantillaComprobante>()
			{
				{ Banco.Local, comprobanteLocal },
				{ Banco.BancoA, comprobanteBancoA },
			};
	}
	
    // La función para obtener la plantilla depende de los distintos tipos de comprobante.
	public static PlantillaComprobante ObtenerPlantilla(Banco banco)
	{
        // Si no se encuentra la plantilla necesaria, se crea y se almacena en el repositorio de plantillas.
		if (!Plantillas.ContainsKey(banco))
		{
            Console.WriteLine($"\r\nCreando plantilla para {banco}...");
			PlantillaComprobante plantilla = new PlantillaComprobante(
				$"Transferencia hacia {banco}",
				$"Transferencia interbancaria de la cuenta {{0}} a {{1}} por {{2:N2}} el día {{3:dd/MM/yyyy}}",
				new byte[] { 126, 40, 61, 94, 226, 128, 165, 94, 41, 227, 131, 142 });
			Plantillas.Add(banco, plantilla);
		}
		
		return Plantillas[banco];
	}
}

public enum Banco
{
	Local,
	BancoA,
	BancoB,
	BancoC,
	BancoD,
}