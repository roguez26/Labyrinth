using LabyrinthCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace GameService
{
    [ServiceContract]
    public interface IGameServiceCallback
    {
       
        [OperationContract]
        void ReceiveGameBoard(TransferGameBoard gameBoard);
        
    }
}
