<Query Kind="Program" />

void Main()
{
	PagarServicio(50M, Moneda.Local, Servicio.Agua);
	PagarServicio(25M, Moneda.DolarUS, Servicio.EnergiaElectrica);
	PagarServicio(125M, Moneda.PesoMX, Servicio.Telefono);
}

// Código cliente donde se usan las implementaciones y las abstracciones para combinar servicios y monedas.
public bool PagarServicio(decimal monto, Moneda monedaOrigen, Servicio servicio)
{
	// Estos switch independientes son mucho más sencillos que los switch anidados si se usara herencia.
	IConversionMoneda conversionMoneda;
	switch (monedaOrigen)
	{
		case Moneda.DolarUS:
			conversionMoneda = new ConversionDolarUS();
			break;
		case Moneda.PesoMX:
			conversionMoneda = new ConversionPesoMX();
			break;
		default:
			conversionMoneda = new ConversionQuetzal();
			break;
	}
	
	PagoServicio pagoServicio = null;
	switch (servicio)
	{
		case Servicio.Agua:
			pagoServicio = new PagoAgua(conversionMoneda);
			break;
		case Servicio.EnergiaElectrica:
			pagoServicio = new PagoEnergiaElectrica(conversionMoneda);
			break;
		case Servicio.Telefono:
			pagoServicio = new PagoTelefono(conversionMoneda);
			break;
	}
	
	return pagoServicio.RealizarPago(monto, monedaOrigen);
}

// Clase base para las abstracciones, en este caso el pago de servicios.
public abstract class PagoServicio
{
	// Composición con la implementación para convertir la moneda.
	protected IConversionMoneda conversionMoneda;
	
	// El que una clase sea abstracta no significa que no pueda tener un constructor para solicitar los parámetros necesarios.
	public PagoServicio(IConversionMoneda conversionMoneda) => this.conversionMoneda = conversionMoneda;
	
	// Método abstracto para realizar el pago.
	public abstract bool RealizarPago(decimal monto, Moneda monedaOrigen);
}

// Clase para pago de agua.
public class PagoAgua : PagoServicio
{
	public PagoAgua(IConversionMoneda conversionMoneda) : base(conversionMoneda) { }
	
	public override bool RealizarPago(decimal monto, Moneda monedaOrigen)
	{
		decimal montoDestino = this.conversionMoneda.ConvertirAQuetzales(monto);
		Console.WriteLine($"Pagando servicio de agua por {montoDestino}...");
		return true;
	}
}

// Clase para pago de energía eléctrica.
public class PagoEnergiaElectrica : PagoServicio
{
	public PagoEnergiaElectrica(IConversionMoneda conversionMoneda) : base(conversionMoneda) { }
	
	public override bool RealizarPago(decimal monto, Moneda monedaOrigen)
	{
		decimal montoDestino = this.conversionMoneda.ConvertirAQuetzales(monto);
		Console.WriteLine($"Pagando servicio de energía eléctrica por {montoDestino}...");
		return true;
	}
}

// Clase para pago de teléfono.
public class PagoTelefono : PagoServicio
{
	public PagoTelefono(IConversionMoneda conversionMoneda) : base(conversionMoneda) { }
	
	public override bool RealizarPago(decimal monto, Moneda monedaOrigen)
	{
		decimal montoDestino = this.conversionMoneda.ConvertirAQuetzales(monto);
		Console.WriteLine($"Pagando servicio de teléfono por {montoDestino}...");
		return true;
	}
}

// Interfaz para las implementaciones de conversión de moneda.
public interface IConversionMoneda
{
	decimal ConvertirAQuetzales(decimal monto);
}

// Clase para conversión de quetzales a quetzales, sólo sirve para no tener un "caso especial" que obligue a cambiar la lógica.
public class ConversionQuetzal : IConversionMoneda
{
	public decimal ConvertirAQuetzales(decimal monto) => monto;
}

// Clase para conversión de dólares a quetzales.
public class ConversionDolarUS : IConversionMoneda
{
	public decimal ConvertirAQuetzales(decimal monto)
	{
		Console.WriteLine("Convirtiendo monto desde dólares estadounidenses...");
		return monto * this.ConsultarTipoCambio();
	}
	
	private decimal ConsultarTipoCambio()
	{
		Console.WriteLine("Consultando tipo de cambio de dólar estadounidense a qutezales...");
		return 8M;
	}
}

// Clase para conversión de pesos a quetzales.
public class ConversionPesoMX : IConversionMoneda
{
	public decimal ConvertirAQuetzales(decimal monto)
	{
		Console.WriteLine("Convirtiendo monto desde peso mexicano...");
		return monto * this.ConsultarTipoCambio();
	}
	
	private decimal ConsultarTipoCambio()
	{
		Console.WriteLine("Consultando tipo de cambio de peso mexicano a qutezales...");
		return 0.8M;
	}
}

public enum Moneda
{
	Local,
	DolarUS,
	PesoMX,
}

public enum Servicio
{
	Agua,
	EnergiaElectrica,
	Telefono,
}