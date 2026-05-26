using DataModels;
using Insfrastructure.DbModels;
using Microsoft.EntityFrameworkCore;
using Repository.PF;

namespace Services.PF
{
    public class DALClass : IPfRegistration
    {
        private readonly EsicPfRegistrationDbContext _context;
        public DALClass(EsicPfRegistrationDbContext context)
        {
            _context = context;
        }
        public async Task<List<PfRegistrationDTO>> GetAllPFEmp()
        {
            var data = await _context.PfRegistrations.Select(x => new PfRegistrationDTO
            {
                Id = x.Id,
                AadhaarFullName = x.AadhaarFullName,
                AadhaarDob = x.AadhaarDob,
                AadhaarGender = x.AadhaarGender,
                AadhaarNo = x.AadhaarNo,
                PanFullName = x.PanFullName,
                PanDob = x.PanDob,
                PanGender = x.PanGender,
                PanNo = x.PanNo,
                Nationality = x.Nationality,
                FatherOrHusbandname = x.FatherOrHusbandname,
                FatherOrHusband = x.FatherOrHusband,
                MaritalStatus = x.MaritalStatus,
                MobileNo = x.MobileNo,
                Emailid = x.Emailid,
                Qualification = x.Qualification,
                Doj = x.Doj
            }).ToListAsync();
            return data;
        }

        public async Task<PfRegistrationDTO> GetPFEmpById(int id)
        {
            var data = await GetAllPFEmp();
            return data.FirstOrDefault(e => e.Id == id)!;
        }

        public async Task<string> SavePFEmployee(PfRegistrationDTO dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var entity = new PfRegistration
                {
                    AadhaarFullName = dto.AadhaarFullName,
                    AadhaarDob = dto.AadhaarDob!.Value,
                    AadhaarGender = dto.AadhaarGender,
                    AadhaarNo = dto.AadhaarNo,
                    PanFullName = dto.PanFullName,
                    PanDob = dto.PanDob!.Value,
                    PanGender = dto.PanGender,
                    PanNo = dto.PanNo,
                    Nationality = dto.Nationality,
                    FatherOrHusbandname = dto.FatherOrHusbandname,
                    FatherOrHusband = dto.FatherOrHusband,
                    MaritalStatus = dto.MaritalStatus,
                    MobileNo = dto.MobileNo,
                    Emailid = dto.Emailid,
                    Qualification = dto.Qualification,
                    Doj = dto.Doj!.Value,
                    Aadhaarpath = dto.Aadhaarpath!,
                    PanPath = dto.PanPath!,
                    Uan = dto.Uan
                };
                _context.PfRegistrations.Add(entity);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return "Success";
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return $"Error: {ex.Message}";
            }
        }
    }
}
