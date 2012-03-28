using System;
using System.Collections.Generic;
using System.IO;
using ConstantContactBO.Entities;

namespace ConstantContactUtility
{
    /// <summary>
    /// Utils class
    /// </summary>
    public class Utils
    {
        #region Static Methods
        /// <summary>
        /// Converts a Enum into a List T
        /// </summary>
        /// <typeparam name="T">Enum</typeparam>
        /// <returns>List</returns>
        public static List<T> EnumToList<T>()
        {
            Type enumType = typeof(T);

            if (enumType.BaseType != typeof(Enum))
            {
                throw new ArgumentException("T must be of type System.Enum");
            }
// ReSharper disable AssignNullToNotNullAttribute
            return new List<T>(Enum.GetValues(enumType) as IEnumerable<T>);
// ReSharper restore AssignNullToNotNullAttribute
        }

        /// <summary>
        /// List of Greetings format
        /// </summary>
        /// <returns></returns>
        public static List<string> GreetingName()
        {
            return new List<string>
                       {
                           "First Name",
                           "Last Name",
                           "First & Last Name",
                           "None"
                       };
        }

        /// <summary>
        /// List of all countries
        /// </summary>
        /// <returns></returns>
        public static List<Country> GetAllCountries()
        {
            List<Country> list = new List<Country> { new Country { Name = "Select Country", Code = "0" } };

            string countriesPath = string.Format(@"{0}InputData\countries.txt", AppDomain.CurrentDomain.BaseDirectory);

            try
            {
                TextReader tr = new StreamReader(countriesPath);
            
                string result;

                while (tr.Peek() != -1)
                {
                    result = tr.ReadLine();
                    if (string.IsNullOrEmpty(result.Trim())) continue;
                    Country country = new Country();
                    string[] info = result.Split('-');

                    if (info.Length < 2) continue;
                    country.Name = info[0].Trim();
                    country.Code = info[1].Trim();
                    list.Add(country);
                }

                tr.Close();
            }
            catch (Exception)
            {
                return list;
            }
            
            return list;
        }

        /// <summary>
        /// List of all US States
        /// </summary>
        /// <returns></returns>
        public static List<State> GetAllUSStates()
        {
            List<State> list = new List<State> { new State { Name = "Non U.S.", Code = "0" } };

            string countriesPath = string.Format(@"{0}InputData\states.txt", AppDomain.CurrentDomain.BaseDirectory);

            try
            {
                TextReader tr = new StreamReader(countriesPath);
                string result;

                while (tr.Peek() != -1)
                {
                    result = tr.ReadLine();
                    if (string.IsNullOrEmpty(result.Trim())) continue;
                    State state = new State();
                    string[] info = result.Split('-');

                    if (info.Length < 2) continue;
                    state.Name = info[0].Trim();
                    state.Code = info[1].Trim();
                    list.Add(state);
                }

                tr.Close();
            }
            catch (Exception)
            {
                return list;
            }

            return list;
        }

        /// <summary>
        /// List of Email Campaign Statuses
        /// </summary>
        /// <returns></returns>
        public static List<string> GetEmailCampaignsStatusList()
        {
            return new List<string> { "ALL", "DRAFT", "RUNNING", "SENT", "SCHEDULED" };
        }

        #endregion
    }
}