using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Calculo.Pages
{
    /// <summary>
    /// PageModel
    /// </summary>
    public class PrivacyModel : PageModel
    {
        private readonly ILogger<PrivacyModel> _logger;
        /// <summary>
        /// logger
        /// </summary>
        /// <param name="logger"></param>
        public PrivacyModel(ILogger<PrivacyModel> logger)
        {
            _logger = logger;
        }
        /// <summary>
        /// Onget
        /// </summary>
        public void OnGet()
        {
        }
    }
}
