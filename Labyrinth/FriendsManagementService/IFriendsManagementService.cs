using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.ServiceModel;
using LabyrinthCommon;
using System.Security.Cryptography;

namespace FriendsManagementService
{
    /// <summary>
    /// Interfaz que define los contratos para el servicio de gestión de amigos.
    /// </summary>
    [ServiceContract]
    public interface IFriendsManagementService
    {
        /// <summary>
        /// Envía una solicitud de amistad a otro usuario.
        /// </summary>
        /// <param name="userId">El ID del usuario que envía la solicitud de amistad.</param>
        /// <param name="friendId">El ID del usuario a quien se le envía la solicitud de amistad.</param>
        /// <returns>El ID de la solicitud de amistad generada.</returns>
        /// <exception cref="LabyrinthException">Lanzada cuando ocurre un error relacionado con la solicitud de amistad.</exception>
        [OperationContract]
        [FaultContract(typeof(LabyrinthCommon.LabyrinthException))]
        int SendFriendRequest(int userId, int friendId);

        /// <summary>
        /// Obtiene la lista de amigos de un usuario.
        /// </summary>
        /// <param name="idUser">El ID del usuario para el que se recuperará la lista de amigos.</param>
        /// <returns>Una lista de los amigos del usuario.</returns>
        [OperationContract]
        TransferUser[] GetMyFriendsList(int idUser);

        /// <summary>
        /// Obtiene la lista de solicitudes de amistad de un usuario.
        /// </summary>
        /// <param name="idUser">El ID del usuario para el que se recuperarán las solicitudes de amistad.</param>
        /// <returns>Una lista de las solicitudes de amistad pendientes para el usuario.</returns>
        [OperationContract]
        TransferFriendRequest[] GetFriendRequestsList(int idUser);

        /// <summary>
        /// Verifica si dos usuarios son amigos.
        /// </summary>
        /// <param name="userId">El ID del primer usuario.</param>
        /// <param name="friendId">El ID del segundo usuario.</param>
        /// <returns>True si son amigos, false en caso contrario.</returns>
        [OperationContract]
        bool IsFriend(int userId, int friendId);

        /// <summary>
        /// Responde a una solicitud de amistad.
        /// </summary>
        /// <param name="friendRequestId">El ID de la solicitud de amistad que se está atendiendo.</param>
        /// <param name="status">El estado de la solicitud de amistad (aceptada o rechazada).</param>
        /// <returns>El ID de la solicitud de amistad que fue atendida.</returns>
        [OperationContract]
        [FaultContract(typeof(LabyrinthCommon.LabyrinthException))]
        int AttendFriendRequest(int friendRequestId, LabyrinthCommon.FriendRequestStatus status);

        [OperationContract]
        int DeleteFriendRequests(int friendRequestId);

        [OperationContract]
        int DeleteFriendList(int friendRequestId);
    }
}
