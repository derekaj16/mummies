using System.ComponentModel.DataAnnotations;

namespace mummies.Models.ViewModels
{
    public class SexModel
    {
        [Required]
        public int samplescollected_unknown { get; set; }
        [Required]
        public double length { get; set; }
        [Required]
        public int preservation_Well_preserved {get; set;}
        [Required]
        public int haircolor_B { get; set; }
        [Required]
        public int area_SE { get; set; }
        [Required]
        public int headdirection_W { get; set; }
        [Required]
        public int wrapping_H { get; set; }
        [Required]
        public double depth { get; set; }
        [Required]
        public double westtohead { get; set;}
        [Required]
        public int burialnumber { get; set; }
        [Required]
        public double westtofeet { get; set; }
        [Required]
        public double southtofeet { get; set; }
        [Required]
        public int adultsubadult_C { get; set; }
        [Required]
        public int squarenorthsouth { get; set; }
        [Required]
        public double southtohead { get; set; }
    }
}
