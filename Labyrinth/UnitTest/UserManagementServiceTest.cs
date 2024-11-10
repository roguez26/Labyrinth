using DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagementService;
using LabyrinthCommon;
using TransferUser = LabyrinthCommon.TransferUser;


namespace UnitTest
{
    [TestClass]
    public class UserManagementServiceTest
    {
        private UserManagementServiceImplementation _userManagementServiceImplementation;
        private int _testCountryId = 0;

        [TestInitialize]
        public void SetUp()
        {
            _userManagementServiceImplementation = new UserManagementServiceImplementation();

            using (var context = new DataAccess.LabyrinthEntities())
            {
                context.VerificationCode.RemoveRange(context.VerificationCode.ToList());

                context.User.RemoveRange(context.User.ToList());

                context.Country.RemoveRange(context.Country.ToList());

                context.SaveChanges();

                var testCountry = new DataAccess.Country
                {
                    name = "Mexico"
                };
                context.Country.Add(testCountry);
                context.SaveChanges();

                _testCountryId = testCountry.idCountry;
            }

        }

        [TestCleanup]
        public void CleanUp()
        {
            using (var context = new DataAccess.LabyrinthEntities())
            {
                context.VerificationCode.RemoveRange(context.VerificationCode.ToList());

                context.User.RemoveRange(context.User.ToList());

                context.Country.RemoveRange(context.Country.ToList());

                context.SaveChanges();
            }
        }

        [TestMethod]
        public void TestAddUserSuccessful()
        {
            var newUser = new TransferUser
            {
                Username = "TestUser",
                //Password = "TestPassword",
                Email = "TestEmail@example.com",
                Country = _testCountryId
            };

            Assert.IsTrue(_userManagementServiceImplementation.AddUser(newUser, "TestPassword") > 0);
        }

        [TestMethod]
        [ExpectedException(typeof(DbUpdateException))]
        public void TestAddUserFailureByInvalidCountry()
        {
            var newUser = new LabyrinthCommon.TransferUser
            {
                Username = "TestUser",
               // Password = "TestPassword",
                Email = "TestEmail@example.com",
                Country = 0
            };

            _userManagementServiceImplementation.AddUser(newUser, "TestPassword");
        }

        [TestMethod]
        public void TestAddVerificationCodeSuccessful()
        {
            string email = "axlvaldez74@gmail.com";

            var newUser = new TransferUser
            {
                Username = "TestUser",
              //  Password = "TestPassword",
                Email = email,
                Country = _testCountryId
            };
            _userManagementServiceImplementation.AddUser(newUser, "TestPassword");
            Assert.IsTrue(_userManagementServiceImplementation.AddVerificationCode(email) == 1);            
        }

        [TestMethod]
        public void TestAddVerificationCodeFailure()
        {
            string email = "axlvaldez74@gmail.com";

            var newUser = new TransferUser
            {
                Username = "TestUser",
               // Password = "TestPassword",
                Email = email,
                Country = _testCountryId
            };
            email = "TestExample@example.com";
            _userManagementServiceImplementation.AddUser(newUser, "TestPassword");
            Assert.IsTrue(_userManagementServiceImplementation.AddVerificationCode(email) != 1);
        }

        [TestMethod]
        public void TestVerificationCodeFailureByInvalidCode()
        {
            string testEmail = "axlvaldez74@gmail.com";
            string code = "123456";
            string invalidCode = "abcdefg";

            var newUser = new TransferUser
            {
                Username = "TestUser",
               // Password = "TestPassword",
                Email = testEmail,
                Country = _testCountryId
            };
            using (var context = new DataAccess.LabyrinthEntities())
            {
                context.VerificationCode.Add(new VerificationCode
                {
                    email = testEmail,
                    code = code
                });
                context.SaveChanges();
                Assert.IsFalse(_userManagementServiceImplementation.VerificateCode(testEmail, invalidCode));
            }
        }

        [TestMethod]
        public void TestUpdateUserSuccessful()
        {
            int testUserId = 0;
            using (var context = new DataAccess.LabyrinthEntities())
            {
                var testUser = new DataAccess.User
                {
                    userName = "TestUser",
                    password = "TestPassword",
                    email = "axlvaldez74@gmail.com",
                    idCountry = _testCountryId

                };
                context.User.Add(testUser);
                context.SaveChanges();
                testUserId = testUser.idUser;

                var userToUpdate = new TransferUser
                {
                    IdUser = testUserId,
                    Username = "UpdatedUser",
                    Email = "updated@example.com",
                  //  Password = "newpassword",
                    Country = _testCountryId
                };

                Assert.AreEqual(1, _userManagementServiceImplementation.UpdateUser(userToUpdate));
            }
        }

        [TestMethod]
        public void TestUpdateUserFailureByIDNotFound()
        {
            int testUserId = 0;
            var userToUpdate = new TransferUser
            {
                IdUser = testUserId,
                Username = "UpdatedUser",
                Email = "updated@example.com",
               // Password = "newpassword",
                Country = _testCountryId
            };

            Assert.AreEqual(-1, _userManagementServiceImplementation.UpdateUser(userToUpdate));
        }

        [TestMethod]
        public void TestUserVerification()
        {
            string testEmail = "TestEmail@example.com";
            using (var context = new DataAccess.LabyrinthEntities())
            {
                context.User.Add(new User
                {
                    userName = "TestUsername",
                    email = testEmail,
                    password = "12345",
                    idCountry = _testCountryId
                });
                context.SaveChanges();
            }

            var testUser = new TransferUser
            {
                Email = testEmail,
               // Password = "12345"
            };
            
            var verificatedUser = _userManagementServiceImplementation.UserVerification(testUser.Email, "12345");
            Assert.IsTrue(verificatedUser.IdUser > 0);            
        }

        [TestMethod]
        public void TestUserVerificationFailureByNotUserNotFound()
        {
            string testEmail = "TestEmail@example.com";
            var testUser = new TransferUser
            {
                Email = testEmail,
               // Password = "12345"
            };

            var verificatedUser = _userManagementServiceImplementation.UserVerification(testUser.Email,  "12345");
            Assert.IsTrue(verificatedUser.IdUser == 0);
        }

        [TestMethod]
        public void TestIsEmailRegisteredSuccessful()
        {
            string testEmail = "TestEmail@example.com";
            using (var context = new DataAccess.LabyrinthEntities())
            {
                context.User.Add(new User
                {
                    userName = "TestUsername",
                    email = testEmail,
                    password = "12345",
                    idCountry = _testCountryId
                });
                context.SaveChanges();
            }
            Assert.IsTrue(_userManagementServiceImplementation.IsEmailRegistered(testEmail));
        }

        [TestMethod]
        public void TestIsEmailRegisteredFailureByEmailNotFound()
        {
            string testEmail = "TestEmail@example.com";
            Assert.IsFalse(_userManagementServiceImplementation.IsEmailRegistered(testEmail));
        }

    }
}
