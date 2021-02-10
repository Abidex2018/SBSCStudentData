using RestSharp;
using StudentData.IStudent;
using StudentData.Model;
using StudentData.StudentDataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentDataService.StudentDataServices
{
    public class StudentServiceData : IStudentData
    {
        private readonly StudentDbContext _context;

        public StudentServiceData(StudentDbContext context)
        {
            _context = context;
        }


        public bool AddStudent(string name, string familyName, string address, string countryOfOrigin, string emailAddress, int age, bool isApproved, out string message)
        {
            bool result = false;
            message = String.Empty;

            try
            {
                if (String.IsNullOrWhiteSpace(name))
                {
                    message = "Student Name is required.";
                    return result;
                }

                if (name.Length < 5)
                {
                    message = "Student Name must be at least 5 characters.";
                    return result;
                }

                if (String.IsNullOrWhiteSpace(familyName))
                {
                    message = "Student Family Name is required.";
                    return result;
                }

                if (familyName.Length < 5)
                {
                    message = "Student Family Name must be at least 5 characters.";
                    return result;
                }

                if (String.IsNullOrWhiteSpace(address))
                {
                    message = "Student Address is required.";
                    return result;
                }

                if (address.Length < 10)
                {
                    message = "Student Address must be at least 10 characters.";
                    return result;
                }

                if (String.IsNullOrWhiteSpace(emailAddress))
                {
                    message = "Student Email Address Is Required.";
                    return result;
                }

               
                if (String.IsNullOrWhiteSpace(countryOfOrigin))
                {
                    message = "Student Country of origin is required";
                    return result;
                }

                if (!IsCountryOfOriginExists(countryOfOrigin, out string errorMsg))
                {
                    message = "Kindly Input a valid country, " + countryOfOrigin + " is not a country. [" + errorMsg + "]";
                    return result;
                }

                if (age < 18 || age > 25)
                {
                    message = "Student Age must be between 18 and 25.";
                    return result;
                }

                Student student = new Student()
                {
                    Name = name,
                    FamilyName = familyName,
                    Address = address,
                    Age = age,
                    Approved = isApproved,
                    EmailAddress = emailAddress,
                    CountryOfOrigin = countryOfOrigin,
                };

                if (!student.IsValidEmail)
                {
                    message = "Email Address is not valid";
                    return result;
                }

                _context.Students.Add(student);
                _context.SaveChanges();
                result = true;
            }
            catch (Exception error)
            {
                message = error.Message;
                return result;
            }

            return result;
        }
       
        public Student GetStudentByID(int studentId, out string message)
        {
            Student result = null;
            message = String.Empty;

            try
            {
                if (studentId <= 0)
                {
                    message = "Invalid Student Id";
                    return result;
                }

                Student student = _context.Students.FirstOrDefault(x => x.StudentId == studentId);
                if (student == null)
                {
                    message = "Error, No Student with this Id (" + student + ") exists.";
                    return result;
                }

                result = student;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                result = null;
            }

            return result;
        }

        public bool DeleteStudent(int studentId, out string message)
        {
            bool result = false;
            message = String.Empty;

            try
            {
                if (studentId <= 0)
                {
                    message = "Invalid Student ID";
                    return result;
                }

                Student student = _context.Students.FirstOrDefault(x => x.StudentId == studentId);
                if (student == null)
                {
                    message = "Error, No Student with this Id (" + student + ") exists.";
                    return result;
                }

                _context.Students.Remove(student);
                
                result = true;
            }
            catch (Exception error)
            {
                message = error.Message;
                return result;
            }

            return result;
        }

        public bool UpdateStudent(int studentId, Student studentDetails, out string message)
        {
            bool result = false;
            message = String.Empty;

            try
            {
                if (studentDetails == null)
                {
                    message = "Invalid Student Object Passed";
                    return result;
                }

                //Check If Student Object Exists in DB
                Student student = _context.Students.FirstOrDefault(x => x.StudentId == studentId);
                if (student == null)
                {
                    message = "No Student record with the specified student ID exists.";
                    return result;
                }

                if (!String.IsNullOrWhiteSpace(studentDetails.Name))
                {
                    if (studentDetails.Name.Length < 5)
                    {
                        message = "Student Name must be at least 5 characters.";
                        return result;
                    }
                    student.Name = studentDetails.Name;
                }

                if (!String.IsNullOrWhiteSpace(studentDetails.FamilyName))
                {
                    if (studentDetails.FamilyName.Length < 5)
                    {
                        message = "Student Family Name must be at least 5 characters.";
                        return result;
                    }
                    student.FamilyName = studentDetails.FamilyName;
                }

                if (!String.IsNullOrWhiteSpace(studentDetails.EmailAddress))
                {
                    if (!studentDetails.IsValidEmail)
                    {
                        message = "Email Address is not valid";
                        return result;
                    }

                    if (CheckStudentDuplicateEmail(studentDetails.EmailAddress) && studentDetails.EmailAddress != student.EmailAddress)
                    {
                        message = "Apologies, an account with this email already exists.";
                        return result;
                    }
                    student.EmailAddress = studentDetails.EmailAddress;
                }

                if (!String.IsNullOrWhiteSpace(studentDetails.Address))
                {
                    if (studentDetails.Address.Length < 10)
                    {
                        message = "Student Address must be at least 10 characters.";
                        return result;
                    }
                    student.Address = studentDetails.Address;
                }

                if (!String.IsNullOrWhiteSpace(studentDetails.CountryOfOrigin))
                {
                    if (!IsCountryOfOriginExists(studentDetails.CountryOfOrigin, out string errorMsg))
                    {
                        message = "Kindly Input a valid country. " + studentDetails.CountryOfOrigin + " is not a country. [" + errorMsg + "]";
                        return result;
                    }
                    student.CountryOfOrigin = studentDetails.CountryOfOrigin;
                }

                if (studentDetails.Age > 0)
                {
                    if (studentDetails.Age < 18 || studentDetails.Age > 25)
                    {
                        message = "Student Age must be between 18 and 25.";
                        return result;
                    }
                    student.Age = studentDetails.Age;
                }

                _context.Students.Update(student);
                _context.SaveChanges();
                result = true;
            }
            catch (Exception error)
            {
                message = error.Message;
                return result;
            }

            return result;
        }

        private bool IsCountryOfOriginExists(string countryOfOrigin, out string errMsg)
        {
            bool result = false;
            errMsg = String.Empty;

            try
            {
                string _baseUrl = "https://restcountries.eu";

                System.Net.ServicePointManager.SecurityProtocol = (System.Net.SecurityProtocolType)3072;
                System.Net.ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                var client = new RestClient(_baseUrl);
                var enquiryRequest = new RestRequest("rest/v2/name/" + countryOfOrigin + "?fullText=true", Method.GET);
                enquiryRequest.AddHeader("Content-Type", "application/json");
                enquiryRequest.AddHeader("Accept", "application/json");
                IRestResponse<string> response = client.Execute<string>(enquiryRequest);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    result = response.Content != null;
                }
                else
                {
                    result = false;
                }
            }
            catch (Exception error)
            {
                errMsg = error.Message;
            }

            return result;
        }

        private bool CheckStudentDuplicateEmail(string emailAddress)
        {
            return _context.Students.Any(x => x.EmailAddress == emailAddress);
        }

    }
}
