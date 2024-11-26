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
    [ServiceContract(CallbackContract = typeof(IUserManagementServiceCallback))]
    public interface IUserManagement
    {
        [OperationContract]
        TransferUser[] GetRanking();

        [OperationContract(IsOneWay = false)]
        [FaultContract(typeof(LabyrinthCommon.LabyrinthException))]
        int AddUser(TransferUser user, string password);

        [OperationContract]
        [FaultContract(typeof(LabyrinthCommon.LabyrinthException))]

        TransferUser VerificateUser(string email, string password);

        [OperationContract]
        [FaultContract(typeof(LabyrinthCommon.LabyrinthException))]

        int UpdateUser(TransferUser newUser);

        [OperationContract]
        [FaultContract(typeof(LabyrinthCommon.LabyrinthException))]
        int UpdatePassword(string pasword, string newPassword, string email);

        [OperationContract]
        [FaultContract(typeof(LabyrinthCommon.LabyrinthException))]
        int DeleteAllUsers();

        [OperationContract]
        [FaultContract(typeof(LabyrinthCommon.LabyrinthException))]
        string ChangeUserProfilePicture(int userId, byte[] imagenData);

        [OperationContract(IsOneWay = true)]
        void GetUserProfilePicture(int userId, string path);

        [OperationContract]
        bool VerificateCode(string email, string code);


        [OperationContract]
        [FaultContract(typeof(LabyrinthCommon.LabyrinthException))]
        int AddVerificationCode(string email, string username);

        [OperationContract]
        [FaultContract(typeof(LabyrinthCommon.LabyrinthException))]
        bool IsEmailRegistered(string email);

        
    }
}
