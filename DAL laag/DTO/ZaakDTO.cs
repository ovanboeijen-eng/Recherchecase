using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DAL_laag.DTO
{
    public class ZaakDTO
    {
        public int Id { get; set; }
        public int Nummer { get; set; }

        
        public required string Titel { get; set; }

        public string? Beschrijving { get; set; }

        public DateTime? AangemaaktOp { get; set; }

        
        public string? Notitie { get; set; }
    }
}
