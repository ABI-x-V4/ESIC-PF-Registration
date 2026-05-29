using DataModels;

namespace Repository.PF
{
    public interface IPfRegistration
    {
        Task<List<PfRegistrationDTO>> GetAllPFEmp();
        Task<PfRegistrationDTO> GetPFEmpById(int id);
        Task<string> SavePFEmployee(PfRegistrationDTO dto);
        Task<PaginatedResult<EmployeeListRowDto>> GetPfEmployeesPagedAsync(int page, int pageSize, string? search, string? gender,
                                                                            string sortBy, string sortDir);
        Task<byte[]> ExportPFEmployeeFullReportAsync(EmployeeListRowDto search);
    }
}
