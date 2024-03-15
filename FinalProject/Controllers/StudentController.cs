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
                ConsoleColor.Red.ConsoleMessage(ResponseMessages.EmptyInput);
                goto Name;
            }

        Surname: ConsoleColor.Cyan.ConsoleMessage("Add surname: ");
            string surname = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(surname))
            {
                ConsoleColor.Red.ConsoleMessage(ResponseMessages.EmptyInput);
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

            if (string.IsNullOrWhiteSpace(groupName))
            {
                ConsoleColor.Red.ConsoleMessage(ResponseMessages.EmptyInput);
                goto GroupName;
            }

            Group group = _groupService.GetByName(groupName);
            if (group == null) throw new DataNotFoundException(ResponseMessages.DataNotFound);

            _studentService.Create(new Student { Name = name, Surname = surname, Age = age, Group = group });

            ConsoleColor.Green.ConsoleMessage("Student successfully added");
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
                        throw new DataNotFoundException("Group with this name does not exist");
                    }
                }
            }
            
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        

        }

        public void GetAll()
        {
            var students = _studentService.GetAll();
            foreach (var student in students)
            {
                Console.WriteLine(student.Id + " " + student.Name + " " + student.Surname + "-" + student.Age + "-" + student.Group.Name);
            }
        }
    }
}
