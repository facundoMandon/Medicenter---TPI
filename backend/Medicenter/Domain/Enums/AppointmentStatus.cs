using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enums
{
    public enum AppointmentStatus
    {
        Requested, // Solicitado por el paciente, pendiente de revisión
        Confirmed, // Aceptado por el profesional
        Rejected,  // Rechazado por el profesional
        CancelledByPatient, // Cancelado por el paciente
        CancelledByProfessional // Cancelado por el profesional
    }
}
