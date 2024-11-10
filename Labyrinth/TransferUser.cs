using System;
using System.Runtime.Serialization;

namespace LabyrinthCommon
{
    [DataContract]
    public class TransferUser
    {
        [DataMember]
        public int IdUser { get; set; }

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
        public string ProfilePicture { get; set; }
    }
}
