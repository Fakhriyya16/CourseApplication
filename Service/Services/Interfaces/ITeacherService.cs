using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.Interfaces
{
    public interface ITeacherService
    {
        public List<Teacher> GetAll();
        public void RegisterTeacher(Teacher teacher);
        public void Delete(int? id);
        public bool Login(string email, string password);
        public void GradeStudent(int id, int grade);
        public List<Student> GetAllStudents();
        public List<Group> GetAllGroups();
        public List<Student> SearchStudents(string searchText);
        List<Group> SearchGroups(string searchText);
    }
}
