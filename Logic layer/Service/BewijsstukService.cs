using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Logic_layer.Models;

namespace Logic_layer.Service
{
    public class BewijsstukService
    {
        public bool MaakBewijsstukAan(BewijsstukModel bewijsstuk)
        {
            if (bewijsstuk == null) throw new ArgumentNullException(nameof(bewijsstuk));
            if (string.IsNullOrEmpty(bewijsstuk.Titel))
            {
                throw new ArgumentException("Titel mag niet leeg zijn.");
            }

            try 
            {
                _bewijsstukRepository.MaakBewijsstukAan(bewijsstuk);
                return true;
            }
            catch (Exception ex)
            {
                // Log de foutmelding of voer andere foutafhandelingslogica uit
                throw new Exception("Er is een fout opgetreden bij het aanmaken van het bewijsstuk.", ex);
            }

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
