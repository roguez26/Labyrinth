using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace LabyrinthCommon
{
    [DataContract]
    public class TransferCountry
    {
        [DataMember]
        public string CountryName { get; set; }
        [DataMember]
        public int CountryId { get; set; } = 0;
    }
}
