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
        [DataMember]
        public string SkinPath { get; set; }

        [DataMember]
        public int InactivityCount { get; set; }

        [DataMember]
        public TransferTreasure[] Treasures { get; set; }

        [DataMember]
        public (int Row, int Col) Position { get; set; }
    }
}
