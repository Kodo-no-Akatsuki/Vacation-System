using System;
using System.Runtime.Serialization;

namespace Web_Service.Mirror_Classes
{
    [DataContract]
    public class UserMirror
    {
        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public string Password { get; set; }

        [DataMember]
        public string PrimerNombre { get; set; }

        [DataMember]
        public string SegundoNombre { get; set; }

        [DataMember]
        public string PrimerApellido { get; set; }

        [DataMember]
        public string SegundoApellido { get; set; }

        [DataMember]
        public DateTime FechaIngreso { get; set; }

        [DataMember]
        public DateTime FechaCreacion { get; set; }

        [DataMember]
        public bool Activo { get; set; }

        [DataMember]
        public int TalentoHumano { get; set; }

        public UserMirror(Usuarios user)
        {
            Email = user.email;
            Activo = user.activo;
            FechaCreacion = user.fecha_creacion;
            FechaIngreso = user.fecha_ingreso;
            Password = user.password;
            PrimerApellido = user.primer_apellido;
            PrimerNombre = user.primer_nombre;
            SegundoApellido = user.segundo_apellido;
            SegundoNombre = user.segundo_nombre;
            TalentoHumano = user.talento_humano;
        }
    }
}
