using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

//Definimos uma conexão com um nó RabbitMQ em localhost
var factory = new ConnectionFactory { HostName = "localhost" };

//Abrimos uma conexão com um nó RabbitMQ
using var connection = factory.CreateConnection();

//Criar o canal na conexão para operar
using var channel = connection.CreateModel();

//Declaramos a fila a partir da qual vamos consumir as mensagens
channel.QueueDeclare(queue: "filaTeste",
                     durable: false,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);

Console.WriteLine("[*] Aguardando mensagens");

//Solicita a entrega das mensagens de forma assíncrona e fornece um retorno de chamada
var consumidor = new EventingBasicConsumer(channel);

//Recebe a mensagem da fila, converte para string e imprime no console
consumidor.Received += (model, ea) =>
{
    var corpo = ea.Body.ToArray();
    var mensagem = Encoding.UTF8.GetString(corpo);

    Console.WriteLine($" [x] Recebido: {mensagem}");
};

//Indicamos o consumo da mensagem
channel.BasicConsume(queue: "filaTeste",
                     autoAck: true,
                     consumer: consumidor);

Console.WriteLine("Aperte [ENTER] para sair");
Console.ReadLine();