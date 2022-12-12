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
                if (dtm > maxDate)
                {
                    lst.Add(new ModelValidationResult("", "UitgifteDatum can't be after 1/1/2022"));
                }
                else if (dtm < minDate)
                {
                    lst.Add(new ModelValidationResult("", "UitgifteDatum can't be before 1980 zijn"));
                }
            }
            else
            { 
                lst.Add(new ModelValidationResult("", "Not a valid UitgifteDatum"));
            }
            return lst;
        }
    }
}
