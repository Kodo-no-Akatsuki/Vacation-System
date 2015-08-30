using System.Runtime.Serialization;

namespace Web_Service
{
    [DataContract]
    public enum PermisosEnum
    {
        [DataMember]
        CrearDepartamento = 1,

        [DataMember]
        EditarDepartamento = 2,

        [DataMember]
        DesactivarDepartamento = 3,

        [DataMember]
        CrearRol = 4,

        [DataMember]
        EditarRol = 5,

        [DataMember]
        DesactivarRol = 6,

        [DataMember]
        CrearUsuario = 7,

        [DataMember]
        EditarUsuario = 8,

        [DataMember]
        DesactivarUsuario = 9,

        [DataMember]
        VerPerfiles = 10,

        [DataMember]
        AdministrarPersonal = 11,

        [DataMember]
        DesactivarPermisosOUsuarios = 12,

        [DataMember]
        ProgramarVacaciones = 13
    }
}