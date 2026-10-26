using System.ComponentModel.DataAnnotations;

namespace Recherchecase.ViewModels
{
    public class ZaakViewModel
    {
        public  int Id { get; set; }

        [Required(ErrorMessage = "Vul een titel in.")]
        public string Titel { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vul een notitie in.")]
        public string Notitie { get; set; } = string.Empty;



    }
}
