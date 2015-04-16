using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tickets.Models
{
    public class AttendeesRepo
    {
        public IEnumerable<Attendee> GetAllAttendees()
        {
            PaypalEntities db = new PaypalEntities();
            return db.Attendees.OrderByDescending(r => r.paymentDate).ToList();
        }
    }
}