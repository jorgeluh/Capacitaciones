<Query Kind="Program" />

void Main()
{
	int entero = 1;
	Console.WriteLine($"Antes de Prueba: {entero}");
	Prueba(entero);
	Console.WriteLine($"Después de Prueba: {entero}");
	PruebaReferencia(ref entero);
	Console.WriteLine($"Después de PruebaReferencia: {entero}");
	
	string cadena = "¡Hola Mundo!";
	Console.WriteLine($"Antes de PruebaCadena: {cadena}");
	PruebaCadena(cadena);
	Console.WriteLine($"Después de PruebaCadena: {cadena}");
	PruebaCadenaReferencia(ref cadena);
	Console.WriteLine($"Después de PruebaCadenaReferencia: {cadena}");
	
	Cliente cliente = new Cliente() { Nombre = "Nombre Apellido" };
	cliente.Dump();
	PruebaObjeto(cliente);
	cliente.Dump();
	PruebaObjetoReferencia(ref cliente);
	cliente.Dump();
}

public void Prueba(int valor)
{
	valor++;
}

public void PruebaReferencia(ref int valor)
{
	valor++;
}

public void PruebaCadena(string valor)
{
	valor = "Otro valor";
}

public void PruebaCadenaReferencia(ref string valor)
{
	valor = "1, 2, 3, probando...";
}

public void PruebaObjeto(Cliente cliente)
{
	cliente.Nombre = "Otro Nombre";
	cliente = new Cliente() { Nombre = "Fulano" };
}

public void PruebaObjetoReferencia(ref Cliente cliente)
{
	cliente = new Cliente() { Nombre = "Cliente Nuevo" };
}

public class Cliente
{
	public string Nombre { get; set; }
}