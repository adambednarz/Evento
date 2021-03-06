﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Evento.Infrastructure.Dto
{
    public class EventDetailsDto : EventDto
    {
        public IEnumerable<TicketDto> Tickets { get; set; }
        public IEnumerable<TicketDto> PurchasedTickets { get; set; }
        public IEnumerable<TicketDto> AvailableTickets { get; set; }
    }
}
