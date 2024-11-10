using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace LabyrinthCommon
{
    [DataContract]
    public class TransferStats
    {
        [DataMember]
        public int StatId { get; set; }

        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public int GamesWon { get; set; }

        [DataMember]
        public int GamesPlayed { get; set; }

    }
}
