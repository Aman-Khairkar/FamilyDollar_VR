using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace JDA
{
    public class GroupManager
    {
        public enum CombiningFunction { Average, Count, Min, Max, Sum, Index };        // mathematical function used to combine the values of group members into a single value representing the group
		public static Random random = new Random();
        /// <summary>
        /// Assigns each row of the input data to a group, based on the value of the row's groupingField.  A single value is then calculated to represent each group, by applying the specified
        /// combining function to the dataField of every row belonging to the group.
        /// </summary>
        /// <param name="data">Rows of data, in the form of Dictionary<rowID, Dictionary<fieldID, value>></param>
        /// <param name="groupingField">Name of data field containing the group identifier</param>
        /// <param name="valueField">Name of data field containing the value to be combined</param>
        /// <param name="combiningFunction">The function used to combine all the values in each group</param>
		/// <param name="groupDatamap">A Dictionary where we will append / create mapping a data ID TO a list of group IDs</param>
        /// <returns>A Dictionary mapping group names to combined values</returns>
        public static Dictionary<string, float> AssignGroups(Dictionary<string, Dictionary<string, string>> data, string groupingField,
		                                                     string valueField, CombiningFunction combiningFunction,
		                                                     ref Dictionary<string, List<string>> groupDatamap,
		                                                     out Dictionary<string, string> groupIdToName)
        {
            Dictionary<string, int> counts = new Dictionary<string,int>();              // maps the normalized group name to a count of how many members belong to the group
            Dictionary<string, float> groups = new Dictionary<string, float>();       // maps the normalized group name to its combined value
			Dictionary<string, float> linearPercents = new Dictionary<string,float> (); //used for the indexing calculation
            groupIdToName = new Dictionary<string,string>();   // maps the normalized group name to an un-normalized version of the group's name

			int idCombiner = random.Next();// used as a sort of salt for creating unique group ids

			if(groupDatamap == null) {
				groupDatamap = new Dictionary<string, List<string>>();
			}

            // find all the unique values of the grouping field and use them to partition the data values.
            foreach (KeyValuePair<string, Dictionary<string, string>> dataRow in data) {
               float dataValue;

                if (dataRow.Value.ContainsKey(groupingField) && dataRow.Value.ContainsKey(valueField) && float.TryParse(dataRow.Value[valueField], out dataValue)) {
                    string groupId = CreateGroupId(dataRow.Value[groupingField], idCombiner);      // the group this dataRow belongs to

                    if (!groups.ContainsKey(groupId)) {
                        // this groupName has not been seen before, so initialize the group values
                        groupIdToName[groupId] = dataRow.Value[groupingField];
                        counts[groupId] = 0;
                    }

                    counts[groupId] += 1;

                    // combine the value of this data row with the value of the group so far
                    switch (combiningFunction) {
                        case CombiningFunction.Average:
						case CombiningFunction.Index:
                        case CombiningFunction.Sum:
                            if (!groups.ContainsKey(groupId)) groups[groupId] = 0;
							if(!linearPercents.ContainsKey(groupId)) linearPercents[groupId] = 0;
                            groups[groupId] += dataValue;
                            break;

                        case CombiningFunction.Max:
                            if (!groups.ContainsKey(groupId)) groups[groupId] = dataValue;
                            else groups[groupId] = Math.Max(groups[groupId], dataValue);
                            break;

                        case CombiningFunction.Min:
                            if (!groups.ContainsKey(groupId)) groups[groupId] = dataValue;
                            else groups[groupId] = Math.Min(groups[groupId], dataValue);
                            break;

                        case CombiningFunction.Count:
                            if (!groups.ContainsKey(groupId)) groups[groupId] = 0;
                            groups[groupId] += 1;
                            break;

                    }
					if(combiningFunction == CombiningFunction.Index)
					{
						float linearPercent; 
						if(float.TryParse(dataRow.Value["Linear%Used"], out linearPercent))
						{
							linearPercents[groupId] += linearPercent; 
						}

					}
					if(groupDatamap.ContainsKey(dataRow.Key) == false) {
						groupDatamap.Add(dataRow.Key, new List<string>());
					}
					groupDatamap[dataRow.Key].Add(groupId);
                }
            }


            if (combiningFunction == CombiningFunction.Average) {
                // calculate the average of each group
                Dictionary<string, float> averages = new Dictionary<string, float>();
                foreach (KeyValuePair<string, float> pair in groups) {
                    averages[pair.Key] = pair.Value / counts[pair.Key];
                }
                groups = averages;
            }

			if (combiningFunction == CombiningFunction.Index) {
				// calculate the average of each group
				Dictionary<string, float> indexes = new Dictionary<string, float>();
				foreach (KeyValuePair<string, float> pair in groups) {
					if(pair.Value != 0)
						indexes[pair.Key] = (linearPercents[pair.Key] / pair.Value ) * 100;
					else
						indexes[pair.Key] = 0; 
				}
				groups = indexes;
			}

            return groups;
        }

		/// <summary>
		/// Gets a list of all the group item name.
		/// </summary>
		/// <returns>The group names.</returns>
		/// <param name="data">Rows of data, in the form of Dictionary<rowID, Dictionary<fieldID, value>></param>
		/// <param name="groupingField">Name of data field containing the group identifier</param>
		/// <param name="valueField">Name of data field containing the value to be combined</param>
		/// <param name="combiningFunction">The function used to combine all the values in each group</param>
		public static List<string> GetGroupNames(Dictionary<string, Dictionary<string, string>> data, string groupingField)
		{
			List<string> names = new List<string>();
			foreach (KeyValuePair<string, Dictionary<string, string>> dataRow in data) {

				if (dataRow.Value.ContainsKey(groupingField)) {

					if(names.Contains(dataRow.Value[groupingField]) == false && !string.IsNullOrEmpty(dataRow.Value[groupingField])) {
						names.Add(dataRow.Value[groupingField]);
					}
				}
			}

			return names;
		}

		[System.Obsolete("ValueField and CombiningFunction are not needed for group names")]
		public static List<string> GetGroupNames(Dictionary<string, Dictionary<string, string>> data, string groupingField,
		                                         string valueField, CombiningFunction combiningFunction)
		{
			return GetGroupNames(data, groupingField);
		}

		/// <summary>
		/// Creates a unique group identifier based on group name and the combining int.
		/// </summary>
		/// <returns>The group identifier.</returns>
		/// <param name="groupName">Group name.</param>
		/// <param name="combiner">Combining integer to make unique from future sets of data</param>
        public static string CreateGroupId(string groupName, int combiner)
        {
			return (combiner + groupName.GetHashCode()).ToString();
        }
    }
}
