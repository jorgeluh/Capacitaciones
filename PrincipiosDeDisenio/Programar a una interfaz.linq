<Query Kind="Program" />

void Main()
{
	// El código cliente no debe depender de cómo se obtiene la información.
	IEnumerable<Movimiento> movimientosMes = ConsultarMovimientosMes("0123456789");
	foreach (Movimiento movimiento in movimientosMes)
	{
		movimiento.Dump();
	}
}

// Declarando una interfaz como tipo de retorno, la implementación es intercambiable.
public IEnumerable<Movimiento> ConsultarMovimientosMes(string numeroCuenta)
{
	// Una lista y un vector ofrecen funcionalidades muy distintas en .NET.
	List<Movimiento> movimientos = new List<Movimiento>()
	////Movimiento[] movimientos = new Movimiento[]
		{
			new Movimiento(new DateTime(2024, 5, 1), -100M, "Pago de teléfono"),
			new Movimiento(new DateTime(2024, 5, 1), -100M, "Pago de internet"),
			new Movimiento(new DateTime(2024, 5, 15), 1000M, "Anticipo de planilla"),
		};
	
	// Una cola es una implementación aún más diferente a una lista o un vector pero también implementa la interfaz IEnumerable.
	////Queue<Movimiento> movimientos = new Queue<Movimiento>();
	////movimientos.Enqueue(new Movimiento(new DateTime(2024, 5, 1), -100M, "Pago de teléfono"));
	////movimientos.Enqueue(new Movimiento(new DateTime(2024, 5, 1), -100M, "Pago de internet"));
	////movimientos.Enqueue(new Movimiento(new DateTime(2024, 5, 15), 1000M, "Anticipo de planilla"));

	return movimientos;
}

// Registro para el movimiento de la cuenta.
public record Movimiento(DateTime Fecha, decimal Monto, string Descripcion)
{
}