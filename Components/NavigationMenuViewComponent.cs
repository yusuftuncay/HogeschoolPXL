using HogeschoolPXL.Data;
using Microsoft.AspNetCore.Mvc;

namespace HogeschoolPXL.Components
{
    public class NavigationMenuViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;
        public NavigationMenuViewComponent(AppDbContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke()
        {
            return View(_context.Inschrijving.Select(x => x.Student.Gebruiker.Voornaam)
                .Distinct().OrderBy(x => x));
        }
    }
}
