using Microsoft.Data.SqlClient;
using SpaceMarineAPI.Models;
using SpaceMarineAPI.Repositories;

namespace SpaceMarineAPI.Services
{
    public class SquadService
    {
        private readonly SquadRepository _squadRepository;

        //CONSTRUCTOR
        public SquadService(SquadRepository squadRepository)
        {
            _squadRepository = squadRepository;
        }


        public void AddSquad(Squad squad)
        {
            _squadRepository.AddSquad(squad);
        }


        public Squad GetSquadById(int id)
        {
            return _squadRepository.GetSquadById(id);
        }



        public List<Squad> GetAllSquads()
        {
            return _squadRepository.GetAllSquads();
        }



        public void UpdateSquad(int id, Squad squad)
        {
            _squadRepository.UpdateSquad(id, squad);
        }



        public void DeleteSquad(int id)
        {
            _squadRepository.DeleteSquad(id);
        }
    }
}
