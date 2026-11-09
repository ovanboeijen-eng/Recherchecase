using System.ComponentModel.DataAnnotations;

namespace Recherchecase.ViewModels
{
    public class ZaakDetailsViewModel
    {
        public int Id { get; set; }
        public int Nummer { get; set; }
        public string Titel { get; set; } = string.Empty;
        public string Notitie { get; set; } = string.Empty;
        public DateTime? AangemaaktOp { get; set; }
        public List<BewijsstukViewModel> Bewijsstukken { get; set; } = new();
        public VoegBewijsstukToeViewModel NieuwBewijsstuk { get; set; } = new();
    }

    public class BewijsstukViewModel
    {
        public int Id { get; set; }
        public string Titel { get; set; } = string.Empty;
        public string Soort { get; set; } = string.Empty;
        public DateTime? Datum { get; set; }
    }

    public class VoegBewijsstukToeViewModel
    {
        [Required(ErrorMessage = "Vul een titel in.")]
        [StringLength(200, ErrorMessage = "De titel mag maximaal 200 tekens bevatten.")]
        public string Titel { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vul een soort in.")]
        [StringLength(100, ErrorMessage = "De soort mag maximaal 100 tekens bevatten.")]
        public string Soort { get; set; } = string.Empty;

        [Required(ErrorMessage = "Kies de datum van het bewijsstuk.")]
        [DataType(DataType.Date)]
        public DateTime? Datum { get; set; }
    }
}
