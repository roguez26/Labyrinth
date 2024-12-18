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
    /// Interfaz que define los contratos del servicio de chat en el juego Labyrinth.
    /// </summary>
    [ServiceContract(CallbackContract = typeof(IChatServiceCallback))]
    public interface IChatService
    {
        /// <summary>
        /// Envía un mensaje al chat de un lobby específico.
        /// </summary>
        /// <param name="message">El mensaje a enviar.</param>
        /// <param name="lobbyCode">El código del lobby al que se enviará el mensaje.</param>
        [OperationContract(IsOneWay = true)]
        void SendMessage(string message, string lobbyCode);

<<<<<<< HEAD
        /// <summary>
        /// Inicia una nueva sesión de chat para un lobby, creando el chat y asignando al creador.
        /// </summary>
        /// <param name="lobbyCode">El código del lobby donde se iniciará el chat.</param>
        /// <param name="lobbyCreator">El usuario que crea el lobby.</param>
        [OperationContract(IsOneWay = true)]
        void Start(string lobbyCode, TransferUser lobbyCreator);

        /// <summary>
        /// Permite a un usuario unirse al chat de un lobby existente.
        /// </summary>
        /// <param name="lobbyCode">El código del lobby al que el usuario desea unirse.</param>
        /// <param name="user">El usuario que se unirá al chat.</param>
        [OperationContract(IsOneWay = true)]
        void JoinToChat(string lobbyCode, TransferUser user);

        /// <summary>
        /// Elimina a un usuario del chat de un lobby específico.
        /// </summary>
        /// <param name="lobbyCode">El código del lobby del cual se eliminará al usuario.</param>
        /// <param name="user">El usuario que será removido del chat.</param>
        [OperationContract(IsOneWay = true)]
        void RemoveUserFromChat(string lobbyCode, TransferUser user);
=======
        [OperationContract ]
        [FaultContract(typeof(LabyrinthCommon.LabyrinthException))]

        int Start(string lobbyCode, TransferUser lobbyCreator);

        [OperationContract ]
        [FaultContract(typeof(LabyrinthCommon.LabyrinthException))]

        int JoinToChat(string lobbyCode, TransferUser user);

        [OperationContract ]
        [FaultContract(typeof(LabyrinthCommon.LabyrinthException))]
        int RemoveUserFromChat(string lobbyCode, TransferUser user);
>>>>>>> ca8e7cd96dbdebd895b5badd47ddb37472a951f9
    }
}
