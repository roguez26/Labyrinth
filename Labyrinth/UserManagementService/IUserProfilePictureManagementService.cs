using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace UserManagementService
{
    /// <summary>
    /// Define los contratos para la gestión de las imágenes de perfil de los usuarios.
    /// </summary>
    [ServiceContract(CallbackContract = typeof(IUserProfilePictureManagementServiceCallback))]
    public interface IUserProfilePictureManagementService
    {
        /// <summary>
        /// Recupera la imagen de perfil de un usuario.
        /// </summary>
        /// <param name="userId">El ID del usuario cuya imagen de perfil se quiere obtener.</param>
        /// <param name="path">El path donde se almacenará o accederá la imagen de perfil del usuario.</param>
        [OperationContract(IsOneWay = true)]
        void GetUserProfilePicture(int userId, string path);
    }
}
