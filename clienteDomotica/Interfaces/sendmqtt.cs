
public interface sendmqtt
{
    bool enviarmqtt(string broker, string topico, string mensaje);
    void desconectar();
}