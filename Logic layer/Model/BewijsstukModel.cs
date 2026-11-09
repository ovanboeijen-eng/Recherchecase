using System;
using System.Collections.Generic;
using System.Text;

namespace Logic_layer.Models
{
    public class BewijsstukModel
    {
       public int Id { get; set; }
       public string Titel { get; set; }
       public string Soort { get; set; }
       public DateTime? Datum { get; set; }

        public BewijsstukModel(int id, string titel, string soort)
        {
            Id = id;
            Titel = titel;
            Soort = soort;
        }


    }
}
