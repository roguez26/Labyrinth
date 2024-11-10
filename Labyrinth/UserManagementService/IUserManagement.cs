using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using DataAccess;
using CatalogManagementService;
using System.Security.Cryptography;
using TransferUser = LabyrinthCommon.TransferUser;

namespace UserManagementService
{
    [ServiceContract]
    public interface IUserManagement
    {
        [OperationContract]
        int AddUser(TransferUser user, string password);

        [OperationContract]
        TransferUser UserVerification(string email, string password);

        [OperationContract]
        int UpdateUser(TransferUser newUser);

        [OperationContract]
        int UpdatePassword(string pasword, string newPassword, string email);

        [OperationContract]
        string ChangeUserProfilePicture(int userId, byte[] imagenData);

        [OperationContract]
        byte[] GetUserProfilePicture(string path);

        [OperationContract]
        Boolean VerificateCode(string email, string code);

        [OperationContract]
        int AddVerificationCode(string email);

        [OperationContract]
        bool IsEmailRegistered(string email);

        [OperationContract]
        int DeleteAllVerificationCodes();
    }

   
}
