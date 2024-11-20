using CatalogManagementService;
using DataAccess;
using TransferUser = LabyrinthCommon.TransferUser;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using LabyrinthCommon;
using log4net;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;


namespace UserManagementService
{
    public class UserManagementServiceImplementation : IUserManagement
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(UserManagementServiceImplementation));

        public int AddUser(TransferUser user, string password)
        {
            int idUser = 0;

            if (IsEmailRegistered(user.Email))
            {
                throw new FaultException<LabyrinthCommon.LabyrinthException>(new LabyrinthCommon.LabyrinthException("FailDuplicatedEmailMessage"));
            }

            try
            {                
                using (var context = new LabyrinthEntities())
                {
                    var newUser = new User
                    {
                        userName = user.Username,
                        password = password,
                        email = user.Email,
                        idCountry = user.Country
                    };
                    context.User.Add(newUser);
                    context.SaveChanges();

                    idUser = newUser.idCountry;
                }
            }
            catch (DbUpdateException ex)
            {
                _log.Error("AddUserError", ex);
            }
            catch (SqlException ex)
            {
                _log.Error("AddUserError", ex);
            }
            Console.WriteLine(idUser);
            return idUser;
        }

        public int AddVerificationCode(string email, string username)
        {
            int response = 0;
            string verificationCode = GenerateVerificationCode();

            try
            {
                if (IsEmailRegistered(email))
                {
                    throw new FaultException<LabyrinthCommon.LabyrinthException>(new LabyrinthCommon.LabyrinthException("FailDuplicatedEmailMessage"));
                }

                if (IsUsernameRegistered(username))
                {
                    throw new FaultException<LabyrinthCommon.LabyrinthException>(new LabyrinthCommon.LabyrinthException("FailDuplicatedUsernameMessage"));
                }

                using (var context = new LabyrinthEntities())
                {
                    context.VerificationCode.Add(new VerificationCode
                    {
                        email = email,
                        code = verificationCode
                    });
                    response = context.SaveChanges();
                }

                if (SendVerificationCode(email, verificationCode) > 0)
                {
                    response = 1;
                }
            }
            catch (DbUpdateException ex)
            {
                _log.Error("AddVerificationCode", ex);
            }
            catch (SqlException ex)
            {
                _log.Error("AddVerificationCode", ex);
            }

            return response;
        }

        public bool VerificateCode(string email, string code)
        {
            bool response = false;

            try
            {
                using (var context = new LabyrinthEntities())
                {
                    var verification = context.VerificationCode.FirstOrDefault(verificaitonForSearching => verificaitonForSearching.email == email);

                    if (verification != null && verification.code == code)
                    {
                        context.VerificationCode.Remove(verification);
                        context.SaveChanges();
                        response = true;
                    }
                    else
                    {
                        throw new FaultException<LabyrinthCommon.LabyrinthException>(new LabyrinthCommon.LabyrinthException("InvalidVerificationCodeMessage"));
                    }
                }
            }
            catch (DbUpdateException ex)
            {
                _log.Error("VerificateCodeError", ex);
            }
            catch (SqlException ex)
            {
                _log.Error("VerificateCodeError", ex);
            }
            return response;
        }

        private int SendVerificationCode(string email, string code)
        {
            int response = 0;
            MailAddress addressFrom = new MailAddress("labyrinththerealgame@gmail.com", "Labyrinth");
            MailAddress addressTo = new MailAddress(email);

            using (MailMessage message = new MailMessage())
            {
                message.From = addressFrom;
                message.To.Add(addressTo);
                message.Subject = "Code Verification";
                message.Body = "This is your code: " + code;

                using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtpClient.EnableSsl = true;
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = new NetworkCredential("labyrinththerealgame@gmail.com", "foso xzhw wsru gtxf");

                    try
                    {
                        smtpClient.Send(message);
                        response = 1;
                    }
                    catch (SmtpException exception)
                    {
                        _log.Error("SendVerificationCodeError", exception);
                        throw new FaultException<LabyrinthCommon.LabyrinthException>(new LabyrinthCommon.LabyrinthException("SendVerificationCodeError"));
                    }
                }
            }

            return response;
        }


        public int UpdateUser(TransferUser newUser)
        {
            int response = 0;

            using (var context = new LabyrinthEntities())
            {
                var userSearched = context.User.FirstOrDefault(userForSearching => userForSearching.idUser == newUser.IdUser);
                if (userSearched == null)
                {
                    throw new FaultException<LabyrinthCommon.LabyrinthException>(new LabyrinthCommon.LabyrinthException("FailUserNotFoundMessage"));
                }
                else
                {
                    User emailDuplicatedUser = null;

                    if (userSearched.email != newUser.Email)
                    {
                        emailDuplicatedUser = context.User.FirstOrDefault(userForEmailConfirmation => userForEmailConfirmation.email == newUser.Email);
                    }

                    if (emailDuplicatedUser != null)
                    {
                        throw new FaultException<LabyrinthCommon.LabyrinthException>(new LabyrinthCommon.LabyrinthException("FailDuplicatedUserFoundMessage"));
                    }
                    else
                    {
                        userSearched.email = newUser.Email;
                        userSearched.userName = newUser.Username;
                        userSearched.idCountry = newUser.Country;
                        context.Entry(userSearched).Property(u => u.email).IsModified = true;
                        context.Entry(userSearched).Property(u => u.userName).IsModified = true;
                        context.Entry(userSearched).Property(u => u.idCountry).IsModified = true;
                        response = context.SaveChanges();
                    }
                }
            }
            return response;
        }

        public bool IsEmailRegistered(string email)
        {
            using (var context = new LabyrinthEntities())
            {
                return context.User.Any(user => user.email == email);
            }
        }

        public bool IsUsernameRegistered(string username)
        {
            using (var context = new LabyrinthEntities())
            {
                return context.User.Any(user => user.userName == username);
            }
        }

        public int UpdatePassword(string password, string newPassword, string email)
        {
            int response = 0;
           
            using (var context = new LabyrinthEntities())
            {
                var userSearched = context.User.FirstOrDefault(userForSearching => userForSearching.email == email);
                if (password.Equals(userSearched.password))
                {
                    userSearched.password = newPassword;
                    context.Entry(userSearched).Property(u => u.password).IsModified = true;
                    response = context.SaveChanges();
                } 
                else
                {
                    throw new FaultException<LabyrinthCommon.LabyrinthException>(new LabyrinthCommon.LabyrinthException("FailIncorrectPasswordMessage"));
                }
            }
            
            return response;
        }

        public int DeleteAllUsers()
        {
            int usersDeletedCount = 0;

            try
            {
                using (var context = new LabyrinthEntities())
                {                    
                    var usersToDelete = context.User.ToList();
                                        
                    context.User.RemoveRange(usersToDelete);
                    usersDeletedCount = context.SaveChanges();
                }
            }
            catch (Exception exception)
            {
                _log.Error("DeleteAllUsersError", exception);
                throw new FaultException<LabyrinthCommon.LabyrinthException>(new LabyrinthCommon.LabyrinthException("DeleteAllUsersError"));
            }

            return usersDeletedCount;
        }


        public TransferUser VerificateUser(string email, string password)
        {
            var userForVerification = new TransferUser();

            using (var context = new LabyrinthEntities())
            {
                var searchedUser = context.User.FirstOrDefault(userForSearching => userForSearching.email == email);

                if (searchedUser == null)
                {
                    throw new FaultException<LabyrinthCommon.LabyrinthException>(new LabyrinthCommon.LabyrinthException("FailUserNotFoundMessage"));
                }
                else
                {
                    if (searchedUser.password == password)
                    {
                        var catalogManagementServiceImplementation = new CatalogManagementServiceImplementation();
                        userForVerification = new TransferUser
                        {
                            IdUser = searchedUser.idUser,
                            Username = searchedUser.userName,
                            Email = searchedUser.email,
                            ProfilePicture = !string.IsNullOrEmpty(searchedUser.profilePicture)
                                ? GetUserProfilePicture(searchedUser.profilePicture)
                                : new byte[0],
                            TransferCountry = catalogManagementServiceImplementation.GetCountryById(searchedUser.idCountry),
                        };
                    }
                    else
                    {
                        throw new FaultException<LabyrinthCommon.LabyrinthException>(new LabyrinthCommon.LabyrinthException("FailIncorrectPasswordMessage"));
                    }
                }
            }
            return userForVerification;
        }




        public string ChangeUserProfilePicture(int userId, byte[] imagenData)
        {
    
            string imageDirectory = Path.Combine("C:\\labyrinthImages", "profilePictures");

            if (!Directory.Exists(imageDirectory))
            {
                Directory.CreateDirectory(imageDirectory);
            }

            string fileName = $"{userId}.jpg";
            string filePath = Path.Combine(imageDirectory, fileName);

            if (!File.Exists(filePath)) 
            {
                File.Delete(filePath);
            }

            File.WriteAllBytes(filePath, imagenData);

            try
            {
                using (var context = new LabyrinthEntities())
                {
                    var userSearched = context.User.FirstOrDefault(userForSearching => userForSearching.idUser == userId);
                    if (userSearched != null)
                    {
                        userSearched.profilePicture = filePath;
                        context.Entry(userSearched).Property(u => u.profilePicture).IsModified = true;
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception exception)
            {
                _log.Error("ChangeUserProfilePictureError", exception);
                throw new FaultException<LabyrinthCommon.LabyrinthException>(new LabyrinthCommon.LabyrinthException("ChangeUserProfilePictureError"));
            }
            return filePath;
        }

        public byte[] GetUserProfilePicture(string path)
        {
            byte[] response = new byte[0];

            if (File.Exists(path))
            {
                response = File.ReadAllBytes(path);
            } 
            else
            {
                throw new FaultException<LabyrinthCommon.LabyrinthException>(new LabyrinthCommon.LabyrinthException("ProfilePictureNotFoundMessage"));
            }
            return response;
        }
          
        private string GenerateVerificationCode()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            StringBuilder result = new StringBuilder(12);
            Random random = new Random();

            for (int i = 0; i < 12; i++)
            {
                result.Append(chars[random.Next(chars.Length)]);
            }

            return result.ToString();
        }

    }
}
