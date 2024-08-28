<Query Kind="Program" />

void Main()
{
	Mediador mediador = new Mediador();
	mediador.Transferir("12345678", "98765432", 123.45M);
}

// Los componentes son clases que se dedican a funciones específicas y no tienen relación entre sí.
public abstract class Componente
{
	// Cada componente mantiene una referencia al mediador que recibe en su constructor, es su única dependencia.
	private readonly IMediador mediador;
	
	public Componente(IMediador mediador) => this.mediador = mediador;
	
	// Se implementa un método de notificación para encapsular la llamada a Notificar del mediador.
	protected void Notificar(params object[] parametros) => this.mediador.Notificar(this, parametros);
}

// La interfaz del mediador únicamente indica que tiene un método de notificación. El mediador puede ser cualquier cosa.
public interface IMediador
{
	// El primer parámetro es el emisor que notifica. El resto de parámetros son libres y dependen del emisor.
	void Notificar(Componente emisor, params object[] parametros);
}

// Este componente se encarga de ejecutar una transferencia entre cuentas.
public class OperadorTransferencia : Componente
{
	public OperadorTransferencia(IMediador mediador) : base(mediador)
	{
	}
	
	public void Transferir(string cuentaDebito, string cuentaCredito, decimal monto)
	{
		Console.WriteLine($"Ejecutando la transferencia {cuentaDebito} -> {cuentaCredito} por {monto:N2}");
		this.Notificar(true, cuentaDebito, cuentaCredito, monto);
	}
}

// Este componente consulta el número de teléfono asociado a un número de cuenta.
public class PerfilCuenta : Componente
{
	public PerfilCuenta(IMediador mediador) : base(mediador)
	{
	}
	
	public string ObtenerNumeroTelefono(string numeroCuenta)
	{
		Console.WriteLine($"Consultando el número de teléfono asociado a la cuenta {numeroCuenta}...");
		string numeroTelefono = "55555555";
		this.Notificar(numeroTelefono);
		return numeroTelefono;
	}
}

// Este componente envía notificaciones de mensaje de texto a un número de teléfono.
public class NotificadorSms : Componente
{
	public NotificadorSms(IMediador mediador) : base(mediador)
	{
	}
	
	public void EnviarMensaje(string numeroTelefono, string mensaje)
	{
		Console.WriteLine($"Enviando el mensaje \"{mensaje}\" al número de teléfono \"{numeroTelefono}\".");
		this.Notificar(numeroTelefono, mensaje);
	}
}

// Este componente graba un mensaje que describe una acción en una bitácora.
public class Bitacora : Componente
{
    public Bitacora(IMediador mediador) : base(mediador)
	{
	}
	
	public void RegistrarAccion(string descripcion)
	{
		Console.WriteLine($"Registrando la acción \"{descripcion}\"...");
		this.Notificar(descripcion);
	}
}

// El mediador es donde se "enlaza" la funcionalidad de los distintos componentes, de modo que no tiene relación directa entre sí.
public class Mediador : IMediador
{
	private readonly OperadorTransferencia operadorTransferencia;
	
	private readonly PerfilCuenta perfilCuenta;
	
	private readonly NotificadorSms notificadorSms;
	
	private readonly Bitacora bitacora;
	
	// El mediador crea sus componentes, pasándose a él mismo como parámetro en sus constructores.
	public Mediador()
	{
		this.operadorTransferencia = new OperadorTransferencia(this);
		this.perfilCuenta = new PerfilCuenta(this);
		this.notificadorSms = new NotificadorSms(this);
		this.bitacora = new Bitacora(this);
	}
	
	// Se expone un método para iniciar la transferencia para no dar acceso a componentes individuales.
	public void Transferir(string cuentaDebito, string cuentaCredito, decimal monto) =>
		this.operadorTransferencia.Transferir(cuentaDebito, cuentaCredito, monto);
	
	// Se reciben notificaciones de los componentes. No es necesario que todos lo hagan pero es mejor si se debe modificar el flujo.
	public void Notificar(Componente emisor, params object[] parametros)
	{
		// Aquí se identifica el tipo del emisor de la notificación y se ejecuta una acción personalizada para algunos.
		switch (emisor)
		{
			case OperadorTransferencia:
				 if ((bool)parametros[0])
				 {
					this.bitacora.RegistrarAccion($"Transferencia realizada: {parametros[1]} -> {parametros[2]}");
					string numeroTelefono = this.perfilCuenta.ObtenerNumeroTelefono((string)parametros[1]);
					this.notificadorSms.EnviarMensaje(
						numeroTelefono,
						$"Transferencia de la cuenta {parametros[1]} a {parametros[2]} por {parametros[3]:N2}");
				}
				else
				{
					this.bitacora.RegistrarAccion($"Error en transferencia: {parametros[1]} -> {parametros[2]}");
				}
				
				break;
			case NotificadorSms:
				this.bitacora.RegistrarAccion($"Notificación enviada al número de teléfono {parametros[0]}");
				break;
			default:
				Console.WriteLine($"Componente sin acción: {emisor.GetType().Name}");
				break;
		}
	}
}