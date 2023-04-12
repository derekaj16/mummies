using Mummies.Models;
using Mummies.Models.ViewModels;
using System;
namespace Mummies.Models.ViewModels
{
	public class BurialsViewModel
	{
		public IQueryable<Burialmain> Burials { get; set; }

		public PageInfo pageInfo { get; set; }

		public string ageAtDeath { get; set; }

		public string burialDepth { get; set; }

		public string hairColor { get; set; }

		public string squareNorthSouth { get; set; }

		public string northSouth { get; set; }

		public string squareEastWest { get; set; }

		public string eastWest { get; set; }

	}
}

