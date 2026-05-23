using DataModels;
using Insfrastructure.DbModels;
using Microsoft.EntityFrameworkCore;
using Repository.State;

namespace Services.States
{
    public class DALClass : IState
    {
        private readonly EsicPfRegistrationDbContext _context;
        public DALClass(EsicPfRegistrationDbContext context)
        {
            _context = context;
        }
        public async Task<List<StateDTO>> GetAllStates()
        {
            return await _context.States
                    .Select(x => new StateDTO
                    {
                        Id = x.Id,
                        StateName = x.Name!
                    }).ToListAsync();

        }
    }
}
