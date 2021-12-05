using GoodMatch.GoodMatchHelper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoodMatch.Pages
{

    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public string Player1Name { get; set; }
        public string Player2Name { get; set; }
        public string singleMacthResult { get; set; }
        public bool isThereAnError { get; set; }
        public IOrderedEnumerable<KeyValuePair<int, List<string>>> sortedResultDictionary { get; set; }
        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet(bool error = false)
        {
            isThereAnError = error;
        }
        public async Task OnPost(string player1Name, string player2Name, string readFromFile)
        {
            try
            {
                if (player1Name != null && player2Name != null && FunctionHelpers.containsOnlyAlphabets(player1Name, player2Name).Result)
                {
                    string charOccurances = FunctionHelpers.CountCharOccurances(player1Name, player2Name).Result;
                    singleMacthResult = await FunctionHelpers.writesResultsIntoFile(player1Name, player2Name, FunctionHelpers.getPercentage(charOccurances).Result);
                    sortedResultDictionary = FunctionHelpers.ReadingResultFile();
                   await FunctionHelpers.WrittingSortedResults(sortedResultDictionary);
                    sortedResultDictionary = null;
                }
                else if (player1Name == null && player2Name == null)
                {
                    Dictionary<string, List<string>> groupOfPlayers = FunctionHelpers.readCSVFile().Result;
                    for (int i = 0; i < groupOfPlayers["Boys"].Count; i++)
                    {
                        foreach (string girlName in groupOfPlayers["Girls"])
                        {
                            if (FunctionHelpers.containsOnlyAlphabets(groupOfPlayers["Boys"][i], girlName).Result)
                            {
                                string charOccurances = FunctionHelpers.CountCharOccurances(groupOfPlayers["Boys"][i], girlName).Result;
                                await FunctionHelpers.writesResultsIntoFile(groupOfPlayers["Boys"][i], girlName, FunctionHelpers.getPercentage(charOccurances).Result);
                            }
                            else
                            {
                                await FunctionHelpers.writesLogs("Error: Invalid inputs between " + groupOfPlayers["Boys"][i] + " and " + girlName);
                                isThereAnError = true;
                            }
                        }
                    }
                    sortedResultDictionary = FunctionHelpers.ReadingResultFile();
                }
                else
                {
                    await FunctionHelpers.writesLogs("Error: Invalid inputs between " + player1Name + " and " + player2Name);
                    isThereAnError = true;
                }
            }
            catch (Exception ex)
            {
                await FunctionHelpers.writesLogs("Error:" + ex.Message);
                isThereAnError = true;
            }
        }
    }
}
