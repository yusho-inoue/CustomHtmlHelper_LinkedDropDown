using HtmlAgilityPack;
using LinkedDropdownExample.ExtensionModels;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq.Expressions;
using System.Text.Encodings.Web;

#nullable disable
namespace LinkedDropdownExample.Extensions
{
    public static class DropDownListForExtensions
    {
        /// <summary>
        /// 親ドロップダウンと連動するドロップダウンを作成する
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <param name="selectList"></param>
        /// <param name="masterExpression"></param>
        /// <returns></returns>
        public static IHtmlContent LinkedDropDownListFor<TModel>(
            this IHtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, int?>> expression,
            IEnumerable<LinkedSelectListItem> selectList,
            Expression<Func<TModel, int?>> masterExpression)
        {
            // デフォルトのDropDownListForを呼び出してドロップダウン用のタグの枠組みを取得してhtmlDodumentに読み込む
            var dropdown = htmlHelper.DropDownListFor(expression, selectList, "選択してください", htmlAttributes: new { }) as TagBuilder;
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(GetHtmlString(dropdown!.InnerHtml));

            // ドロップダウンのタグからoptionタグ（選択肢の部分）を取得し、key属性を追加する
            var options = htmlDocument.DocumentNode.SelectNodes("//option");
            foreach (var option in options)
            {
                var value = option.Attributes["value"]?.Value;
                option.Attributes.Add("data-key", selectList.FirstOrDefault(e => e.Value == value)?.Key ?? "default");
            }

            // key属性を付与したoptionタグでドロップダウンの中身を入れ替え
            dropdown.InnerHtml.Clear();
            dropdown.InnerHtml.AppendHtml(htmlDocument.DocumentNode.OuterHtml);

            // ドロップダウンのidを取得する
            var masterDropdown = htmlHelper.DropDownListFor(masterExpression, selectList, htmlAttributes: new { }) as TagBuilder;
            var masterDropdownId = masterDropdown.Attributes["id"];
            var slaveDropdownId = dropdown.Attributes["id"];

            // ドロップダウンを連動させるJavaScript
            var scriptTag = new TagBuilder("script");
            var script =
                @"
                    const masterDropdown_" + masterDropdownId + @" = document.getElementById('" + masterDropdownId + @"');
                    const slaveDropdown_" + slaveDropdownId + @" = document.getElementById('" + slaveDropdownId + @"');
                    window.addEventListener('DOMContentLoaded', () => {
                        let masterDropdown = masterDropdown_" + masterDropdownId + @";
                        let slaveDropdown = slaveDropdown_" + slaveDropdownId + @";
                        let options = slaveDropdown.children;
                        masterDropdown.addEventListener('change', (e) => {
                            let parentValue = masterDropdown.value;
                            let slaveValue = slaveDropdown.value;

                            if (parentValue) {
                                slaveDropdown.removeAttribute('disabled');

                                Array.from(options).forEach(elm => {
                                    if(elm.tagName.toLowerCase() == 'span') {
                                        let option = elm.firstElementChild;                                    
                                        elm.parentNode.insertBefore(option, elm);
                                        elm.parentNode.removeChild(elm);
                                        elm = option;
                                    }

                                    let key = elm.dataset.key;
                                    if (key == parentValue || key == 'default') {
                                        elm.style.display = 'block';
                                    }
                                    else {
                                        let value = elm.attributes['value'].value;
                                        if (value == slaveValue) {
                                            slaveDropdown.value = '';
                                        }
                                        let span = document.createElement('span');
                                        span.style.display = 'none';
                                        elm.parentNode.insertBefore(span, elm);
                                        span.appendChild(elm);
                                    }
                                });
                            }
                            else {
                                slaveDropdown.value = '';
                                slaveDropdown.setAttribute('disabled', 'disabled');
                            }
                        });
                        let change = new Event('change');
                        masterDropdown.dispatchEvent(change);
                    });
                ";
            scriptTag.InnerHtml.AppendHtml(script);

            // ドロップダウンとJavaScriptをdivタグに入れる
            var outerDiv = new TagBuilder("div");
            outerDiv.Attributes.Add("style", "display: inline-block");
            outerDiv.InnerHtml.AppendHtml(dropdown);
            outerDiv.InnerHtml.AppendHtml(scriptTag);
            return outerDiv;
        }

        private static string GetHtmlString(IHtmlContent htmlContent)
        {
            using (var writer = new System.IO.StringWriter())
            {
                htmlContent.WriteTo(writer, HtmlEncoder.Default);
                return writer.ToString();
            }
        }
    }
}
