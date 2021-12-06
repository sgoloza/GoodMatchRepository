using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GoodMatch.GoodMatchHelper
{
    public class FunctionHelpers
    {
        // Count each Character frequency and returning the result as a string 
        public static async Task<string> CountCharOccurances(string Player1, string Player2)
        {
            try
            {
                string matchString = Player1.ToLower() + "matches" + Player2.ToLower();
                string resultFromMatch = string.Empty;
                Dictionary<char, int> characterOccurances = new Dictionary<char, int>();

                foreach (char myChar in matchString)
                {
                    if (characterOccurances.ContainsKey(myChar))
                    {
                        characterOccurances[myChar] = characterOccurances[myChar] + 1;
                    }
                    else
                    {
                        characterOccurances[myChar] = 1;
                    }
                }

                // Get all the frequencies from the dictionary
                foreach (KeyValuePair<char, int> myChar in characterOccurances)
                {
                    resultFromMatch += myChar.Value.ToString();
                }

                return resultFromMatch;
            }
            catch (Exception ex)
            {
                
                await writesLogs("Error:" + ex.Message);
                return string.Empty;
            }

        }
        // This is a recursive fuction, It will only break if the length occurances of characters is equal to two.
        public static async Task<string> getPercentage(string occurancesOfChars)
        {
            try
            {
                if (occurancesOfChars.Length == 2)
                {
                    return occurancesOfChars;
                }

                string _countChars = "";
                char[] strArray = occurancesOfChars.ToArray();
                int len = 0;
                
                if (strArray.Length % 2 == 0)
                {
                    len = (strArray.Length / 2);
                }
                else
                {
                    len = Convert.ToInt32((strArray.Length / 2));
                }

                for (int i = 0; i < len; i++)
                {
                    if (!(strArray.Length % 2 == 0) && (i + 1 == len))
                    {
                        char leftMostNumber = strArray[len - 1];
                        char rightMostNumber = strArray[len + 1];
                        _countChars += (Convert.ToInt32(leftMostNumber.ToString()) + Convert.ToInt32(rightMostNumber.ToString()));
                        char middleNumber = strArray[len];
                        _countChars += (Convert.ToInt32(middleNumber.ToString()));
                        break;
                    }
                    else
                    {
                        char leftMostNumber = strArray[i];
                        char rightMostNumber = strArray[strArray.Length - 1 - i];

                        _countChars += (Convert.ToInt32(leftMostNumber.ToString()) + Convert.ToInt32(rightMostNumber.ToString()));
                    }
                }
                return getPercentage(_countChars).Result;
            }
            catch (Exception ex)
            {
                await writesLogs("Error:" + ex.Message);
                return string.Empty;
            }

        }
        // Checking if player names consist of only alphabets
        public static async Task<bool> containsOnlyAlphabets(string Player1name, string Player2name)
        {
            try
            {
                return (Player1name.All(mychar => Char.IsLetter(mychar)) && Player2name.All(mychar => Char.IsLetter(mychar)));
            }
            catch (Exception ex)
            {
                await writesLogs("Error:" + ex.Message);
                return false;
            }
        }
        public static async Task<string> writesResultsIntoFile(string Player1Name, string Player2Name, string Percent)
        {
            try
            {
                string macthResult = string.Empty;
                if (Convert.ToInt32(Percent) >= 80)
                {
                    macthResult = Player1Name + " matches " + Player2Name + " " + Percent + "%, good match";
                }
                else
                {
                    macthResult = Player1Name + " matches " + Player2Name + " " + Percent + "%";
                }
                using StreamWriter file = new StreamWriter("output.txt", append: true);
                await file.WriteLineAsync(macthResult);
                return macthResult;
            }
            catch (Exception ex)
            {
                await writesLogs("Error:" + ex.Message);
                return string.Empty;
            }
        }

        // Reading player names from a csv file and group them using gender 
        public static async Task<Dictionary<string, List<string>>> readCSVFile()
        {
            try
            {
                Dictionary<string, List<string>> groupOfPlayers = new Dictionary<string, List<string>>();
                groupOfPlayers["Boys"] = new List<string>();
                groupOfPlayers["Girls"] = new List<string>();
                using (StreamReader reader = new StreamReader("playerNames.csv"))
                {
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        string[] values = line.Split(',');
                        if (values[1].ToLower().Trim().Equals("m"))
                        {
                            // Chenking if the name already exist
                            if (!groupOfPlayers["Boys"].Contains(values[0].Trim()))
                            {
                                groupOfPlayers["Boys"].Add(values[0].Trim());
                            }
                            else {
                                await writesLogs(" Duplicate found: "+ line);
                            }
                                

                        }
                        else
                        {
                            // Chenking if the name already exist
                            if (!groupOfPlayers["Girls"].Contains(values[0].Trim()))
                                groupOfPlayers["Girls"].Add(values[0].Trim());
                        }
                    }
                }
                return groupOfPlayers;
            }
            catch (Exception ex)
            {
                await writesLogs("Error:" + ex.Message);
                return new Dictionary<string, List<string>>();
            }
        }

        // Writing logs into a text file called logs.txt
        public static async Task writesLogs(string log)
        {
            try
            {
                using StreamWriter file = new StreamWriter("logs.txt", append: true);
                await file.WriteLineAsync(DateTime.Now+"-"+log);
            }
            catch (Exception ex)
            {

                await writesLogs("Error:" + ex.Message);

            }
        }
        // Sorting result from a text file
        public static IOrderedEnumerable<KeyValuePair<int, List<string>>> ReadingResultsFile()
        {
            Dictionary<int, List<string>> resultlist = new Dictionary<int, List<string>>();
            FileStream fileStream = new FileStream("output.txt", FileMode.Open, FileAccess.Read);
            using (StreamReader streamReader = new StreamReader(fileStream, Encoding.UTF8))
            {
                string line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    string[] token = line.Split(' ');
                    //Chenking if the pecentage already exist
                    if (resultlist.ContainsKey(Convert.ToInt32(token[3].Trim().Substring(0, 2))))
                    {
                        // If the percentage already exist I add only Results to the list 
                        string myKey = token[3].Trim().Substring(0, 2);
                        resultlist[Convert.ToInt32(myKey)].Add(line);
                        //Sorting the list result alpabetically as the percentage is the same
                        resultlist[Convert.ToInt32(myKey)].Sort();
                    }
                    else
                    {
                        string myKey = token[3].Trim().Substring(0, 2);
                        resultlist[Convert.ToInt32(myKey)] = new List<string>();
                        resultlist[Convert.ToInt32(myKey)].Add(line);
                    }
                }
            }
            // Sorting the Result  
            return resultlist.OrderByDescending(key => key.Key);
        }
        public static async Task WrittingSortedResults(IOrderedEnumerable<KeyValuePair<int, List<string>>> sortedResultDictionary) 
        {
            try 
            {
                ClearFile();
                using StreamWriter file = new StreamWriter("output.txt", append: true);
                foreach (KeyValuePair<int, List<string>> results in sortedResultDictionary)
                {
                    foreach (string result in results.Value)
                    {
                        await file.WriteLineAsync(result);
                    }
                }
                
            } catch ( Exception ex) {
                await writesLogs("Error:" + ex.Message);
            }
        
        }

        public static List<string> getLogs() {

            List<string> listOsLogs = new List<string>();
            FileStream fileStream = new FileStream("logs.txt", FileMode.Open, FileAccess.Read);
            using (StreamReader streamReader = new StreamReader(fileStream, Encoding.UTF8))
            {
                string line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    listOsLogs.Add(line);
                }
            }
            return listOsLogs;

         }
        private static void ClearFile()
        {
            if (!File.Exists("output.txt"))
                File.Create("output.txt");

            TextWriter tw = new StreamWriter("output.txt", false);
            tw.Write("");
            tw.Close();
        }
    }
}
