// interface by Carl �slund
/// <summary>
/// Interface for object that wants to send activation calls through activation caller
/// </summary>
public interface IActivator
{
    public ActivationCaller ActivationCaller {  get; set; }
}    
