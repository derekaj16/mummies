using Mummies.Models;
using Mummies.Models.ViewModels;
using System;
namespace Mummies.Models.ViewModels
{
	public class BurialsViewModel
	{
		public IQueryable<Mummy> Burials { get; set; }

        public List<Mummy> Mummies { get; set; }

		public PageInfo pageInfo { get; set; }

        public FilterSettings filterSettings { get; set; }

        public FormValues formValues { get; set; }

        public string? Squarenorthsouth { get; set; }
        public string? Headdirection { get; set; }
        public string? Northsouth { get; set; }
        public string? Depth { get; set; }
        public string? Eastwest { get; set; }
        public string? Squareeastwest { get; set; }
        public string? Area { get; set; }
        public string? Burialnumber { get; set; }
        public string? Wrapping { get; set; }
        public string? Haircolor { get; set; }
        public string? Ageatdeath { get; set; }
        public string? Sex { get; set; }
        public double? Estimatestature { get; set; }
        public string? TextileDescription { get; set; }
        public string? TextileFunction { get; set; }
        public string? TextileColor { get; set; }
        public string? TextileStructure { get; set; }

    }
}

