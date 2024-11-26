using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace UserManagementService
{
    [ServiceContract]
    public interface IUserManagementServiceCallback
    {
        [OperationContract]
        void ReceiveProfilePicture(int userId, byte[] dataImage);
    }
}
