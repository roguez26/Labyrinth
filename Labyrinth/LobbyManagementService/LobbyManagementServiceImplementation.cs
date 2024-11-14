using TransferUser = LabyrinthCommon.TransferUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using LabyrinthCommon;
using log4net;

namespace LobbyManagementService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Reentrant)]
    public class LobbyManagementServiceImplementation : ILobbyManagementService
    {
        private Dictionary<string, Dictionary<ILobbyManagementCallback, TransferUser>> _lobbies = new Dictionary<string, Dictionary<ILobbyManagementCallback, TransferUser>>();
        private static readonly ILog _log = LogManager.GetLogger(typeof(LobbyManagementServiceImplementation));

        public string CreateLobby(TransferUser lobbyCreator)
        {
            ILobbyManagementCallback callback = OperationContext.Current.GetCallbackChannel<ILobbyManagementCallback>();
            string lobbyCode = GenerateLobbyCode();

            while (_lobbies.ContainsKey(lobbyCode))
            {
                lobbyCode = GenerateLobbyCode();
            }

            _lobbies[lobbyCode] = new Dictionary<ILobbyManagementCallback, TransferUser>();
            _lobbies[lobbyCode].Add(callback, lobbyCreator);

            return lobbyCode;
        }

        public List<TransferUser> JoinToGame(string lobbyCode, TransferUser user)
        {
            List<TransferUser> members = new List<TransferUser>();
            ILobbyManagementCallback callback = OperationContext.Current.GetCallbackChannel<ILobbyManagementCallback>();

            if (callback == null)
            {
                _log.Error("NullBackError");
                throw new FaultException<LabyrinthException>(new LabyrinthException("NullCallbackError"));
            }

            if (!_lobbies.TryGetValue(lobbyCode, out var lobbyMembers))
            {
                throw new FaultException<LabyrinthException>(new LabyrinthException("LobbyCodeNotFound"));
            }

            if (lobbyMembers.ContainsKey(callback))
            {
                throw new FaultException<LabyrinthException>(new LabyrinthException("UserAlreadyJoined"));
            }

            lobbyMembers[callback] = user;
            members.Add(user);

            foreach (var member in lobbyMembers)
            {
                if (!member.Value.Equals(user))
                {
                    member.Key.NotifyUserHasJoined(user);
                    members.Add(member.Value);
                }
            }

            return members;
        }

        public string GenerateLobbyCode()
        {
            Random random = new Random();
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var stringBuilder = new StringBuilder();

            for (int i = 0; i < 3; i++)
            {
                if (i > 0)
                {
                    stringBuilder.Append('-');
                }

                for (int j = 0; j < 3; j++)
                {
                    stringBuilder.Append(chars[random.Next(chars.Length)]);
                }
            }

            return stringBuilder.ToString();
        }

    }
}