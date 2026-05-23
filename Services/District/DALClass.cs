using DataModels;
using Insfrastructure.DbModels;
using Microsoft.EntityFrameworkCore;
using Repository.District;

namespace Services.District
{
    public class DALClass : IDistrict
    {
        private readonly EsicPfRegistrationDbContext _context;
        public DALClass(EsicPfRegistrationDbContext context)
        {
            _context = context;
        }
        public async Task<List<DistrictDTO>> GetDistrictsByStateId(int stateId)
        {
            var districts = await _context.Districts
                    .Where(d => d.StateId == stateId)
                    .Select(d => new DistrictDTO
                    {
                        Id = d.Id,
                        DistrictName = d.Name!,
                        StateId = d.StateId
                    }).ToListAsync();
            return districts;
        }
    }
}
