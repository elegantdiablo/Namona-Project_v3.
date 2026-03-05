using System.Collections;
using NamonaProject_v3_.DTO;
using NamonaProject_v3_.Persistance;

namespace NamonaProject_v3_.Model
{
    public class GenderModel
    {
        private readonly NamonaDbContext _context;

        public GenderModel(NamonaDbContext context)
        {
            _context = context;
        }

        public IEnumerable<AllGendersDto> GetAllGenders()
        {
            return _context.genders.Select(x => new AllGendersDto
            {
                Id = x.GenderId,
                Type = x.GenderType
            });
        }
        public async Task AddGender(AddGenderDto dto)
        {
            using(var trx = _context.Database.BeginTransaction())
            {
                _context.genders.Add(new Gender
                {
                    GenderType = dto.Type

                });
                await _context.SaveChangesAsync();
                await trx.CommitAsync();
            }

            await Task.CompletedTask;
        }
        public async Task EditGender(EditGenderDto dto)
        {
            if (!_context.genders.Any(x => x.GenderId == dto.Id))
            {
                throw new KeyNotFoundException("nincs ilyen ruha");
            }

            int Id = _context.genders.Where(x => x.GenderId == dto.Id).First().GenderId;
            using (var trx = _context.Database.BeginTransaction())
            {
                _context.genders.Where(x => x.GenderId == Id).First().GenderType = dto.Type;

                await _context.SaveChangesAsync();
                await trx.CommitAsync();
            }

            await Task.CompletedTask;
        }

        public async Task DeleteGender(int id)
        {
            using(var trx = _context.Database.BeginTransaction())
            {
                _context.genders.Remove(_context.genders.Where(x => x.GenderId == id).First());

                await _context.SaveChangesAsync();
                await trx.CommitAsync();
            }
            await Task.CompletedTask;
        }

    }
}
