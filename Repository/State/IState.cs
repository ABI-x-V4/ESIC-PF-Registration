using DataModels;

namespace Repository.State
{
    public interface IState
    {
        Task<List<StateDTO>> GetAllStates();
    }
}
