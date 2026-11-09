using System.ComponentModel.DataAnnotations;

namespace Recherchecase.ViewModels
{
    public class ZaakViewModel
    {
        public  int Id { get; set; }
        public int Nummer { get; set; }
        public DateTime? AangemaaktOp { get; set; }
        [Required(ErrorMessage = "Vul een titel in.")]
        [StringLength(50, ErrorMessage = "De zaaktitel mag maximaal 50 tekens bevatten.")]
        public string Titel { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vul een notitie in.")]
        public string Notitie { get; set; } = string.Empty;



    }
}
