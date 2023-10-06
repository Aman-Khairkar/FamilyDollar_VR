using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JDA
{

    /// <summary>
    /// Range Specification
    /// Ranges partition the number line into disjoint sets of values.
    /// </summary>
    public class RangeSpec
    {
        public string Name;         // Name of the range class
        public float Min;           // Minimum value to be included in range (inclusive)
        public float Max;           // Maximum value to be included in range (exclusive)

        public RangeSpec(string name, float min, float max)
        {
            Name = name;
            Min = min;
            Max = max;
        }


        /// <summary>
        /// Tests membership in the range
        /// </summary>
        /// <param name="val">Value to test</param>
        /// <returns>Boolean indicating membership in range</returns>
        public bool Member(float val)
        {
            return (val >= Min) && (val < Max);
        }
    }


    public class RangeManager
    {

        /// <summary>
        /// Determines what range (if any) each data row belongs to, based on the value in a specified field and a list of range specifications.
        /// </summary>
        /// <param name="data">Rows of data, in the form of Dictionary<rowID, Dictionary<fieldID, value>></param>
        /// <param name="valueField">Name of data field containing the value used to determine range membership</param>
        /// <param name="ranges">List of Range Specifications</param>
        /// <param name="rangeHistogram">Upon return, contains a Dictionary mapping range names to the number of data rows assigned to that range</param>
        /// <returns>Dictionary mapping data row keys to range names</returns>
		public static Dictionary<string, string> AssignRanges(Dictionary<string, Dictionary<string, string>> data, string valueField, List<RangeSpec> ranges,
		                                                      out Dictionary<string, int> rangeHistogram, out Dictionary<string, string> rangeIndices)
		{
			Dictionary<string, string> rangeAssignments = new Dictionary<string, string>();

			// initialize the range histogram
			rangeHistogram = new Dictionary<string, int>();
			rangeIndices = new Dictionary<string, string>();

			int combiner = GroupManager.random.Next();
			foreach (RangeSpec r in ranges) {
				string rangeIndex = GroupManager.CreateGroupId(r.Name, combiner);
				rangeIndices[r.Name] = rangeIndex;
				rangeHistogram[rangeIndex] = 0;
			}
			
			// assign a range to each data row that contains valueField
			foreach (KeyValuePair<string, Dictionary<string, string>> dataRow in data) {

				float value;

				if (dataRow.Value.ContainsKey(valueField) && float.TryParse(dataRow.Value[valueField], out value)) {
					
					foreach (RangeSpec range in ranges) {
						if (range.Member(value)) {
							string rangeIndex = rangeIndices[range.Name];
							rangeAssignments[dataRow.Key] = rangeIndex;
							rangeHistogram[rangeIndex] += 1;
						}
					}

				}
				
			}

			return rangeAssignments;
		}
	}
}
