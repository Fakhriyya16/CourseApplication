using Domain.Models;
using Service.Helpers.Constants;
using Service.Helpers.Exceptions;
using Service.Helpers.Extensions;
using Service.Services;
using System.Diagnostics.Metrics;
using System.Net;
using System.Numerics;
using System;
using System.Text.RegularExpressions;
using Group = Domain.Models.Group;
using System.ComponentModel;
using System.Xml;
using Service.Helpers.Enums;

namespace FinalProject.Controllers
{
    public class StudentController
    {
        private readonly StudentService _studentService;
        private readonly GroupService _groupService;

        public StudentController()
        {
            _studentService = new StudentService();
            _groupService = new GroupService();
        }

        public void RegisterStudent()
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

        Age: ConsoleColor.Cyan.ConsoleMessage("Add age: ");
            int age;
            bool isAccurateNumber = int.TryParse(Console.ReadLine(), out age);

            if (!isAccurateNumber)
            {
                ConsoleColor.Red.ConsoleMessage(string.Format(ResponseMessages.WrongFormat, "Age"));
                goto Age;
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
            var students = _studentService.GetAll();
            foreach(var student in students)
            {
                if(email == student.Email)
                {
                    ConsoleColor.Red.ConsoleMessage("Student with the email address you provided already exists. Please use a different email address.");
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
                ConsoleColor.Red.ConsoleMessage(string.Format(ResponseMessages.EmptyInput, "Email"));
                goto Email;
            }
            Match match4 = regex4.Match(password);
            if (!match4.Success)
            {
                ConsoleColor.Red.ConsoleMessage(string.Format(ResponseMessages.WrongFormat, "Password") + " " + "Please enter a password that is at least 8 characters long and includes at least one uppercase letter, one lowercase letter, one digit, and one special character");
                goto Password;
            }
            if(_studentService.CheckPasswordStrength(password, name, surname) == false)
            {
                ConsoleColor.Red.ConsoleMessage("Warning: Your password should not contain your name or surname as it poses a security risk.Please choose a password that is unique and not easily guessable.");
                goto Password;
            }

        GroupName: ConsoleColor.Cyan.ConsoleMessage("Add group name: ");
            string groupName = Console.ReadLine();
        CheckGroupName: if (string.IsNullOrWhiteSpace(groupName))
            {
                ConsoleColor.Red.ConsoleMessage(string.Format(ResponseMessages.EmptyInput, "Group name"));
                goto GroupName;
            }
            try
            { 
                Group group = _groupService.GetByName(groupName);
                if (group == null) throw new DataNotFoundException(string.Format(ResponseMessages.DoesNotExist, "Student", "name"));

                _studentService.RegisterStudent(new Student { Name = name, Surname = surname, Age = age, Group = group, Email = email, Password = password});
                ConsoleColor.Green.ConsoleMessage(ResponseMessages.SuccessfullMessage);
            }
            catch (ArgumentNullException ex)
            {
                ConsoleColor.Red.ConsoleMessage(ex.Message);
                goto GroupName;
            }
            catch (DataNotFoundException ex)
            {
                ConsoleColor.Red.ConsoleMessage(ex.Message);
                ConsoleColor.Cyan.ConsoleMessage("Add Group Name again or type word - exit: ");
                groupName = Console.ReadLine();
                if(groupName.Trim().ToLower() == "exit")
                {
                    throw new RegistrationFailedException("Registration failed");
                }
                else
                {
                    goto CheckGroupName;
                }
            }
        }
        public void UpdateStudent()
        {
            try
            {
            Id: ConsoleColor.Cyan.ConsoleMessage("Enter your id:");
                int id;
                bool isCorrectFormat = int.TryParse(Console.ReadLine(), out id);

                if (!isCorrectFormat)
                {
                    ConsoleColor.Red.ConsoleMessage(ResponseMessages.WrongFormat + "Add your Id again");
                    goto Id;
                }

                Student foundStudent = _studentService.UpdateStudent(id);
                ConsoleColor.Cyan.ConsoleMessage("Update name:");
                string studentName = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(studentName))
                {
                    foundStudent.Name = _studentService.UpdateStudent(id).Name;
                }
                else
                {
                    foundStudent.Name = studentName;
                }

                ConsoleColor.Cyan.ConsoleMessage("Update surname:");
                string studentSurname = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(studentSurname))
                {
                    foundStudent.Surname = _studentService.UpdateStudent(id).Surname;
                }
                else
                {
                    foundStudent.Surname = studentSurname;
                }

            Age: ConsoleColor.Cyan.ConsoleMessage("Update age:");
                int age;
                string result = Console.ReadLine();
                bool isCorrectFormatOfAge = int.TryParse(result, out age);
                if (!isCorrectFormatOfAge)
                {
                    if (string.IsNullOrEmpty(result))
                    {
                        foundStudent.Age = _studentService.UpdateStudent(id).Age;
                    }
                    else
                    {
                        ConsoleColor.Red.ConsoleMessage(ResponseMessages.WrongFormat);
                        goto Age;
                    }
                }
                else
                {
                    foundStudent.Age = age;
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


        }
        public void DeleteStudent()
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
                _studentService.Delete(id);
                ConsoleColor.Green.ConsoleMessage(ResponseMessages.SuccessfullMessage);
            }
        }
        public void GetAll()
        {
            var students = _studentService.GetAll();
            foreach (var student in students)
            {
                Console.WriteLine(string.Format(ResponseMessages.StudentDataForDisplay, student.Id, student.Name, student.Surname, student.Age, student.Group.Name));
            }
        }
        public void GetStudentById()
        {
        Id: ConsoleColor.Cyan.ConsoleMessage("Add your id: ");
            int id;
            bool isCorrectFormat = int.TryParse(Console.ReadLine(), out id);

            try
            {
                if (!isCorrectFormat)
                {
                    ConsoleColor.Red.ConsoleMessage(ResponseMessages.WrongFormat);
                    goto Id;
                }
                else
                {
                    var student = _studentService.GetStudentById(id);
                    ConsoleColor.Green.ConsoleMessage(string.Format(ResponseMessages.StudentDataForDisplay, student.Id, student.Name, student.Surname, student.Age, student.Group.Name));
                }
            }
            catch (ArgumentNullException ex)
            {
                ConsoleColor.Red.ConsoleMessage(ex.Message);
                goto Id;
            }
            catch (DataNotFoundException ex)
            {
                ConsoleColor.Red.ConsoleMessage(ex.Message);
            }
        }
        public void GetStudentsByAge()
        {
        Age: ConsoleColor.Cyan.ConsoleMessage("Add Age For Search:");
            int age;
            bool isCorrectFormat = int.TryParse(Console.ReadLine(), out age);

            try
            {
                if (!isCorrectFormat)
                {
                    ConsoleColor.Red.ConsoleMessage(ResponseMessages.WrongFormat);
                    goto Age;
                }
                else
                {
                    var students = _studentService.GetStudentByAge(age);
                    foreach (var student in students)
                    {
                        ConsoleColor.Green.ConsoleMessage(string.Format(ResponseMessages.StudentDataForDisplay, student.Id, student.Name, student.Surname, student.Age, student.Group.Name));
                    }
                }
            }
            catch (ArgumentNullException ex)
            {
                ConsoleColor.Red.ConsoleMessage(ex.Message);
                goto Age;
            }
            catch (DataNotFoundException ex)
            {
                ConsoleColor.Red.ConsoleMessage(ex.Message);
            }
        }
        public void GetStudentsByGroupId()
        {
        Id: ConsoleColor.Cyan.ConsoleMessage("Add Group Id: ");
            int id;
            bool isCorrectFormat = int.TryParse(Console.ReadLine(), out id);

            try
            {
                if (!isCorrectFormat)
                {
                    ConsoleColor.Red.ConsoleMessage(ResponseMessages.WrongFormat + "Add Id again");
                    goto Id;
                }
                else
                {
                    var students = _studentService.GetAllStudentsByGroupId(id);
                    foreach (var student in students)
                    {
                        ConsoleColor.Green.ConsoleMessage(string.Format(ResponseMessages.StudentDataForDisplay, student.Id, student.Name, student.Surname, student.Age, student.Group.Name));
                    }
                }
            }
            catch (ArgumentNullException ex)
            {
                ConsoleColor.Red.ConsoleMessage(ex.Message);
                goto Id;
            }
            catch (DataNotFoundException ex)
            {
                ConsoleColor.Red.ConsoleMessage(ex.Message);
            }
        }
        public void SearchStudent()
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
        public void ChangeGroup()
        {
        Id: ConsoleColor.Cyan.ConsoleMessage("Enter your id:");
            int id;
            bool isCorrectFormat = int.TryParse(Console.ReadLine(), out id);

            if (!isCorrectFormat)
            {
                ConsoleColor.Red.ConsoleMessage(ResponseMessages.WrongFormat + "Add your Id again");
                goto Id;
            }
            var foundStudent = _studentService.GetStudentById(id);

        GroupId: ConsoleColor.Cyan.ConsoleMessage("Enter id of the group you want to choose:\nAvailable Groups: ");
            var availableGroups = _groupService.GetAll();
            foreach (var group in availableGroups)
            {
                ConsoleColor.DarkYellow.ConsoleMessage(string.Format(ResponseMessages.GroupDataForDisplay, group.Id, group.Name, group.Teacher, group.Room));
            }

            int groupId;
            bool isCorrectFormatOfId = int.TryParse(Console.ReadLine(), out groupId);

            if (!isCorrectFormatOfId)
            {
                ConsoleColor.Red.ConsoleMessage(ResponseMessages.WrongFormat + "Add Group Id again");
                goto GroupId;
            }

            var foundGroup = _groupService.GetById(groupId);

            foundStudent.Group = foundGroup;


            ConsoleColor.Green.ConsoleMessage(ResponseMessages.SuccessfullMessage);
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

            if(!_studentService.Login(email, password))
            {
                ConsoleColor.Red.ConsoleMessage("Email or password is wrong. Try again: ");
                goto Email;
            }
        }
    }
}
