using System.Collections.Generic;
using System.ServiceModel;
using Web_Service.Mirror_Classes;

namespace Web_Service
{
    
    [ServiceContract]
    public interface IService
    {
        [OperationContract]
        string GetData(int value);

        [OperationContract]
        Empleado LogIn(string email, string password);

        [OperationContract]
        void CreateUser(Empleado empleado);

        [OperationContract]
        void CreateDepartment(DepartamentoMirror depto);

        [OperationContract]
        void EditDepartment(DepartamentoMirror depto);

        [OperationContract]
        void CreateRol(RolesMirror rol);

        [OperationContract]
        List<DepartamentoMirror> LoadDepartments();

        [OperationContract]
        List<RolesMirror> LoadRoles();

        [OperationContract]
        List<string> LoadPermisosData();

        [OperationContract]
        void SaveRoleChanges(RolesMirror rolMirror);

        [OperationContract]
        Empleado LoadEmpleado(int talentoHumano);

        [OperationContract]
        List<UserMirror> LoadUsers(string sessionUser);

    }

}
