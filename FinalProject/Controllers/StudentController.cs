using Domain.Models;
using Service.Helpers.Constants;
using Service.Helpers.Exceptions;
using Service.Helpers.Extensions;
using Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public void CreateStudent()
        {
        Name: ConsoleColor.Cyan.ConsoleMessage("Add name: ");
            string name = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(name))
            {
                ConsoleColor.Red.ConsoleMessage(string.Format(ResponseMessages.EmptyInput, "Name"));
                goto Name;
            }

        Surname: ConsoleColor.Cyan.ConsoleMessage("Add surname: ");
            string surname = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(surname))
            {
                ConsoleColor.Red.ConsoleMessage(string.Format(ResponseMessages.EmptyInput, "Surname"));
                goto Surname;
            }

        Age: ConsoleColor.Cyan.ConsoleMessage("Add age: ");
            int age;
            bool isAccurateNumber = int.TryParse(Console.ReadLine(), out age);

            if (!isAccurateNumber)
            {
                ConsoleColor.Red.ConsoleMessage(ResponseMessages.WrongFormat);
                goto Age;
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

                _studentService.Create(new Student { Name = name, Surname = surname, Age = age, Group = group });
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
                    ConsoleColor.Green.ConsoleMessage("You are back to menu page");
                    return;
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



                ConsoleColor.Cyan.ConsoleMessage("Update Group name:");
                string newGroupName = Console.ReadLine();

                Group oldGroup = _groupService.GetByName(_studentService.UpdateStudent(id).Group.Name);

                if (string.IsNullOrWhiteSpace(newGroupName))
                {
                    foundStudent.Group.Name = oldGroup.Name;
                }
                else
                {
                    if (newGroupName == _groupService.GetByName(newGroupName).Name)
                    {
                        foundStudent.Group.Name = newGroupName;
                    }
                    else
                    {
                        throw new DataNotFoundException(string.Format(ResponseMessages.DoesNotExist, "Group", "name"));
                    }
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
    }
}
