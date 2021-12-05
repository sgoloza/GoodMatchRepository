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
        // This is a recursive fuction, It will only break if the occurancesOfChars.length is equal to two.
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
                            if (!groupOfPlayers["Boys"].Contains(values[0].Trim()))
                                groupOfPlayers["Boys"].Add(values[0].Trim());

                        }
                        else
                        {
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
        public static async Task writesLogs(string log)
        {
            try
            {
                using StreamWriter file = new StreamWriter("logs.txt", append: true);
                await file.WriteLineAsync(log);
            }
            catch (Exception ex)
            {

                await writesLogs("Error:" + ex.Message);

            }
        }

        public static IOrderedEnumerable<KeyValuePair<int, List<string>>> ReadOutPutFile()
        {
            Dictionary<int, List<string>> resultlist = new Dictionary<int, List<string>>();
            FileStream fileStream = new FileStream("output.txt", FileMode.Open, FileAccess.Read);
            using (StreamReader streamReader = new StreamReader(fileStream, Encoding.UTF8))
            {
                string line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    string[] token = line.Split(' ');
                    if (resultlist.ContainsKey(Convert.ToInt32(token[3].Trim().Substring(0, 2))))
                    {
                        string myKey = token[3].Trim().Substring(0, 2);
                        resultlist[Convert.ToInt32(myKey)].Add(line);
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
            return resultlist.OrderByDescending(key => key.Key);
        }
    }
}
