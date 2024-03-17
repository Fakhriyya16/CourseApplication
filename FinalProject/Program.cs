using FinalProject.Controllers;
using Service.Helpers.Constants;
using Service.Helpers.Enums;
using Service.Helpers.Exceptions;
using Service.Helpers.Extensions;

StudentController studentController = new StudentController();
GroupController groupController = new GroupController();
TeacherController teacherController = new TeacherController();
void IntroMenu()
{
    ConsoleColor.Magenta.ConsoleMessage("Welcome to Home Page. Press\n 1. Register as student\n 2. Register as teacher\n 3. Login as student\n 4. Login as teacher\n 5. Login as admin ");
}
void AppMenuForAdmin()
{
    ConsoleColor.Magenta.ConsoleMessage("Choose one operation:\n 1. Create a group\n 2. Update student info\n 3. Update group data\n 4. Delete student\n 5. Delete group\n 6. Get Student by id\n 7. Get Students by age\n 8. Get all students\n 9. Get Students by group id\n 10. Search Students\n 11. Get Group by id\n 12. Get All Groups\n 13. Get Groups by teacher\n 14. Get Groups by room\n 15. Get Group by name\n 16. Search Group\n 17. Change your group\n 18. Return to menu\n 19. Log Out");
}
void AppMenuForTeacher()
{
    ConsoleColor.Magenta.ConsoleMessage("Choose one operation:\n 1. Delete your account\n 2. Show all teachers\n 3. Grade student\n 4. Show all students\n 5. Show all groups\n 6. Search students\n 7. Search groups\n 8. Return to menu\n 9. Log out");
}
void AppMenuForStudent()
{
    ConsoleColor.Magenta.ConsoleMessage("Choose one operation:\n 1. Update student info\n 2. Delete account\n 3. Search students\n 4. Get All Groups\n 5. Get Groups by teacher\n 6. Search Group\n 7. Change your group\n 8. Return to menu\n 9.Log out");
}
while (true)
{
    IntroMenu: IntroMenu(); // HomePage
    IntroOperator: string introOperationStr = Console.ReadLine();
    int introOperationNum;
    bool isCorrectFormatForIntro = int.TryParse(introOperationStr, out introOperationNum);

    if (!isCorrectFormatForIntro)
    {
        ConsoleColor.Red.ConsoleMessage(ResponseMessages.WrongFormat + " Add input again:");
        goto IntroOperator;
    }
    switch (introOperationNum)
    {
        case (int)IntroPageOperations.RegisterAsStudent:
            try
            {
                MenuForStudents: studentController.RegisterStudent();
                StudentMenu: AppMenuForStudent();
                string studentOperationStr = Console.ReadLine();
                int studentOperationNum;
                isCorrectFormatForIntro = int.TryParse(studentOperationStr, out studentOperationNum);

                if (!isCorrectFormatForIntro)
                {
                    ConsoleColor.Red.ConsoleMessage(string.Format(ResponseMessages.WrongFormat, "operator"));
                    goto MenuForStudents;
                }
                switch (studentOperationNum)
                {
                    case (int)StudentOperationType.UpdateStudentInfo:
                        studentController.UpdateStudent();
                        goto StudentMenu;
                    case (int)StudentOperationType.DeleteStudentAccount:
                        studentController.DeleteStudent();
                        break;
                    case (int)StudentOperationType.SearchStudents:
                        studentController.SearchStudent();
                        goto StudentMenu;
                    case (int)StudentOperationType.GetAllGroups:
                        groupController.GetAll();
                        goto StudentMenu;
                    case (int)StudentOperationType.GetGroupsByTeacher:
                        groupController.GetGroupsByTeacher();
                        goto StudentMenu;
                    case (int)StudentOperationType.SearchGroups:
                        groupController.SearchGroup();
                        goto StudentMenu;
                    case (int)StudentOperationType.ChangeGroup:
                        studentController.ChangeGroup();
                        goto StudentMenu;
                    case (int)StudentOperationType.Return:
                        goto StudentMenu;
                    case (int)StudentOperationType.LogOut:
                        break;
                }
                break;
            }
            catch (RegistrationFailedException ex)
            {
                ConsoleColor.Red.ConsoleMessage(ex.Message);
                goto IntroMenu;
            }
        case (int)IntroPageOperations.RegisterAsTeacher:
            teacherController.RegisterTeacher();
            MenuForTeacher: AppMenuForTeacher();

            string teacherOperationStr = Console.ReadLine();
            int teacherOperationNum;
            isCorrectFormatForIntro = int.TryParse(teacherOperationStr, out teacherOperationNum);

            if (!isCorrectFormatForIntro)
            {
                ConsoleColor.Red.ConsoleMessage(string.Format(ResponseMessages.WrongFormat, "operation"));
                goto MenuForTeacher;
            }
            switch (teacherOperationNum)
            {
                case (int)TeacherOperationType.DeleteTeacherAccount:
                    teacherController.DeleteTeacher();
                    break;
                case (int)TeacherOperationType.GetAllTeachers:
                    teacherController.GetAll();
                    break;
                case (int)TeacherOperationType.GradeStudent:
                    teacherController.GradeStudent();
                    break;
                case (int)TeacherOperationType.GetAllStudents:
                    teacherController.GetAllStudents();
                    break;
                case (int)TeacherOperationType.GetAllGroups:
                    teacherController.GetAllGroups();
                    break;
                case (int)TeacherOperationType.SearchStudents:
                    teacherController.SearchStudents();
                    break;
                case (int)TeacherOperationType.SearchGroups:
                    teacherController.SearchGroups();
                    break;
                case (int)TeacherOperationType.Return:
                    break;
                case (int)TeacherOperationType.LogOut:
                    IntroMenu();
                    break;
            }
            break;
        case (int)IntroPageOperations.LoginAsStudent:
            studentController.Login();
            break;
        case (int)IntroPageOperations.LoginAsTeacher:
            teacherController.Login();
            break;
        case (int)IntroPageOperations.AdminLogin:
         Username: ConsoleColor.Cyan.ConsoleMessage("Enter username:");
            string username = Console.ReadLine();
         Password: ConsoleColor.Cyan.ConsoleMessage("Enter password:");
            string password = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(username))
            {
                ConsoleColor.Red.ConsoleMessage(string.Format(ResponseMessages.EmptyInput, "Username"));
                goto Username;
            }
            else if (string.IsNullOrWhiteSpace(password))
            {
                ConsoleColor.Red.ConsoleMessage(string.Format(ResponseMessages.EmptyInput, "Password"));
                goto Password;
            }
            else if (username != "admin" || password != "admin")
            {
                ConsoleColor.Red.ConsoleMessage("Username or password is wrong, please try again.");
                goto Username;
            }
            else
            {
            AdminMenu: AppMenuForAdmin();
            Operator: string operationStr = Console.ReadLine();
                int operationNum;
                bool isCorrectFormat = int.TryParse(operationStr, out operationNum);

                if (!isCorrectFormat)
                {
                    ConsoleColor.Red.ConsoleMessage(ResponseMessages.WrongFormat + " Add input again:");
                    goto Operator;
                }
                switch (operationNum)
                {
                    case (int)OperationType.CreateGroup:
                        groupController.CreateGroup();
                        goto AdminMenu;
                    case (int)OperationType.UpdateStudent:
                        studentController.UpdateStudent();
                        goto AdminMenu;
                    case (int)OperationType.UpdateGroupData:
                        groupController.UpdateGroupData();
                        goto AdminMenu;
                    case (int)OperationType.DeleteStudent:
                        studentController.DeleteStudent();
                        goto AdminMenu;
                    case (int)OperationType.DeleteGroup:
                        groupController.DeleteGroup();
                        goto AdminMenu;
                    case (int)OperationType.GetStudentById:
                        studentController.GetStudentById();
                        goto AdminMenu;
                    case (int)OperationType.GetStudentsByAge:
                        studentController.GetStudentsByAge();
                        goto AdminMenu;
                    case (int)OperationType.GetAllStudents:
                        studentController.GetAll();
                        goto AdminMenu;
                    case (int)OperationType.GetStudentsByGroupId:
                        studentController.GetStudentsByGroupId();
                        goto AdminMenu;
                    case (int)OperationType.SearchStudents:
                        studentController.SearchStudent();
                        goto AdminMenu;
                    case (int)OperationType.GetGroupById:
                        groupController.GetGroupById();
                        goto AdminMenu;
                    case (int)OperationType.GetAllGroups:
                        groupController.GetAll();
                        goto AdminMenu;
                    case (int)OperationType.GetGroupsByTeacher:
                        groupController.GetGroupsByTeacher();
                        goto AdminMenu;
                    case (int)OperationType.GetGroupsByRoom:
                        groupController.GetGroupsByRoom();
                        goto AdminMenu;
                    case (int)OperationType.GetGroupByName:
                        groupController.GetGroupsByName();
                        goto AdminMenu;
                    case (int)OperationType.SearchGroup:
                        groupController.SearchGroup();
                        goto AdminMenu;
                    case (int)OperationType.ChangeGroup:
                        studentController.ChangeGroup();
                        goto AdminMenu;
                    case (int)OperationType.ReturnToMenu:
                        goto AdminMenu;
                    case (int)OperationType.LogOut:
                        break;
                }
            }
        break;
    }
}
