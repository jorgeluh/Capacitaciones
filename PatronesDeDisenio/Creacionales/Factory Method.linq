<Query Kind="Program" />

void Main()
{
	// Consulta de cuenta en mainframe Z.
	ValidadorSaldo validador = new ValidadorSaldoMainframeZ();
	validador.ValidarSaldoTransferencia(200M, "01234");
	
	// Consulta de cuenta en AS400.
	validador = new ValidadorSaldoAS400();
	validador.ValidarSaldoTransferencia(200M, "56789");
}

// La clase que contiene al método fábrica no se especializa en crear objetos, tiene su propia función que cumplir.
public abstract class ValidadorSaldo
{
	public bool ValidarSaldoTransferencia(decimal montoTransferencia, string cuentaDebito)
	{
		// Aquí varía la forma como se consulta el saldo de la cuenta pero en esencia es la misma funcionalidad.
		IClienteNucleoBancario cliente = this.CrearCliente();
		if (cliente.ObtenerSaldo(cuentaDebito) < montoTransferencia)
		{
			Console.WriteLine($"El saldo de la cuenta {cuentaDebito} es insuficiente.");
			return false;
		}
		
		Console.WriteLine($"El saldo de la cuenta {cuentaDebito} es suficiente.");
		return true;
	}
	
	// Delega la creación de los objetos que varían al método fábrica.
	protected abstract IClienteNucleoBancario CrearCliente();
}

// Consulta el saldo de la cuenta en mainframe Z.
public class ValidadorSaldoMainframeZ : ValidadorSaldo
{
	protected override IClienteNucleoBancario CrearCliente() => new ClienteMainframeZ();
}

// Consulta el saldo de la cuenta en AS400.
public class ValidadorSaldoAS400 : ValidadorSaldo
{
	protected override IClienteNucleoBancario CrearCliente() => new ClienteAS400();
}

// Las clases que varían en funcionaldiad deben tener una operación común para que sea útil en la clase base.
public interface IClienteNucleoBancario
{
	decimal ObtenerSaldo(string numeroCuenta);
}

// Implementa la consulta del saldo en mainframe Z.
public class ClienteMainframeZ : IClienteNucleoBancario
{
	public decimal ObtenerSaldo(string numeroCuenta)
	{
		Console.WriteLine($"Consultando el saldo de la cuenta {numeroCuenta} en mainframe Z...");
		return 123M;
	}
}

// Implementa la consulta del saldo en AS400.
public class ClienteAS400 : IClienteNucleoBancario
{
	public decimal ObtenerSaldo(string numeroCuenta)
	{
		Console.WriteLine($"Consultando el saldo de la cuenta {numeroCuenta} en AS400...");
		return 456M;
	}
}