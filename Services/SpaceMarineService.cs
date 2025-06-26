using SpaceMarineAPI.Models;
using SpaceMarineAPI.Repositories;

namespace SpaceMarineAPI.Services
{
    public class SpaceMarineService
    {
        private readonly SpaceMarineRepository _marineRepository;

        public SpaceMarineService(SpaceMarineRepository marineRepository)
        {
            _marineRepository = marineRepository;
        }

        public void AddMarine(SpaceMarine marine)
        {
            _marineRepository.AddMarine(marine);
        }

        public List<SpaceMarine> GetMarinesBySquad(int squadId)
        {
            return _marineRepository.GetMarinesBySquad(squadId);
        }

        public void UpdateMarine(int id, SpaceMarine marine)
        {
            _marineRepository.UpdateMarine(id, marine);
        }

        public void DeleteMarine(int id)
        {
            _marineRepository.DeleteMarine(id);
        }


        public string GetRank(int experience)
        {
            if (experience < 100)
                return "Initiate";
            else if (experience < 250)
                return "Battle Brother";
            else if (experience < 500)
                return "Veteran";
            else if (experience < 1000)
                return "Seasoned Veteran";
            else
                return "Elite Veteran";
        }
    }
}
