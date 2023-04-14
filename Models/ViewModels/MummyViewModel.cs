using Mummies.Models;

namespace mummies.Models.ViewModels
{
    public class MummyViewModel
    {
        public IQueryable<Burialmain> Burial { get; set; }
        public IQueryable<BurialmainTextile> burialmainTextiles { get; set; }
        public IQueryable<Textile> Textile { get; set; }
        public IQueryable<TextilefunctionTextile> textilefunctionTextiles { get; set; }
        public IQueryable<Textilefunction> textilefunctions { get; set; }
        public IQueryable<StructureTextile> structureTextiles { get; set; }
        public IQueryable<Structure> structures { get; set; }
        public IQueryable<ColorTextile> colorTextiles { get; set; } 
        public IQueryable<Color> colors { get; set; }
    }
}
