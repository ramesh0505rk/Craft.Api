using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Craft.Application.ResponseDTOs
{
    public class GetUserPreferencesDTO : BaseDTO
    {
        public bool DarkTheme { get; set; }
        public bool LightTheme { get; set; }
        public bool EnableNotification { get; set; }
    }
}
