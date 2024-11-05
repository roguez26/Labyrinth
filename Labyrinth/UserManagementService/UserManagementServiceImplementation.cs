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
        public int addUser(TransferUser user)
        {
            using (var context = new LabyrinthEntities())
            {
                context.User.Add(new User() { 
                    userName = user.Username, 
                    password = user.Password, 
                    email = user.Email, 
                    idCountry = user.Country });
                return context.SaveChanges();
            }
        }

        public Boolean verificateCode(string email, string code)
        {
            Boolean response = false;
            using (var context = new LabyrinthEntities())
            {
                var verification = context.VerificationCode.FirstOrDefault(verificaitonForSearching => verificaitonForSearching.email == email);

                if (verification != null && verification.code == code)
                {
                    context.VerificationCode.Remove(verification);
                    context.SaveChanges();
                    response = true;
                }
                return response;
            }

            
        }

        public int addVerificationCode(string email)
        {
            int response = 0;
            string verificationCode = generateVerificationCode();
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

            if (sendVerificationCode(email, verificationCode) > 0)
            {
                response = 1;
            }

            return response;
        }

        private int sendVerificationCode(string email, string code)
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


        public int updateUser(TransferUser newUser)
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

        

        public TransferUser userVerification(TransferUser user)
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
                            TransferCountry = catalogManagementServiceImplementation.getCountryById(searchedUser.idCountry),

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

        public string changeUserProfilePicture(int userId, byte[] imagenData)
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

        public byte[] getUserProfilePicture(string path)
        {
            byte[] response = new byte[0];

            if (File.Exists(path))
            {
                response = File.ReadAllBytes(path);
            }
            return response;
        }

        private string generateVerificationCode()
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
