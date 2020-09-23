using Evento.Core.Domain;
using Evento.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evento.Infrastructure.Repositories
{
    public class InMemoryEventRepository : IEventRepository
    {
        private readonly static ISet<Event> _events = new HashSet<Event>
        {
            new Event(Guid.NewGuid(),"event1", "desciption eventu1", DateTime.UtcNow.AddHours(2), DateTime.UtcNow.AddHours(4)),
            new Event(Guid.NewGuid(),"event2", "desciption eventu2", DateTime.UtcNow.AddHours(4), DateTime.UtcNow.AddHours(8)),
        };


        public async Task<Event> GetAsync(Guid id)
            => await Task.FromResult(_events.SingleOrDefault(x => x.Id == id));
        public async Task<Event> GetAsync(string name)
            => await Task.FromResult(_events.SingleOrDefault(x => x.Name.ToLowerInvariant() == name.ToLowerInvariant()));
        public async Task<IEnumerable<Event>> BrowseAsync(string name = "")
        {
            var events = _events.AsEnumerable();
            if(!string.IsNullOrWhiteSpace(name))
            {
                events = events.Where(x => x.Name.ToLowerInvariant()
                    .Contains(name.ToLowerInvariant()));
            }
            return await Task.FromResult(events);
        }
        public async Task AddAsync(Event @event)
        {
            _events.Add(@event);
            await Task.CompletedTask;
        }
        public async Task UpdateAsync(Event @event)
        {
            await Task.CompletedTask;
        }
        public async Task DeleteAsync(Event @event)
        {
            _events.Remove(@event);
            await Task.CompletedTask;
        }      
    }
}
