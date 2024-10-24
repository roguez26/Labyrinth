using DataAccess;
using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using static DataAccess.LabyrinthEntities;

namespace CatalogManagementService
{
    public class CatalogManagementServiceImplementation : ICatalogManagement
    {
        public List<TransferCountry> getAllCountries()
        {
            using (var context = new LabyrinthEntities())
            {
                var countries = context.Country.ToList();
                var transferCountries = countries.Select(countryToTransfer => new TransferCountry
                {
                    CountryId = countryToTransfer.idCountry,
                    CountryName = countryToTransfer.name,
                }).ToList();
                return transferCountries;
            }
        }

        public TransferCountry getCountryById(int idCountry)
        {
            using (var context = new LabyrinthEntities())
            {
               
                var country = context.Country.FirstOrDefault(countryForSearching => countryForSearching.idCountry == idCountry);
               
                var transferCountry = new TransferCountry
                {
                    CountryId = country.idCountry,
                    CountryName = country.name,
                };
                return transferCountry;
            }
           
        }

        public TransferStats getStatsByUserId(int userId)
        {
            using (var context = new LabyrinthEntities())
            {
                var stats = context.Stats.FirstOrDefault(statsForSearching => statsForSearching.idUser == userId);

                var TransferStats = new TransferStats();
                if (stats != null)
                {
                    TransferStats = new TransferStats
                    {
                        GamesPlayed = (int)stats.gamesPlayed,
                        GamesWon = (int)stats.gamesWon,
                        UserId = stats.idUser,
                        StatId = stats.idStats,
                    };
                }
                else
                {
                    TransferStats.StatId = 0;
                }
                
                return TransferStats;
            }
        }
    }
}
