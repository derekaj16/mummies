using Mummies.Models;
using Mummies.Models.ViewModels;
using System;
namespace mummies.Models.ViewModels
{
	public class BurialsViewModel
	{
		public IQueryable<Burialmain> Burials { get; set; }

		public PageInfo pageInfo { get; set; }
	}
}

