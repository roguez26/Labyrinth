using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using DataAccess;
using CatalogManagementService;
using System.Security.Cryptography;
using TransferUser = LabyrinthCommon.TransferUser;
using LabyrinthCommon;

namespace UserManagementService
{
    /// <summary>
    /// Define los contratos para la gestión de usuarios dentro del sistema.
    /// </summary>
    [ServiceContract]
    public interface IUserManagement
    {
        /// <summary>
        /// Obtiene el ranking de usuarios.
        /// </summary>
        /// <returns>Una lista de usuarios en el ranking.</returns>
        [OperationContract]
        TransferUser[] GetRanking();

        /// <summary>
        /// Agrega un nuevo usuario al sistema.
        /// </summary>
        /// <param name="user">El usuario que se agregará.</param>
        /// <param name="password">La contraseña del usuario.</param>
        /// <returns>El ID del nuevo usuario.</returns>
        /// <exception cref="LabyrinthException">Lanzada cuando ocurre un error al agregar un usuario.</exception>
        [OperationContract]
        [FaultContract(typeof(LabyrinthCommon.LabyrinthException))]
        int AddUser(TransferUser user, string password);

        /// <summary>
        /// Verifica las credenciales de un usuario.
        /// </summary>
        /// <param name="email">El correo electrónico del usuario.</param>
        /// <param name="password">La contraseña del usuario.</param>
        /// <returns>El usuario verificado si las credenciales son correctas, de lo contrario, null.</returns>
        /// <exception cref="LabyrinthException">Lanzada cuando ocurre un error al verificar al usuario.</exception>
        [OperationContract]
        [FaultContract(typeof(LabyrinthCommon.LabyrinthException))]
        TransferUser VerificateUser(string email, string password);

        /// <summary>
        /// Actualiza los datos de un usuario.
        /// </summary>
        /// <param name="newUser">El nuevo usuario con los datos actualizados.</param>
        /// <returns>El ID del usuario actualizado.</returns>
        /// <exception cref="LabyrinthException">Lanzada cuando ocurre un error al actualizar un usuario.</exception>
        [OperationContract]
        [FaultContract(typeof(LabyrinthCommon.LabyrinthException))]
        int UpdateUser(TransferUser newUser);

        /// <summary>
        /// Actualiza la contraseña de un usuario.
        /// </summary>
        /// <param name="pasword">La contraseña actual del usuario.</param>
        /// <param name="newPassword">La nueva contraseña del usuario.</param>
        /// <param name="email">El correo electrónico del usuario.</param>
        /// <returns>El ID del usuario cuya contraseña fue actualizada.</returns>
        /// <exception cref="LabyrinthException">Lanzada cuando ocurre un error al actualizar la contraseña.</exception>
        [OperationContract]
        [FaultContract(typeof(LabyrinthCommon.LabyrinthException))]
        int UpdatePassword(string password, string newPassword, string email);

        /// <summary>
        /// Elimina todos los usuarios del sistema.
        /// </summary>
        /// <returns>El número de usuarios eliminados.</returns>
        /// <exception cref="LabyrinthException">Lanzada cuando ocurre un error al eliminar los usuarios.</exception>
        [OperationContract]
        [FaultContract(typeof(LabyrinthCommon.LabyrinthException))]
        int DeleteAllUsers();

        /// <summary>
        /// Elimina un usuario del sistema.
        /// </summary>
        /// <param name="username">El nombre de usuario del usuario a eliminar.</param>
        /// <returns>El número de usuarios eliminados.</returns>
        /// <exception cref="LabyrinthException">Lanzada cuando ocurre un error al eliminar el usuario.</exception>
        [OperationContract]
        [FaultContract(typeof(LabyrinthCommon.LabyrinthException))]
        int DeleteUser(int userId);

        [OperationContract]
        [FaultContract(typeof(LabyrinthCommon.LabyrinthException))]
        int ChangeUserProfilePicture(int userId, byte[] imagenData);

        /// <summary>
        /// Verifica un código de verificación para un usuario.
        /// </summary>
        /// <param name="email">El correo electrónico del usuario.</param>
        /// <param name="code">El código de verificación a comprobar.</param>
        /// <returns>True si el código es válido, de lo contrario, false.</returns>
        [OperationContract]
        [FaultContract(typeof(LabyrinthCommon.LabyrinthException))]
        bool VerificateCode(string email, string code);

        /// <summary>
        /// Agrega un código de verificación para un usuario.
        /// </summary>
        /// <param name="email">El correo electrónico del usuario.</param>
        /// <param name="username">El nombre de usuario del usuario.</param>
        /// <returns>El ID del código de verificación agregado.</returns>
        /// <exception cref="LabyrinthException">Lanzada cuando ocurre un error al agregar el código de verificación.</exception>
        [OperationContract]
        [FaultContract(typeof(LabyrinthCommon.LabyrinthException))]
        int AddVerificationCode(string email, string username);

        /// <summary>
        /// Verifica si un correo electrónico ya está registrado en el sistema.
        /// </summary>
        /// <param name="email">El correo electrónico a verificar.</param>
        /// <returns>True si el correo electrónico está registrado, de lo contrario, false.</returns>
        [OperationContract]
        [FaultContract(typeof(LabyrinthCommon.LabyrinthException))]
        bool IsEmailRegistered(string email);

        [OperationContract]
        int DeleteVerificationCode(string email);
    }
}
