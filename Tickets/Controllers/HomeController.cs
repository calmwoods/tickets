using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList; 

using Tickets.BusinessLogic;
using Tickets.Models; 

namespace Tickets.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Tickets()
        {
            SessionHelper sessionHelper = new SessionHelper();
            ViewBag.custom = sessionHelper.SessionID; 
            return View();
        }

        public ActionResult Attendees(int? page)
        {
            AttendeesRepo attendeesRepo = new AttendeesRepo();
            IEnumerable<Attendee> attendees = attendeesRepo.GetAllAttendees();

            const int LISTING_COUNT_PER_PAGE = 10;
            int pageNumber = (page ?? 1);
            attendees = attendees.ToPagedList(pageNumber, LISTING_COUNT_PER_PAGE);

            return View(attendees);
        }

        public ActionResult Thankyou()
        {
            return View(); 
        }

        public ActionResult Paypal()
        {
            Paypal_IPN paypalIPN = new Paypal_IPN("test");

            if (paypalIPN.TXN_ID != null)
            {
                PaypalEntities context = new PaypalEntities();
                Attendee attendee = new Attendee();
                attendee.custom = paypalIPN.Custom;
                attendee.txnID = paypalIPN.TXN_ID;
                attendee.paymentDate =  DateTime.Now;
                attendee.payerFisrtName = paypalIPN.PayerFirstName;
                attendee.payerLastName = paypalIPN.PayerLastName;
                attendee.payerEmail = paypalIPN.PayerEmail;
                attendee.quantity = Convert.ToInt32(paypalIPN.Quantity);
                attendee.paymentGross = Convert.ToDecimal(paypalIPN.PaymentGross);
                attendee.paymentStatus = paypalIPN.PaymentStatus;
                context.Attendees.Add(attendee);
                context.SaveChanges(); 

            }

            return View(); 
        }
    }
}