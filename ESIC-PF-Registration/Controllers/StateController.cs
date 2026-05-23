using Microsoft.AspNetCore.Mvc;
using Repository.State;

namespace Insfrastructure.DbModels.Controllers
{
    public class StateController : Controller
    {
        private readonly IState _istate;

        public StateController(IState istate)
        {
            _istate = istate;
        }

        public IActionResult GetRelationships()
        {
            var list = _istate.GetAllStates();
            return Json(list);
        }

    }
}
