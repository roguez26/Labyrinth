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
        TransferCountry AddCountry(TransferCountry country);

        [OperationContract]
        [FaultContract(typeof(LabyrinthException))]
        int DeleteAllCountries();

        [OperationContract]
        [FaultContract(typeof(LabyrinthException))]
        List<TransferCountry> GetAllCountries();

        [OperationContract]
        [FaultContract(typeof(LabyrinthException))]
        TransferCountry GetCountryById(int idCountry);

        [OperationContract]
        [FaultContract(typeof(LabyrinthException))]
        TransferStats GetStatsByUserId(int userId);
    }
   

    
}

