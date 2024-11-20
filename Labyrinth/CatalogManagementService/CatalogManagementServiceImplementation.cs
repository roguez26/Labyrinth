using DataAccess;
using LabyrinthCommon;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

namespace CatalogManagementService
{
    public class CatalogManagementServiceImplementation : ICatalogManagement
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(CatalogManagementServiceImplementation));

        public TransferCountry AddCountry(TransferCountry country)
        {
            using (var context = new LabyrinthEntities())
            {
                var newCountry = new Country
                {
                    name = country.CountryName
                };

                context.Country.Add(newCountry);
                context.SaveChanges();
                country.CountryId = newCountry.idCountry;
            }
            return country;
        }

        public List<TransferCountry> GetAllCountries()
        {
            try
            {
                using (var context = new LabyrinthEntities())
                {
                    var countries = context.Country.ToList();
                    var transferCountries = countries.Select(countryToTransfer => new TransferCountry
                    {
                        CountryId = countryToTransfer.idCountry,
                        CountryName = countryToTransfer.name,
                    }).ToList();

                    if (transferCountries.Count == 0)
                    {
                        throw new FaultException<LabyrinthCommon.LabyrinthException>(new LabyrinthCommon.LabyrinthException("FailNoCountriesMessage"));
                    }

                    return transferCountries;
                }
            }
            catch (Exception exception)
            {
                _log.Error("GetAllCountriesError", exception);
                throw new FaultException<LabyrinthCommon.LabyrinthException>(new LabyrinthCommon.LabyrinthException("GetAllCountriesException"));
            }
        }

        public int DeleteAllCountries()
        {
            using (var context = new LabyrinthEntities())
            {
                int countriesRemoved = context.Country.Count();
                context.Country.RemoveRange(context.Country);
                context.SaveChanges();
                return countriesRemoved;
            }
        }

        public TransferCountry GetCountryById(int idCountry)
        {
            var transferCountry = new TransferCountry();

            try
            {
                using (var context = new LabyrinthEntities())
                {
                    var _country = context.Country.FirstOrDefault(country => country.idCountry == idCountry);

                    if (_country != null)
                    {
                        transferCountry.CountryId = _country.idCountry;
                        transferCountry.CountryName = _country.name;
                    }
                }
            }
            catch (Exception exception)
            {
                _log.Error("GetCountryByIdError", exception);
                throw new FaultException<LabyrinthCommon.LabyrinthException>(new LabyrinthCommon.LabyrinthException("GetCountryByIdException"));
            }
            return transferCountry;
        }

        public TransferStats GetStatsByUserId(int userId)
        {
            try
            {
                using (var context = new LabyrinthEntities())
                {
                    var stats = context.Stats.FirstOrDefault(statsForSearching => statsForSearching.idUser == userId);
                    var transferStats = new TransferStats();

                    if (stats != null)
                    {
                        transferStats = new TransferStats
                        {
                            GamesPlayed = (int)stats.gamesPlayed,
                            GamesWon = (int)stats.gamesWon,
                            UserId = stats.idUser,
                            StatId = stats.idStats,
                        };
                    }
                    else
                    {
                        transferStats.StatId = 0;
                    }
                    return transferStats;
                }
            }
            catch (Exception exception)
            {
                _log.Error("GetStatsByUserIdError", exception);
                throw new FaultException<LabyrinthCommon.LabyrinthException>(new LabyrinthCommon.LabyrinthException("GetStatsByUserIdException"));
            }
        }
    }
}
