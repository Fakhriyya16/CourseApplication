using Domain.Models;
using Repository.Repositories;
using Repository.Repositories.Interfaces;
using Service.Helpers.Constants;
using Service.Helpers.Exceptions;
using Service.Services.Interfaces;

namespace Service.Services
{
    public class TeacherService : ITeacherService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly ITeacherRepository _teacherRepository;
        private int count = 1;
        public TeacherService()
        {
            _studentRepository = new StudentRepository();
            _groupRepository = new GroupRepository();
            _teacherRepository = new TeacherRepository();

        }
        public void Delete(int? id)
        {
            if (id is null) throw new ArgumentNullException(string.Format(ResponseMessages.EmptyInput,"Id"));
            Teacher teacher = _teacherRepository.GetById(id);

            if (teacher is null) throw new DataNotFoundException(string.Format(ResponseMessages.DoesNotExist,"Teacher","id"));
            _teacherRepository.Delete(teacher);
        }
        public List<Teacher> GetAll()
        {
            return _teacherRepository.GetAll();
        }
        public void GradeStudent(int id,int grade)
        {
            var student = _studentRepository.GetById(id);
            if (student == null) throw new DataNotFoundException(string.Format(ResponseMessages.DoesNotExist, "Student", "id"));
            student.Grade = grade;
        }
        public bool Login(string email, string password)
        {
            var teachers = _teacherRepository.GetAll();
            foreach (var teacher in teachers)
            {
                if (email == teacher.Email && password == teacher.Password)
                {
                    return true;
                }
            }
            return false;
        }
        public void RegisterTeacher(Teacher teacher)
        {
            if (teacher is null) throw new ArgumentNullException(ResponseMessages.EmptyInput, "Teacher");
            teacher.Id = count++;
            _teacherRepository.Create(teacher);
        }
        public List<Student> GetAllStudents()
        {
           return _studentRepository.GetAll();
        }
        public List<Group> GetAllGroups()
        {
            return _groupRepository.GetAll();
        }
        public List<Student> SearchStudents(string searchText)
        {
            if (searchText is null) throw new ArgumentNullException("You cannot leave field empty");
            List<Student> students = _studentRepository.Search(m => m.Name.Contains(searchText) || m.Surname.Contains(searchText));

            if (students is null) throw new DataNotFoundException(string.Format(ResponseMessages.DoesNotExist, "Student", "search text"));
            return students;
        }
        public List<Group> SearchGroups(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText)) throw new ArgumentNullException(string.Format(ResponseMessages.EmptyInput,"Search text"));
            List<Group> groups = _groupRepository.Search(m => m.Name.Contains(searchText));
            if (groups is null) throw new DataNotFoundException(string.Format(ResponseMessages.DoesNotExist, "Group", "search text"));
            return groups;
        }
    }
}
