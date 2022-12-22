using HogeschoolPXL.Models.ViewModels;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace HogeschoolPXL.TagHelpers
{
    // You may need to install the Microsoft.AspNetCore.Razor.Runtime package into your project
    [HtmlTargetElement("student-card")]
    public class StudentCardTagHelper : TagHelper
    {
        public StudentCardViewModel StudentCardViewModel { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (StudentCardViewModel.InschrijvingId == 0)
            {
                string content = $@"<div>";
                content += $@"<h4 class='card-student-empty d-flex justify-content-center'>Geen Inschrijvingen</h4>";

                output.TagName = "div";
                output.Content.SetHtmlContent(content);
            }
            else
            {
                string content = $@"<div class='d-flex flex-column' style='margin:auto;width:50%;'>";
                content += $@"<h4 class='card-student-name text-center'>{StudentCardViewModel.VoorNaam} {StudentCardViewModel.Naam}</h4>";
                content += $@"<h4 class='card-student-email text-center'>{StudentCardViewModel.Email}</h4>";
                for (int i = 0; i < StudentCardViewModel.InschrijvingId; i++)
                {
                    content += $@"<div class='card p-3 m-1 text-center border-2'>";
                        content += $@"<span class='card-info'>Academiejaar: {StudentCardViewModel.Academiejaar[i].ToShortDateString()}</span>";
                        content += $@"<span class='card-info'>Vak: {StudentCardViewModel.Vak[i]}</span>";
                    content += $@"</div>";
                }
                output.TagName = "div";
                output.Content.SetHtmlContent(content);
            }
        }
    }
}
