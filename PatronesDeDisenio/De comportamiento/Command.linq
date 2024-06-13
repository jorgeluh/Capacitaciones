<Query Kind="Program" />

void Main()
{
	IComando comando = CrearPagoServicio("01234", 234.56M, Servicio.Telefono);
	comando.Ejecutar();
	comando.Revertir();
	
	comando = CrearTransferencia("01234", "56789", 100M);
	comando.Ejecutar();
	comando.Revertir();
}

// Función para crear un comando de pago de servicio.
public IComando CrearPagoServicio(string cuentaDebito, decimal monto, Servicio servicio)
{
	return new PagoServicio(cuentaDebito, monto, servicio);
}

// Función para crear un comando de transferencia.
public IComando CrearTransferencia(string cuentaDebito, string cuentaCredito, decimal monto)
{
	return new Transferencia(cuentaDebito, cuentaCredito, monto);
}

// La interfaz del comando declara operaciones simples sin parámetros, sólo provee una forma de "ejecutar algo".
public interface IComando
{
	void Ejecutar();
	
	void Revertir();
}

// Implementación del comando para pago de servicios.
public class PagoServicio : IComando
{
	// Crea su propia instancia de la clase de negocio.
	private readonly OperadorPagoServicio operadorPago = new OperadorPagoServicio();

	private readonly Servicio proveedor;
	
	private readonly string cuentaDebito;
	
	private readonly decimal monto;
	
	// Recibe los parámetros del pago en el constructor.
	public PagoServicio(string cuentaDebito, decimal monto, Servicio proveedor)
	{
		this.cuentaDebito = cuentaDebito;
		this.monto = monto;
		this.proveedor = proveedor;
	}
	
	// Implementación del método de la interfaz.
	public void Ejecutar()
	{
		this.operadorPago.Pagar(this.cuentaDebito, this.monto, this.proveedor);
	}
	
	public void Revertir() => Console.WriteLine("El pago de servicios no soporta reversiones.");
}

// Implementación del comando para transferencias.
public class Transferencia : IComando
{
	private readonly OperadorTransferencia operadorTransferencia = new OperadorTransferencia();

	private readonly string cuentaDebito;
	
	private readonly string cuentaCredito;
	
	private readonly decimal monto;
	
	public Transferencia(string cuentaDebito, string cuentaCredito, decimal monto)
	{
		this.cuentaDebito = cuentaDebito;
		this.cuentaCredito = cuentaCredito;
		this.monto = monto;
	}

	public void Ejecutar()
	{
		this.operadorTransferencia.Transferir(cuentaDebito, cuentaCredito, monto);
	}
	
	// Las transferencias sí soportan reversiones.
	public void Revertir()
	{
		this.operadorTransferencia.Transferir(cuentaCredito, cuentaDebito, monto);
	}
}

// Clase de negocio que realiza el pago del servicio.
public class OperadorPagoServicio
{
	public bool Pagar(string cuentaDebito, decimal monto, Servicio servicio)
	{
		Console.WriteLine($"Pagando el servicio {servicio} por {monto:N2} con la cuenta {cuentaDebito}...");
		return true;
	}
}

// Clase de negocio que ejecuta la transferencia.
public class OperadorTransferencia
{
	public bool Transferir(string cuentaDebito, string cuentaCredito, decimal monto)
	{
		Console.WriteLine($"Transfiriendo {monto:N2} de la cuenta {cuentaDebito} hacia {cuentaCredito}...");
		return true;
	}
}

// Enumeración de los servicios disponibles.
public enum Servicio
{
	Agua,
	EnergiaElectrica,
	Telefono,
}