using DataAccess;
using LabyrinthCommon;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Data.SqlClient;
using System.Linq;
using System.ServiceModel;

namespace CatalogManagementService
{
    public class CatalogManagementServiceImplementation : ICatalogManagement
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(CatalogManagementServiceImplementation));
        public TransferStats GetStatsByUserId(int userId)
        {
            TransferStats result = new TransferStats { StatId = 0};
            try
            {
                using (var context = new LabyrinthEntities())
                {
                    Stats stats = context.Stats.FirstOrDefault(statsForSearching => statsForSearching.idUser == userId);

                    if (stats != null)
                    {
                        result = new TransferStats
                        {
                            GamesPlayed = (int)stats.gamesPlayed,
                            GamesWon = (int)stats.gamesWon,
                            UserId = stats.idUser,
                            StatId = stats.idStats,
                        };
                    }
                }
            }
            catch (SqlException exception) 
            {
                LogAndWrapException("GetStatsByUserId", exception, "FailStatsNotFoundError");
            }
            catch (EntityException exception) {
                LogAndWrapException("GetStatsByUserId", exception, "FailStatsNotFoundError");
            }
            return result;
        }

        private void LogAndWrapException(string reason, Exception exception, string errorCode)
        {
            _log.Error(reason, exception);
            throw new FaultException<LabyrinthCommon.LabyrinthException>(
                new LabyrinthCommon.LabyrinthException(errorCode)
            );
        }

    }
}
