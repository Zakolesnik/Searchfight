using System;
using System.Linq;

namespace Searchfight.ConsoleApp
{
    /// <summary>
    /// Program parameters parcer, verify correctness of input parameters
    /// </summary>
    public static class InputParamParser
    {
        private const string MSG_WRONG_PARAMS_NO = "Wrong number of parameters, command call should contain at least two parameters";
        private const string MSG_WRONG_PARAMS_EMPTY = "Wrong input parameters format, it should not contain empty strings";
        private const string MSG_WRONG_PARAMS_DUPLICATE = "Wrong input parameters format, it should not contain duplicate strings";

        /// <summary>
        /// Verify of parameters correctness
        /// </summary>
        /// <param name="parameters">Array of source parameters</param>
        /// <param name="errorMessage">This string will contain error description if source queries is wrong</param>
        /// <returns>True if queries is correct, otherwise - false</returns>
        public static bool Verify(string[] parameters, ref string errorMessage) 
        {
            if (parameters.Count() < 2)
            {
                errorMessage = MSG_WRONG_PARAMS_NO;
                return false;
            }
            else if (parameters.Contains(string.Empty))
            {
                errorMessage = MSG_WRONG_PARAMS_EMPTY;
                return false;
            }
            else if (parameters.Count() != parameters.Distinct().Count())
            {
                errorMessage = MSG_WRONG_PARAMS_DUPLICATE;
                return false;
            }

            return true;
        }
    }
}