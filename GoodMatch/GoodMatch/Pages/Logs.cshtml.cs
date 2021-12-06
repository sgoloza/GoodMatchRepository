using GoodMatch.GoodMatchHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GoodMatch.Pages
{
    public class LogsModel : PageModel
    {
        public List<string> listOfLogs;
        public void OnGet()
        {
            listOfLogs = FunctionHelpers.getLogs();
        }
    }
}
