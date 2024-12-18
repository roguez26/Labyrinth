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
    /// <summary>
    /// Interfaz que define los contratos del servicio de gestión de catálogo.
    /// </summary>
    [ServiceContract]
    public interface ICatalogManagement
    {
        /// <summary>
        /// Obtiene las estadísticas de un usuario por su ID.
        /// </summary>
        /// <param name="userId">El ID del usuario para el que se recuperarán las estadísticas.</param>
        /// <returns>Las estadísticas del usuario solicitadas.</returns>
        /// <exception cref="LabyrinthException">Lanzada cuando ocurre un error relacionado con el catálogo.</exception>
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
