using System.ComponentModel.DataAnnotations;

namespace mummies.Models.ViewModels
{
    public class HeadDirectionModel
    {
        [Required]
        public double Depth { get; set; }
        [Required]
        public string Sex { get; set; }
        [Required]
        public bool Goods { get; set; }
        [Required]
        public string Wrapping { get; set; }
        [Required]
        public string AgeAtDeath { get; set; }
        [Required]
        public int Count { get; set; }
        [Required]
        public double Length { get; set; }
    }
}
