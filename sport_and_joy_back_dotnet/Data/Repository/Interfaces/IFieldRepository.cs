using sport_and_joy_back_dotnet.Entities;
using sport_and_joy_back_dotnet.Models;

namespace sport_and_joy_back_dotnet.Data.Repository.Interfaces
{
    public interface IFieldRepository
    {
        //////// GET ////////
        public Field GetFieById(int id);
        public List<Field> GetAllFie(int userId); //para el admin q ve todo
        public List<Field> GetAllFieByUser(int userId);


        //////// POST ////////
        public void CreateFie(FieldForCreationDTO dto, int userId);
        public void CreateFieAdmin(FieldForCreationDTO dto, int IdUser);


        //////// PUT ////////
        public void UpdateFie(FieldForCreationDTO dto, int userId, int id);
        public void UpdateFieAdmin(FieldForCreationDTO dto, int IdUser, int id);


        //////// DELETE ////////
        public void DeleteFie(int id, int userId);
        public void DeleteFieAdmin(int id);

    }
}
