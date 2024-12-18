using LabyrinthCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using TransferUser = LabyrinthCommon.TransferUser;

namespace LobbyManagementService
{
    /// <summary>
    /// Define los contratos para la gestión de salas de juego (lobbies).
    /// </summary>
    [ServiceContract(CallbackContract = typeof(ILobbyManagementCallback))]
    public interface ILobbyManagementService
    {
        /// <summary>
        /// Crea una nueva sala de juego (lobby).
        /// </summary>
        /// <param name="lobbyCreator">El usuario que está creando la sala de juego.</param>
        /// <returns>El código único que identifica la sala de juego creada.</returns>
        /// <exception cref="LabyrinthException">Lanzada cuando ocurre un error en la creación de la sala.</exception>
        [OperationContract]
        [FaultContract(typeof(LabyrinthCommon.LabyrinthException))]
        string CreateLobby(TransferUser lobbyCreator);

        /// <summary>
        /// Permite a un usuario unirse a una sala de juego existente.
        /// </summary>
        /// <param name="lobbyCode">El código único de la sala a la que el usuario desea unirse.</param>
        /// <param name="user">El usuario que está intentando unirse a la sala de juego.</param>
        [OperationContract(IsOneWay = true)]
        void JoinToGame(string lobbyCode, TransferUser user);
    }
}
