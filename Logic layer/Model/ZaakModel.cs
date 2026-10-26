using System;
using System.Collections.Generic;
using System.Text;

namespace Logic_layer.Models

{
    public class ZaakModel
    {
        public int Id { get; set; }
        public string Titel { get; set; }
        public string Notitie { get; set; }

        public ZaakModel(int id, string titel, string notitie)
        {
            Id = id;
            Titel = titel;
            Notitie = notitie;
        }
    }
}

