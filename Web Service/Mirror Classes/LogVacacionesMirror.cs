using System;

namespace Web_Service.Mirror_Classes
{
    public class LogVacacionesMirror
    {
        public int LogId { get; set; }
        public int VacacionesId { get; set; }
        public int Modifico { get; set; }
        public int EstatusAnterior { get; set; }
        public int EstatusActual { get; set; }
        public DateTime FechaModificacion { get; set; }
    }
}
