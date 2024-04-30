using System.ComponentModel.DataAnnotations;
using Clinic.Models;

namespace Clinic.ViewModels
{
    public class HomeViewModel
    {
        public List<Ticket> Tickets { get; set; }
        public List<Schedule> Schedules { get; set; }
    }
}