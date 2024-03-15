using FinalProject.Controllers;
using Service.Helpers.Constants;
using Service.Helpers.Enums;
using Service.Helpers.Extensions;

StudentController studentController = new StudentController();
GroupController groupController = new GroupController();

void AppMenu()
{
    ConsoleColor.Magenta.ConsoleMessage("Choose one operation:\n 1. Create a group\n 2. Create a student\n " +
        "3. Update student info\n 4. Update group data\n 5. Delete student\n 6. Delete group\n " +
        "7. Get Student by id\n 8. Get Students by age\n 9. Get all students\n 10. Get Students by group id\n " +
        "11. Search Students\n 12. Get Group by id\n 13. Get All Groups\n 14. Get Groups by teacher\n " +
        "15. Get Groups by room\n 16. Get Group by name\n 17. Search Group\n 18. Return to menu");
}
while (true)
{
    AppMenu();
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
            break;
        case (int)OperationType.CreateStudent:
            studentController.CreateStudent();
            break;
        case (int)OperationType.UpdateStudent:
            studentController.UpdateStudent();
            break;
    }


}
