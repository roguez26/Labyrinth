using TransferUser = LabyrinthCommon.TransferUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace LobbyManagementService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Reentrant)]
    public class LobbyManagementServiceImplementation : ILobbyManagementService
    {
        private Dictionary<string, Dictionary<ILobbyManagementCallback, TransferUser>> _lobbies = new Dictionary<string, Dictionary<ILobbyManagementCallback, TransferUser>>();

        public string CreateLobby(TransferUser lobbyCreator)
        {
            ILobbyManagementCallback callback = OperationContext.Current.GetCallbackChannel<ILobbyManagementCallback>();
            string lobbyCode = GenerateLobbyCode();

            // Genera un código de lobby único
            while (_lobbies.ContainsKey(lobbyCode))
            {
                lobbyCode = GenerateLobbyCode();
            }

            // Crea un nuevo diccionario para el lobby y añade el creador
            _lobbies[lobbyCode] = new Dictionary<ILobbyManagementCallback, TransferUser>();
            _lobbies[lobbyCode].Add(callback, lobbyCreator);

            // Retorna el código del lobby recién creado
            return lobbyCode;
        }

        public List<TransferUser> JoinToGame(string lobbyCode, TransferUser user)
        {
            List<TransferUser> members = new List<TransferUser>();
            ILobbyManagementCallback callback = OperationContext.Current.GetCallbackChannel<ILobbyManagementCallback>();

            if (callback == null)
            {
                throw new InvalidOperationException("No se pudo obtener el canal de callback.");
            }

            if (!_lobbies.TryGetValue(lobbyCode, out var lobbyMembers))
            {
                throw new KeyNotFoundException("El lobby con el código especificado no existe.");
            }

            if (lobbyMembers.ContainsKey(callback))
            {
                throw new InvalidOperationException("El usuario ya se encuentra en el lobby.");
            }

            // Agregar el nuevo usuario al lobby
            lobbyMembers[callback] = user;
            members.Add(user);

            // Notificar a otros usuarios y agregar sus datos a la lista de miembros
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
