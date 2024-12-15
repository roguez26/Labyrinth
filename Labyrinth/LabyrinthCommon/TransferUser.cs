using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices.WindowsRuntime;

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
        public string CountryCode { get; set; }

        [DataMember]
        public string ProfilePicture { get; set; }

        [DataMember]
        public TransferStats TransferStats { get; set; }

        public override bool Equals(object obj)
        {
            bool result = false;
            if (obj != null && GetType() == obj.GetType())
            {
                TransferUser other = (TransferUser)obj;
                result = IdUser == other.IdUser;
            }
            return result;
        }

        public override int GetHashCode()
        {
            return IdUser != 0 ? IdUser.GetHashCode() : 0; // Genera un hash basado en el Id
        }
    }
}
