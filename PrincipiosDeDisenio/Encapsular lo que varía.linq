<Query Kind="Program" />

void Main()
{
	decimal monto = 19M;
	Moneda monedaDestino = Moneda.DolarUS;
	TipoCambio tipoCambio = new TipoCambio();
	decimal montoDestino = monto / tipoCambio.ObtenerTipoCambio(monedaDestino);
	Console.WriteLine($"Resultado de la conversión a {monedaDestino}: {monto:N2} -> {montoDestino:N2}");
}

// En lugar de tener este código donde se hace la conversión, se traslada a su propia clase.
public class TipoCambio
{
	// Los cambios se pueden controlar aún más encapsulando la consulta del tipo de cambio.
	public decimal ObtenerTipoCambio(Moneda moneda)
	{
		switch (moneda)
		{
			case Moneda.DolarUS:
				return this.ConsultarCambioDolarUS();
			case Moneda.PesoMX:
				return this.ConsultarCambioPesoMX();
			default:
				return 1M;
		}
	}
	
	private decimal ConsultarCambioDolarUS()
	{
		Console.WriteLine("Consultando tipo de cambio para dólar estadounidense...");
		return 8M;
	}
	
	private decimal ConsultarCambioPesoMX()
	{
		Console.WriteLine("Consultando tipo de cambio para peso mexicano...");
		return 1.25M;
	}
}

public enum Moneda
{
	Local,
	DolarUS,
	PesoMX,
}