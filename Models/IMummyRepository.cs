using System;
using Mummies.Models;
using Mummies.Models.ViewModels;

namespace mummies.Models
{
	public interface IMummyRepository
	{
        public IQueryable<Burialmain> Mummies { get; }

        public IQueryable<Mummy> GetBurials(Dictionary<string, string?>? burialParams = null);
    }
}

