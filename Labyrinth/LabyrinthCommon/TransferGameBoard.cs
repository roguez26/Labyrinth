using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace LabyrinthCommon
{
    public class TransferGameBoard
    {
        [DataMember]
        public List<List<TransferTile>> Board { get; set; }

        [DataMember]
        public (int Row, int Col)[] PlayerPositions { get; set; }

        [DataMember]
        public TransferTile ExtraTile { get; set; }

        [DataMember]
        public List<TransferPlayer> Players { get; set; }

    }
}
