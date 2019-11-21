﻿using AutoMapper;
using Evento.Core.Repositories;
using Evento.Infrastructure.Dto;
using Evento.Infrastructure.Extensions;
using Evento.Infrastructure.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evento.Infrastructure.Services
{
    public class TicketService : ITicketService
    {
        private readonly IEventRepository _eventRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public TicketService(IEventRepository eventRepository,
            IUserRepository userRepository,
            IMapper mapper)
        {
            _eventRepository = eventRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }
        
        public async Task<TicketDto> GetAsync(Guid userId, Guid eventId, Guid ticketId)
        {
            //var user = await _userRepository.GetOrFailAsync(userId);
            var ticket = await _eventRepository.GetTicketOrFailAsync(eventId, ticketId);

            return _mapper.Map<TicketDto>(ticket);
        }

        public async Task<IEnumerable<TicketDetailsDto>> GetTicketsForUser(Guid userId)
        {
            var user = await _userRepository.GetOrFailAsync(userId);
            var events = await _eventRepository.BrowseAsync();
            var allTickets = new List<TicketDetailsDto>(); 

            foreach (var @event in events)
            {
                var tickets = _mapper.Map<IEnumerable<TicketDetailsDto>>(@event.GetTicketsPurchasedByUser(user)).ToList();
                tickets.ForEach(x =>
                {
                    x.EventId = @event.Id;
                    x.EventName = @event.Name;
                });
                allTickets.AddRange(tickets);
            }

            return allTickets;
        }


        public async Task PurchaseAsync(Guid userId, Guid eventId, int amount)
        {
            var user = await _userRepository.GetOrFailAsync(userId);
            var @event = await _eventRepository.GetOrFailAsync(eventId);

            @event.PurchaseTickets(user, amount);
            await _eventRepository.UpdatedAsync(@event);
        }


        public async Task CancelAsync(Guid userId, Guid eventId, int amount)
        {
            var user = await _userRepository.GetOrFailAsync(userId);
            var @event = await _eventRepository.GetOrFailAsync(eventId);

            @event.CancelPurchasedTickets(user, amount);
            await _eventRepository.UpdatedAsync(@event);
        }
    }
}