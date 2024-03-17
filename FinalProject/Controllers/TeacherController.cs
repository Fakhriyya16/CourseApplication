using Domain.Models;
using Service.Helpers.Constants;
using Service.Helpers.Exceptions;
using Service.Helpers.Extensions;
using Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FinalProject.Controllers
{
    public class TeacherController
    {
        private readonly TeacherService _teacherService;
        private readonly StudentService _studentService;
        private readonly GroupService _groupService;

        public TeacherController()
        {
            _teacherService = new TeacherService();
            _studentService = new StudentService();
            _groupService = new GroupService();
        }
        public void RegisterTeacher()
        {

        Name: ConsoleColor.Cyan.ConsoleMessage("Add name: ");
            string name = Console.ReadLine();
            string namePattern = @"^[A-Z][a-z]*$";
            Regex regex = new(namePattern);

            if (string.IsNullOrWhiteSpace(name))
            {
                ConsoleColor.Red.ConsoleMessage(string.Format(ResponseMessages.EmptyInput, "Name"));
                goto Name;
            }
            Match match = regex.Match(name);
            if (!match.Success)
            {
                ConsoleColor.Red.ConsoleMessage(string.Format(ResponseMessages.WrongFormat, "name") + " " + "Name must start with uppercase letter and contain only letters");
                goto Name;
            }

        Surname: ConsoleColor.Cyan.ConsoleMessage("Add surname: ");
            string surname = Console.ReadLine();
            string surnamePattern = @"^[A-Z][a-z]*$";
            Regex regex2 = new(surnamePattern);

            if (string.IsNullOrWhiteSpace(surname))
            {
                ConsoleColor.Red.ConsoleMessage(string.Format(ResponseMessages.EmptyInput, "Surname"));
                goto Surname;
            }
            Match match2 = regex2.Match(surname);
            if (!match2.Success)
            {
                ConsoleColor.Red.ConsoleMessage(string.Format(ResponseMessages.WrongFormat, "Surname") + " " + "Surname must start with uppercase letter and contain only letters");
                goto Surname;
            }

        Email: ConsoleColor.Cyan.ConsoleMessage("Create email: ");
            string email = Console.ReadLine();
            string emailPattern = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";
            Regex regex3 = new(emailPattern);
            if (string.IsNullOrWhiteSpace(email))
            {
                ConsoleColor.Red.ConsoleMessage(string.Format(ResponseMessages.EmptyInput, "Email"));
                goto Email;
            }
            var teachers = _teacherService.GetAll();
            foreach (var teacher in teachers)
            {
                if (email == teacher.Email)
                {
                    ConsoleColor.Red.ConsoleMessage("Teacher with the email address you provided already exists. Please use a different email address.");
                    goto Email;
                }
            }
            Match match3 = regex3.Match(email);
            if (!match3.Success)
            {
                ConsoleColor.Red.ConsoleMessage(string.Format(ResponseMessages.WrongFormat, "Email") + " " + "Please enter a valid email address in the format 'example@example.com'.");
                goto Email;
            }

        Password: ConsoleColor.Cyan.ConsoleMessage("Create password: ");
            string password = Console.ReadLine();
            string passwordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$";
            Regex regex4 = new(passwordPattern);
            if (string.IsNullOrWhiteSpace(password))
            {
                ConsoleColor.Red.ConsoleMessage(string.Format(ResponseMessages.EmptyInput, "Password"));
                goto Password;
            }
            Match match4 = regex4.Match(password);
            if (!match4.Success)
            {
                ConsoleColor.Red.ConsoleMessage(string.Format(ResponseMessages.WrongFormat, "Password") + " " + "Please enter a password that is at least 8 characters long and includes at least one uppercase letter, one lowercase letter, one digit, and one special character");
                goto Password;
            }
            if (_studentService.CheckPasswordStrength(password, name, surname) == false)
            {
                ConsoleColor.Red.ConsoleMessage("Warning: Your password should not contain your name or surname as it poses a security risk.Please choose a password that is unique and not easily guessable.");
                goto Password;
            }

            _teacherService.RegisterTeacher(new Teacher { Name = name, Surname = surname, Email = email, Password = password });
            ConsoleColor.Green.ConsoleMessage(ResponseMessages.SuccessfullMessage);
        }
        public void DeleteTeacher()
        {
        Id: ConsoleColor.Cyan.ConsoleMessage("Add your id: ");
            int id;
            bool isCorrectFormat = int.TryParse(Console.ReadLine(), out id);

            if (!isCorrectFormat)
            {
                ConsoleColor.Red.ConsoleMessage(ResponseMessages.WrongFormat + "Add Id again");
                goto Id;
            }
            else
            {
                _teacherService.Delete(id);
                ConsoleColor.Green.ConsoleMessage(ResponseMessages.SuccessfullMessage);
            }
        }
        public void GetAll()
        {
            var teachers = _teacherService.GetAll();
            foreach (var teacher in teachers)
            {
                Console.WriteLine(string.Format(ResponseMessages.TeacherDataForDisplay, teacher.Id, teacher.Name, teacher.Surname));
            }
        }
        public void Login()
        {
        Email: ConsoleColor.Cyan.ConsoleMessage("Enter your email: ");
            string email = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(email))
            {
                ConsoleColor.Red.ConsoleMessage(string.Format(ResponseMessages.EmptyInput, "Email"));
                goto Email;
            }

        Password: ConsoleColor.Cyan.ConsoleMessage("Enter your email: ");
            string password = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(password))
            {
                ConsoleColor.Red.ConsoleMessage(string.Format(ResponseMessages.EmptyInput, "Password"));
                goto Password;
            }

            if (!_teacherService.Login(email, password))
            {
                ConsoleColor.Red.ConsoleMessage("Email or password is wrong. Try again: ");
                goto Email;
            }
        }
        public void GetAllStudents()
        {
            var students = _studentService.GetAll();
            foreach (var student in students)
            {
                Console.WriteLine(string.Format(ResponseMessages.StudentDataForDisplay, student.Id, student.Name, student.Surname, student.Age, student.Group.Name));
            }
        }
        public void GetAllGroups()
        {
            var groups = _groupService.GetAll();
            foreach (var group in groups)
            {
                Console.WriteLine(string.Format(ResponseMessages.GroupDataForDisplay, group.Id, group.Name, group.Teacher, group.Room));
            }
        }
        public void SearchStudents()
        {
        Search: ConsoleColor.Cyan.ConsoleMessage("Add search text: ");
            string searchText = Console.ReadLine();

            try
            {
                var students = _studentService.SearchByNameOrSurname(searchText.Trim().ToLower());

                foreach (var student in students)
                {
                    Console.WriteLine(string.Format(ResponseMessages.StudentDataForDisplay, student.Id, student.Name, student.Surname, student.Age, student.Group.Name));
                }
            }
            catch (ArgumentNullException ex)
            {
                ConsoleColor.Red.ConsoleMessage(ex.Message);
                goto Search;
            }
            catch (DataNotFoundException ex)
            {
                ConsoleColor.Red.ConsoleMessage(ex.Message);
            }
        }
        public void SearchGroups()
        {
            ConsoleColor.Cyan.ConsoleMessage("Add Search Text: ");
            string searchText = Console.ReadLine();

            var groups = _groupService.SearchByName(searchText.Trim().ToLower());
            foreach (var group in groups)
            {
                Console.WriteLine(string.Format(ResponseMessages.GroupDataForDisplay, group.Id, group.Name, group.Teacher, group.Room));
            }
        }
        public void GradeStudent()
        {
        Id: ConsoleColor.Cyan.ConsoleMessage("Enter ID of the student you want to grade: ");
            ConsoleColor.Cyan.ConsoleMessage("Available students: ");
            var students = _studentService.GetAll();
            foreach(var student in students)
            {
                Console.WriteLine(string.Format(ResponseMessages.StudentDataForDisplay, student.Id, student.Name, student.Surname, student.Age, student.Group.Name));
            }

            string studentIdStr = Console.ReadLine();
            int studentId;
            bool isCorrectFormat = int.TryParse(studentIdStr, out studentId);

            if (!isCorrectFormat)
            {
                ConsoleColor.Red.ConsoleMessage(ResponseMessages.WrongFormat);
                goto Id;
            }
            else
            {
                var student = _studentService.GetStudentById(studentId);
                _teacherService.GradeStudent(studentId, 100);
            }
        }
    }
}
