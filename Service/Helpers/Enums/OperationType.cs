using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Helpers.Enums
{
    public enum OperationType
    {
        CreateGroup = 1,
        CreateStudent,
        UpdateStudent,
        UpdateGroupData,
        DeleteStudent,
        DeleteGroup,
        GetStudentById,
        GetStudentsByAge,
        GetAllStudents,
        GetStudentsByGroupId,
        SearchStudents,
        GetGroupById,
        GetAllGroups,
        GetGroupsByTeacher,
        GetGroupsByRoom,
        GetGroupByName,
        SearchGroup,
        ReturnToMenu,
    }
}
