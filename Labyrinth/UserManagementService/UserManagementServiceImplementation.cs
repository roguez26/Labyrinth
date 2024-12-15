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
using System.Data.Entity.Core;


namespace UserManagementService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Reentrant)]
    public class UserManagementServiceImplementation : IUserManagement
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(UserManagementServiceImplementation));
        private const int TOP_MAX = 10;

        public int AddUser(TransferUser user, string password)
        {
            int idUser = 0;

            if (user == null || string.IsNullOrEmpty(password))
            {
                throw new FaultException<LabyrinthCommon.LabyrinthException>(new LabyrinthCommon.LabyrinthException("FailUserRegistrationError"));
            }
            if (IsEmailRegistered(user.Email) )
            {
                throw new FaultException<LabyrinthCommon.LabyrinthException>(new LabyrinthCommon.LabyrinthException("FailDuplicatedEmailMessage"));
            }
            if (IsUsernameRegistered(user.Username))
            {
                throw new FaultException<LabyrinthCommon.LabyrinthException>(new LabyrinthCommon.LabyrinthException("FailDuplicatedUsernameMessage"));
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
                        countryCode = user.CountryCode
                    };
                    context.User.Add(newUser);
                    idUser = context.SaveChanges();
                }
            }
            catch (SqlException ex)
            {
                LogAndWrapException("AddUserError", ex, "FailUserRegistrationError");
            }
            catch (EntityException ex)
            {
                LogAndWrapException("AddUserError", ex, "FailUserRegistrationError");
            }
            return idUser;
        }

        private void LogAndWrapException(string reason, Exception exception, string errorCode)
        {
            _log.Error(reason, exception);
            throw new FaultException<LabyrinthCommon.LabyrinthException>(
                new LabyrinthCommon.LabyrinthException(errorCode)
            );
        }

        public int AddVerificationCode(string email, string username)
        {
            int response = 0;
            string verificationCode = GenerateVerificationCode();

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(username))
            {
                throw new FaultException<LabyrinthCommon.LabyrinthException>(new LabyrinthCommon.LabyrinthException("FailUserRegistrationError"));
            }
            if (IsEmailRegistered(email))
            {
                throw new FaultException<LabyrinthCommon.LabyrinthException>(new LabyrinthCommon.LabyrinthException("FailDuplicatedEmailMessage"));
            }

            if (IsUsernameRegistered(username))
            {
                throw new FaultException<LabyrinthCommon.LabyrinthException>(new LabyrinthCommon.LabyrinthException("FailDuplicatedUsernameMessage"));
            }

            try
            {
                using (var context = new LabyrinthEntities())
                {
                    context.VerificationCode.Add(new VerificationCode
                    {
                        email = email,
                        code = verificationCode
                    });
                    response = context.SaveChanges();
                }
                response = SendVerificationCode(email, verificationCode);
            }
            catch (SqlException ex)
            {
                LogAndWrapException("AddVerificationCode", ex, "FailUserRegistrationError");
            }
            catch (EntityException ex)
            {
                LogAndWrapException("AddVerificationCode", ex, "FailUserRegistrationError");
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
                    catch (SmtpException ex)
                    {
                        LogAndWrapException("SendVerificationCodeError", ex, "FailUserRegistrationError");
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
                        userSearched.countryCode = newUser.CountryCode;
                        context.Entry(userSearched).Property(u => u.email).IsModified = true;
                        context.Entry(userSearched).Property(u => u.userName).IsModified = true;
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

        public int DeleteUser(string username)
        {
            int result = 0;
            try
            {
                using (var context = new LabyrinthEntities())
                {
                    var user = context.User.FirstOrDefault(userForSearching => userForSearching.userName == username);

                    if (user != null)
                    {
                        context.User.Remove(user);
                        result = context.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error eliminando el usuario: {ex.Message}");
            }
            return result; 
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
                            ProfilePicture = searchedUser.profilePicture,
                            CountryCode = searchedUser.countryCode,
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

        public void ChangeUserProfilePicture(int userId, byte[] imagenData)
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
            }
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

        public TransferUser[] GetRanking()
        {
            using (var context = new LabyrinthEntities())
            {
                var topPlayers = (from stat in context.Stats
                                  join user in context.User on stat.idUser equals user.idUser
                                  orderby stat.gamesWon descending
                                  select new TransferUser
                                  {
                                      IdUser = user.idUser,
                                      Username = user.userName,
                                      ProfilePicture = user.profilePicture,
                                      CountryCode = user.countryCode,

                                      TransferStats = new TransferStats
                                      {
                                          StatId = stat.idStats,
                                          GamesWon = stat.gamesWon.Value,
                                          GamesPlayed = stat.gamesPlayed.Value
                                      }
                                  })
                                  .Take(TOP_MAX)
                                  .ToArray();

                return topPlayers;
            }
        }


    }
}
