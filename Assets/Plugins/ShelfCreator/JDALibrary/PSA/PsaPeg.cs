using System;
using System.Collections.Generic;
using System.Text;

namespace JDA
{
    public partial class PSA
    {
        /// <summary>
        /// Pegs are objects from which products are hung. Pegs can be attached to pegboards, multi-row pegboards, and bars. 
        /// </summary>
        public class Peg
        {

            # region Field Names
            public class FieldNames
            {
                public const string ID = "ID";
                public const string Description = "Description";
                public const string Length = "Length";
                public const string BackHoles = "Back Holes";
                public const string Height = "Height";
                public const string TagOffset = "Tag Offset";
                public const string TagHeight = "Tag Height";
                public const string TagWidth = "Tag Width";
                public const string Type = "Type";
                public const string BackOffset = "Back Offset";
                public const string BackSpacing = "Back Spacing";
                public const string FrontBars = "Front Bars";
                public const string FrontSpacing = "Front Spacing";
                public const string PartID = "PartID";
                public const string TagXOffset = "Tag X Offset";
            }
            # endregion


            public Dictionary<string, string> Fields;           // Data for each field read from the peg line of the PSA file        

            #region data fields
            private static string[] _fieldNames = new string[]
        {
            "ID",
            "Description",
            "Length",
            "Back Holes",
            "Height",
            "(Obsolete1)",
            "Tag Offset",
            "Tag Height",
            "Tag Width",
            "Type",
            "Back Offset",
            "(Obsolete2)",
            "Back Spacing",
            "Front Bars",
            "Front Spacing",
            "PartID",
            "Tag X Offset"
        };
            #endregion

            public Peg() {
                Fields = new Dictionary<string, string>();
            }


            public static Peg ParsePSA(string line) {
                string[] fields = SplitLine(line);
                Peg obj = new Peg();

                for (int i = 0; (i < _fieldNames.Length) && (i + 1 < fields.Length); i++) {
                    obj.Fields[_fieldNames[i]] = fields[i + 1];
                }

                return obj;
            }

            /// <summary>
            /// Generates the PSA output string representation for a peg.
            /// </summary>
            /// <returns>String</returns>
            public string WriteString() {
                StringBuilder sb = new StringBuilder();
                string replaceComma = "\\" + ",";

                sb.Append("Peg");
                for (int i = 0; i < _fieldNames.Length; i++) {
                    sb.Append(PSA.PsaDelimiters[0]);
                    if (Fields.ContainsKey(_fieldNames[i])) {
                        string val = Fields[_fieldNames[i]];
                        if (!val.Contains(replaceComma) && val.Contains(",")) {
                            val = val.Replace(",", replaceComma);
                        }
                        sb.Append(val);
                    }
                }

                return sb.ToString();
            }

        }
    }
}
