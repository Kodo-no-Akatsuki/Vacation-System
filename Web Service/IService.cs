using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
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
        void CreateRol(RolesMirror rol);

        [OperationContract]
        List<DepartamentoMirror> LoadDepartments();

        [OperationContract]
        List<RolesMirror> LoadRoles();

    }

}
