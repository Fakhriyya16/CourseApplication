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

        public void DeleteStudent()
        {
            Id: ConsoleColor.Cyan.ConsoleMessage("Add your id: ");
            int id;
            bool isCorrectFormat = int.TryParse(Console.ReadLine(), out id);
            
            if(!isCorrectFormat)
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
                Console.WriteLine($"Id: {student.Id}, Name: {student.Name}, Surname: {student.Surname}, Age: {student.Age}, Group Name: {student.Group.Name}");
            }
        }
        public void GetStudentById()
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
                _studentService.GetStudentById(id);
                ConsoleColor.Green.ConsoleMessage(ResponseMessages.SuccessfullMessage);
            }
        }
        public void GetStudentsByAge()
        {
            Age:  ConsoleColor.Cyan.ConsoleMessage("Add Age For Search:");
            int age;
            bool isCorrectFormat = int.TryParse(Console.ReadLine(), out age);

            if (!isCorrectFormat)
            {
                ConsoleColor.Red.ConsoleMessage(ResponseMessages.WrongFormat + "Add Age Again");
                goto Age;
            }
            else
            {
                _studentService.GetStudentByAge(age);
                ConsoleColor.Green.ConsoleMessage(ResponseMessages.SuccessfullMessage);
            }
        }
        public void GetStudentsByGroupId()
        {
        Id: ConsoleColor.Cyan.ConsoleMessage("Add Group Id: ");
            int id;
            bool isCorrectFormat = int.TryParse(Console.ReadLine(), out id);

            if (!isCorrectFormat)
            {
                ConsoleColor.Red.ConsoleMessage(ResponseMessages.WrongFormat + "Add Id again");
                goto Id;
            }
            else
            {
                _studentService.GetAllStudentsByGroupId(id);
                ConsoleColor.Green.ConsoleMessage(ResponseMessages.SuccessfullMessage);
            }
        }
        public void SearchStudent()
        {
            Search: ConsoleColor.Cyan.ConsoleMessage("Add search text: ");
            string searchText = Console.ReadLine(); 

            if(string.IsNullOrWhiteSpace(searchText))
            {
                ConsoleColor.Red.ConsoleMessage(ResponseMessages.EmptyInput);
                goto Search;
            }
            else
            {
               var foundStudents = _studentService.SearchByNameOrSurname(searchText);
               foreach( var foundStudent in foundStudents) 
               {
                    Console.WriteLine($"Id: {foundStudent.Id}, Name: {foundStudent.Name}, Surname: {foundStudent.Surname}, Age: {foundStudent.Age}, Group Name: {foundStudent.Group.Name}"); 
               }
            }  
        }
    }
}
