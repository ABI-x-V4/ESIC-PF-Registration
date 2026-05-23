using DataModels;

namespace Repository.District
{
    public interface IDistrict
    {
        Task<List<DistrictDTO>> GetDistrictsByStateId(int stateId);
    }
}
