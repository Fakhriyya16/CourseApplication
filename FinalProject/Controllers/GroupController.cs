using Domain.Models;
using Service.Helpers.Constants;
using Service.Helpers.Extensions;
using Service.Services;
using System.Text.RegularExpressions;
using Group = Domain.Models.Group;

namespace FinalProject.Controllers
{
    public class GroupController
    {
        private readonly GroupService _groupService;

        public GroupController()
        {
            _groupService = new GroupService();
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
                ConsoleColor.Red.ConsoleMessage(ResponseMessages.WrongFormat + "Add your Id again");
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
            }
            catch (ArgumentNullException ex)
            {
                ConsoleColor.Red.ConsoleMessage(ex.Message);
                goto Id;
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
            int id;
            bool isCorrectFormat = int.TryParse(Console.ReadLine(), out id);

            if (!isCorrectFormat)
            {
                ConsoleColor.Red.ConsoleMessage(ResponseMessages.WrongFormat);
                goto Id;
            }
            else
            {
                _groupService.Delete(id);
                ConsoleColor.Green.ConsoleMessage(ResponseMessages.SuccessfullMessage);
            }
        }
        public void GetGroupById()
        {
        Id: ConsoleColor.Cyan.ConsoleMessage("Add Group Id:");
            int id;
            bool isCorrectFormat = int.TryParse(Console.ReadLine(), out id);

            if (!isCorrectFormat)
            {
                ConsoleColor.Red.ConsoleMessage(ResponseMessages.WrongFormat);
                goto Id;
            }
            else
            {
                var group = _groupService.GetById(id);
                Console.WriteLine(string.Format(ResponseMessages.GroupDataForDisplay, group.Id, group.Name, group.Teacher, group.Room));
            }
        }
        public void GetGroupsByTeacher()
        {
            ConsoleColor.Cyan.ConsoleMessage("Add Teacher's name: ");
            string teacher = Console.ReadLine();

            var groups = _groupService.GetAllByTeacher(teacher.Trim().ToLower());
            foreach( var group in groups )
            {
                Console.WriteLine(string.Format(ResponseMessages.GroupDataForDisplay, group.Id, group.Name, group.Teacher, group.Room));
            }  
        }
        public void GetGroupsByRoom()
        {
            ConsoleColor.Cyan.ConsoleMessage("Add Room: ");
            string room = Console.ReadLine();

            var groups = _groupService.GetAllByRoom(room.Trim().ToLower());
            foreach (var group in groups)
            {
                Console.WriteLine(string.Format(ResponseMessages.GroupDataForDisplay, group.Id, group.Name, group.Teacher, group.Room));
            }
        }
        public void GetGroupsByName()
        {
            ConsoleColor.Cyan.ConsoleMessage("Add Group Name: ");
            string groupName = Console.ReadLine();

            var group = _groupService.GetByName(groupName.Trim().ToLower());

            Console.WriteLine(string.Format(ResponseMessages.GroupDataForDisplay, group.Id, group.Name, group.Teacher, group.Room));           
        }
        public void SearchGroup()
        {
            ConsoleColor.Cyan.ConsoleMessage("Add Search Text: ");
            string searchText = Console.ReadLine();

            var groups = _groupService.SearchByName(searchText.Trim().ToLower());
            foreach (var group in groups)
            {
                Console.WriteLine(string.Format(ResponseMessages.GroupDataForDisplay, group.Id, group.Name, group.Teacher, group.Room));
            }
        }
    }
}
