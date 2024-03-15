using Service.Helpers.Constants;
using Service.Helpers.Extensions;
using Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        Name: ConsoleColor.Cyan.ConsoleMessage("Add group name:");
            string name = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(name))
            {
                ConsoleColor.Red.ConsoleMessage(ResponseMessages.EmptyInput);
                goto Name;
            }

        Teacher: ConsoleColor.Cyan.ConsoleMessage("Add teacher's name:");
            string teacher = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(teacher))
            {
                ConsoleColor.Red.ConsoleMessage(ResponseMessages.EmptyInput);
                goto Teacher;
            }

        Room: ConsoleColor.Cyan.ConsoleMessage("Add room:");
            string room = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(room))
            {
                ConsoleColor.Red.ConsoleMessage(ResponseMessages.EmptyInput);
                goto Room;
            }

            _groupService.Create(new Group { Name = name, Teacher = teacher, Room = room });

            ConsoleColor.Green.ConsoleMessage("Group successfully added");

        }
    }
}
