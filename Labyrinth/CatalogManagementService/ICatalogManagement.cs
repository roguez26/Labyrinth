using DataAccess;
using LabyrinthCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace CatalogManagementService
{
    [ServiceContract]
    public interface ICatalogManagement
    {

        [OperationContract]
        [FaultContract(typeof(LabyrinthException))]
        TransferStats GetStatsByUserId(int userId);

        [OperationContract]
        [FaultContract(typeof(LabyrinthException))]

        int AddStat(int userId, bool isWon);

        [OperationContract]
        int DeleteStats();
    }
}

