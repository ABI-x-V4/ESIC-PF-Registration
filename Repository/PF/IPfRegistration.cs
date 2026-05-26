using DataModels;

namespace Repository.PF
{
    public interface IPfRegistration
    {
        Task<List<PfRegistrationDTO>> GetAllPFEmp();
        Task<PfRegistrationDTO> GetPFEmpById(int id);
        Task<string> SavePFEmployee(PfRegistrationDTO dto);
    }
}
