using Domain.Models;
using Repository.Repositories.Interfaces;
using Repository.Repositories;
using Service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        private int count = 1;
        public StudentService()
        {
            _studentRepository = new StudentRepository();
        }
        public void Create(Student student)
        {
            if (student is null) throw new ArgumentNullException();
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
            if (id is null) throw new ArgumentNullException();
            List<Student> students = _studentRepository.GetAllByExpression(m => m.Group.Id == id);

            if (students is null) throw new DirectoryNotFoundException();
            return students;
        }

        public List<Student> GetStudentByAge(int? age)
        {
            if (age is null) throw new ArgumentNullException();
            List<Student> students = _studentRepository.GetAllByExpression(m => m.Age == age);

            if (students is null) throw new DirectoryNotFoundException();
            return students;
        }

        public Student GetStudentById(int? id)
        {
            if (id is null) throw new ArgumentNullException();
            Student student = _studentRepository.GetById(id);

            if (student == null) throw new DirectoryNotFoundException();
            return student;
        }

        public List<Student> SearchByNameOrSurname(string searchText)
        {
            if (searchText is null) throw new ArgumentNullException();
            List<Student> students = _studentRepository.Search(m => m.Name.Contains(searchText) || m.Surname.Contains(searchText));

            if (students is null) throw new DirectoryNotFoundException();
            return students;
        }

        public Student UpdateStudent(int? id)
        {
            if (id is null) throw new ArgumentNullException();
            return _studentRepository.Update(id, _studentRepository.GetById(id));
        }

    }
}
