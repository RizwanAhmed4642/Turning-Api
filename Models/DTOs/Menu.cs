using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Meeting_App.Models.DTOs
{

    public partial class Json
    {
        [JsonProperty("menu")]
        public List<Menu> Menu { get; set; }
    }

    public partial class Menu
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("order")]

        public long Order { get; set; }

        [JsonProperty("submenu")]
        public List<MenuSubmenu> Submenu { get; set; }
    }

    public partial class MenuSubmenu
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("submenu", NullValueHandling = NullValueHandling.Ignore)]
        public List<SubmenuSubmenu> Submenu { get; set; }
    }

    public partial class SubmenuSubmenu
    {
        [JsonProperty("title")]
        public string Title { get; set; }
    }

    
}
