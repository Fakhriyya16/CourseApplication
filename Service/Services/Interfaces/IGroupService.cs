using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.Interfaces
{
    public interface IGroupService
    {
        void Create(Group data);
        Group UpdateGroup(int? id); //Id or Object
        void Delete(int? id);
        Group GetById(int? id);
        List<Group> GetAll();
        List<Group> GetAllByTeacher(string teacher);
        List<Group> GetAllByRoom(string room);
        List<Group> SearchByName(string searchText);
        Group GetByName(string name);
    }
}
