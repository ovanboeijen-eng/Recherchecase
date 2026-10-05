using System;
using System.Collections.Generic;
using System.Text;

namespace DAL_laag.DTO
{
    public class BewijsstukDTO
    {
        public int Id { get; set; }
        public required string Titel { get; set; }
        public required string Soort { get; set; }

        public DateTime Datum { get; set; }
    }
}
