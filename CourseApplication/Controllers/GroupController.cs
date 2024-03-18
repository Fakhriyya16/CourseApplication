using Service.Helpers.Constants;
using Service.Helpers.Exceptions;
using Service.Helpers.Extensions;
using Service.Services;
using System.Text.RegularExpressions;
using Group = Domain.Models.Group;

namespace FinalProject.Controllers
{
    public class GroupController
    {
        private readonly GroupService _groupService;
        private readonly StudentService _studentService;

        public GroupController()
        {
            _groupService = new GroupService();
            _studentService = new StudentService();
        }

        public void CreateGroup()
        {
            try
            {
            Name: ConsoleColor.Cyan.ConsoleMessage("Add group name:");
                string name = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(name.Trim().ToLower()))
                {
                    ConsoleColor.Red.ConsoleMessage(string.Format(ResponseMessages.EmptyInput, "Name"));
                    goto Name;
                }
                var availability = _groupService.CheckAvailability(name);
                if (availability)
                {
                Teacher: ConsoleColor.Cyan.ConsoleMessage("Add teacher's name:");
                    string teacher = Console.ReadLine();
                    string namePattern = @"^[A-Z][a-z]*$";
                    Regex regex = new(namePattern);

                    if (string.IsNullOrWhiteSpace(teacher))
                    {
                        ConsoleColor.Red.ConsoleMessage(string.Format(ResponseMessages.EmptyInput, "Teacher's name"));
                        goto Teacher;
                    }
                    Match match = regex.Match(teacher);
                    if (!match.Success)
                    {
                        ConsoleColor.Red.ConsoleMessage(string.Format(ResponseMessages.WrongFormat, "name") + " " + "Name must start with uppercase letter and contain only letters");
                        goto Teacher;
                    }

                Room: ConsoleColor.Cyan.ConsoleMessage("Add room:");
                    string room = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(room))
                    {
                        ConsoleColor.Red.ConsoleMessage(string.Format(ResponseMessages.EmptyInput, "Room"));
                        goto Room;
                    }

                    _groupService.Create(new Group { Name = name, Teacher = teacher, Room = room });

                    ConsoleColor.Green.ConsoleMessage("Group successfully added");
                }
                else
                {
                    ConsoleColor.Red.ConsoleMessage("A group with this name already exists. Please choose a different name for the new group:");
                    goto Name;
                }
            }
            catch (ArgumentNullException ex)
            {
                ConsoleColor.Red.ConsoleMessage(ex.Message);
            }
        }
        
        public void UpdateGroupData()
        {
            Id: ConsoleColor.Cyan.ConsoleMessage("Enter group id:");
            int id;
            bool isCorrectFormat = int.TryParse(Console.ReadLine(), out id);

            if (!isCorrectFormat)
            {
                ConsoleColor.Red.ConsoleMessage(string.Format(ResponseMessages.WrongFormat,"id"));
                goto Id;
            }
            try
            {
                Group foundGroup = _groupService.UpdateGroup(id);

                ConsoleColor.Cyan.ConsoleMessage("Update name of the group:");
                string roomName = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(roomName))
                {
                    foundGroup.Name = _groupService.UpdateGroup(id).Name;
                }
                else
                {
                    foundGroup.Name = roomName;
                }

                ConsoleColor.Cyan.ConsoleMessage("Update teacher's name");
                string teacherName = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(teacherName))
                {
                    foundGroup.Teacher = _groupService.UpdateGroup(id).Teacher;
                }
                else
                {
                    foundGroup.Teacher = teacherName;
                }

                ConsoleColor.Cyan.ConsoleMessage("Update room:");
                string room = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(room))
                {
                    foundGroup.Room = _groupService.UpdateGroup(id).Room;
                }
                else
                {
                    foundGroup.Teacher = room;
                }
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

        public void GetAll()
        {
            var groups = _groupService.GetAll();
            foreach (var group in groups)
            {
                Console.WriteLine(string.Format(ResponseMessages.GroupDataForDisplay, group.Id, group.Name, group.Teacher, group.Room));
            }
        }

        public void DeleteGroup()
        {
            Id: ConsoleColor.Cyan.ConsoleMessage("Add Group Id:");
            try
            {
                int id;
                bool isCorrectFormat = int.TryParse(Console.ReadLine(), out id);

                if (!isCorrectFormat)
                {
                    ConsoleColor.Red.ConsoleMessage(string.Format(ResponseMessages.WrongFormat,"id"));
                    goto Id;
                }
                else
                {
                    var students = _studentService.GetAllStudentsByGroupId(id);
                    foreach( var student in students)
                    {
                        int studentId = student.Id;
                        _studentService.Delete(studentId);
                    }
                    _groupService.Delete(id);

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
        public void GetGroupById()
        {
        Id: ConsoleColor.Cyan.ConsoleMessage("Add Group Id:");
            try
            {
                int id;
                bool isCorrectFormat = int.TryParse(Console.ReadLine(), out id);

                if (!isCorrectFormat)
                {
                    ConsoleColor.Red.ConsoleMessage(string.Format(ResponseMessages.WrongFormat,"id"));
                    goto Id;
                }
                else
                {
                    var group = _groupService.GetById(id);
                    Console.WriteLine(string.Format(ResponseMessages.GroupDataForDisplay, group.Id, group.Name, group.Teacher, group.Room));
                }
            }
            catch (ArgumentNullException ex)
            {
                ConsoleColor.Red.ConsoleMessage(ex.Message);
                goto Id;
            }
            catch(DataNotFoundException ex)
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
        public void GetGroupsByTeacher()
        {
        TeacherName: ConsoleColor.Cyan.ConsoleMessage("Add Teacher's name: ");
            try
            {
                string teacher = Console.ReadLine();

                var groups = _groupService.GetAllByTeacher(teacher.Trim().ToLower());
                foreach (var group in groups)
                {
                    Console.WriteLine(string.Format(ResponseMessages.GroupDataForDisplay, group.Id, group.Name, group.Teacher, group.Room));
                }
            }
            catch (ArgumentNullException ex)
            {
                ConsoleColor.Red.ConsoleMessage(ex.Message);
                goto TeacherName;
            }
            catch (DataNotFoundException ex)
            {
                ConsoleColor.Red.ConsoleMessage(ex.Message);
                Operation: ConsoleColor.Cyan.ConsoleMessage("Type\n again - to try again\n exit - to go back to menu");
                string response = Console.ReadLine();
                if(response != null)
                {
                    if(response == "again")
                    {
                        goto TeacherName;
                    }
                    else if(response == "exit")
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
        public void GetGroupsByRoom()
        {
        Room: ConsoleColor.Cyan.ConsoleMessage("Add Room: ");
            try
            {
                string room = Console.ReadLine();

                var groups = _groupService.GetAllByRoom(room.Trim().ToLower());
                foreach (var group in groups)
                {
                    Console.WriteLine(string.Format(ResponseMessages.GroupDataForDisplay, group.Id, group.Name, group.Teacher, group.Room));
                }
            }
            catch (ArgumentNullException ex)
            {
                ConsoleColor.Red.ConsoleMessage(ex.Message);
                goto Room;
            }
            catch(DataNotFoundException ex)
            {
                ConsoleColor.Red.ConsoleMessage(ex.Message);
            Operation: ConsoleColor.Cyan.ConsoleMessage("Type\n again - to try again\n exit - to go back to menu");
                string response = Console.ReadLine();
                if (response != null)
                {
                    if (response == "again")
                    {
                        goto Room; ;
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
        public void GetGroupsByName()
        {
        GroupName: ConsoleColor.Cyan.ConsoleMessage("Add Group Name: ");

            try
            {
                string groupName = Console.ReadLine();

                var group = _groupService.GetByName(groupName.Trim().ToLower());

                Console.WriteLine(string.Format(ResponseMessages.GroupDataForDisplay, group.Id, group.Name, group.Teacher, group.Room));
            }
            catch (ArgumentNullException ex)
            {
                ConsoleColor.Red.ConsoleMessage(ex.Message);
                goto GroupName;
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
                        goto GroupName;
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
        public void SearchGroup()
        {
        Search: ConsoleColor.Cyan.ConsoleMessage("Add Search Text: ");
            try
            {
                string searchText = Console.ReadLine();

                var groups = _groupService.SearchByName(searchText.Trim().ToLower());
                foreach (var group in groups)
                {
                    Console.WriteLine(string.Format(ResponseMessages.GroupDataForDisplay, group.Id, group.Name, group.Teacher, group.Room));
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
    }
}
