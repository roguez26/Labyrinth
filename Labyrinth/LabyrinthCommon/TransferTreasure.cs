using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace LabyrinthCommon
{
    public class TransferTreasure
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public bool IsFound { get; set; }

        [DataMember]
        public string ImagePath { get; set; }


    }
}
