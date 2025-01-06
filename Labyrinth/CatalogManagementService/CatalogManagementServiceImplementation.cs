using DataAccess;
using LabyrinthCommon;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
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

            if (userId <= 0)
            {
                throw new FaultException<LabyrinthCommon.LabyrinthException>(new LabyrinthCommon.LabyrinthException("FailStatsNotFoundError"));
            }

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
            catch (DbUpdateException exception) 
            {
                LogAndWrapException("GetStatsByUserId", exception, "FailStatsNotFoundError");
            }
            catch (EntityException exception) {
                LogAndWrapException("GetStatsByUserId", exception, "FailStatsNotFoundError");
            }
            return result;
        }

        public int AddStat(int userId, bool hasWon)
        {
            int result = 0;
            if (userId <= 0)
            {
                throw new FaultException<LabyrinthCommon.LabyrinthException>(new LabyrinthCommon.LabyrinthException("FailStatsNotFoundError"));
            }

            try
            {
                using (var context = new LabyrinthEntities())
                {
                    Stats stats = context.Stats.FirstOrDefault(statsForSearching => statsForSearching.idUser == userId);
                    if (stats != null)
                    {
                        if (hasWon)
                        {
                            stats.gamesWon = stats.gamesWon + 1;
                            context.Entry(stats).Property(stat => stat.gamesWon).IsModified = true;
                        }
                        stats.gamesPlayed = stats.gamesPlayed + 1;
                        context.Entry(stats).Property(stat => stat.gamesPlayed).IsModified = true;
                        result = context.SaveChanges();
                    } 
                    else
                    {
                        var stat = new Stats
                        {
                            idUser = userId,
                            gamesWon = hasWon ? 1 : 0,
                            gamesPlayed = 1
                        };
                        context.Stats.Add(stat);
                        result = context.SaveChanges();
                    }
                }
            }
            catch (DbUpdateException exception)
            {
                LogAndWrapException("GetStatsByUserId", exception, "FailStatsNotFoundError");
            }
            catch (EntityException exception)
            {
                LogAndWrapException("GetStatsByUserId", exception, "FailStatsNotFoundError");
            }
            return result;
        }

        private static void LogAndWrapException(string reason, Exception exception, string errorCode)
        {
            _log.Error(reason, exception);
            throw new FaultException<LabyrinthCommon.LabyrinthException>(
                new LabyrinthCommon.LabyrinthException(errorCode)
            );
        }

        public int DeleteStats()
        {
            int result = 0;

            try
            {
                using (var context = new LabyrinthEntities())
                {
                    var stats = context.Stats.ToList();

                    context.Stats.RemoveRange(stats);
                    result = context.SaveChanges();
                }
            }
            catch (Exception exception)
            {
                _log.Error("DeleteStats", exception);
            }

            return result;
        }

    }
}
