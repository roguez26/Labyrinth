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
        List<TransferCountry> getAllCountries();

        [OperationContract]
        TransferCountry getCountryById(int idCountry);

        [OperationContract]
        TransferStats getStatsByUserId (int userId);

        
    }
    [DataContract]
    public class TransferCountry
    {
        [DataMember]
        public string CountryName { get; set; }
        [DataMember]
        public int CountryId { get; set; }
    }

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

