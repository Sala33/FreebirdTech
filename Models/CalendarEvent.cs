using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreebirdTech.Models
{
    /// <summary>
    /// This class represents the Calendar Object <see cref="CalendarEvent"/> which can be used for creating events and acessing 
    /// the Calendar in other part of the software
    /// </summary>
    public class CalendarEvent
    {
        public int Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Text { get; set; }
        public string Color { get; set; }
        public Artist Artist { get; set; }
        public Servico Servico { get; set; }
    }
}
