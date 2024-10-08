﻿using Domain.Models;
using Service.Helpers.Constants;
using Service.Helpers.Exceptions;
using Service.Helpers.Extensions;
using Service.Services;
using System.Text.RegularExpressions;
using Group = Domain.Models.Group;


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
                ConsoleColor.Red.ConsoleMessage(string.Format(ResponseMessages.WrongFormat, "surname") + " " + "Surname must start with uppercase letter and contain only letters");
                goto Surname;
            }

        Age: ConsoleColor.Cyan.ConsoleMessage("Add age: ");
            int age;
            bool isAccurateNumber = int.TryParse(Console.ReadLine(), out age);

            if (!isAccurateNumber)
            {
                ConsoleColor.Red.ConsoleMessage(string.Format(ResponseMessages.WrongFormat, "age"));
                goto Age;
            }
            else if(age <15 || age > 50)
            {
                throw new AgeRequirementException("Sorry, your age does not meet the requirements for registration.");
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
                ConsoleColor.Red.ConsoleMessage(string.Format(ResponseMessages.WrongFormat, "email") + " " + "Please enter a valid email address in the format 'example@example.com'.");
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
                ConsoleColor.Red.ConsoleMessage(string.Format(ResponseMessages.WrongFormat, "password") + " " + "Please enter a password that is at least 8 characters long and includes at least one uppercase letter, one lowercase letter, one digit, and one special character");
                goto Password;
            }
            if(_studentService.CheckPasswordStrength(password, name, surname) == false)
            {
                ConsoleColor.Red.ConsoleMessage("Warning: Your password should not contain your name or surname as it poses a security risk.Please choose a password that is unique and not easily guessable.");
                goto Password;
            }

        GroupName: ConsoleColor.Cyan.ConsoleMessage("Choose group you want to join: ");
            var groups = _groupService.GetAll();
            if(groups.Count == 0)
            {
                throw new NoGroupsAvailableException("Currently, there are no available groups to join.Please check back later");
            }
            else
            {
                foreach (var group in groups)
                {
                    ConsoleColor.Yellow.ConsoleMessage(string.Format(ResponseMessages.GroupDataForDisplay, group.Id, group.Name, group.Teacher, group.Room));
                }
            }
            
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
            Id: ConsoleColor.Cyan.ConsoleMessage("Enter student id:");
                int id;
                bool isCorrectFormat = int.TryParse(Console.ReadLine(), out id);

                if (!isCorrectFormat)
                {
                    ConsoleColor.Red.ConsoleMessage(string.Format(ResponseMessages.WrongFormat,"id"));
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
                        ConsoleColor.Red.ConsoleMessage(string.Format(ResponseMessages.WrongFormat, "age"));
                        goto Age;
                    }
                }
                else
                {
                    foundStudent.Age = age;
                }
                ConsoleColor.Green.ConsoleMessage(ResponseMessages.SuccessfullMessage);
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


        }
        public void DeleteStudent()
        {
        Id: ConsoleColor.Cyan.ConsoleMessage("Add student id: ");
            int id;
            bool isCorrectFormat = int.TryParse(Console.ReadLine(), out id);
            try
            {
                if (!isCorrectFormat)
                {
                    ConsoleColor.Red.ConsoleMessage(string.Format(ResponseMessages.WrongFormat, "id"));
                    goto Id;
                }
                else
                {
                    _studentService.Delete(id);
                    ConsoleColor.Green.ConsoleMessage(ResponseMessages.SuccessfullMessage);
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
            Operation: ConsoleColor.Cyan.ConsoleMessage("Type\n again - to try again\n exit - to go back to menu");
                string response = Console.ReadLine();
                if (response != null)
                {
                    if (response == "again")
                    {
                        goto Id;
                    }
                    else if (response == "exit")
                    {
                        return;
                    }
                    else
                    {
                        ConsoleColor.Red.ConsoleMessage(string.Format(ResponseMessages.WrongFormat, "operation"));
                        goto Operation;
                    }
                }
                else
                {
                    ConsoleColor.Red.ConsoleMessage(string.Format(ResponseMessages.WrongFormat, "operation"));
                    goto Operation;
                }
            }
        }
        public void GetAll()
        {
            var students = _studentService.GetAll();
            foreach (var student in students)
            {
                Console.WriteLine(string.Format(ResponseMessages.StudentDataForDisplay, student.Id, student.Name, student.Surname, student.Age, student.Group.Name, student.Email));
            }
        }
        public void GetStudentById()
        {
        Id: ConsoleColor.Cyan.ConsoleMessage("Add student id: ");
            int id;
            bool isCorrectFormat = int.TryParse(Console.ReadLine(), out id);

            try
            {
                if (!isCorrectFormat)
                {
                    ConsoleColor.Red.ConsoleMessage(string.Format(ResponseMessages.WrongFormat, "id"));
                    goto Id;
                }
                else
                {
                    var student = _studentService.GetStudentById(id);
                    ConsoleColor.Green.ConsoleMessage(string.Format(ResponseMessages.StudentDataForDisplay, student.Id, student.Name, student.Surname, student.Age, student.Group.Name, student.Email));
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
            Operation: ConsoleColor.Cyan.ConsoleMessage("Type\n again - to try again\n exit - to go back to menu");
                string response = Console.ReadLine();
                if (response != null)
                {
                    if (response == "again")
                    {
                        goto Id;
                    }
                    else if (response == "exit")
                    {
                        return;
                    }
                    else
                    {
                        ConsoleColor.Red.ConsoleMessage(string.Format(ResponseMessages.WrongFormat, "operation"));
                        goto Operation;
                    }
                }
                else
                {
                    ConsoleColor.Red.ConsoleMessage(string.Format(ResponseMessages.WrongFormat, "operation"));
                    goto Operation;
                }
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
                    ConsoleColor.Red.ConsoleMessage(string.Format(ResponseMessages.WrongFormat,"age"));
                    goto Age;
                }
                else
                {
                    var students = _studentService.GetStudentByAge(age);
                    foreach (var student in students)
                    {
                        ConsoleColor.Green.ConsoleMessage(string.Format(ResponseMessages.StudentDataForDisplay, student.Id, student.Name, student.Surname, student.Age, student.Group.Name, student.Email));
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
            Operation: ConsoleColor.Cyan.ConsoleMessage("Type\n again - to try again\n exit - to go back to menu");
                string response = Console.ReadLine();
                if (response != null)
                {
                    if (response == "again")
                    {
                        goto Age;
                    }
                    else if (response == "exit")
                    {
                        return;
                    }
                    else
                    {
                        ConsoleColor.Red.ConsoleMessage(string.Format(ResponseMessages.WrongFormat, "operation"));
                        goto Operation;
                    }
                }
                else
                {
                    ConsoleColor.Red.ConsoleMessage(string.Format(ResponseMessages.WrongFormat, "operation"));
                    goto Operation;
                }
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
                    ConsoleColor.Red.ConsoleMessage(string.Format(ResponseMessages.WrongFormat,"id"));
                    goto Id;
                }
                else if(id > _groupService.GetAll().Count)
                {
                    ConsoleColor.Red.ConsoleMessage(String.Format(ResponseMessages.DoesNotExist, "Group", "id"));
                    goto Id;
                }
                else
                {
                    var students = _studentService.GetAllStudentsByGroupId(id);
                    foreach (var student in students)
                    {
                        ConsoleColor.Green.ConsoleMessage(string.Format(ResponseMessages.StudentDataForDisplay, student.Id, student.Name, student.Surname, student.Age, student.Group.Name, student.Email));
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
            Operation: ConsoleColor.Cyan.ConsoleMessage("Type\n again - to try again\n exit - to go back to menu");
                string response = Console.ReadLine();
                if (response != null)
                {
                    if (response == "again")
                    {
                        goto Id;
                    }
                    else if (response == "exit")
                    {
                        return;
                    }
                    else
                    {
                        ConsoleColor.Red.ConsoleMessage(string.Format(ResponseMessages.WrongFormat, "operation"));
                        goto Operation;
                    }
                }
                else
                {
                    ConsoleColor.Red.ConsoleMessage(string.Format(ResponseMessages.WrongFormat, "operation"));
                    goto Operation;
                }
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
                    Console.WriteLine(string.Format(ResponseMessages.StudentDataForDisplay, student.Id, student.Name, student.Surname, student.Age, student.Group.Name, student.Email));
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
            Operation: ConsoleColor.Cyan.ConsoleMessage("Type\n again - to try again\n exit - to go back to menu");
                string response = Console.ReadLine();
                if (response != null)
                {
                    if (response == "again")
                    {
                        goto Search;
                    }
                    else if (response == "exit")
                    {
                        return;
                    }
                    else
                    {
                        ConsoleColor.Red.ConsoleMessage(string.Format(ResponseMessages.WrongFormat, "operation"));
                        goto Operation;
                    }
                }
                else
                {
                    ConsoleColor.Red.ConsoleMessage(string.Format(ResponseMessages.WrongFormat, "operation"));
                    goto Operation;
                }
            }
        }
        public void ChangeGroup()
        {
        Id: ConsoleColor.Cyan.ConsoleMessage("Enter your id:");
            try
            {
                int id;
                bool isCorrectFormat = int.TryParse(Console.ReadLine(), out id);

                if (!isCorrectFormat)
                {
                    ConsoleColor.Red.ConsoleMessage(string.Format(ResponseMessages.WrongFormat, "id"));
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
                    ConsoleColor.Red.ConsoleMessage(string.Format(ResponseMessages.WrongFormat, "id"));
                    goto GroupId;
                }

                var foundGroup = _groupService.GetById(groupId);

                foundStudent.Group = foundGroup;


                ConsoleColor.Green.ConsoleMessage(ResponseMessages.SuccessfullMessage);
            }
            catch (ArgumentNullException ex)
            {
                ConsoleColor.Red.ConsoleMessage(ex.Message);
                goto Id;
            }
            catch (DataNotFoundException ex)
            {
                ConsoleColor.Red.ConsoleMessage(ex.Message);
            Operation: ConsoleColor.Cyan.ConsoleMessage("Type\n again - to try again\n exit - to go back to menu");
                string response = Console.ReadLine();
                if (response != null)
                {
                    if (response == "again")
                    {
                        goto Id;
                    }
                    else if (response == "exit")
                    {
                        return;
                    }
                    else
                    {
                        ConsoleColor.Red.ConsoleMessage(string.Format(ResponseMessages.WrongFormat, "operation"));
                        goto Operation;
                    }
                }
                else
                {
                    ConsoleColor.Red.ConsoleMessage(string.Format(ResponseMessages.WrongFormat, "operation"));
                    goto Operation;
                }
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

        Password: ConsoleColor.Cyan.ConsoleMessage("Enter your password: ");
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
        public void ShowGrade()
        {
            Id: ConsoleColor.Cyan.ConsoleMessage("Enter student id:");
            try
            {
                string studentIdStr = Console.ReadLine();
                bool isCorrectFormat = int.TryParse(studentIdStr, out int studentId);
                if (!isCorrectFormat)
                {
                    ConsoleColor.Red.ConsoleMessage(string.Format(ResponseMessages.WrongFormat, "id"));
                    goto Id;
                }
                else
                {
                    var student = _studentService.ShowGrade(studentId);
                    if (student.Grade is null)
                    {
                        ConsoleColor.Red.ConsoleMessage("You haven't been graded yet.");
                    }
                    else
                    {
                        ConsoleColor.Green.ConsoleMessage(student.Grade.ToString());
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
            Operation: ConsoleColor.Cyan.ConsoleMessage("Type\n again - to try again\n exit - to go back to menu");
                string response = Console.ReadLine();
                if (response != null)
                {
                    if (response == "again")
                    {
                        goto Id;
                    }
                    else if (response == "exit")
                    {
                        return;
                    }
                    else
                    {
                        ConsoleColor.Red.ConsoleMessage(string.Format(ResponseMessages.WrongFormat, "operation"));
                        goto Operation;
                    }
                }
                else
                {
                    ConsoleColor.Red.ConsoleMessage(string.Format(ResponseMessages.WrongFormat, "operation"));
                    goto Operation;
                }
            }
        }
    }
}
