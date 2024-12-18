using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace LabyrinthCommon
{
    public class TransferTile
    {

        [DataMember]
        public int Type { get; set; }

        [DataMember]
        public bool IsTopOpen { get; set; }

        [DataMember]
        public bool IsRightOpen { get; set; }

        [DataMember]
        public bool IsBottomOpen { get; set; }

        [DataMember]
        public bool IsLeftOpen { get; set; }

        [DataMember]
        public string ImagePath {  get; set; }

        [DataMember]
        public int RotationAngle { get; set; }

        [DataMember]
        public List<TransferPlayer> PlayerOnTile { get; set; }

        [DataMember]
        public TransferTreasure Treasure { get; set; }

    }
}
