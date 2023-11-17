using sport_and_joy_back_dotnet.Entities;

namespace sport_and_joy_back_dotnet.Data.Repository.Interfaces
{
    public interface IFieldRepository
    {
        public Field GetFieById(int contactId);
        //public List<Field> GetAllFie(int userId); xq tenemos q traer todos los de un user
        public List<Field> GetAllFieByUser(int userId);
        public void CreateFie(ContactForCreationDTO dto, int userId);
        public void UpdateFie(ContactForCreationDTO dto, int userId, int id);
        public void DeleteFie(int id, int userId);
    }
}
