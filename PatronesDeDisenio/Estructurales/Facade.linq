<Query Kind="Program" />

void Main()
{
	// El código cliente sólo ve la clase fachada sin conocer el detalle de las implementaciones.
	ConsultaCuentas consulta = new ConsultaCuentas();
	DatosCuenta datos = consulta.Consultar("01234");
	datos.Dump();
}

// Esta clase se vale de las implementaciones para exponer una operación simplificada al cliente.
public class ConsultaCuentas
{
	private readonly ConsultaNombreCuenta consultaNombre = new ConsultaNombreCuenta();
	
	private readonly ConsultaSaldoCuenta consultaSaldo = new ConsultaSaldoCuenta();
	
	private readonly ConsultaMovimientosMesCuenta consultaMovimientosMes = new ConsultaMovimientosMesCuenta();

	public DatosCuenta Consultar(string numero)
	{
		string nombreCuenta = this.consultaNombre.ConsultarNombre(numero);
		decimal saldo = this.consultaSaldo.ConsultarSaldo(numero);
		IEnumerable<Movimiento> movimientosMes = this.consultaMovimientosMes.ConsultarMovimientosMes(numero);
	
		DatosCuenta resultado = new DatosCuenta(numero, nombreCuenta, saldo, movimientosMes);
		return resultado;
	}
}

// Implementación para consultar el nombre de la cuenta.
public class ConsultaNombreCuenta
{
	public string ConsultarNombre(string numeroCuenta)
	{
		Console.WriteLine($"Consultando el nombre de la cuenta {numeroCuenta}...");
		return "Cuenta " + numeroCuenta;
	}
}

// Implementación para consultar el saldo de la cuenta.
public class ConsultaSaldoCuenta
{
	public decimal ConsultarSaldo(string numeroCuenta)
	{
		Console.WriteLine($"Consultando el saldo de la cuenta {numeroCuenta}...");
		return 2000M;
	}
}

// Implementación para consultar los movimientos del mes de la cuenta.
public class ConsultaMovimientosMesCuenta
{
	public IEnumerable<Movimiento> ConsultarMovimientosMes(string numeroCuenta)
	{
		Console.WriteLine($"Consultando los movimientos del mes de la cuenta {numeroCuenta}...");
		return new List<Movimiento>()
			{
				new Movimiento(new DateTime(2024, 5, 1), 1000M, "Ahorro del mes anterior"),
				new Movimiento(new DateTime(2024, 5, 2), -200M, "Pago de servicios"),
			};
	}
}

// Registro para el movimiento de la cuenta.
public record Movimiento(DateTime Fecha, decimal Monto, string Descripcion)
{
}

// Registro para el conjunto de datos de una cuenta.
public record DatosCuenta(string Numero, string Nombre, decimal Saldo, IEnumerable<Movimiento> MovimientosMes)
{
}