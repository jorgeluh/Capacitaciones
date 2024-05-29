<Query Kind="Program" />

void Main()
{
	// Con el código actual, se pueden hacer pagos de agua o energía eléctrica en quetzales o dólares.
	Pago pago = new Pago();
	pago.RealizarPago("ABCDEFG", "0123456789", 12.34M, Moneda.DolarUS, Servicio.EnergiaElectrica);
}

// Si m es la cantidad de monedas soportadas y n la cantidad de servicios:
// * Usando herencia, se tendrían m*n subclases
// * Usando composición son m+n subclases.
public class Pago
{
	public bool RealizarPago(string identificadorPago, string cuentaDebito, decimal monto, Moneda monedaOrigen, Servicio servicio)
	{
		// Se pueden combinar diferentes principios, esto se debería encapsular o mejor, aplicar un patrón de diseño.
		IConversionMonto conversionMonto = null;
		switch (monedaOrigen)
		{
			case Moneda.Local:
				conversionMonto = new ConversionQuetzales();
				break;
			case Moneda.DolarUS:
				conversionMonto = new ConversionDolaresUS();
				break;
		}
	
		decimal montoQuetzales = conversionMonto.Convertir(monto);
		IPagoServicio pagoServicio = null;
		switch (servicio)
		{
			case Servicio.Agua:
				pagoServicio = new PagoAgua();
				break;
			case Servicio.EnergiaElectrica:
				pagoServicio = new PagoEnergiaElectrica();
				break;
		}
		
		return pagoServicio.Pagar(identificadorPago, montoQuetzales);
	}
}

// Se crea una implementación de esta interfaz por moneda.
public interface IConversionMonto
{
	public decimal Convertir(decimal monto);
}

public class ConversionQuetzales : IConversionMonto
{
	public decimal Convertir(decimal monto)
	{
		Console.WriteLine($"Convirtiendo Q.{monto:N2} a quetzales...");
		return monto;
	}
}

public class ConversionDolaresUS : IConversionMonto
{
	public decimal Convertir(decimal monto)
	{
		Console.WriteLine($"Convirtiendo US $.{monto:N2} a quetzales...");
		return monto * 8M;
	}
}

// Se crea una implementación de esta interfaz por servicio.
public interface IPagoServicio
{
	bool Pagar(string identificador, decimal monto);
}

public class PagoAgua : IPagoServicio
{
	public bool Pagar(string identificador, decimal monto)
	{
		Console.WriteLine($"Pagando servicio de agua por Q.{monto:N2} del identificador {identificador}...");
		return true;
	}
}

public class PagoEnergiaElectrica : IPagoServicio
{
	public bool Pagar(string identificador, decimal monto)
	{
		Console.WriteLine($"Pagando servicio de energía eléctrica por Q.{monto:N2} del identificador {identificador}...");
		return true;
	}
}

public enum Moneda
{
	Local,
	DolarUS,
}

public enum Servicio
{
	Agua,
	EnergiaElectrica,
}