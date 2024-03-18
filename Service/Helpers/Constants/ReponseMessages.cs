using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Helpers.Constants
{
    public class ResponseMessages
    {
        public const string EmptyInput = "{0} cannot be empty";
        public const string WrongFormat = "Wrong {0} format.";
        public const string SuccessfullMessage = "Successfull operation";
        public const string DoesNotExist = "{0} with given {1} does not exist.";
        public const string GroupDataForDisplay = "Id: {0}, Group Name: {1}, Teacher's Name: {2}, Room: {3}";
        public const string TeacherDataForDisplay = "Id: {0}, Name: {1}, Surname: {2}, Email: {3}";
        public const string StudentDataForDisplay = "Id: {0}, Name: {1}, Surname: {2}, Age: {3}, Group Name: {4}, Email: {5}";
    }
}
