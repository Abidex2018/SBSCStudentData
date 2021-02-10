using StudentData.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentData.IStudent
{
    public interface IStudentData
    {
        bool AddStudent(string name, string familyName, string address, string countryOfOrigin, string emailAddress, int age, bool isApproved, out string message);
        bool UpdateStudent(int studentId, Student studentDetails, out string message);
        Student GetStudentByID(int studentId, out string message);
        bool DeleteStudent(int studentId, out string message);
    }
}
