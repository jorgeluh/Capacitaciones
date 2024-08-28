<Query Kind="Program" />

void Main()
{
	// Usando las interfaces de enumeración de .NET se puede emplear la instrucción foreach.
	VectorCuentasUsuario vectorCuentas = new VectorCuentasUsuario(9);
	foreach (Cuenta cuenta in vectorCuentas)
	{
		cuenta.Dump();
	}
	
	CuboCuentasUsuario cuboCuentas = new CuboCuentasUsuario(3);
	foreach (Cuenta cuenta in cuboCuentas)
	{
		cuenta.Dump();
	}
}

// Esta es una colección que almacena cuentas en un vector.
public class VectorCuentasUsuario : IEnumerable<Cuenta>
{
	private Cuenta[] cuentas;
	
	public VectorCuentasUsuario(byte cantidad)
	{
		this.cuentas = new Cuenta[cantidad];
		string numeroCuenta;
		for (int i = 0; i < cantidad; i++)
		{
			numeroCuenta = (i + 1).ToString().PadLeft(9, '0');
			cuentas[i] = new Cuenta(numeroCuenta, $"Nombre cuenta {numeroCuenta}", i + 1, (byte)(i % 3));
		}
	}
	
	// Implementación de la interfaz IEnumerable.
	public IEnumerator<Cuenta> GetEnumerator()
	{
		Console.WriteLine($"Creando {nameof(IteradorVector)}...");
		return new IteradorVector(this.cuentas);
	}
	
	IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
}

// Esta es una colección que almacena cuentas en un cubo.
public class CuboCuentasUsuario : IEnumerable<Cuenta>
{
	private Cuenta[,,] cuentas;
	
	public CuboCuentasUsuario(byte cantidad)
	{
		this.cuentas = new Cuenta[cantidad, cantidad, cantidad];
		string numeroCuenta;
		for (int i = 0; i < cantidad; i++)
		{
			for (int j = 0; j < cantidad; j++)
			{
				for (int k = 0; k < cantidad; k++)
				{
					numeroCuenta =
                        $"{(i + 1).ToString().PadLeft(3, '0')}{(j + 1).ToString().PadLeft(3, '0')}{(k + 1).ToString().PadLeft(3, '0')}";
					cuentas[i, j, k] = new Cuenta(
						numeroCuenta,
						$"Nombre cuenta {numeroCuenta}",
						i + j + k + 1,
						(byte)((i + j + k) % 3));
				}
			}
		}
	}
	
	// Implementación de la interfaz IEnumerable.
	public IEnumerator<Cuenta> GetEnumerator()
	{
		Console.WriteLine($"Creando {nameof(IteradorCubo)}...");
		return new IteradorCubo(this.cuentas);
	}
	
	IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
}

// Iterador para la colección de cuentas en un vector.
public class IteradorVector : IEnumerator<Cuenta>
{
	private int indice = -1;
	
	private Cuenta[] vector;
	
	public IteradorVector(Cuenta[] vector) => this.vector = vector;
	
	public Cuenta Current => this.vector[this.indice];
	
	object IEnumerator.Current => this.Current;
	
	public bool MoveNext()
	{
		this.indice++;
		return this.indice < this.vector.Length;
	}
	
	public void Reset() => this.indice = -1;
	
	public void Dispose() { }
}

// Iterador para la colección de cuentas en un cubo.
public class IteradorCubo : IEnumerator<Cuenta>
{
	private int i = 0;
	
	private int j = 0;
	
	private int k = -1;
	
	private Cuenta[,,] cubo;
	
	public IteradorCubo(Cuenta[,,] cubo) => this.cubo = cubo;
	
	public Cuenta Current => this.cubo[this.i, this.j, this.k];
	
	object IEnumerator.Current => this.Current;
	
	public bool MoveNext()
	{
		this.k++;
		if (this.k == this.cubo.GetLength(2))
		{
			this.k = 0;
			this.j++;
		}
		
		if (this.j == this.cubo.GetLength(1))
		{
			this.j = 0;
			this.i++;
		}
		
		return this.i < this.cubo.GetLength(0) && this.j < this.cubo.GetLength(1) && this.k < this.cubo.GetLength(2);
	}
	
	public void Reset()
	{
		this.i = 0;
		this.j = 0;
		this.k = -1;
	}
	
	public void Dispose() { }
}

public record Cuenta(string Numero, string Cliente, decimal Monto, byte Estado);