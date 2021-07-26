using System;
using System.Collections.Generic;

namespace API.Entities
{
    public class Lap
    {
        public int Id{ get; set; }
        public int LapNumber { get; set; }
        public int RunId{ get; set; }
        public Run Run { get; set; }
        public DateTime LapTime { get; set; } = DateTime.Now;
    }
}