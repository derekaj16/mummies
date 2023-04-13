using System;
using Mummies.Models;
using Mummies.Models.ViewModels;
using System.Linq;

namespace mummies.Models;

    public class EFMummyRepository : IMummyRepository
    {
        private MummyDbContext mummyContext { get; set; }

        public EFMummyRepository(MummyDbContext temp)
        {
            mummyContext = temp;
        }

        public IQueryable<Burialmain> Mummies => mummyContext.Burialmains;

        public IQueryable<Mummy> GetBurials(Dictionary<string, string?>? burialParams = null)
        {
            var query = mummyContext.Burialmains.Select(x => new Mummy
            {
                Id = x.Id,
                Ageatdeath = x.Ageatdeath,
                Haircolor = x.Haircolor,
                Sex = x.Sex,
                Wrapping = x.Wrapping,
                Depth = x.Depth,
                Northsouth = x.Northsouth,
                Squarenorthsouth = x.Squarenorthsouth,
                Eastwest = x.Eastwest,
                Squareeastwest = x.Squareeastwest
            });

            if (burialParams != null) // Check if a dictionary was passed in. If not,
                                      // the user only navigated to the page, not filtered
            {
                query = !string.IsNullOrEmpty(burialParams?["Ageatdeath"])
                ? query.Where(a => a.Ageatdeath == burialParams["Ageatdeath"])
                : query;

                query = !string.IsNullOrEmpty(burialParams?["Haircolor"])
                    ? query.Where(h => h.Haircolor == burialParams["Haircolor"])
                    : query;

                query = !string.IsNullOrEmpty(burialParams?["Sex"])
                    ? query.Where(s => s.Sex == burialParams["Sex"])
                    : query;

                query = !string.IsNullOrEmpty(burialParams?["Wrapping"])
                    ? query.Where(w => w.Wrapping == burialParams["Wrapping"])
                    : query;

                double result;
                query = !string.IsNullOrEmpty(burialParams?["Depth"])
                    ? (
                        query.AsEnumerable().Where(d =>
                            double.TryParse(d.Depth, out result)
                                ? (result > double.Parse(burialParams["Depth"]) && result < (double.Parse(burialParams["Depth"]) + 0.5))
                                : false
                        ).AsQueryable()
                      ) : query;

                query = !string.IsNullOrEmpty(burialParams["Northsouth"])
                    ? query.Where(n => n.Northsouth == burialParams["Northsouth"])
                    : query;

                query = !string.IsNullOrEmpty(burialParams["Eastwest"])
                    ? query.Where(n => n.Eastwest == burialParams["Eastwest"])
                    : query;

                query = !string.IsNullOrEmpty(burialParams["Squarenorthsouth"])
                    ? query.Where(s => s.Squarenorthsouth == burialParams["Squarenorthsouth"])
                    : query;

                query = !string.IsNullOrEmpty(burialParams["Squareeastwest"])
                    ? query.Where(s => s.Squareeastwest == burialParams["Squareeastwest"])
                    : query;
            }

            return query;
        }
    }
