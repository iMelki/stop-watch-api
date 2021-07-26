using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using API.Entities;

namespace API.DTOs
{
    public class LapDto
    {
        public int Id{ get; set; }
        public DateTime LapTime { get; set; }
    }
}