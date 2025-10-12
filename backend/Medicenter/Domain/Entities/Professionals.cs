using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Entities.Relations;

namespace Domain.Entities
{
    public class Professionals : Users
    {
        public int n_matricula { get; set; }

        // 1. Relación Uno-a-Muchos (1:N) con Turnos (Profesional tiene muchos Turnos)
        // Se usa ICollection para que EF Core maneje la lista.
        public ICollection<Appointments> Appointments { get; private set; } = new HashSet<Appointments>();

        // 2. Relación Muchos-a-Muchos (M:M) con Especialidad (a través de Entidad de Unión)
        // Esta colección contiene las entidades intermedias.
        public ICollection<ProfessionalSpecialty> EspecialidadesEnlazadas { get; private set; } = new HashSet<ProfessionalSpecialty>();

        // 3. Relación Muchos-a-Muchos (M:M) con Hospitales (a través de Entidad de Unión)
        public ICollection<ProfessionalHospital> HospitalesEnlazados { get; private set; } = new HashSet<ProfessionalHospital>();
    }
}
