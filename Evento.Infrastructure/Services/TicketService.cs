using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Evento.Core.Domain;
using Evento.Core.Repositories;
using Evento.Infrastructure.Dto;
using Evento.Infrastructure.Extensions;

namespace Evento.Infrastructure.Services
{
    public class TicketService : ITicketService
    {
        private readonly IUserRepository _userRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IMapper _mapper;
        public TicketService(IUserRepository userRepository, IEventRepository eventRepository, IMapper mapper)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _eventRepository = eventRepository;
        }
        public async Task<TicketDto> GetAsync(Guid userId, Guid eventId, Guid ticketId)
        {
            var user = await _userRepository.GetOrFailAsync(userId);
            var ticket = await _eventRepository.GetOrFailAsync(eventId, ticketId);
            
            return _mapper.Map<TicketDto>(ticket);
        }
        public async Task PurchaseAsync(Guid userId, Guid eventId, int amount)
        {
            var user = await _userRepository.GetOrFailAsync(userId);
            var @event = await _eventRepository.GetOrFailAsync(eventId);
            @event.PurchaseTickets(user, amount);

            await Task.CompletedTask;
        }
        public async Task CancelAsync(Guid userId, Guid eventId, int amount)
        {
            var user = await _userRepository.GetOrFailAsync(userId);
            var @event = await _eventRepository.GetOrFailAsync(eventId);
            @event.CancelPurchasedTickets(user, amount);

            await Task.CompletedTask;
        }

        public async Task<IEnumerable<TicketDetailsDto >> GetForUserAsync(Guid userId)
        {
            var user = await _userRepository.GetOrFailAsync(userId);
            var events = await _eventRepository.BrowseAsync();
            var allTickets = new List<TicketDetailsDto>();
            foreach(var @event in events)
            {
                var tickets = _mapper.Map<IEnumerable<TicketDetailsDto>>(@event.GetTicketsPurchasedByUser(user)).ToList();
                foreach(var ticket in tickets)
                {
                    ticket.EventId = @event.Id;
                    ticket.EventName = @event.Name;
                    allTickets.Add(ticket);
                }
            } 
            return allTickets;
        }
    }
}
