using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NUnit.Mocks;
using AdvisingSystem.Lib.Model;
using AdvisingSystem.Lib.Entity;
using System.Data.SqlClient;

namespace UnitTest
{
    [TestFixture]
    public class UserModelTest
    {
        UserModel userModel;
        
        [SetUp]
        public void InitializeTest()
        {
            userModel = new UserModel();
            AdvisingSystem.Lib.DBHelper.SetConnection("Data Source=LEVANLONG;Initial Catalog=AdvisingSystem;Integrated Security=True");
        }

        [Test]
        public void GetUser_UserName_Password()
        {
            //Role role = new Role(1, "Sinh viên");
            //User expected = new User("0712255", "Lê Văn Long", "15/09/1989", 0, "lv.long.tn@gmail.com", role);

            User real = userModel.GetUser("0712255", "123456");
            Assert.That(real.DOB, Is.EqualTo(new DateTime(1989, 9, 15)));
            Assert.That(real.Email, Is.EqualTo("lv.long.tn@gmail.com"));
            Assert.That(real.Name, Is.EqualTo("Lê Văn Long"));
            Assert.That(real.Role.ID, Is.EqualTo(1));
            Assert.That(real.Sex, Is.EqualTo(0));
            Assert.That(real.Username, Is.EqualTo("0712255"));
        }

        [Test]
        public void GetUser_NotExist()
        {
            User real1 = userModel.GetUser("000000", "123456");
            Assert.That(real1, Is.EqualTo(null));
        }

        [Test]
        public void GetUser_ExistButWrongPassword()
        {
            User real = userModel.GetUser("0712255", "123457");
            Assert.That(real, Is.EqualTo(null));
        }

        [Test]
        public void GetStudent_NotExist()
        {
            Student real = userModel.GetStudent("000000");
            Assert.That(real, Is.EqualTo(null));
        }

        [Test]
        public void GetStudent_UserName()
        {
            Student real = userModel.GetStudent("0712255");

            Assert.That(real.DOB, Is.EqualTo(new DateTime(1989, 9, 15)));
            Assert.That(real.Email, Is.EqualTo("lv.long.tn@gmail.com"));
            Assert.That(real.Name, Is.EqualTo("Lê Văn Long"));
            Assert.That(real.Role.ID, Is.EqualTo(1));
            Assert.That(real.Sex, Is.EqualTo(0));
            Assert.That(real.Username, Is.EqualTo("0712255"));
            Assert.That(real.CourseYear, Is.EqualTo(2007));
            Assert.That(real.Curriculum.ID, Is.EqualTo(2));
            Assert.That(real.GPA, Is.EqualTo(null));
            Assert.That(real.Major, Is.EqualTo(null));
        }

        [Test]
        public void GetStudent_Exist_NotStudent()
        {
            Student real = userModel.GetStudent("lqvu");
            Assert.That(real, Is.EqualTo(null));
        }

        [Test]
        public void GetUser_UserName()
        {
            User real = userModel.GetUser("0712255");
            Assert.That(real.DOB, Is.EqualTo(new DateTime(1989, 9, 15)));
            Assert.That(real.Email, Is.EqualTo("lv.long.tn@gmail.com"));
            Assert.That(real.Name, Is.EqualTo("Lê Văn Long"));
            Assert.That(real.Role.ID, Is.EqualTo(1));
            Assert.That(real.Sex, Is.EqualTo(0));
            Assert.That(real.Username, Is.EqualTo("0712255"));
        }

        [Test]
        public void GetUser_UserName_NotExist()
        {
            User real = userModel.GetUser("000000");
            Assert.That(real, Is.EqualTo(null));
        }

        [Test]
        public void GetUsers_RoleID_Exist()
        {
            List<User> real = userModel.GetUsers(1);
            foreach (User user in real)
            {
                Assert.That(user.Role.ID, Is.EqualTo(1));
            }
        }

        [Test]
        public void GetUsers_RoleID_NotExist()
        {
            List<User> real = userModel.GetUsers(5);
            Assert.That(real.Count, Is.EqualTo(0));
        }

        [Test]
        public void GetRoles()
        {
            List<Role> expected = new List<Role>();
            expected.Add(new Role(1, "Sinh viên"));
            expected.Add(new Role(2, "Quản lý"));
            expected.Add(new Role(3, "Giáo viên"));

            List<Role> real = userModel.GetRoles();
            //CollectionAssert.AreEqual(expected, real);
            //CollectionAssert.AreEquivalent(expected, real);

            Assert.That(real[0].ID, Is.EqualTo(1));
            Assert.That(real[0].Name, Is.EqualTo("Sinh viên"));

            Assert.That(real[1].ID, Is.EqualTo(2));
            Assert.That(real[1].Name, Is.EqualTo("Quản lý"));

            Assert.That(real[2].ID, Is.EqualTo(3));
            Assert.That(real[2].Name, Is.EqualTo("Giáo viên"));
        }
    }
}
