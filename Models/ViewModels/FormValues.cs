using System;
namespace Mummies.Models.ViewModels
{
	public class FormValues
	{
        public Dictionary<string, string> ages { get; set; }
        public Dictionary<string, string> hairColors { get; set; }
        public Dictionary<string, string> sexes { get; set; }
        public Dictionary<string, string> wrapping { get; set; }
        public List<string> directions { get; set; }
        public List<string> areas { get; set; }

        public FormValues()
		{
            ages = new Dictionary<string, string>
            {
                { "N", "Newborn" },
                { "I", "Infant" },
                { "C", "Child" },
                { "A", "Adult" }
            };

           hairColors = new Dictionary<string, string>
           {
                {"B", "Brown/Dark Brown/Lt Brown"},
                {"K", "Black"},
                {"A", "Brown-red"},
                {"R", "red/Red-Bl"},
                {"D", "Blond" },
                {"U", "Unknown" }
            };

            sexes = new Dictionary<string, string>
            {
                { "M", "Male" },
                { "F", "Female" },
                { "U", "Unkown" },
            };

            wrapping = new Dictionary<string, string>
            {
                {"W", "Full or nearly full wrapping remains"},
                {"H", "Partial wrapping remain"},
                {"B", "Bones and/or only partial remnants of wrapping remains"},
                {"S", "Skeletalized or no wrapping remains"},
                {"U", "Unknown"}
            };

            directions = new List<string>
            {
                "N",
                "S",
                "E",
                "W"
            };

            areas = new List<string>
            {
                "NE",
                "NW",
                "SE",
                "SW"
            };
    }
	}
}

