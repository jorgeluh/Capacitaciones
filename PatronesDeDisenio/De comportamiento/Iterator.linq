<Query Kind="Program" />

void Main()
{
	VectorCuentasUsuario vectorCuentas = new VectorCuentasUsuario(9);
	MostrarCuentas(vectorCuentas.CrearIterador());
	
	CuboCuentasUsuario cuboCuentas = new CuboCuentasUsuario(3);
	MostrarCuentas(cuboCuentas.CrearIterador());
}

// Método auxiliar que sólo usa un iterador para recorrer y mostrar cada cuenta en la colección.
public void MostrarCuentas(IIterador iterador)
{
	while (iterador.TieneMas)
	{
		iterador.ObtenerSiguiente().Dump();
	}
}

// Interfaz del iterador. Sólo tiene métodos para saber si aún hay más elementos y para obtener cada uno.
public interface IIterador
{
	bool TieneMas { get; }
	
	Cuenta ObtenerSiguiente();
}

// Esta interfaz sólo declara una función para que se le pueda solicitar un iterador a la colección.
public interface IIterable
{
	IIterador CrearIterador();
}

// Esta es una colección que almacena cuentas en un vector.
public class VectorCuentasUsuario : IIterable
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
	
	// Implementación de la interfaz IIterable.
	public IIterador CrearIterador() => new IteradorVector(this.cuentas);
}

// Esta es una colección que almacena cuentas en un cubo.
public class CuboCuentasUsuario : IIterable
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
	
	// Implementación de la interfaz IIterable.
	public IIterador CrearIterador() => new IteradorCubo(this.cuentas);
}

// Iterador para la colección de cuentas en un vector.
public class IteradorVector : IIterador
{
	private int indice = 0;
	
	private Cuenta[] vector;
	
	public IteradorVector(Cuenta[] vector) => this.vector = vector;
	
	public bool TieneMas => this.vector != null && this.indice < this.vector.Length;
	
	public Cuenta ObtenerSiguiente() => this.vector[this.indice++];
}

// Iterador para la colección de cuentas en un cubo.
public class IteradorCubo : IIterador
{
	private int i = 0;
	
	private int j = 0;
	
	private int k = 0;
	
	private Cuenta[,,] cubo;
	
	public IteradorCubo(Cuenta[,,] cubo) => this.cubo = cubo;
	
	public bool TieneMas =>
        this.cubo != null && this.i < this.cubo.GetLength(0) && this.j < this.cubo.GetLength(1) && this.k < this.cubo.GetLength(2);
	
	public Cuenta ObtenerSiguiente()
	{
		Cuenta valor = this.cubo[this.i, this.j, this.k++];
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
		
		return valor;
	}
}

public record Cuenta(string Numero, string Cliente, decimal Monto, byte Estado);