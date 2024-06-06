<Query Kind="Program" />

void Main()
{
	Menu menu = new Menu("Menú");
	
	Menu cuentas = new Menu("Información de cuentas");
	Menu cuentasMonetarias = new Menu("Monetarias");
	cuentasMonetarias.AgregarElemento(new Opcion("Cuentas disponibles"));
	cuentasMonetarias.AgregarElemento(new Opcion("Estado de cuenta"));
	cuentas.AgregarElemento(cuentasMonetarias);
	Menu cuentasAhorro = new Menu("De ahorro");
	cuentasAhorro.AgregarElemento(new Opcion("Cuentas disponibles"));
	cuentasAhorro.AgregarElemento(new Opcion("Estado de cuenta"));
	cuentas.AgregarElemento(cuentasAhorro);
	Menu tarjetaCredito = new Menu("Tarjeta de crédito");
	tarjetaCredito.AgregarElemento(new Opcion("Tarjetas disponibles"));
	tarjetaCredito.AgregarElemento(new Opcion("Estado de cuenta"));
	tarjetaCredito.AgregarElemento(new Opcion("Saldos"));
	cuentas.AgregarElemento(tarjetaCredito);
	menu.AgregarElemento(cuentas);
	
	Menu transferencias = new Menu("Transferencias");
	transferencias.AgregarElemento(new Opcion("Propias"));
	transferencias.AgregarElemento(new Opcion("A terceros"));
	transferencias.AgregarElemento(new Opcion("ACH"));
	transferencias.AgregarElemento(new OpcionNueva("Internacionales"));
	menu.AgregarElemento(transferencias);
	
	Menu pagoServicios = new Menu("Pago de servicios");
	pagoServicios.AgregarElemento(new Opcion("Búsqueda"));
	pagoServicios.AgregarElemento(new Opcion("Pagos programados"));
	pagoServicios.AgregarElemento(new Opcion("Historial de pagos"));
	menu.AgregarElemento(pagoServicios);
	
	// Tras crear el menú, lo único que le interesa al código cliente es mostrarlo.
	MostrarMenu(menu);
}

// Este método sólo sirve para encapsular la llamada a la raíz y ocultar el parámetro para el primer nivel.
public void MostrarMenu(ElementoMenu menu)
{
	byte nivelInicial = 1;
	menu.Mostrar(nivelInicial);
}

// Esta es la abstracción de los objetos compuestos. Se usó una clase porque el texto es común a todos los elementos.
public abstract class ElementoMenu
{
	public ElementoMenu(string texto) => this.Texto = texto;

	// Esta es una "implementación concreta" de la propiedad para el texto del elemento.
	public string Texto { get; private set; }

	// Se obliga a las subclases a implementar cómo se deben mostrar. Esto es lo que diferencia al patrón de diseño de una estructura de
    // árbol.
	public abstract void Mostrar(byte nivel);
}

// Implementación para un objeto compuesto, que contiene más elementos.
public class Menu : ElementoMenu
{
	// El tipo de la colección es el de la abstracción, más allá de eso no importan sus elementos.
	private readonly List<ElementoMenu> elementos = new List<ElementoMenu>();
	
	public Menu(string texto) : base(texto) { }
	
	public void AgregarElemento(ElementoMenu elemento) => this.elementos.Add(elemento);

	public override void Mostrar(byte nivel)
	{
		Console.Write(new string(' ', nivel * 8));
		Console.WriteLine($"<h{nivel}>{this.Texto}</h{nivel}>");
		nivel++;
		foreach (ElementoMenu elemento in this.elementos)
		{
			// Sólo se muestra cada elemento. ¿Acaso importa el tipo?
			elemento.Mostrar(nivel);
		}
	}
}

// Implementación de una opción del menú a la que se puede acceder.
public class Opcion : ElementoMenu
{
	public Opcion(string texto) : base(texto) { }

	// Cómo se muestra una opción individual es mucho más sencillo.
	public override void Mostrar(byte nivel) => Console.WriteLine($"{new string(' ', nivel * 8)}<div>{this.Texto}</div>");
}

// Se extiende la clase Opcion para hacer un pequeño cambio cuando la opción es nueva e ilustrar el polimorfismo.
public class OpcionNueva : ElementoMenu
{
	public OpcionNueva(string texto) : base(texto) { }

	public override void Mostrar(byte nivel) => Console.WriteLine($"{new string(' ', nivel * 8)}<div class=\"nuevo\">{this.Texto}</div>");
}