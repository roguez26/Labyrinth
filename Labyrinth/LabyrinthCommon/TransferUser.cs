using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public string Email { get; set; }

        [DataMember]
        public int Country { get; set; }

        [DataMember]
        public string ProfilePicture { get; set; }

        [DataMember]
        public TransferCountry TransferCountry { get; set; }

        [DataMember]
        public TransferStats TransferStats { get; set; }
    }
}
