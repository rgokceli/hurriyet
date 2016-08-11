using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hurriyet.Data.Infrastructure;
using Hurriyet.Data.Repositories;
using Hurriyet.Model;

namespace Hurriyet.Caching
{
    public class NewsCacheManagerWithRabbitClient : INewsCacheManager
    {
        List<News> products = new List<News>();
        ConnectionFactory factory = new ConnectionFactory() { HostName = "localhost" };
        IConnection connection;
        INewsRepository _repository;
        public NewsCacheManagerWithRabbitClient(INewsRepository repository)
        {
            _repository = repository;
        }

        public void InitCache()
        {
            LoadNewsFromDb();
            connection = factory.CreateConnection();

            var channel = connection.CreateModel();
            channel.ExchangeDeclare(exchange: "crud", type: "fanout");
            var queueName = channel.QueueDeclare().QueueName;
            channel.QueueBind(queue: queueName, exchange: "crud", routingKey: "");
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var message = JsonConvert.DeserializeObject<CRUDEventArgs<News>>(Encoding.UTF8.GetString(ea.Body));
                switch (message.EventType)
                {
                    case CrudType.INSERT:
                        {
                            products.Add(message.Args);
                            break;
                        }

                    case CrudType.UPDATE:
                        {
                            // veya propertyler tek tek map edilebilir.
                            var product = products.Where(p => p.Id == message.Args.Id).Single();
                            if(product != null)
                            {
                                product.Name = message.Args.Name;
                                product.UpdatedDate = message.Args.UpdatedDate;
                                product.CreatedDate = message.Args.CreatedDate;

                            }
                            break;
                        }
                    case CrudType.DELETE:
                        {
                            products.Remove(products.Where(p => p.Id == message.Args.Id).Single());
                            break;
                        }
                    default:
                        break;
                }
            };
            channel.BasicConsume(queue: queueName, noAck: true, consumer: consumer);
        }

        private void LoadNewsFromDb()
        {
            products.AddRange(_repository.GetAll());
        }

        public void AddNews(News product)
        {
            //products.Add(product);
            PublishEvent(product, CrudType.INSERT);
        }

        private void PublishEvent(News product, CrudType eventType)
        {
            var channel = connection.CreateModel();

            channel.ExchangeDeclare(exchange: "crud", type: "fanout");

            var message = JsonConvert.SerializeObject(new CRUDEventArgs<News>(product, eventType));
            var body = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish(exchange: "crud",
                                 routingKey: "",
                                 basicProperties: null,
                                 body: body);

        }

        public News[] GetNews(int start, int take, string key = null)
        {
            if (string.IsNullOrEmpty(key))
                return products.Skip(start).Take(take).ToArray();
            else
                return products.Where(p => p.Name.Contains(key)).Skip(start).Take(take).ToArray();
        }

        public int NewsCount()
        {
            return products.Count();
        }

        public void RemoveNews(News product)
        {
            PublishEvent(product, CrudType.DELETE);
        }

        public void UpdateNews(News product)
        {
            PublishEvent(product, CrudType.UPDATE);
        }
    }
}
