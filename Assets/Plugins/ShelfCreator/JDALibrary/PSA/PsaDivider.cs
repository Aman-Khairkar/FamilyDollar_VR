using System;
using System.Collections.Generic;
using System.Text;

namespace JDA
{
    public partial class PSA
    {

        /// <summary>
        /// Dividers are fixture elements that divide the space on a fixture to create separate merchandising areas.
        /// </summary>
        public class Divider
        {

            # region Field Names
            public class FieldNames
            {
                public const string Key = "Key";
                public const string X = "X";
                public const string Width = "Width";
                public const string Y = "Y";
                public const string Height = "Height";
                public const string Z = "Z";
                public const string Depth = "Depth";
                public const string Color = "Color";
                public const string AutoCreated = "Auto-created";
                public const string Desc1 = "Desc 1";
                public const string Desc2 = "Desc 2";
                public const string Desc3 = "Desc 3";
                public const string Value1 = "Value 1";
                public const string Value2 = "Value 2";
                public const string Value3 = "Value 3";
                public const string Changed = "Changed";
                public const string HideIfPrinting = "Hide if printing";
                public const string PartID = "PartID";
                public const string CustomData = "Custom data";

            }
            # endregion


            public Dictionary<string, string> Fields;           // Data for each field read from the divider line of the PSA file        
            public Fixture Fixture { get; private set; }                     // Parent fixture

            #region data fields
            private static string[] _fieldNames = new string[]
    {
        "Key",
        "X",
        "Width",
        "Y",
        "Height",
        "Z",
        "Depth",
        "Color",
        "Auto-created",
        "Desc 1",
        "Desc 2",
        "Desc 3",
        "Value 1",
        "Value 2",
        "Value 3",
        "Changed",
        "Hide if printing",
        "PartID",
        "Custom data"
    };
            #endregion

            public Divider(Fixture parent) {
                Fields = new Dictionary<string, string>();
                Fixture = parent;
            }


            public PfaColor Color {
                get {
                    long c = long.Parse(Fields[FieldNames.Color]);
                    return new PfaColor(c);
                }
                set {
                    long c = (value.B << 16) | (value.G << 8) | value.R;
                    Fields[FieldNames.Color] = c.ToString();
                }
            }


            public float Depth {
                get { return float.Parse(Fields[FieldNames.Depth]); }
                set { Fields[FieldNames.Depth] = value.ToString(); }
            }

            public float Height {
                get { return float.Parse(Fields[FieldNames.Height]); }
                set { Fields[FieldNames.Height] = value.ToString(); }
            }

            public string Key {
                get { return Fields[FieldNames.Key]; }
            }


            public static Divider ParsePSA(string line, Fixture parent) {
                string[] fields = SplitLine(line);
                Divider obj = new Divider(parent);

                for (int i = 0; (i < _fieldNames.Length) && (i + 1 < fields.Length); i++) {
                    obj.Fields[_fieldNames[i]] = fields[i + 1];
                }

                return obj;
            }


            public override string ToString() {
                return "PSA Divider " + Key;
            }


            public float Width {
                get { return float.Parse(Fields[FieldNames.Width]); }
                set { Fields[FieldNames.Width] = value.ToString(); }
            }


            /// <summary>
            /// Generates the PSA output string representation for a Divider.
            /// </summary>
            /// <returns>String</returns>
            public string WriteString() {
                StringBuilder sb = new StringBuilder();
                string replaceComma = "\\" + ",";

                sb.Append("Divider");
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


            public float X {
                get { return float.Parse(Fields[FieldNames.X]); }
                set { Fields[FieldNames.X] = value.ToString(); }
            }

            public float Y {
                get { return float.Parse(Fields[FieldNames.Y]); }
                set { Fields[FieldNames.Y] = value.ToString(); }
            }

            public float Z {
                get { return float.Parse(Fields[FieldNames.Z]); }
                set { Fields[FieldNames.Z] = value.ToString(); }
            }
        }
    }
}

