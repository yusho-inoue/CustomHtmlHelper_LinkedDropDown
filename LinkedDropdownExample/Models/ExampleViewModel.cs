using LinkedDropdownExample.ExtensionModels;
using Microsoft.AspNetCore.Mvc.Rendering;

#nullable disable
namespace LinkedDropdownExample.Models
{
    public class ExampleViewModel
    {
        // ドロップダウンの選択値
        public IEnumerable<SelectListItem> RegionListItems { get; set; }
        public IEnumerable<LinkedSelectListItem> CountryListItems { get; set; }

        // ドロップダウンで選択された値
        public int? RegionValue { get; set; }
        public int? CountryValue { get; set;}
    }
}
