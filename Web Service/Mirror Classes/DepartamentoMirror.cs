using System.Runtime.Serialization;

namespace Web_Service.Mirror_Classes
{
    [DataContract]
    public class DepartamentoMirror
    {
        [DataMember]
        public int DepartamentoId { get; set; }

        [DataMember]
        public string Descripcion { get; set; }

        [DataMember]
        public bool Activo { get; set; }
    }
}
