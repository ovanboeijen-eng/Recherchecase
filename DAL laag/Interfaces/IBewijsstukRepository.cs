using System;
using System.Collections.Generic;
using System.Text;
using DAL_laag.DTO;

namespace DAL_laag.Interfaces
{
    public interface IBewijsstukRepository
    {
        bool MaakBewijsstukAan(BewijsstukDTO bewijsstuk);
        void WijzigBewijsstuk(BewijsstukDTO bewijsstuk);
        BewijsstukDTO? BekijkBewijsstuk(int bewijsstukId);
        void VerwijderBewijsstuk(int bewijsstukId);


    }
}
