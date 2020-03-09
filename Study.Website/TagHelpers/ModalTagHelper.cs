using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Study.Website.TagHelpers
{
    /// <summary>
    /// 模态框 【setTitle】【function onModalShow(relatedTarget) {}】
    /// 示例
    /// ***********
    /// **onModalShow 必须放到modal里面
    /// ** 定义一个函数，用于执行模态框显示的时候所需要执行的动作（可以为空，命名必须是function onModalShow(relatedTarget)）
    /// ** relatedTarget 表面是哪个控件点击了模态框
    /// **
    /// ***********
    /// <modal id="modalTable" title="模态框标题">
    ///      <script>
    ///         function onModalShow(relatedTarget) {// }
    ///      </script>
    /// </modal>  
    ///***********
    /// </summary>
    public class ModalTagHelper : TagHelper
    {
        public string Id { get; set; }


        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }



        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (string.IsNullOrEmpty(Id))
            {
                throw new Exception("Taghelper Model Id不能为空");
            }

            output.TagName = string.Empty;    // Replaces <email> with <a> tag

            var childContent = (await output.GetChildContentAsync()).GetContent();

            string script = string.Empty;

            Regex regexJs = new Regex("(?<=<script(.)*?>)([\\s\\S](?!<script))*?(?=</script>)", RegexOptions.IgnoreCase);
            var math = regexJs.Match(childContent);
            if (math.Success)
            {
                if (math.Value.IndexOf("onModalShow") != -1)
                {
                    childContent = childContent.Replace("onModalShow", $"onModalShow_{Id}");
                    script = $"<script>" +
                               "window.onload = function() { " +
                                   $"$('#{Id}').on('shown.bs.modal', function(event) " +
                                       "{  var button = $(event.relatedTarget);" +
                                       $"onModalShow_{Id}(button)" +
                                   "})" +
                               "};" +
                               "    function setTitle(value){$('#title_"+Id+"').text(value)}"+
                           "</script>";
                }
            }



            output.Content.SetHtmlContent(
$"<div id=\"{Id}\" class=\"modal fade\" tabindex=\"- 1\" role=\"dialog\" >" +
    "<div class=\"modal-dialog\" role=\"document\">" +
       " <div class=\"modal-content\">" +
            "<div class=\"modal-header\">" +
                $"<h5 class=\"modal-title\" id='title_{Id}'>{Title}</h5>" +
                "<button type = \"button\" class=\"close\" data-dismiss=\"modal\" aria-label=\"Close\">" +
                   " <span aria-hidden=\"true\">&times;</span>" +
                "</button>" +
            "</div>" +
            "<div class=\"modal-body\">" +
                  $"{childContent}" +
                  $"{script}" +
            "</div>" +
            "<div class=\"modal-footer\">" +
                "<button type =\"button\" class=\"btn btn-secondary\" data-dismiss=\"modal\">关闭</button>" +
            "</div>" +
        "</div>" +
    "</div>" +
"</div>");

        
        }

    }
}
