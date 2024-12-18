using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using TransferUser = LabyrinthCommon.TransferUser;

namespace MenuManagementService
{
    /// <summary>
    /// Define los contratos para la gestión de las solicitudes de acceso a una partida.
    /// </summary>
    [ServiceContract(CallbackContract = typeof(IMenuManagementServiceCallback))]
    public interface IMenuManagementService
    {
        /// <summary>
        /// Inicia una partida para el usuario proporcionado.
        /// </summary>
        /// <param name="user">El usuario que inicia la partida.</param>
        [OperationContract]
        [FaultContract(typeof(LabyrinthCommon.LabyrinthException))]
        void Start(TransferUser user);

        /// <summary>
        /// Cambia la disponibilidad de un usuario para unirse a la partida.
        /// </summary>
        /// <param name="user">El usuario cuya disponibilidad se desea cambiar.</param>
        /// <param name="availability">La nueva disponibilidad del usuario (true si está disponible, false si no lo está).</param>
        [OperationContract]
        [FaultContract(typeof(LabyrinthCommon.LabyrinthException))]
        void ChangeAvailability(TransferUser user, bool availability);

        /// <summary>
        /// Invita a un amigo a unirse a la partida mediante un código de sala.
        /// </summary>
        /// <param name="inviter">El usuario que envía la invitación.</param>
        /// <param name="invitee">El usuario que recibe la invitación.</param>
        /// <param name="lobbyCode">El código de la sala de juego a la que el invitado debe unirse.</param>
        [OperationContract(IsOneWay = true)]
        void InviteFriend(TransferUser inviter, TransferUser invitee, string lobbyCode);

        /// <summary>
        /// Actualiza el callback del usuario.
        /// </summary>
        /// <param name="user">El usuario cuyo callback se debe actualizar.</param>
        [OperationContract]
        [FaultContract(typeof(LabyrinthCommon.LabyrinthException))]
        void UpdateCallback(TransferUser user);
    }
}
