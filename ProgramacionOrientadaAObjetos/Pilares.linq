<Query Kind="Program" />

void Main()
{
	Hispanohablante objetoA = new Hispanohablante();
	Anglohablante objetoB = new Anglohablante();
	
	Persona prueba = objetoA;
	prueba.Saludar();
	
	prueba = objetoB;
	prueba.Saludar();
}

// Abstracción, todo lo que interesa simular de un ser humano es que salude.
public abstract class Persona
{
	public virtual void Saludar()
	{
		// ¿Más abstracción?
		Console.WriteLine("Hola mundo.");
	}
}

// Herencia, esta clase no tiene nada. ¿O sí? Y se puede usar donde se requiera una Persona.
public class Hispanohablante : Persona
{
}

public class Anglohablante : Persona
{
	// Encapsulación, ningún objeto de OTRA CLASE puede cambiar o siquiera leer este valor (a menos que la clase lo permita).
	private string saludo = "Hello world.";
	
	// Polimorfisno, el objeto recuerda que aunque se use como Persona, específicamente es Anglohablante.
	public override void Saludar()
	{
		Console.WriteLine(this.saludo);
	}
}