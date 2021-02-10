using Microsoft.EntityFrameworkCore;
using StudentData.Model;
using StudentData.StudentDataContext;
using StudentDataService.StudentDataServices;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace UnitTestForStudentData
{
    public class StudentDataUnitTest
    {

        [Fact]
        public void NameNotLessThan_5_Characters()
        {
            var options = new DbContextOptionsBuilder<StudentDbContext>()
                .UseInMemoryDatabase(databaseName: "StudentDB")
                .Options;

            var context = new StudentDbContext(options);
            var studentService = new StudentServiceData(context);

            var studentTest = new Student()
            {
                Name = "Tola",
                Address = "4 Simbi",
                Age = 20,
                Approved = true,
                CountryOfOrigin = "Nigeria",
                EmailAddress = "tolakoye@.com",
                FamilyName = "Adesanya",
            };

            Assert.False(studentService.AddStudent(studentTest.Name, studentTest.FamilyName, studentTest.Address, studentTest.CountryOfOrigin, studentTest.EmailAddress, studentTest.Age, studentTest.Approved, out string msg));
        }

        [Fact]
        public void FamilyNameNotLessThan_5_Characters()
        {
            var options = new DbContextOptionsBuilder<StudentDbContext>()
                .UseInMemoryDatabase(databaseName: "StudentDB")
                .Options;

            var context = new StudentDbContext(options);
            var studentService = new StudentServiceData(context);

            var studentTest = new Student()
            {
                Name = "Tola",
                Address = "4 Simbi",
                Age = 20,
                Approved = true,
                CountryOfOrigin = "Togo",
                EmailAddress = "tolakoye@.com",
                FamilyName = "Oyemo",
            };

            Assert.False(studentService.AddStudent(studentTest.Name, studentTest.FamilyName, studentTest.Address, studentTest.CountryOfOrigin, studentTest.EmailAddress, studentTest.Age, studentTest.Approved, out string msg));
        }

        [Fact]
        public void AddressNotLessThan_10_Characters()
        {
            var options = new DbContextOptionsBuilder<StudentDbContext>()
                 .UseInMemoryDatabase(databaseName: "StudentDB")
                 .Options;

            var context = new StudentDbContext(options);
            var studentService = new StudentServiceData(context);

            var studentTest = new Student()
            {
                Name = "Tola",
                Address = "4 Simbi Shogunle Oshodi",
                Age = 25,
                Approved = true,
                CountryOfOrigin = "Togo",
                EmailAddress = "tolakoye@.com",
                FamilyName = "Oyemo",
            };

            Assert.False(studentService.AddStudent(studentTest.Name, studentTest.FamilyName, studentTest.Address, studentTest.CountryOfOrigin, studentTest.EmailAddress, studentTest.Age, studentTest.Approved, out string msg));
        }

        [Fact]
        public void IsCountryOfOriginValid()
        {
            var options = new DbContextOptionsBuilder<StudentDbContext>()
                .UseInMemoryDatabase(databaseName: "StudentDB")
                .Options;

            var context = new StudentDbContext(options);
            var studentService = new StudentServiceData(context);

            var studentTest = new Student()
            {
                Name = "Tola",
                Address = "4 Simbi",
                Age = 20,
                Approved = true,
                CountryOfOrigin = "miami",
                EmailAddress = "tolakoye@.com",
                FamilyName = "Oyemo",
            };

            Assert.False(studentService.AddStudent(studentTest.Name, studentTest.FamilyName, studentTest.Address, studentTest.CountryOfOrigin, studentTest.EmailAddress, studentTest.Age, studentTest.Approved, out string msg));
        }

        [Fact]
        public void AgeBetween_18_And_25()
        {
            var options = new DbContextOptionsBuilder<StudentDbContext>()
                .UseInMemoryDatabase(databaseName: "StudentDB")
                .Options;

            var context = new StudentDbContext(options);
            var studentService = new StudentServiceData(context);

            var studentTest = new Student()
            {
                Name = "Tola",
                Address = "4 Simbi",
                Age = 30,
                Approved = true,
                CountryOfOrigin = "Togo",
                EmailAddress = "tolakoye@.com",
                FamilyName = "Oyemo",
            };

            Assert.False(studentService.AddStudent(studentTest.Name, studentTest.FamilyName, studentTest.Address, studentTest.CountryOfOrigin, studentTest.EmailAddress, studentTest.Age, studentTest.Approved, out string msg));
        }

    }
}
