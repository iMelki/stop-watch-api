using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using API.Entities;
using API.DTOs;

namespace API.DTOs
{
    public class ResultsDto
    {
        public IEnumerable<string> Laps { get; set; }
        public string TotalTime { get; set; }
    }
}