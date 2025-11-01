using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IHolidaysService
    {
        Task<string> GetHolidaysAsync(string country, string year, string? month = null, string? day = null);
    }
}

