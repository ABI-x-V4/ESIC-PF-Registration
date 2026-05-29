using DataModels;

namespace Repository.Employee
{
    public interface IEmployee
    {
        Task<List<EmployeeRegistrationDTO>> GetAllEmp();
        Task<EmployeeRegistrationDTO> GetEmpById(int id);
        Task<int> SaveEmployee(EmployeeRegistrationDTO dto);
        Task<PaginatedResult<EmployeeListRowDto>> GetEmployeesPagedAsync(int page, int pageSize, string? search, string? gender,  string sortBy, string sortDir);
        Task<byte[]> ExportEmployeeFullReportAsync(EmployeeListRowDto search);
    }
}
