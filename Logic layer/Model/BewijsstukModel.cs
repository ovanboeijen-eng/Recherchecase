using System;
using System.Collections.Generic;
using System.Text;

namespace Logic_layer.Classes
{
    public class BewijsstukModel
    {
       required public int Id { get; set; }
       required  public string Titel { get; set; }
       required public string Soort { get; set; }

        public BewijsstukModel(int id, string titel, string soort)
        {
            Id = id;
            Titel = titel;
            Soort = soort;
        }


    }
}
