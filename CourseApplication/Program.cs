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
    ConsoleColor.Yellow.ConsoleMessage("Press\n 1. Register as student\n 2. Register as teacher\n 3. Login as student\n 4. Login as teacher\n 5. Login as admin\n 6. Exit the program");
}
void AppMenuForAdmin()
{
    ConsoleColor.Magenta.ConsoleMessage("Choose one operation:\n 1. Create a group\n 2. Update student info\n 3. Update group data\n 4. Delete student\n 5. Delete group\n 6. Get Student by id\n 7. Get Students by age\n 8. Get all students\n 9. Get Students by group id\n 10. Search Students\n 11. Get Group by id\n 12. Get All Groups\n 13. Get Groups by teacher\n 14. Get Groups by room\n 15. Get Group by name\n 16. Search Group\n 17. Log Out");
}
void AppMenuForTeacher()
{
    ConsoleColor.Magenta.ConsoleMessage("Choose one operation:\n 1. Delete your account\n 2. Show all teachers\n 3. Grade student\n 4. Show all students\n 5. Show all groups\n 6. Search students\n 7. Search groups\n 8. Log out");
}
void AppMenuForStudent()
{
    ConsoleColor.Magenta.ConsoleMessage("Choose one operation:\n 1. Update student info\n 2. Delete account\n 3. Search students\n 4. Get All Groups\n 5. Get Groups by teacher\n 6. Search Group\n 7. Change your group\n 8. Show student grade\n 9. Log out");
}
ConsoleColor.Cyan.ConsoleMessage(@"
 __    __     _                            _         
/ / /\ \ \___| | ___ ___  _ __ ___   ___  | |_ ___   
\ \/  \/ / _ \ |/ __/ _ \| '_ ` _ \ / _ \ | __/ _ \  
 \  /\  /  __/ | (_| (_) | | | | | |  __/ | || (_) | 
  \/  \/ \___|_|\___\___/|_| |_| |_|\___|  \__\___/  
                                                     
                                ___                  
  /\  /\___  _ __ ___   ___    / _ \__ _  __ _  ___  
 / /_/ / _ \| '_ ` _ \ / _ \  / /_)/ _` |/ _` |/ _ \ 
/ __  / (_) | | | | | |  __/ / ___/ (_| | (_| |  __/ 
\/ /_/ \___/|_| |_| |_|\___| \/    \__,_|\__, |\___| 
                                         |___/       
        ");
IntroMenu: IntroMenu();
while (true)
{
    bool exit = false;
    IntroOperator: string introOperationStr = Console.ReadLine();
    int introOperationNum;
    bool isCorrectFormatForIntro = int.TryParse(introOperationStr, out introOperationNum);

    if (!isCorrectFormatForIntro)
    {
        ConsoleColor.Red.ConsoleMessage(string.Format(ResponseMessages.WrongFormat, "id") + "Add input again:");
        goto IntroOperator;
    }
    if(introOperationNum <= 6)
    {
        switch (introOperationNum)
        {
            case (int)IntroPageOperations.RegisterAsStudent:
                try
                {
                studentController.RegisterStudent();
                StudentMenu: AppMenuForStudent();
                    string studentOperationStr = Console.ReadLine();
                    int studentOperationNum;
                    isCorrectFormatForIntro = int.TryParse(studentOperationStr, out studentOperationNum);

                    if (!isCorrectFormatForIntro)
                    {
                        ConsoleColor.Red.ConsoleMessage(string.Format(ResponseMessages.WrongFormat, "operator"));
                        goto StudentMenu;
                    }
                    switch (studentOperationNum)
                    {
                        case (int)StudentOperationType.UpdateStudentInfo:
                            studentController.UpdateStudent();
                            goto StudentMenu;
                        case (int)StudentOperationType.DeleteStudentAccount:
                            studentController.DeleteStudent();
                            goto IntroMenu;
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
                        case (int)StudentOperationType.ShowGrade:
                            studentController.ShowGrade();
                            goto StudentMenu;
                        case (int)StudentOperationType.LogOut:
                            IntroMenu();
                            break;
                    }
                    break;
                }
                catch (AgeRequirementException ex)
                {
                    ConsoleColor.Red.ConsoleMessage(ex.Message);
                    goto IntroMenu;
                }
                catch(NoGroupsAvailableException ex)
                {
                    ConsoleColor.Red.ConsoleMessage(ex.Message);
                    goto IntroMenu;
                }
                catch (Exception ex)
                {
                    ConsoleColor.Red.ConsoleMessage(ex.Message);
                    goto IntroMenu;
                }
            case (int)IntroPageOperations.RegisterAsTeacher:
                try
                {
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
                            goto IntroMenu;
                        case (int)TeacherOperationType.GetAllTeachers:
                            teacherController.GetAll();
                            goto MenuForTeacher;
                        case (int)TeacherOperationType.GradeStudent:
                            teacherController.GradeStudent();
                            goto MenuForTeacher;
                        case (int)TeacherOperationType.GetAllStudents:
                            teacherController.GetAllStudents();
                            goto MenuForTeacher;
                        case (int)TeacherOperationType.GetAllGroups:
                            teacherController.GetAllGroups();
                            goto MenuForTeacher;
                        case (int)TeacherOperationType.SearchStudents:
                            teacherController.SearchStudents();
                            goto MenuForTeacher;
                        case (int)TeacherOperationType.SearchGroups:
                            teacherController.SearchGroups();
                            goto MenuForTeacher;
                        case (int)TeacherOperationType.LogOut:
                            goto IntroMenu;
                    }
                    break;
                }
                catch (Exception ex)
                {
                    ConsoleColor.Red.ConsoleMessage(ex.Message);
                    goto IntroMenu;
                }
            case (int)IntroPageOperations.LoginAsStudent:
                try
                {
                    studentController.Login();
                MenuForStudent: AppMenuForStudent();
                    string studentOperationString = Console.ReadLine();
                    int studentOperationNumber;
                    isCorrectFormatForIntro = int.TryParse(studentOperationString, out studentOperationNumber);

                    if (!isCorrectFormatForIntro)
                    {
                        ConsoleColor.Red.ConsoleMessage(string.Format(ResponseMessages.WrongFormat, "operator"));
                        goto MenuForStudent;
                    }
                    switch (studentOperationNumber)
                    {
                        case (int)StudentOperationType.UpdateStudentInfo:
                            studentController.UpdateStudent();
                            goto MenuForStudent;
                        case (int)StudentOperationType.DeleteStudentAccount:
                            studentController.DeleteStudent();
                            goto IntroMenu;
                        case (int)StudentOperationType.SearchStudents:
                            studentController.SearchStudent();
                            goto MenuForStudent;
                        case (int)StudentOperationType.GetAllGroups:
                            groupController.GetAll();
                            goto MenuForStudent;
                        case (int)StudentOperationType.GetGroupsByTeacher:
                            groupController.GetGroupsByTeacher();
                            goto MenuForStudent;
                        case (int)StudentOperationType.SearchGroups:
                            groupController.SearchGroup();
                            goto MenuForStudent;
                        case (int)StudentOperationType.ChangeGroup:
                            studentController.ChangeGroup();
                            goto MenuForStudent;
                        case (int)StudentOperationType.ShowGrade:
                            studentController.ShowGrade();
                            goto MenuForStudent;
                        case (int)StudentOperationType.LogOut:
                            goto IntroMenu;
                    }
                    break;
                }
                catch (Exception ex)
                {
                    ConsoleColor.Red.ConsoleMessage(ex.Message);
                    goto IntroMenu;
                }
            case (int)IntroPageOperations.LoginAsTeacher:
                try
                {
                    teacherController.Login();
                TeacherMenu: AppMenuForTeacher();

                    string teacherOperationString = Console.ReadLine();
                    int teacherOperationNumber;
                    isCorrectFormatForIntro = int.TryParse(teacherOperationString, out teacherOperationNumber);

                    if (!isCorrectFormatForIntro)
                    {
                        ConsoleColor.Red.ConsoleMessage(string.Format(ResponseMessages.WrongFormat, "operation"));
                        goto TeacherMenu;
                    }
                    switch (teacherOperationNumber)
                    {
                        case (int)TeacherOperationType.DeleteTeacherAccount:
                            teacherController.DeleteTeacher();
                            goto IntroMenu;
                        case (int)TeacherOperationType.GetAllTeachers:
                            teacherController.GetAll();
                            goto TeacherMenu;
                        case (int)TeacherOperationType.GradeStudent:
                            teacherController.GradeStudent();
                            goto TeacherMenu;
                        case (int)TeacherOperationType.GetAllStudents:
                            teacherController.GetAllStudents();
                            goto TeacherMenu;
                        case (int)TeacherOperationType.GetAllGroups:
                            teacherController.GetAllGroups();
                            goto TeacherMenu;
                        case (int)TeacherOperationType.SearchStudents:
                            teacherController.SearchStudents();
                            goto TeacherMenu;
                        case (int)TeacherOperationType.SearchGroups:
                            teacherController.SearchGroups();
                            goto TeacherMenu;
                        case (int)TeacherOperationType.LogOut:
                            goto IntroMenu;
                    }
                    break;
                }
                catch (Exception ex)
                {
                    ConsoleColor.Red.ConsoleMessage(ex.Message);
                    goto IntroMenu;
                }
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
                else if (username.Trim().ToLower() != "admin" || password != "admin")
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
                        case (int)OperationType.LogOut:
                            IntroMenu();
                            break;
                    }
                }
                break;
            case (int)IntroPageOperations.Exit:
                exit = true;
                goto ExitProgram;
        }
    }
    else
    {
        ConsoleColor.Red.ConsoleMessage(string.Format(ResponseMessages.DoesNotExist, "Operation", "operation number"));
    }
    ExitProgram: if (exit) break;
}
