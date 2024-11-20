using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace LabyrinthCommon
{
    public class TransferFriendRequest
    {
        [DataMember]
        public int IdFriendRequest { get; set; }

        [DataMember]
        public TransferUser Requester { get; set; }

        [DataMember]
        public FriendRequestStatus Status { get; set; }
    }
}

namespace LabyrinthCommon
{
    public enum FriendRequestStatus
    {
        Pending,
        Accepted,
        Rejected
    }
}
