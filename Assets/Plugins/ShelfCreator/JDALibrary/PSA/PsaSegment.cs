using System;
using System.Collections.Generic;
using System.Text;

namespace JDA
{
    public partial class PSA
    {
        /// <summary>
        /// A vertical division on a planogram backboard. Segments usually are identified at places where notch bars represent the 
        /// physical divisions on an actual planogram.
        /// </summary>
        public class Segment
        {

            # region Field Names
            public class FieldNames
            {
                public const string Name = "Name";
                public const string Key = "Key";
                public const string X = "X";
                public const string Width = "Width";
                public const string Y = "Y";
                public const string Height = "Height";
                public const string Z = "Z";
                public const string Depth = "Depth";
                public const string Angle = "Angle";
                public const string OffsetX = "Offset X";
                public const string OffsetY = "Offset Y";
                public const string Door = "Door";
                public const string DoorDirection = "Door direction";
                public const string Desc1 = "Desc 1";
                public const string Desc2 = "Desc 2";
                public const string Desc3 = "Desc 3";
                public const string Desc4 = "Desc 4";
                public const string Desc5 = "Desc 5";
                public const string Desc6 = "Desc 6";
                public const string Desc7 = "Desc 7";
                public const string Desc8 = "Desc 8";
                public const string Desc9 = "Desc 9";
                public const string Desc10 = "Desc 10";
                public const string Value1 = "Value 1";
                public const string Value2 = "Value 2";
                public const string Value3 = "Value 3";
                public const string Value4 = "Value 4";
                public const string Value5 = "Value 5";
                public const string Value6 = "Value 6";
                public const string Value7 = "Value 7";
                public const string Value8 = "Value 8";
                public const string Value9 = "Value 9";
                public const string Value10 = "Value 10";
                public const string Flag1 = "Flag 1";
                public const string Flag2 = "Flag 2";
                public const string Flag3 = "Flag 3";
                public const string Flag4 = "Flag 4";
                public const string Flag5 = "Flag 5";
                public const string Flag6 = "Flag 6";
                public const string Flag7 = "Flag 7";
                public const string Flag8 = "Flag 8";
                public const string Flag9 = "Flag 9";
                public const string Flag10 = "Flag 10";
                public const string FrameWidth = "Frame width";
                public const string FrameHeight = "Frame height";
                public const string Changed = "Changed";
                public const string FrameColor = "Frame color";
                public const string FrameFillPattern = "Frame fill pattern";
                public const string PartID = "PartID";
                public const string GLN = "GLN";
                public const string CustomData = "Custom data";
                public const string CanSeparate = "Can separate";
            }
            # endregion


            public Dictionary<string, string> Fields;           // Data for each field read from the segment line of the PSA file        

            #region data fields
            private static string[] _fieldNames = new string[]
        {
            "Name",
            "Key",
            "X",
            "Width",
            "Y",
            "Height",
            "Z",
            "Depth",
            "Angle",
            "Offset X",
            "Offset Y",
            "Door",
            "Door direction",
            "Desc 1",
            "Desc 2",
            "Desc 3",
            "Desc 4",
            "Desc 5",
            "Desc 6",
            "Desc 7",
            "Desc 8",
            "Desc 9",
            "Desc 10",
            "Value 1",
            "Value 2",
            "Value 3",
            "Value 4",
            "Value 5",
            "Value 6",
            "Value 7",
            "Value 8",
            "Value 9",
            "Value 10",
            "Flag 1",
            "Flag 2",
            "Flag 3",
            "Flag 4",
            "Flag 5",
            "Flag 6",
            "Flag 7",
            "Flag 8",
            "Flag 9",
            "Flag 10",
            "Frame width",
            "Frame height",
            "Changed",
            "Frame color",
            "Frame fill pattern",
            "PartID",
            "GLN",
            "Custom data",
            "Can separate"
        };
            #endregion

            public Segment() {
                Fields = new Dictionary<string, string>();
            }

            public float Angle {
                get { return float.Parse(Fields[FieldNames.Angle]); }
                set { Fields[FieldNames.Angle] = value.ToString(); }
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


            public string Name {
                get { return Fields[FieldNames.Name]; }
                set { Fields[FieldNames.Name] = value.Substring(0, Math.Min(100, value.Length)); }
            }


            public static Segment ParsePSA(string line) {
                string[] fields = SplitLine(line);
                Segment obj = new Segment();

                for (int i = 0; (i < _fieldNames.Length) && (i + 1 < fields.Length); i++) {
                    obj.Fields[_fieldNames[i]] = fields[i + 1];
                }

                return obj;
            }


            public override string ToString() {
                return "PSA Segment " + Name;
            }


            public float Width {
                get { return float.Parse(Fields[FieldNames.Width]); }
                set { Fields[FieldNames.Width] = value.ToString(); }
            }


            /// <summary>
            /// Generates the PSA output string representation for a segment.
            /// </summary>
            /// <returns>String</returns>
            public string WriteString() {
                StringBuilder sb = new StringBuilder();
                string replaceComma = "\\" + ",";

                sb.Append("Segment");
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
