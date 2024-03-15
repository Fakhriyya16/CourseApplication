using Domain.Models;
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

            Group foundGroup = _groupService.UpdateGroup(id);

            ConsoleColor.Cyan.ConsoleMessage("Update name of the group:");
            string roomName = Console.ReadLine();

            if(string.IsNullOrWhiteSpace(roomName))
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

        public void GetAll()
        {
            var groups = _groupService.GetAll();
            foreach (var group in groups)
            {
                Console.WriteLine(group.Id + "-" + group.Name + " - " + group.Teacher + " - " + group.Room);
            }
        }
    }
}
