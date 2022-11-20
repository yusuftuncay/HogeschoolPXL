using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace HogeschoolPXL.CustomModelValidation
{
    public class CustomHandboek : Attribute, IModelValidator
    {
        public IEnumerable<ModelValidationResult> Validate(ModelValidationContext context)
        {
            var maxDate = DateTime.Parse("1/1/" + DateTime.Now.Date.Year); /* 1.1.2022 */
            var minDate = DateTime.Parse("1/1/1980");
            var dtm = DateTime.Now;

            var lst = new List<ModelValidationResult>();

            if (DateTime.TryParse(context.Model.ToString(), out dtm))
            {
                if (dtm < maxDate)
                {
                    lst.Add(new ModelValidationResult("", "UitgifteDatum kan niet in de toekomst zijn"));
                }
                else if (dtm > minDate)
                {
                    lst.Add(new ModelValidationResult("", "UitgifteDatum kan niet voor 1/1/2022 zijn"));
                }
            }
            else
            { 
                lst.Add(new ModelValidationResult("", "Geen geldig UitgifteDatum"));
            }
            return lst;
        }
    }
}
