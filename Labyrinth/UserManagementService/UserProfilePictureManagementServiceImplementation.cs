using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace UserManagementService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Reentrant)]

    public class UserProfilePictureManagementServiceImplementation : IUserProfilePictureManagementService
    {
        public void GetUserProfilePicture(int userId, string path)
        {
            IUserProfilePictureManagementServiceCallback callback = OperationContext.Current.GetCallbackChannel<IUserProfilePictureManagementServiceCallback>();

            if (userId > 0 && !string.IsNullOrEmpty(path) && callback != null) 
            {
                if (!string.IsNullOrEmpty(path) && File.Exists(path))
                {
                    callback.ReceiveProfilePicture(userId, File.ReadAllBytes(path));
                }
            }
        }
    }
}
