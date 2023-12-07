using MagicVilla_Web.Models.Dto;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MagicVilla_Web.Models.VM
{
    public class VillaNumberCreateVM
    {
        public VillaNumberDTO VillaNumber { get; set; }

        public VillaNumberCreateVM()
        {
            VillaNumber = new VillaNumberDTO();
        }
        [ValidateNever]
        public IEnumerable<SelectListItem> VillaList { get; set; }
    }
}
