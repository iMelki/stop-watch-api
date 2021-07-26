using System;
using System.Collections.Generic;

namespace API.Entities
{
    public class Run
    {
        public int Id{ get; set; }
        public string RunToken { get; set; }
        public DateTime StartTime { get; set; } = DateTime.Now;
        public ICollection<Lap> LapTimes { get; set; }
        public DateTime EndTime { get; set; }
        public String Status { get; set; }
    }
}