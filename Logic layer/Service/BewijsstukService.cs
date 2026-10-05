using System;
using System.Collections.Generic;
using System.Text;
using Logic_layer.Classes;

namespace Logic_layer.Service
{
    public class BewijsstukService
    {
        public void MaakBewijsstukAan(BewijsstukModel bewijsstuk)
        {
            if (string.IsNullOrEmpty(bewijsstuk.Titel))
            {
                throw new ArgumentException("Titel mag niet leeg zijn.");
            }
            
            if (string.IsNullOrEmpty(bewijsstuk.Soort))
            {
                throw new ArgumentException("Soort mag niet leeg zijn.");
            }
            // _bewijsstukRepository.MaakBewijsstukAan(bewijsstuk);
        }
        public void WijzigBewijsstuk(BewijsstukModel bewijsstuk)
        {
            if (string.IsNullOrEmpty(bewijsstuk.Titel))
            {
                throw new ArgumentException("Titel mag niet leeg zijn.");
            }
            if (string.IsNullOrEmpty(bewijsstuk.Soort))
            {
                throw new ArgumentException("Soort mag niet leeg zijn.");
            }
            // _bewijsstukRepository.WijzigBewijsstuk(bewijsstuk);
        }

        public int BekijkBewijsstuk(int bewijsstukId)
        {
            // _bewijsstukRepository.BekijkBewijsstuk(bewijsstukId);
            return 0;
        }
        public int VerwijderBewijsstuk(int bewijsstukId)
        {
            // _bewijsstukRepository.VerwijderBewijsstuk(bewijsstukId);
            return 0;
        }
    }
}
