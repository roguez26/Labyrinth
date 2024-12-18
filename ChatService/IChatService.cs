using LabyrinthCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ChatService
{
    /// <summary>
    /// Interfaz que define los contratos para el servicio de chat dentro de un lobby.
    /// </summary>
    [ServiceContract(CallbackContract = typeof(IChatServiceCallback))]
    public interface IChatService
    {
        /// <summary>
        /// Envía un mensaje a todos los miembros del chat en un lobby.
        /// </summary>
        /// <param name="message">El mensaje a enviar.</param>
        /// <param name="lobbyCode">El código del lobby donde se enviará el mensaje.</param>
        [OperationContract(IsOneWay = true)]
        void SendMessage(string message, string lobbyCode);

        /// <summary>
        /// Inicia un nuevo chat en el lobby.
        /// </summary>
        /// <param name="lobbyCode">El código del lobby en el que se creará el chat.</param>
        /// <param name="lobbyCreator">El creador del lobby.</param>
        /// <returns>Un código de estado que indica el éxito o fracaso de la operación.</returns>
        /// <exception cref="LabyrinthException">Lanzada cuando ocurre un error al iniciar el chat.</exception>
        [OperationContract]
        [FaultContract(typeof(LabyrinthException))]
        int Start(string lobbyCode, TransferUser lobbyCreator);

        /// <summary>
        /// Permite a un usuario unirse a un chat en un lobby.
        /// </summary>
        /// <param name="lobbyCode">El código del lobby al que el usuario quiere unirse.</param>
        /// <param name="user">El usuario que se unirá al chat.</param>
        /// <returns>Un código de estado que indica el éxito o fracaso de la operación.</returns>
        /// <exception cref="LabyrinthException">Lanzada cuando ocurre un error al unir al usuario al chat.</exception>
        [OperationContract]
        [FaultContract(typeof(LabyrinthException))]
        int JoinToChat(string lobbyCode, TransferUser user);

        /// <summary>
        /// Elimina a un usuario del chat en un lobby.
        /// </summary>
        /// <param name="lobbyCode">El código del lobby del que se eliminará al usuario.</param>
        /// <param name="user">El usuario que será removido del chat.</param>
        /// <returns>Un código de estado que indica el éxito o fracaso de la operación.</returns>
        /// <exception cref="LabyrinthException">Lanzada cuando ocurre un error al eliminar al usuario del chat.</exception>
        [OperationContract]
        [FaultContract(typeof(LabyrinthException))]
        int RemoveUserFromChat(string lobbyCode, TransferUser user);
    }
}
