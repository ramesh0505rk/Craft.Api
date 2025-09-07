using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Craft.Domain.Entities
{
    public class GetUserPreferencesResponse
    {
        public bool DarkTheme { get; set; }
        public bool LightTheme { get; set; }
        public bool EnableNotification { get; set; }
    }
}
