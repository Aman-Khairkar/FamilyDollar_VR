using System;
using System.Collections.Generic;
using System.Text;

namespace JDA
{
    public partial class PSA
    {
        public class Point3D
        {

            # region Field Names
            public class FieldNames
            {
                public const string X = "X";
                public const string Y = "Y";
                public const string Z = "Z";
                public const string Key = "Key";
            }
            # endregion


            public Dictionary<string, string> Fields;           // Data for each field read from the 3D point line of the PSA file        

            #region data fields
            private static string[] _fieldNames = new string[]
        {
            "X",
            "Z",
            "Y",
            "Key"
        };
            #endregion

            public Point3D() {
                Fields = new Dictionary<string, string>();
            }

            public Point3D(float x, float y, float z, string key) {
                Fields = new Dictionary<string, string>();
                Fields[FieldNames.X] = x.ToString();
                Fields[FieldNames.Y] = y.ToString();
                Fields[FieldNames.Z] = z.ToString();
                Fields[FieldNames.Key] = key;
            }


            public string Key {
                get { return Fields[FieldNames.Key]; }
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


            public static Point3D ParsePSA(string line) {
                string[] fields = SplitLine(line);
                Point3D obj = new Point3D();

                for (int i = 0; (i < _fieldNames.Length) && (i + 1 < fields.Length); i++) {
                    obj.Fields[_fieldNames[i]] = fields[i + 1];
                }

                return obj;
            }

            /// <summary>
            /// Generates the PSA output string representation for a 3D point.
            /// </summary>
            /// <returns>String</returns>
            public string WriteString() {
                StringBuilder sb = new StringBuilder();

                sb.Append("3D point");
                for (int i = 0; i < _fieldNames.Length; i++) {
                    sb.Append(PSA.PsaDelimiters[0]);
                    if (Fields.ContainsKey(_fieldNames[i])) sb.Append(Fields[_fieldNames[i]]);
                }

                return sb.ToString();
            }

        }
    }
    
}
