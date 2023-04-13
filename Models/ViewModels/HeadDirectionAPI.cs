using System.ComponentModel.DataAnnotations;

namespace mummies.Models.ViewModels
{
    public class HeadDirectionAPI
    {
        [Required]
        public double depth { get; set; }
        [Required]
        public int sex_Unknown { get; set; }
        [Required]
        public int sex_M { get; set; }
        [Required]
        public int goods_Yes { get; set; }
        [Required]
        public int wrapping_Unknown { get; set; }
        [Required]
        public int ageatdeath_A { get; set; }
        [Required]
        public double length { get; set; }
        [Required]
        public int adultsubadult_C { get; set; }
        [Required]
        public int ageatdeath_I { get; set; }
        [Required]
        public int ageatdeath_N { get; set; }
        [Required]
        public int count { get; set; }
        [Required]
        public int sex_F { get; set; }
        [Required]
        public int wrapping_B { get; set; }
        [Required]
        public int wrapping_W { get; set; }
    }
}
