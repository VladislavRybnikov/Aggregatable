using Aggregatable.Storage.InMemory;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Aggregatable.Sample
{
    class Program
    {
        public class User 
        {
            public string Email { get; set; }
            public string Phone { get; set; }
            public string Name { get; set; }
            public string Id { get; set; }

            public override string ToString() => $"User(Email: {Email}, Name: {Name}, Id: {Id})";
        }

        public class EmailUpdate 
        {
            public string Email { get; set; }
            public string Id { get; set; }
        }

        public class NameUpdate 
        {
            public string Name { get; set; }
            public string Id { get; set; }
        }

        public class PhoneUpdate
        {
            public string Phone { get; set; }
            public string Id { get; set; }
        }

        public class UserAggregateRoot : AggregateRoot<User>, 
            IAmUpdateFrom<NameUpdate>, 
            IAmUpdateFrom<EmailUpdate>
        {
            public UserAggregateRoot() 
            {
                On<PhoneUpdate>(message 
                    => Update(entity => entity
                        .By(e => e.Id, message.Id)
                        .Set(e => e.Phone, message.Phone)));

            }

            public IAggregateUpdate Handle(EmailUpdate message) 
                => Update(entity => entity
                    .By(e => e.Id, message.Id)
                    .Set(e => e.Email, message.Email));

            public IAggregateUpdate Handle(NameUpdate message) 
                => Update(entity => entity
                     .By(e => e.Id, message.Id)
                     .Set(e => e.Name, message.Name));
        }

        static async Task Main(string[] args)
        {
            var store = new InMemoryStorage();
            store.Add("123", new User { Name = "ABC", Email = "abc@mail.com", Id = "123" });
            Console.WriteLine(store.Get<User>("123"));

            var aggregationContext = new AggregationContext(Assembly.GetExecutingAssembly(), store);

            await aggregationContext.SendAsync<User, NameUpdate>(new NameUpdate { Name = "ABCD", Id = "123" });
            Console.WriteLine(store.Get<User>("123"));

            Console.ReadLine();
        }
    }
}
