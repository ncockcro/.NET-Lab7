/*
 * Written by: Nicholas Cockcroft
 * Date: March 6, 2018
 * Course: .NET Environment
 * Assignment: Lab 7
 * 
 * Description: Write a console application which will allow the user to enter a directory
 * name and date as command line arguments and display all those files that have been
 * changed on or after a given date. The date format may be of your choosing. Make
 * this process handle all subdirectories no matter how many levels down you need
 * to go.
 * 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Lab7
{
    class lab7
    {
        static void Main(string[] args)
        {
            string directoryName;
            string yearDate;
            string monthDate;
            string dayDate;
            lab7 directoryObj = new lab7();

            // Prompt the user for a directory along with a specified date
            Console.Write("Enter a directory: ");
            directoryName = Console.ReadLine();

            Console.WriteLine("Enter a date to see the files modified after then: ");

            Console.Write("Year: ");
            yearDate = Console.ReadLine();
            Console.Write("Month: ");  
            monthDate = Console.ReadLine();
            Console.Write("Day: ");
            dayDate = Console.ReadLine();

            // Checking to make sure the date the user entered is an actual date
            if(directoryObj.checkDate(Int32.Parse(yearDate), Int32.Parse(monthDate), Int32.Parse(dayDate)) == false)
            {
                MessageBox.Show("Error: Invalid Date.");
                System.Environment.Exit(0);
            }
            
            // Checking to make sure the directory the user entered is an actual directory
            if(Directory.Exists(directoryName) == false)
            {
                MessageBox.Show("Error: That is not a directory.");
                System.Environment.Exit(0);
            }

            // Sending the user specified directory to get the sub directories
            Console.WriteLine("Here are the files before {0}/{1}/{2}", monthDate, dayDate, yearDate);
            directoryObj.getSubDirectories(directoryName, Int32.Parse(yearDate), Int32.Parse(monthDate), Int32.Parse(dayDate));
        }

        // Returns true if the date the user entered in is valid, false otherwise
        private bool checkDate(int a_year, int a_month, int a_day)
        {
            bool status = true;
            int numDays = 100;
            if (a_month > 0 && a_month < 13)
            {
                numDays = GetNumDaysMonth(a_year, a_month, a_day);
            }

            if(a_year < 1 || a_year > 9999)
            {
                status = false;
            }

            if(a_month < 1 || a_month > 9999)
            {
                status = false;
            }

            if(a_day < 1 || a_day > numDays)
            {
                status = false;
            }

            return status;
        }

        // Gets the number of days in a month for cases such as leap year and different months like February
        private int GetNumDaysMonth(int a_year, int a_month, int a_day)
        {
            int[] daysInMonth = { 0, 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };

            // Works for any month except February
            if (a_month != 2)
            {
                return daysInMonth[a_month];
            }
            // Making sure it is NOT a leap year
            if (a_year % 4 != 0)
            {
                return 28;
            }
            // Making sure it is not one of the exceptions for a leap year
            if (a_year % 100 == 0 && a_year % 400 != 0)
            {
                return 28;
            }
            // If we got passed everything, then it is a leap year and it needs to be 29
            return 29;
        }

        private void getSubDirectories(string a_directory, int a_year, int a_month, int a_day)
        {
            string[] subFiles;
            string[] subDirectories;
            subFiles = Directory.GetFiles(a_directory); // Gets the files for the current directory passed in

            DateTime userDate = new DateTime(a_year, a_month, a_day); // DateTime object of user's date
            DateTime fileDate;
            int result;

            // For loop cycling through all of the files in a specific directory
            for (int i = 0; i < subFiles.Length; i++)
            {
                fileDate = File.GetLastWriteTime(subFiles[i]);
                result = DateTime.Compare(fileDate, userDate); // Comparing the user's date with a file's date
                if (result >= 0)
                {
                    Console.WriteLine(subFiles[i]);
                }
            }

            // If the length of directories is 0, there are no more directories to search through
            if (Directory.GetDirectories(a_directory).Length == 0)
            {
                return;
            }
            // Otherwise, we'll create a for loop to cycle through the directories, and recursively
            // call this function til we go through all of the directories
            else
            {
                subDirectories = Directory.GetDirectories(a_directory);
                for(int j = 0; j < subDirectories.Length; j++)
                {
                    getSubDirectories(subDirectories[j], a_year, a_month, a_day);
                }
            }
        }
    }
}
