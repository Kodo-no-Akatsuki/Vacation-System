using System.Runtime.Serialization;

namespace Web_Service
{
    [DataContract]
    public enum PermisosEnum
    {
        [DataMember]
        CrearDepartamento,

        [DataMember]
        EditarDepartamento,

        [DataMember]
        DesactivarDepartamento,

        [DataMember]
        CrearRol,

        [DataMember]
        EditarRol,

        [DataMember]
        DesactivarRol,

        [DataMember]
        CrearUsuario,

        [DataMember]
        EditarUsuario,

        [DataMember]
        DesactivarUsuario,

        [DataMember]
        VerPerfiles,

        [DataMember]
        AdministrarPersonal,

        [DataMember]
        DesactivarPermisosOUsuarios,

        [DataMember]
        ProgramarVacaciones
    }
}