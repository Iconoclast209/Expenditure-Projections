using System;
using System.Collections.Generic;
using System.IO;

namespace Expenditure_Projector
{
    class Program
    {
        static void Main(string[] args)
        {
            #region FIELDS
            StreamReader input = null;
            List<string> lines = new List<string>();
            string line = null;
            string columnNames = null;
            string inputFilename = null;
            string outputFilename = null;
            int remainingPayPeriods = 0;
            int remainingMonths = 0;
            #endregion

            Console.WriteLine("Enter input filename including file extension:  ");
            inputFilename = Console.ReadLine();
            Console.WriteLine("Enter output filename including file extension:  ");
            outputFilename = Console.ReadLine();
            Console.WriteLine("Enter Remaining Pay Periods:  ");
            remainingPayPeriods = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter Remaining Months:  ");
            remainingMonths = int.Parse(Console.ReadLine());
            Console.WriteLine();
            Console.WriteLine();


            try
            {
                //Open the CSV file
                input = File.OpenText(inputFilename);
                Console.WriteLine("CSV file opened.");
                //Read the first line
                string columnTitles = input.ReadLine();
                columnNames = ',' + columnTitles;
                Console.WriteLine("First line read");

                //Continue to read the lines of data

                line = input.ReadLine();

                while (line != null)
                {
                    Console.WriteLine("Read line: " + line);
                    lines.Add(line);
                    line = input.ReadLine();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                if (input != null)
                {
                    input.Close();
                    Console.WriteLine("Input File closed.");
                }
            }

            //lines list now contains all of the lines except for the column titles
            List<string> newList = new List<string>();
            for (int i = 0; i < lines.Count; i++)
            {
                //For each line, find the expense type (third column), budget (fifth column), 
                //and YTD Expenditures (sixth column)
                string tempString = lines[i];
                
                //Remove the first two columns from the tempString
                for (int j=0; j<2; j++)
                {
                    int comma = tempString.IndexOf(',');
                    tempString = tempString.Substring(comma + 1);
                }
                //Grab out the expense type code
                int expenseType = int.Parse(tempString.Substring(0, 5));
                Console.WriteLine(expenseType);

                //Remove two more columns, leaving only budget and YTD expend
                for (int j = 0; j < 2; j++)
                {
                    int comma = tempString.IndexOf(',');
                    tempString = tempString.Substring(comma + 1);
                }
                Console.WriteLine(tempString);
                
                //Find the index of the comma that separates budget and YTD expend
                int budgetComma = tempString.IndexOf(',');
                //Set the budget as an float
                float budget = float.Parse(tempString.Substring(0, budgetComma));
                Console.WriteLine("Budget is " + budget.ToString());
                //Remove budget from the temp string
                tempString = tempString.Substring(budgetComma + 1);
                
                //Set the YTD expenditures as a float
                float ytdExpenditures = float.Parse(tempString);
                Console.WriteLine("YTD Expenditures are " + ytdExpenditures.ToString());

                float projected = 0;

                if (expenseType == 42000 || expenseType == 42500)
                {
                    //These are personnel expenses and should be projected based on remaining payrolls
                    projected = (ytdExpenditures / (26 - remainingPayPeriods)) * remainingPayPeriods;
                    Console.WriteLine("Projected Expenditures are:  " + projected.ToString());
                }
                else
                {
                    //These are services and supplies or capital improvements
                    projected = (ytdExpenditures / (12 - remainingMonths)) * remainingMonths;
                    Console.WriteLine("Projected Expenditures are:  " + projected.ToString());
                }
                //Grab the budget type

                /*
                //Add a comma to each line before the first space
                int location = lines[i].IndexOf(' ');
                Console.WriteLine("The first space is located at index " + location);
                string str1 = lines[i].Substring(0, location);
                Console.WriteLine("The first substring is:" + str1);
                string str2 = lines[i].Substring(location + 1);
                Console.WriteLine("The second substring is:" + str2);
                string newString = str1 + ',' + str2;
                Console.WriteLine("The newString is:" + newString);
                newList.Add(newString);
                */
            }


            //Add "Projected Expenditures" to the columnNames
            columnNames += ',' + "Projected Expenditures";

            //Output all of the values in the newList back into a file.

            /*try
            {
                using (StreamWriter writer = new StreamWriter(outputFilename))
                {
                    writer.WriteLine(columnNames);

                    for (int i = 0; i < newList.Count; i++)
                    {
                        writer.WriteLine(newList[i]);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }*/

        }
    }
}
