using Microsoft.AspNetCore.Mvc.Rendering;

namespace LinkedDropdownExample.ExtensionModels
{
    /// <summary>
    /// 連動ドロップダウン用のItemモデル
    /// </summary>
    public class LinkedSelectListItem : SelectListItem
    {
        public LinkedSelectListItem(string text, string value, string key) : base(text, value)
        {
            Key = key;
        }

        public string Key { get; set; }
    }
}
