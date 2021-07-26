using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using API.Entities;

namespace API.DTOs
{
    public class RunDto
    {
        public int Id{ get; set; }
        public string RunToken { get; set; }
    }
}