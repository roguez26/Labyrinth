using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FriendsManagementService
{
    public class FriendsManagementServiceImplementation: IFriendsManagementService
    {
        public int SendFriendRequest(int idUser, int idFriend)
        {
            int idFriendRequest = 0;

            using (var context = new LabyrinthEntities())
            {
                var newFriendRequest = new FriendRequest
                {
                   
                    idRequester = idUser,
                    idRequested = idFriend,
                    status = "Pendiente"
                };

                context.FriendRequest.Add(newFriendRequest);
                context.SaveChanges();
                idFriendRequest = newFriendRequest.idFriendRequest;
            }

            return idFriendRequest;
        }


    }
}
