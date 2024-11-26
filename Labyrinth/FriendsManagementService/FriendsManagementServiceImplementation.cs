using DataAccess;
using LabyrinthCommon;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using log4net;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Web.UI.WebControls;


namespace FriendsManagementService
{
    public class FriendsManagementServiceImplementation: IFriendsManagementService
    {
        public int SendFriendRequest(int userId, int friendId)
        {
            int idFriendRequest = 0;

            if (IsFriendRequestRegistered(userId, friendId))
            {
                throw new FaultException<LabyrinthCommon.LabyrinthException>(new LabyrinthCommon.LabyrinthException("FailFriendRequestDuplicated"));
            }

            using (var context = new LabyrinthEntities())
            {
                var newFriendRequest = new FriendRequest
                {
                   
                    idRequester = userId,
                    idRequested = friendId,
                    status = FriendRequestStatus.Pending.ToString()
                };

                context.FriendRequest.Add(newFriendRequest);
                context.SaveChanges();
                idFriendRequest = newFriendRequest.idFriendRequest;
            }

            return idFriendRequest;
        }

        public bool IsFriendRequestRegistered(int userId, int friendId)
        {
            using (var context = new LabyrinthEntities())
            {
                return context.FriendRequest.Any(friendRequest => (friendRequest.idRequester == userId && friendRequest.idRequested == friendId) || (friendRequest.idRequested == userId && friendRequest.idRequester == friendId));
            }
        }

        public TransferUser[] GetMyFriendsList(int idUser)
        {
            TransferUser[] friends;

            UserManagementService.UserManagementServiceImplementation userManagement =
                new UserManagementService.UserManagementServiceImplementation();

            using (var context = new LabyrinthEntities())
            {
                var friendIds = context.FriendList
                    .Where(fr => fr.idUserOne == idUser || fr.idUserTwo == idUser)
                    .Select(fr => fr.idUserOne == idUser ? fr.idUserTwo : fr.idUserOne)
                    .ToList();

                var users = context.User
                    .Where(u => friendIds.Contains(u.idUser))
                    .ToList(); 

                friends = users.Select(u => new TransferUser
                {
                    IdUser = u.idUser,
                    Username = u.userName,
                    ProfilePicture = u.profilePicture
                }).ToArray();
            }

            return friends;
        }



        public TransferFriendRequest[] GetFriendRequestsList(int idUser)
        {
            List<TransferFriendRequest> friendRequests = new List<TransferFriendRequest>();
            UserManagementService.UserManagementServiceImplementation userManagement =
                new UserManagementService.UserManagementServiceImplementation();

            using (var context = new LabyrinthEntities())
            {
                var friendRequestsData = context.FriendRequest
                    .Where(fr => fr.idRequested == idUser && fr.status == FriendRequestStatus.Pending.ToString())
                    .Select(fr => new
                    {
                        fr.idFriendRequest,
                        fr.status,
                        Requester = context.User
                            .Where(u => u.idUser == fr.idRequester)
                            .Select(u => new
                            {
                                u.idUser,
                                u.userName,
                                u.profilePicture
                            })
                            .FirstOrDefault()
                    })
                    .ToList(); 

                 friendRequests = friendRequestsData
                    .Select(fr => new TransferFriendRequest
                    {
                        IdFriendRequest = fr.idFriendRequest,
                        Status = (FriendRequestStatus)Enum.Parse(typeof(FriendRequestStatus), fr.status),
                        Requester = new TransferUser
                        {
                            IdUser = fr.Requester.idUser,
                            Username = fr.Requester.userName,
                            ProfilePicture = fr.Requester.profilePicture
                        }
                    })
                    .ToList();
            }

            return friendRequests.ToArray();
        }

        public bool IsFriend(int userId, int friendId)
        {
            bool isFriend = false;
            using (var context = new LabyrinthEntities())
            {
                isFriend = context.FriendList.Any(friends => (friends.User.idUser == userId && friends.User1.idUser == friendId) || (friends.User.idUser == userId && friends.User1.idUser == friendId));
            }
            return isFriend || IsFriendRequestRegistered(userId, friendId);
        }

        public int AttendFriendRequest(int friendRequestId, LabyrinthCommon.FriendRequestStatus status)
        {
            int result = 0;

            using (var context = new LabyrinthEntities())
            {
                var friendRequest = context.FriendRequest.FirstOrDefault(fr => fr.idFriendRequest == friendRequestId);

                if (friendRequest != null)
                {
                    friendRequest.status = status.ToString();
                }

                if (status == FriendRequestStatus.Accepted)
                {
                    var newFriendship = new FriendList
                    {
                        idUserOne = friendRequest.idRequester,
                        idUserTwo = friendRequest.idRequested
                    };
                    context.FriendList.Add(newFriendship);
                }
                result = context.SaveChanges();    
            }

            return result;
        }
    }
}
