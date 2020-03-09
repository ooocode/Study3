using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Study.Website.TagHelpers
{
    public class Box3TagHelper : TagHelper
    {
        /// <summary>
        /// Box的标题
        /// </summary>
        public string Title { get; set; }

        public string ClassName { get; set; }


        public override void Process(TagHelperContext context, TagHelperOutput output)
        {

        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = null;    // Replaces <email> with <a> tag

            var childContent = await output.GetChildContentAsync();
            var content = childContent.GetContent() ?? "";

            if (string.IsNullOrEmpty(Title))
            {
                output.Content.SetHtmlContent(
              "<div class=\"box box-solid\">" +
                  "<div class=\"box-header with-border\">" +
                      "<div class=\"box-tools\">" +
                          "<button type = \"button\" class=\"btn btn-box-tool\" data-widget=\"collapse\"><i class=\"fa fa-minus\"></i>" +
                          "</button>" +
                       "</div>" +
                  "</div>" +
                  "<div class=\"box-body\">" +
                      $"{content}" +
                  "</div>" +
              "</div>"
        );
            }
            else
            {
                output.Content.SetHtmlContent(
             "<div class=\"box box-solid\">" +
                 "<div class=\"box-header with-border\">" +
                     $"<h3 class=\"box-title\">{Title}</h3>" +
                     "<div class=\"box-tools\">" +
                         "<button type = \"button\" class=\"btn btn-box-tool\" data-widget=\"collapse\"><i class=\"fa fa-minus\"></i>" +
                         "</button>" +
                      "</div>" +
                 "</div>" +
                 "<div class=\"box-body\">" +
                     $"{content}" +
                 "</div>" +
             "</div>"
       );
            }
         

            //            ClassName = ClassName ?? "";

            //            if (string.IsNullOrEmpty(Title))
            //            {
            //                output.Content.SetHtmlContent($"<div class=\"p-3 bg-white rounded shadow-sm {ClassName}\">" +
            //          "<div class=\"text-muted pt-3\">" +
            //            "  <div>" +
            //               $"{content}" +
            //              "</div>" +
            //          "</div>" +
            //      "</div>");
            //            }
            //            else
            //            {
            //                output.Content.SetHtmlContent($"<div class=\"p-3 bg-white rounded shadow-sm {ClassName}\">" +
            //$"<h6 class=\"border-bottom border-gray pb-2 mb-0\">{Title}</h6>" +
            //"<div class=\"text-muted pt-3\">" +
            //  "  <div>" +
            //     $"{content}" +
            //    "</div>" +
            //"</div>" +
            //"</div>");
            //            }
        }
    }
}
