using System;
namespace mummies.Models.ViewModels
{
	public class HairColorViewModel
	{
		public HairColorViewModel()
		{
		}

        public Dictionary<string, string> HairColorDict = new Dictionary<string, string>
        {
            {"B", "Brown/Dark Brown/Lt Brown"},
            {"K", "Black"},
            {"A", "Brown-red"},
            {"R", "red/Red-Bl"},
            {"D", "Blond" },
            {"U", "Unknown" }
        };
    }
}

