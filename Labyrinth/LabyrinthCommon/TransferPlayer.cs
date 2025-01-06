using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace LabyrinthCommon
{
    public class TransferPlayer
    {

        public string Username { get; set; }
        [DataMember]
        public string SkinPath { get; set; }

        [DataMember]
        public int InactivityCount { get; set; }

        [DataMember]
        public string[] TreasuresForSearching { get; set; }

        [DataMember]
        public (int Row, int Col) InitialPosition { get; set; }
    }
}
