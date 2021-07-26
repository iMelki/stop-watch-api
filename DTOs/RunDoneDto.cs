using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using API.Entities;
using API.DTOs;

namespace API.DTOs
{
    public class RunDoneDto
    {
        public int Id{ get; set; }
        public string RunToken { get; set; }
        public DateTime StartTime { get; set; }
        public ICollection<LapDto> LapTimes { get; set; }
        public DateTime EndTime { get; set; }
        public String Status { get; set; }
    }
}