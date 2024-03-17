using Domain.Models;
using Repository.Repositories.Interfaces;
using Repository.Repositories;
using Service.Services.Interfaces;
using Service.Helpers.Exceptions;
using Service.Helpers.Constants;
using Service.Helpers.Extensions;

namespace Service.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IGroupRepository _groupRepository;
        private int count = 1;
        public StudentService()
        {
            _studentRepository = new StudentRepository();
            _groupRepository = new GroupRepository();
        }
        public void RegisterStudent(Student student)
        {
            if (student is null) throw new ArgumentNullException(ResponseMessages.EmptyInput, "Student");
            student.Id = count++;
            _studentRepository.Create(student);
        }

        public void Delete(int? id)
        {
            if (id is null) throw new ArgumentNullException();
            Student student = _studentRepository.GetById(id);

            if (student is null) throw new DirectoryNotFoundException();
            _studentRepository.Delete(student);
        }

        public List<Student> GetAll()
        {
            return _studentRepository.GetAll();
        }

        public List<Student> GetAllStudentsByGroupId(int? id)
        {
            if (id is null) throw new ArgumentNullException(string.Format(ResponseMessages.EmptyInput, "Id"));
            List<Student> students = _studentRepository.GetAllByExpression(m => m.Group.Id == id);

            if (students.Count == 0) throw new DataNotFoundException(string.Format(ResponseMessages.DoesNotExist, "Student", "group id"));
            return students;
        }

        public List<Student> GetStudentByAge(int? age)
        {
            if (age is null) throw new ArgumentNullException("Age cannot be empty, please add age again ");
            List<Student> students = _studentRepository.GetAllByExpression(m => m.Age == age);

            if (students.Count == 0) throw new DataNotFoundException(string.Format(ResponseMessages.DoesNotExist, "Student", "age"));
            return students;
        }

        public Student GetStudentById(int? id)
        {
            if (id is null) throw new ArgumentNullException(string.Format(ResponseMessages.EmptyInput,"Id"));
            Student student = _studentRepository.GetById(id);

            if (student == null) throw new DataNotFoundException(string.Format(ResponseMessages.DoesNotExist,"Student","id"));
            return student;
        }

        public List<Student> SearchByNameOrSurname(string searchText)
        {
            if (searchText is null) throw new ArgumentNullException("You cannot leave field empty");
            List<Student> students = _studentRepository.Search(m => m.Name.Contains(searchText) || m.Surname.Contains(searchText));

            if (students is null) throw new DataNotFoundException(string.Format(ResponseMessages.DoesNotExist, "Student", "search text"));
            return students;
        }

        public Student UpdateStudent(int? id)
        {
            if (id is null) throw new ArgumentNullException();
            return _studentRepository.Update(id, _studentRepository.GetById(id));
        }
        public Student ChangeGroup(int? id)
        {
            if (id is null) throw new ArgumentNullException();
            return _studentRepository.Update(id, _studentRepository.GetById(id));
        }
        public bool CheckPasswordStrength(string password,string name, string surname)
        {
            if(password.Contains(name) || password.Contains(surname))
            {
                return false;
            }
            return true;
        }
        public Student GetStudentByEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) throw new ArgumentNullException("Email cannot be empty, please add age again ");
            Student student = _studentRepository.GetByExpression(m => m.Email == email);

            return student;
        }
        public bool Login(string email, string password)
        {
            var students = _studentRepository.GetAll();
            foreach(var student in students)
            {
                if(email == student.Email && password == student.Password)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
