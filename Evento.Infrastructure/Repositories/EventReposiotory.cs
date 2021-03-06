﻿using Evento.Core.Domain;
using Evento.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Evento.Infrastructure.Repositories
{
    public class EventRepository : IEventRepository
    {
        private static readonly ISet<Event> _events = new HashSet<Event>
            {
                new Event(Guid.NewGuid(), "Event1", "Event1 - description", DateTime.UtcNow.AddDays(5), DateTime.UtcNow.AddDays(6)), 
                new Event(Guid.NewGuid(), "Eve2", "Event2 - description", DateTime.UtcNow.AddDays(7), DateTime.UtcNow.AddDays(8)), 
                new Event(Guid.NewGuid(), "Ev3", "Event3 - description", DateTime.UtcNow.AddDays(6), DateTime.UtcNow.AddDays(7)), 
                new Event(Guid.NewGuid(), "E4", "Event4 - description", DateTime.UtcNow.AddDays(5), DateTime.UtcNow.AddDays(6)) 
            };

        public async Task AddAsync(Event @event)
        {
            _events.Add(@event);
            await Task.CompletedTask;
        }

        public async Task<Event> GetAsync(Guid id)
            => await Task.FromResult(_events.SingleOrDefault(x => x.Id == id));

        public async Task<Event> GetAsync(string name)
            => await Task.FromResult(_events.SingleOrDefault(x => x.Name.ToLowerInvariant() == name.ToLowerInvariant()));


        public async Task<IEnumerable<Event>> BrowseAsync(string name = "")
        {
            var events = _events.AsEnumerable();
            var assembly3= Assembly.GetEntryAssembly();
            if (!string.IsNullOrWhiteSpace(name))
            {
                events = events.Where(x => x.Name.Contains(name));
            }
            return await Task.FromResult(events);
        }
        public async Task DeleteAsync(Event @event)
        {
            _events.Remove(@event);
            await Task.CompletedTask;
        }

        public async Task UpdatedAsync(Event @event)
        {
            await Task.CompletedTask;
        }
    }
}
