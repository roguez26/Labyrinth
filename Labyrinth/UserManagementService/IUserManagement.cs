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
        int addUser(TransferUser user);

        [OperationContract]
        TransferUser userVerification(TransferUser user);

        [OperationContract]
        int updateUser(TransferUser newUser);

        [OperationContract]
        string changeUserProfilePicture(int userId, byte[] imagenData);

        [OperationContract]
        byte[] getUserProfilePicture(string path);

        [OperationContract]
        Boolean verificateCode(string email, string code);

        [OperationContract]
        int addVerificationCode(string email);
    }

    [DataContract]
    public class TransferUser
    {

        [DataMember]
        public int IdUser {  get; set; }

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
