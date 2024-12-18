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
using System.Data.Entity.Core;
using UserManagementService;
using System.Runtime.CompilerServices;


namespace FriendsManagementService
{
    public class FriendsManagementServiceImplementation: IFriendsManagementService
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(FriendsManagementServiceImplementation));
        public int SendFriendRequest(int userId, int friendId)
        {
            int idFriendRequest = 0;
            if (userId <= 0 || friendId <= 0)
            {
                throw new FaultException<LabyrinthCommon.LabyrinthException>(new LabyrinthCommon.LabyrinthException("FailFriendManagementError"));
            }
            if (IsFriendRequestRegistered(userId, friendId))
            {
                throw new FaultException<LabyrinthCommon.LabyrinthException>(new LabyrinthCommon.LabyrinthException("FailFriendRequestDuplicated"));
            }
            try
            {
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
            }
            catch (DbUpdateException ex)
            {
                LogAndWrapException("SendFriendRequest", ex, "FailFriendManagementError");
            }
            catch (EntityException ex)
            {
                LogAndWrapException("SendFriendRequest", ex, "FailFriendManagementError");
            }
            return idFriendRequest;
        }

        private static void LogAndWrapException(string reason, Exception exception, string errorCode)
        {
            _log.Error(reason, exception);
            throw new FaultException<LabyrinthCommon.LabyrinthException>(
                new LabyrinthCommon.LabyrinthException(errorCode)
            );
        }

        public static bool IsFriendRequestRegistered(int userId, int friendId)
        {
            using (var context = new LabyrinthEntities())
            {
                return context.FriendRequest.Any(friendRequest => (friendRequest.idRequester == userId && friendRequest.idRequested == friendId) || (friendRequest.idRequested == userId && friendRequest.idRequester == friendId));
            }
        }

        public TransferUser[] GetMyFriendsList(int userId)
        {
            TransferUser[] friends = new TransferUser[0];

            try
            {
                using (var context = new LabyrinthEntities())
                {
                    var friendIds = context.FriendList
                        .Where(fr => fr.idUserOne == userId || fr.idUserTwo == userId)
                        .Select(fr => fr.idUserOne == userId ? fr.idUserTwo : fr.idUserOne)
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
            }
            catch (DbUpdateException ex)
            {
                LogAndWrapException("SendFriendRequest", ex, "FailFriendManagementError");
            }
            catch (EntityException ex)
            {
                LogAndWrapException("SendFriendRequest", ex, "FailFriendManagementError");
            }

            return friends;
        }

        public TransferFriendRequest[] GetFriendRequestsList(int idUser)
        {
            List<TransferFriendRequest> friendRequests = new List<TransferFriendRequest>();

            try
            {
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
            }
            catch (DbUpdateException ex)
            {
                LogAndWrapException("SendFriendRequest", ex, "FailFriendManagementError");
            }
            catch (EntityException ex)
            {
                LogAndWrapException("SendFriendRequest", ex, "FailFriendManagementError");
            }

            return friendRequests.ToArray();
        }

        public bool IsFriend(int userId, int friendId)
        {
            bool isFriend = false;
            try
            {
                using (var context = new LabyrinthEntities())
                {
                    isFriend = context.FriendList.Any(friends => (friends.User.idUser == userId && friends.User1.idUser == friendId) || (friends.User.idUser == userId && friends.User1.idUser == friendId));
                }
            }
            catch (DbUpdateException ex)
            {
                LogAndWrapException("SendFriendRequest", ex, "FailFriendManagementError");
            }
            catch (EntityException ex)
            {
                LogAndWrapException("SendFriendRequest", ex, "FailFriendManagementError");
            }

            return isFriend || IsFriendRequestRegistered(userId, friendId);
        }

        public int AttendFriendRequest(int friendRequestId, LabyrinthCommon.FriendRequestStatus status)
        {
            int result = 0;

            if (friendRequestId <= 0)
            {
                throw new FaultException<LabyrinthCommon.LabyrinthException>(new LabyrinthCommon.LabyrinthException("FailFriendManagementError"));
            }
            try
            {
                using (var context = new LabyrinthEntities())
                {
                    var friendRequest = context.FriendRequest.FirstOrDefault(fr => fr.idFriendRequest == friendRequestId);

                    if (friendRequest != null)
                    {
                        friendRequest.status = status.ToString();
                        if (status == FriendRequestStatus.Accepted)
                        {
                            var newFriendship = new FriendList
                            {
                                idUserOne = friendRequest.idRequester,
                                idUserTwo = friendRequest.idRequested
                            };
                            context.FriendList.Add(newFriendship);
                            context.SaveChanges();
                            result = newFriendship.idFriendList;
                        }
                    }
                }
            }
            catch (DbUpdateException ex)
            {
                LogAndWrapException("AttendFriendRequest", ex, "FailFriendManagementError");
            }
            catch (EntityException ex)
            {
                LogAndWrapException("AttendFriendRequest", ex, "FailFriendManagementError");
            }

            return result;
        }

        public int DeleteFriendRequests(int friendRequestId)
        {
            int result = 0;
            try
            {
                using (var context = new LabyrinthEntities())
                {
                    var newFriendRequest = context.FriendRequest.FirstOrDefault(request => request.idFriendRequest == friendRequestId);

                    if (newFriendRequest != null)
                    {
                        context.FriendRequest.Remove(newFriendRequest);
                        result =  context.SaveChanges();
                    }
                }
            }
            catch (DbUpdateException ex)
            {
                LogAndWrapException("SendFriendRequest", ex, "FailFriendManagementError");
            }
            catch (EntityException ex)
            {
                LogAndWrapException("SendFriendRequest", ex, "FailFriendManagementError");
            }
            return result;
        }

        public int DeleteFriendList(int friendRequestId)
        {
            int result = 0;
            try
            {
                using (var context = new LabyrinthEntities())
                {
                    var newFriendRequest = context.FriendList.FirstOrDefault(request => request.idFriendList == friendRequestId);

                    if (newFriendRequest != null)
                    {
                        context.FriendList.Remove(newFriendRequest);
                        result = context.SaveChanges();
                    }
                }
            }
            catch (DbUpdateException ex)
            {
                LogAndWrapException("SendFriendRequest", ex, "FailFriendManagementError");
            }
            catch (EntityException ex)
            {
                LogAndWrapException("SendFriendRequest", ex, "FailFriendManagementError");
            }
            return result;
        }
    }
}
