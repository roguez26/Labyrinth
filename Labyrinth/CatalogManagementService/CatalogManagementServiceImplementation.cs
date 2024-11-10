using DataAccess;
using LabyrinthCommon;
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
        public List<TransferCountry> GetAllCountries()
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

        public TransferCountry GetCountryById(int idCountry)
        {
            var transferCountry = new TransferCountry();

            using (var context = new LabyrinthEntities())
            {
                var _country = context.Country.FirstOrDefault(country => country.idCountry == idCountry);

                if (_country != null)
                {
                    transferCountry.CountryId = _country.idCountry;
                    transferCountry.CountryName = _country.name;
                }
            }

            return transferCountry;
        }



        public TransferStats GetStatsByUserId(int userId)
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
