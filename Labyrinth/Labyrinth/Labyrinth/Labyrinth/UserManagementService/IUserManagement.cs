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

namespace UserManagementService
{
    [ServiceContract]
    public interface IUserManagement
    {
        [OperationContract]
        int AddUser(TransferUser user);

        [OperationContract]
        TransferUser UserVerification(TransferUser user);

        [OperationContract]
        int UpdateUser(TransferUser newUser);

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

    [DataContract]
    public class TransferUser
    {

        [DataMember]
        public int IdUser { get; set; } = 0;

        [DataMember]
        public string Username { get; set; }

        [DataMember]
        public string Password { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public int Country { get; set; }

        [DataMember]
        public string ErrorCode { get; set; }

        [DataMember]
        public TransferCountry TransferCountry { get; set; }

        [DataMember]
        public string ProfilePicture {  get; set; }


    }
}
