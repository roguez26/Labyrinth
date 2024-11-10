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
        List<TransferCountry> GetAllCountries();

        [OperationContract]
        TransferCountry GetCountryById(int idCountry);

        [OperationContract]
        TransferStats GetStatsByUserId(int userId);


    }
   

    
}

