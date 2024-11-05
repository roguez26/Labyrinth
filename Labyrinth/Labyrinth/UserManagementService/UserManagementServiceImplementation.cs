using CatalogManagementService;
using DataAccess;
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


namespace UserManagementService
{
    public class UserManagementServiceImplementation : IUserManagement
    {
        public int AddUser(TransferUser user)
        {
            int idUser = 0;
            using (var context = new LabyrinthEntities())
            {
                var newUser = new User
                {
                    userName = user.Username,
                    password = user.Password,
                    email = user.Email,
                    idCountry = user.Country
                };
                context.User.Add(newUser);
                context.SaveChanges();

                idUser = newUser.idCountry;
            }
            return idUser;
        }

        public Boolean VerificateCode(string email, string code)
        {
            using (var context = new LabyrinthEntities())
            {
                var verification = context.VerificationCode.FirstOrDefault(verificaitonForSearching => verificaitonForSearching.email == email);

                return (verification != null && verification.code == code);
            }            
        }

        public int AddVerificationCode(string email)
        {
            int response = 0;
            string verificationCode = GenerateVerificationCode();
            using (var context = new LabyrinthEntities())
            {
                var userForDuplicationVerification = context.User.FirstOrDefault(userForSearching => userForSearching.email == email);

                if (userForDuplicationVerification != null)
                {
                    response = -1;
                } else
                {
                    context.VerificationCode.Add(new VerificationCode()
                    {
                        email = email,
                        code = verificationCode,
                    });
                    response = context.SaveChanges();
                }
            }
            if (SendVerificationCode(email, verificationCode) > 0)
            {
                response = 1;
            }
            return response;
        }

        public int DeleteAllVerificationCodes()
        {
            using (var context = new LabyrinthEntities())
            {
                var allVerificationCodes = context.VerificationCode.ToList();

                context.VerificationCode.RemoveRange(allVerificationCodes);
                int rowsAffected = context.SaveChanges();
                return rowsAffected;
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
                        response = 1; // Envío exitoso
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error al enviar el correo: " + ex.ToString());
                        response = -1; // Envío fallido
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
                    response = -1;
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
                        response = -2;
                    }
                    else
                    {
                        userSearched.email = newUser.Email;
                        userSearched.userName = newUser.Username;
                        userSearched.idCountry = newUser.Country;
                        userSearched.password = newUser.Password;

                        context.Entry(userSearched).Property(u => u.email).IsModified = true;
                        context.Entry(userSearched).Property(u => u.userName).IsModified = true;
                        context.Entry(userSearched).Property(u => u.idCountry).IsModified = true;
                        context.Entry(userSearched).Property(u => u.password).IsModified = true;

                        response = context.SaveChanges();
                    }
                }
            }
            return response;
        }

        

        public TransferUser UserVerification(TransferUser user)
        {
            var userForVerification = new TransferUser();
            using (var context = new LabyrinthEntities())
            {
                var searchedUser = context.User.FirstOrDefault(userForSearching => userForSearching.email == user.Email);
                if (searchedUser == null)
                {
                    userForVerification.ErrorCode = "FailUserNotFoundMessage";
                }
                else
                {
                    if (searchedUser.password == user.Password)
                    {
                        CatalogManagementServiceImplementation catalogManagementServiceImplementation = new CatalogManagementServiceImplementation();
                        userForVerification = new TransferUser
                        {
                            IdUser = searchedUser.idUser,
                            Username = searchedUser.userName,
                            Password = searchedUser.password,
                            Email = searchedUser.email,
                            ProfilePicture = searchedUser.profilePicture,
                            TransferCountry = catalogManagementServiceImplementation.GetCountryById(searchedUser.idCountry),

                        };
                    }
                    else
                    {
                        userForVerification.ErrorCode = "FailIncorrectPasswordMessage";
                    }
                }
                return userForVerification;
            }
        }

        public string ChangeUserProfilePicture(int userId, byte[] imagenData)
        {
    
            string imageDirectory = Path.Combine("C:\\labyrinthImages", "profilePictures");

            if (!Directory.Exists(imageDirectory))
            {
                Directory.CreateDirectory(imageDirectory);
            }

            string fileName = $"{userId}_{Guid.NewGuid()}.jpg";
            string filePath = Path.Combine(imageDirectory, fileName);

            File.WriteAllBytes(filePath, imagenData);

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

            return filePath;
        }

        public byte[] GetUserProfilePicture(string path)
        {
            byte[] response = new byte[0];

            if (File.Exists(path))
            {
                response = File.ReadAllBytes(path);
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

        public bool IsEmailRegistered(string email)
        {
            using (var context = new LabyrinthEntities())
            {
                return context.User.Any(user => user.email == email);
            }
        }
    }
}
