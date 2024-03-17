using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Helpers.Enums
{
    public enum TeacherOperationType
    {
        DeleteTeacherAccount = 1,
        GetAllTeachers,
        GradeStudent,
        GetAllStudents,
        GetAllGroups,
        SearchStudents,
        SearchGroups,
        Return,
        LogOut,
    }
}
