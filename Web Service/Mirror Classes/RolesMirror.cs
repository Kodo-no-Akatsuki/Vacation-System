using System.Runtime.Serialization;
using System.Collections.Generic;

namespace Web_Service.Mirror_Classes
{
    [DataContract]
    public class RolesMirror
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Descripcion { get; set; }

        [DataMember]
        public bool Activo { get; set; }

        [DataMember]
        public List<PermisosMirror> Permisos { get; set; }
    }
}
