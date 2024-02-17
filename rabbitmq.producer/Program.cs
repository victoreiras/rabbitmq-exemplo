using System.Text;
using RabbitMQ.Client;

//Definimos uma conexão com um nó RabbitMQ em localhost
var factory = new ConnectionFactory { HostName = "localhost" };

//Abrimos uma conexão com um nó RabbitMQ
using var connection = factory.CreateConnection();

//Criamos um canal onde vamos definir uma fila, uma mensagem e publicar a mensagem
using var channel = connection.CreateModel();

/*
Criamos a fila definindo os seguintes valores:
queue - o nome da fila
durable - se igual a true a fila permanece ativa após o servidor ser reiniciado
exclusive - se igual a true ela só pode ser acessada via conexão atual e são excluídas ao fechar conexão
autoDelete - se igual a true será deletada automaticamente após os consumidores usarem a fila
*/
channel.QueueDeclare(queue: "filaTeste",
                     durable: false,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);

Console.WriteLine("Digite sua mensagem e aperte <ENTER>");

while (true)
{
    string mensagem = Console.ReadLine();

    if (mensagem == "")
        break;

    //Criamos a mensagem a ser posta na fila e codificamos a mensagem como um array de bytes
    var corpo = Encoding.UTF8.GetBytes(mensagem);

    //Publicamos a mensagem informando a fila e o corpo da mensagem
    channel.BasicPublish(exchange: string.Empty,
                         routingKey: "filaTeste",
                         basicProperties: null,
                         body: corpo);

    Console.WriteLine($"[x] Enviado {mensagem}");
}