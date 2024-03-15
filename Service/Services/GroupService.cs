using Repository.Repositories.Interfaces;
using Repository.Repositories;
using Service.Helpers.Constants;
using Service.Helpers.Exceptions;
using Service.Services.Interfaces;
using Domain.Models;

namespace Service.Services
{
    public class GroupService : IGroupService
    {
        private readonly IGroupRepository _groupRepository;
        private int count = 1;

        public GroupService()
        {
            _groupRepository = new GroupRepository();
        }
        public void Create(Group entity)
        {
            if (entity is null) throw new ArgumentNullException();
            entity.Id = count++;
            _groupRepository.Create(entity);
        }
        public void Delete(int? id)
        {
            if (id is null) throw new ArgumentNullException();
            Group group = _groupRepository.GetById(id);

            if (group is null) throw new DirectoryNotFoundException();
            _groupRepository.Delete(group);
        }

        public List<Group> GetAll()
        {
            return _groupRepository.GetAll();
        }
        public List<Group> GetAllByRoom(string room)
        {
            if (room is null) throw new ArgumentNullException();
            return _groupRepository.GetAllByExpression(m => m.Room == room);
        }

        public List<Group> GetAllByTeacher(string teacher)
        {
            if (teacher is null) throw new ArgumentNullException();
            List<Group> groups = _groupRepository.GetAllByExpression(m => m.Teacher == teacher);

            if (groups is null) throw new DirectoryNotFoundException();
            return groups;
        }

        public Group GetById(int? id)
        {
            if (id is null) throw new ArgumentNullException();
            Group group = _groupRepository.GetById(id);

            if (group is null) throw new DirectoryNotFoundException();
            return group;
        }
        public List<Group> SearchByName(string searchText) //by symbol
        {
            if (string.IsNullOrWhiteSpace(searchText)) throw new ArgumentNullException();
            List<Group> groups = _groupRepository.Search(m => m.Name.Contains(searchText));
            if (groups is null) throw new DirectoryNotFoundException();
            return groups;
        }

        public Group UpdateGroup(int? id)
        {
            if (id is null) throw new ArgumentNullException();
            return _groupRepository.Update(id, _groupRepository.GetById(id));
        }
        public Group GetByName(string name)
        {
            Group group = _groupRepository.GetByExpression(m => m.Name == name);
            if (group is null) throw new DataNotFoundException(ResponseMessages.DataNotFound);
            return group;
        }
    }
}
