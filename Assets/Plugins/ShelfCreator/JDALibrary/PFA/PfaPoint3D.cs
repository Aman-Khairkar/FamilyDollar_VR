using System;
using System.Collections.Generic;
using System.Text;

namespace JDA
{
    public partial class PFA
    {
        public class Point3D
        {

            # region Field Names
            public class FieldNames
            {
                public const string X = "X";
                public const string Y = "Y";
                public const string Z = "Z";
            }
            # endregion


            public Dictionary<string, string> Fields;           // Data for each field read from the 3D point line of the PFA file        

            #region data fields
            private static string[] _fieldNames = new string[]
        {
            "X",
            "Z",
            "Y"
        };
            #endregion

            public Point3D() {
                Fields = new Dictionary<string, string>();
            }

            public Point3D(float x, float y, float z) {
                Fields = new Dictionary<string, string>();
                Fields[FieldNames.X] = x.ToString();
                Fields[FieldNames.Y] = y.ToString();
                Fields[FieldNames.Z] = z.ToString();
            }

            public float X {
                get { return float.Parse(Fields[FieldNames.X]); }
            }

            public float Y {
                get { return float.Parse(Fields[FieldNames.Y]); }
            }

            public float Z {
                get { return float.Parse(Fields[FieldNames.Z]); }
            }


            public static Point3D ParsePFA(string line) {
                string[] fields = PFA.SplitLine(line);
                Point3D obj = new Point3D();

                for (int i = 0; (i < _fieldNames.Length) && (i + 1 < fields.Length); i++) {
                    obj.Fields[_fieldNames[i]] = fields[i + 1];
                }

                return obj;
            }

            /// <summary>
            /// Generates the PFA output string representation for a 3D point.
            /// </summary>
            /// <returns>String</returns>
            public string WriteString() {
                StringBuilder sb = new StringBuilder();

                sb.Append("3D point");
                for (int i = 0; i < _fieldNames.Length; i++) {
                    sb.Append(PFA.PfaDelimiters[0]);
                    if (Fields.ContainsKey(_fieldNames[i])) sb.Append(Fields[_fieldNames[i]]);
                }

                return sb.ToString();
            }

        }
    }
    
}
