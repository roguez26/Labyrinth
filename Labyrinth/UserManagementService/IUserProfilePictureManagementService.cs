using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace UserManagementService
{
    [ServiceContract(CallbackContract = typeof(IUserProfilePictureManagementServiceCallback))]

    public interface IUserProfilePictureManagementService
    {
        [OperationContract(IsOneWay = true)]
        void GetUserProfilePicture(int userId, string path);

    }
}
