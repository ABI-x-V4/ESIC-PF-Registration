using DataModels;

namespace Repository.Employee
{
    public interface IEmployee
    {
        Task<List<EmployeeRegistrationDTO>> GetAllEmp();
        Task<EmployeeRegistrationDTO> GetEmpById(int id);
        Task<string> SaveEmployee(EmployeeRegistrationDTO dto);
    }
}
