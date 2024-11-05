using DataAccess;
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
    [DataContract]
    public class TransferCountry
    {
        [DataMember]
        public string CountryName { get; set; }
        [DataMember]
        public int CountryId { get; set; } = 0;
    }

    [DataContract]
    public class TransferStats
    {
        [DataMember]
        public int StatId { get; set; }

        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public int GamesWon { get; set; }

        [DataMember]
        public int GamesPlayed { get; set; }

    }
}

