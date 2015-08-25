using System.Runtime.Serialization;

namespace Web_Service.Mirror_Classes
{
    [DataContract]
    public class RolesMirror
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string rolname { get; set; }

        [DataMember]
        public bool Activo { get; set; }
    }
}
