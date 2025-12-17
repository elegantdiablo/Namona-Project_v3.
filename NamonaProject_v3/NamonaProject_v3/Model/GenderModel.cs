using Namona_v3.DTO;
using NamonaProject_v3_.Persistance;

namespace Namona_v3.Model
{
    public class GenderModel
    {
        public NamonaDbContext _context;
        public GenderModel(NamonaDbContext context)
        {
            _context = context;
        }

        public IEnumerable<GenderDto> ShowGenders()
        {
            return _context.genders.OrderBy(x => x.GenderType).Select(x => new GenderDto
            {
                Genderid = x.GenderId,
                Gendertype = x.GenderType
            });
        }
    }
}