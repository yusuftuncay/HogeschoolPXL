using HogeschoolPXL.Models.ViewModels;
using Microsoft.AspNetCore.Razor.Runtime.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace HogeschoolPXL.TagHelpers
{
    // You may need to install the Microsoft.AspNetCore.Razor.Runtime package into your project
    [HtmlTargetElement("student-card")]
    public class StudentCardTagHelper : TagHelper
    {
        public StudentCard StudentCardViewModel { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (StudentCardViewModel.InschrijvingId == 0)
            {
                string content = $@"<div>";
                content += $@"<h2 class='card-student-empty'>Geen Inschrijvingen</h2>";

                output.TagName = "div";
                output.Content.SetHtmlContent(content);
            }
            else
            {
                string content = $@"<div>";
                content += $@"<h4 class='card-student-name'>{StudentCardViewModel.VoorNaam} {StudentCardViewModel.Naam} | {StudentCardViewModel.Email}</h4>";
                for (int i = 0; i < StudentCardViewModel.InschrijvingId; i++)
                {
                    content += $@"<div class='card p-3 m-1'>";
                    content += $@"<span class='card-vak-{i}'><strong>Vak</strong> {StudentCardViewModel.Vak[i]}</span>";
                    content += $@"<span class='card-academiejaar-{i}'><strong>AcademieJaar</strong> {StudentCardViewModel.AcademieJaar[i].ToShortDateString()}</span>";
                    content += $@"</div>";
                }
                output.TagName = "div";
                output.Content.SetHtmlContent(content);
            }
        }
    }
}
