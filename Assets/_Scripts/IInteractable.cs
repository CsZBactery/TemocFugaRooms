public interface IInteractable
{
    // Todos los objetos interactuables deberán tener un mensaje para el UI
    string MensajeInteraccion { get; }

    // Todos los objetos interactuables deberán tener una función para interactuar
    void Interactuar();
}