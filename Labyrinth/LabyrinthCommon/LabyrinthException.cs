using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace LabyrinthCommon
{
    [DataContract]
    public class LabyrinthException
    {

        [DataMember]
        public string Code { get; set; }

        public LabyrinthException(string code)
        {
            Code = code;
        }

    }
}
