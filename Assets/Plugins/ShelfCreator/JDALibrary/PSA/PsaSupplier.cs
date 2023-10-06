using System;
using System.Collections.Generic;
using System.Text;

namespace JDA
{
    public partial class PSA
    {

        /// <summary>
        /// Supplier contains fields of information about delivery days for a supplier.
        /// </summary>
        public class Supplier
        {

            # region Field Names
            public class FieldNames
            {
                public const string Name = "Name";
                public const string Key = "Key";
                public const string Delivery1 = "Delivery 1";
                public const string Delivery2 = "Delivery 2";
                public const string Delivery3 = "Delivery 3";
                public const string Delivery4 = "Delivery 4";
                public const string Delivery5 = "Delivery 5";
                public const string Delivery6 = "Delivery 6";
                public const string Delivery7 = "Delivery 7";
                public const string Delivery8 = "Delivery 8";
                public const string Delivery9 = "Delivery 9";
                public const string Delivery10 = "Delivery 10";
                public const string Delivery11 = "Delivery 11";
                public const string Delivery12 = "Delivery 12";
                public const string Delivery13 = "Delivery 13";
                public const string Delivery14 = "Delivery 14";
                public const string Delivery15 = "Delivery 15";
                public const string Delivery16 = "Delivery 16";
                public const string Delivery17 = "Delivery 17";
                public const string Delivery18 = "Delivery 18";
                public const string Delivery19 = "Delivery 19";
                public const string Delivery20 = "Delivery 20";
                public const string Delivery21 = "Delivery 21";
                public const string Delivery22 = "Delivery 22";
                public const string Delivery23 = "Delivery 23";
                public const string Delivery24 = "Delivery 24";
                public const string Delivery25 = "Delivery 25";
                public const string Delivery26 = "Delivery 26";
                public const string Delivery27 = "Delivery 27";
                public const string Delivery28 = "Delivery 28";
                public const string Changed = "Changed";
            }
            # endregion


            public Dictionary<string, string> Fields;           // Data for each field read from the product line of the PSA file        

            #region data fields
            private static string[] _fieldNames = new string[]
        {
            "Name",
            "Key",
            "Delivery 1",
            "Delivery 2",
            "Delivery 3",
            "Delivery 4",
            "Delivery 5",
            "Delivery 6",
            "Delivery 7",
            "Delivery 8",
            "Delivery 9",
            "Delivery 10",
            "Delivery 11",
            "Delivery 12",
            "Delivery 13",
            "Delivery 14",
            "Delivery 15",
            "Delivery 16",
            "Delivery 17",
            "Delivery 18",
            "Delivery 19",
            "Delivery 20",
            "Delivery 21",
            "Delivery 22",
            "Delivery 23",
            "Delivery 24",
            "Delivery 25",
            "Delivery 26",
            "Delivery 27",
            "Delivery 28",
            "Changed"
        };
            #endregion

            public Supplier() {
                Fields = new Dictionary<string, string>();
            }


            public string Key {
                get { return Fields[FieldNames.Key]; }
            }


            public string Name {
                get { return Fields[FieldNames.Name]; }
            }


            public static Supplier ParsePSA(string line) {
                string[] fields = SplitLine(line);
                Supplier obj = new Supplier();

                for (int i = 0; (i < _fieldNames.Length) && (i + 1 < fields.Length); i++) {
                    obj.Fields[_fieldNames[i]] = fields[i + 1];
                }

                return obj;
            }


            public override string ToString() {
                return "PSA Supplier " + Name;
            }



            /// <summary>
            /// Generates the PSA output string representation for a supplier.
            /// </summary>
            /// <returns>String</returns>
            public string WriteString() {
                StringBuilder sb = new StringBuilder();
                string replaceComma = "\\" + ",";

                sb.Append("Supplier");
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
