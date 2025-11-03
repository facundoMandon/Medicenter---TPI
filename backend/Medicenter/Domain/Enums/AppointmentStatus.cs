using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enums
{
    public enum AppointmentStatus
    {
        Requested,      // Solicitado (pedirTurno)
        Confirmed,      // Confirmado (confirmarTurno)
        Accepted,       // Aceptado por profesional (aceptarTurno)
        Rejected,       // Rechazado por profesional (rechazarTurno)
        Cancelled       // Cancelado (cancelarTurno) -> por cualquier involucrado
    }
}
