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
using LabyrinthCommon;

namespace UserManagementService
{
    [ServiceContract]
    public interface IUserManagement
    {
        [OperationContract]
        [FaultContract(typeof(LabyrinthException))]
        int AddUser(TransferUser user, string password);

        [OperationContract]
        [FaultContract(typeof(LabyrinthException))]
        TransferUser UserVerification(string email, string password);

        [OperationContract]
        [FaultContract(typeof(LabyrinthException))]
        int UpdateUser(TransferUser newUser);

        [OperationContract]
        [FaultContract(typeof(LabyrinthException))]
        int UpdatePassword(string pasword, string newPassword, string email);

        [OperationContract]
        [FaultContract(typeof(LabyrinthException))]
        string ChangeUserProfilePicture(int userId, byte[] imagenData);

        [OperationContract]
        [FaultContract(typeof(LabyrinthException))]
        byte[] GetUserProfilePicture(string path);

        [OperationContract]
        bool VerificateCode(string email, string code);


        [OperationContract]
        [FaultContract(typeof(LabyrinthException))]
        int AddVerificationCode(string email);

        [OperationContract]
        [FaultContract(typeof(LabyrinthException))]
        bool IsEmailRegistered(string email);

        [OperationContract]
        [FaultContract(typeof(LabyrinthException))]
        int DeleteAllVerificationCodes();
    }

   
}
