using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Evento.Core.Domain
{
    public class Event : Entity
    {
        private ISet<Ticket> _tickets = new HashSet<Ticket>(); 
        public string Name { get; protected set; }
        public string Description { get; protected set; }
        public DateTime CreatedAt { get; protected set; }
        public DateTime StartDate { get; protected set; }
        public DateTime EndDate { get; protected set; }
        public DateTime UpdatedAt { get; protected set; }
        public IEnumerable<Ticket> Tickets => _tickets;
        public IEnumerable<Ticket> PurchasedTickets => Tickets.Where(x => x.Purchased == true);
        public IEnumerable<Ticket> AvailableTickets => Tickets.Except(PurchasedTickets);

        protected Event() { }
        public Event(Guid id,string name, string description, DateTime startDate, DateTime endDate)
        {
            Id = id;
            SetName(name);
            SetDescription(description);
            CreatedAt = DateTime.UtcNow;
            SetDates(startDate, endDate);
            UpdatedAt = DateTime.UtcNow;
        }

        public void AddTickets(int amount, decimal price)
        {
            for(int i=0; i<amount; i++)
            {
                var seating = _tickets.Count + 1;
                _tickets.Add(new Ticket(this, seating, price));
            }
        }
        public void SetName(string name)
        {
            if(string.IsNullOrWhiteSpace(name))
            {
                throw new Exception("Event can not have an empty name");
            }
            Name = name;
            UpdatedAt = DateTime.UtcNow;
        }
        public void SetDescription(string description)
        {
            if(string.IsNullOrWhiteSpace(description))
            {
                throw new Exception("Event can not have a empty description");
            }
            Description = description;
            UpdatedAt = DateTime.UtcNow;
        }
        public void PurchaseTickets(User user, int amount)
        {
            if (AvailableTickets.Count() < amount)
                throw new Exception("Not enough available tickets to purchase");
            var tickets = AvailableTickets.Take(amount);
            foreach(var ticket in tickets)
            {
                ticket.Purchase(user);
            }
        }
        public void CancelPurchasedTickets(User user, int amount)
        {
            var tickets = GetTicketsPurchasedByUser(user);
            if (tickets.Count() < amount)
                throw new Exception("Not enough purchased ticktes to be canceled");
            foreach(var ticket in tickets)
            {
                ticket.Cancel(user);
            }
        }
        public IEnumerable<Ticket> GetTicketsPurchasedByUser(User user)
            => PurchasedTickets.Where(x => x.UserId == user.Id);
        public void SetDates(DateTime startDate, DateTime endDate)
        {
            if (startDate >= endDate)
                throw new Exception("StartDate can not be equal EndDate");
            StartDate = startDate;
            EndDate = endDate;
        }
    }
}
