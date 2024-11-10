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


namespace UserManagementService
{
    public class UserManagementServiceImplementation : IUserManagement
    {
        public int AddUser(TransferUser user, string password)
        {
            int idUser = 0;

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
            } catch (Exception exception)
            {
                throw new FaultException<LabyrinthException>(new LabyrinthException("AddUserError"));
            }
            return idUser;
        }

        public Boolean VerificateCode(string email, string code)
        {
            Boolean response = false;

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
                }
            }
            catch (Exception exception)
            {
                throw new FaultException<LabyrinthException>(new LabyrinthException("VerificateCodeError"));

            }
            return response;
        }

        public int AddVerificationCode(string email)
        {
            int response = 0;
            string verificationCode = GenerateVerificationCode();

            try
            {                
                using (var context = new LabyrinthEntities())
                {
                    var userForDuplicationVerification = context.User.FirstOrDefault(userForSearching => userForSearching.email == email);

                    if (userForDuplicationVerification != null)
                    {
                        response = -1;
                    }
                    else
                    {
                        context.VerificationCode.Add(new VerificationCode()
                        {
                            email = email,
                            code = verificationCode,
                        });
                        response = context.SaveChanges();
                    }
                }
            } catch (Exception exception)
            {
                throw new FaultException<LabyrinthException>(new LabyrinthException("AddVerificationCodeError"));
            }

            if (SendVerificationCode(email, verificationCode) > 0)
            {
                response = 1;
            }

            return response;
        }

        public int DeleteAllVerificationCodes()
        {
            try
            {
                using (var context = new LabyrinthEntities())
                {
                    var allVerificationCodes = context.VerificationCode.ToList();

                    context.VerificationCode.RemoveRange(allVerificationCodes);
                    int rowsAffected = context.SaveChanges();
                    return rowsAffected;
                }
            } catch (Exception exception)
            {
                throw new FaultException<LabyrinthException>(new LabyrinthException("DeleteAllVerificationCodesError"));
            }
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
                        throw new FaultException<LabyrinthException>(new LabyrinthException("SendVerificationCodeError"));
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
                    throw new FaultException<LabyrinthException>(new LabyrinthException("FailUserNotFoundMessage"));
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
                        throw new FaultException<LabyrinthException>(new LabyrinthException("FailDuplicatedUserFoundMessage"));
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

        public int UpdatePassword(string password, string newPassword, string email)
        {
            int response = 0;

            try
            {
                using (var context = new LabyrinthEntities())
                {
                    var userSearched = context.User.FirstOrDefault(userForSearching => userForSearching.email == email);
                    if (password.Equals(userSearched.password))
                    {
                        userSearched.password = newPassword;
                        context.Entry(userSearched).Property(u => u.password).IsModified = true;
                        response = context.SaveChanges();
                    }
                }
            } catch (Exception exception)
            {
                throw new FaultException<LabyrinthException>(new LabyrinthException("UpdatePasswordError"));
            }
            return response;
        }

        

        public TransferUser UserVerification(string email, string password)
        {
            var userForVerification = new TransferUser();

            try
            {
                using (var context = new LabyrinthEntities())
                {
                    var searchedUser = context.User.FirstOrDefault(userForSearching => userForSearching.email == email);

                    if (searchedUser == null)
                    {
                        throw new FaultException<LabyrinthException>(new LabyrinthException("FailUserNotFoundMessage"));
                    }
                    else
                    {
                        if (searchedUser.password == password)
                        {
                            CatalogManagementServiceImplementation catalogManagementServiceImplementation = new CatalogManagementServiceImplementation();
                            userForVerification = new TransferUser

                            {
                                IdUser = searchedUser.idUser,
                                Username = searchedUser.userName,
                                //Password = searchedUser.password,
                                Email = searchedUser.email,
                                ProfilePicture = searchedUser.profilePicture,
                                TransferCountry = catalogManagementServiceImplementation.GetCountryById(searchedUser.idCountry),
                            };
                        }
                        else
                        {
                            throw new FaultException<LabyrinthException>(new LabyrinthException("FailIncorrectPasswordMessage"));
                        }
                    }
                }
            } catch (Exception exception)
            {
                throw new FaultException<LabyrinthException>(new LabyrinthException("UserVerificationError"));
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
            } catch (Exception exception)
            {
                throw new FaultException<LabyrinthException>(new LabyrinthException("ChangeUserProfilePictureError"));
            }
            return filePath;
        }

        public byte[] GetUserProfilePicture(string path)
        {
            byte[] response = new byte[0];

            if (File.Exists(path))
            {
                response = File.ReadAllBytes(path);
            } else
            {
                throw new FaultException<LabyrinthException>(new LabyrinthException("ProfilePictureNotFoundMessage"));
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
