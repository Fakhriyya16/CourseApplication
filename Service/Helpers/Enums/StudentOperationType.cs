using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Helpers.Enums
{
    public enum StudentOperationType
    {
        UpdateStudentInfo = 1,
        DeleteStudentAccount,
        SearchStudents,
        GetAllGroups,
        GetGroupsByTeacher,
        SearchGroups,
        ChangeGroup,
        Return,
        LogOut,
    }
}
