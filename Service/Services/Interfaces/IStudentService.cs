using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.Interfaces
{
    internal interface IStudentService
    {
        public void RegisterStudent(Student student);
        public Student UpdateStudent(int? id);
        public void Delete(int? id);
        public Student GetStudentById(int? id);
        public List<Student> GetStudentByAge(int? age);
        public List<Student> GetAll();
        public List<Student> GetAllStudentsByGroupId(int? id);
        public List<Student> SearchByNameOrSurname(string searchText);
        public Student ChangeGroup(int? id);
        public Student GetStudentByEmail(string email);

        public bool CheckPasswordStrength(string password, string name,string surname);
        public bool Login(string email, string password);
    }
}
