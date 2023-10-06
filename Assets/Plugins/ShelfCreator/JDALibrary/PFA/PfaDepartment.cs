using System;
using System.Collections.Generic;
using System.Text;

namespace JDA
{
        public partial class PFA
    {
    /// <summary>
    /// Departments are just polygons that mark the outline of a department.
    /// </summary>
            public class Department
            {

                # region Field Names
                public class FieldNames
                {
                    public const string Name = "Name";
                    public const string Key = "Key";
                    public const string X = "X";
                    public const string Width = "Width";
                    public const string Z = "Z";
                    public const string Height = "Height";
                    public const string Y = "Y";
                    public const string Depth = "Depth";
                    public const string Angle = "Angle";
                    public const string Label1X = "Label 1 X";
                    public const string Label1Y = "Label 1 Y";
                    public const string Changed = "Changed";
                    public const string Color = "Color";
                    public const string UserArea = "User Area";
                    public const string TextureOverride = "Texture Override";
                    public const string FillPattern = "Fill pattern";
                    public const string ModelFilename = "Model Filename";
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
                    public const string Desc11 = "Desc 11";
                    public const string Desc12 = "Desc 12";
                    public const string Desc13 = "Desc 13";
                    public const string Desc14 = "Desc 14";
                    public const string Desc15 = "Desc 15";
                    public const string Desc16 = "Desc 16";
                    public const string Desc17 = "Desc 17";
                    public const string Desc18 = "Desc 18";
                    public const string Desc19 = "Desc 19";
                    public const string Desc20 = "Desc 20";
                    public const string Desc21 = "Desc 21";
                    public const string Desc22 = "Desc 22";
                    public const string Desc23 = "Desc 23";
                    public const string Desc24 = "Desc 24";
                    public const string Desc25 = "Desc 25";
                    public const string Desc26 = "Desc 26";
                    public const string Desc27 = "Desc 27";
                    public const string Desc28 = "Desc 28";
                    public const string Desc29 = "Desc 29";
                    public const string Desc30 = "Desc 30";
                    public const string Desc31 = "Desc 31";
                    public const string Desc32 = "Desc 32";
                    public const string Desc33 = "Desc 33";
                    public const string Desc34 = "Desc 34";
                    public const string Desc35 = "Desc 35";
                    public const string Desc36 = "Desc 36";
                    public const string Desc37 = "Desc 37";
                    public const string Desc38 = "Desc 38";
                    public const string Desc39 = "Desc 39";
                    public const string Desc40 = "Desc 40";
                    public const string Desc41 = "Desc 41";
                    public const string Desc42 = "Desc 42";
                    public const string Desc43 = "Desc 43";
                    public const string Desc44 = "Desc 44";
                    public const string Desc45 = "Desc 45";
                    public const string Desc46 = "Desc 46";
                    public const string Desc47 = "Desc 47";
                    public const string Desc48 = "Desc 48";
                    public const string Desc49 = "Desc 49";
                    public const string Desc50 = "Desc 50";
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
                    public const string Value11 = "Value 11";
                    public const string Value12 = "Value 12";
                    public const string Value13 = "Value 13";
                    public const string Value14 = "Value 14";
                    public const string Value15 = "Value 15";
                    public const string Value16 = "Value 16";
                    public const string Value17 = "Value 17";
                    public const string Value18 = "Value 18";
                    public const string Value19 = "Value 19";
                    public const string Value20 = "Value 20";
                    public const string Value21 = "Value 21";
                    public const string Value22 = "Value 22";
                    public const string Value23 = "Value 23";
                    public const string Value24 = "Value 24";
                    public const string Value25 = "Value 25";
                    public const string Value26 = "Value 26";
                    public const string Value27 = "Value 27";
                    public const string Value28 = "Value 28";
                    public const string Value29 = "Value 29";
                    public const string Value30 = "Value 30";
                    public const string Value31 = "Value 31";
                    public const string Value32 = "Value 32";
                    public const string Value33 = "Value 33";
                    public const string Value34 = "Value 34";
                    public const string Value35 = "Value 35";
                    public const string Value36 = "Value 36";
                    public const string Value37 = "Value 37";
                    public const string Value38 = "Value 38";
                    public const string Value39 = "Value 39";
                    public const string Value40 = "Value 40";
                    public const string Value41 = "Value 41";
                    public const string Value42 = "Value 42";
                    public const string Value43 = "Value 43";
                    public const string Value44 = "Value 44";
                    public const string Value45 = "Value 45";
                    public const string Value46 = "Value 46";
                    public const string Value47 = "Value 47";
                    public const string Value48 = "Value 48";
                    public const string Value49 = "Value 49";
                    public const string Value50 = "Value 50";
                    public const string PrimaryDepartmentLabelFormatName = "Primary department label format name";
                    public const string SecondaryDepartmentLabelFormatName = "Secondary department label format name";
                    public const string DatePending = "Date Pending";
                    public const string DateLive = "Date Live";
                    public const string DateFinished = "Date Finished";
                    public const string Label2X = "Label 2 X";
                    public const string Label2Y = "Label 2 Y";
                    public const string TileTextures = "Tile Textures";
                    public const string TextureAllFaces = "Texture All Faces";
                    public const string Transparency = "Transparency";
                    public const string TileX = "Tile X";
                    public const string TileZ = "Tile Z";
                    public const string TileY = "Tile Y";
                    public const string PartID = "Part ID";
                    public const string GLN = "GLN";
                }
                # endregion


                public Dictionary<string, string> Fields;           // Data for each field read from the department line of the PFA file   
                public List<Point3D> PointsInFile;                  // List of points describing the department polygonal shape, as they were read from PFA file
                public List<PointF> Points;                         // List of rotated points describing the actual department polygonal shape

                #region data fields

                private static string[] _fieldNames = new string[]
        {
            "Name",
            "Key",
            "X",
            "Width",
            "Z",
            "Height",
            "Y",
            "Depth",
            "Angle",
            "Label 1 X",
            "Label 1 Y",
            "Changed",
            "Color",
            "User Area",
            "Texture Override",
            "Fill pattern",
            "Model Filename",
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
            "Desc 11",
            "Desc 12",
            "Desc 13",
            "Desc 14",
            "Desc 15",
            "Desc 16",
            "Desc 17",
            "Desc 18",
            "Desc 19",
            "Desc 20",
            "Desc 21",
            "Desc 22",
            "Desc 23",
            "Desc 24",
            "Desc 25",
            "Desc 26",
            "Desc 27",
            "Desc 28",
            "Desc 29",
            "Desc 30",
            "Desc 31",
            "Desc 32",
            "Desc 33",
            "Desc 34",
            "Desc 35",
            "Desc 36",
            "Desc 37",
            "Desc 38",
            "Desc 39",
            "Desc 40",
            "Desc 41",
            "Desc 42",
            "Desc 43",
            "Desc 44",
            "Desc 45",
            "Desc 46",
            "Desc 47",
            "Desc 48",
            "Desc 49",
            "Desc 50",
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
            "Value 11",
            "Value 12",
            "Value 13",
            "Value 14",
            "Value 15",
            "Value 16",
            "Value 17",
            "Value 18",
            "Value 19",
            "Value 20",
            "Value 21",
            "Value 22",
            "Value 23",
            "Value 24",
            "Value 25",
            "Value 26",
            "Value 27",
            "Value 28",
            "Value 29",
            "Value 30",
            "Value 31",
            "Value 32",
            "Value 33",
            "Value 34",
            "Value 35",
            "Value 36",
            "Value 37",
            "Value 38",
            "Value 39",
            "Value 40",
            "Value 41",
            "Value 42",
            "Value 43",
            "Value 44",
            "Value 45",
            "Value 46",
            "Value 47",
            "Value 48",
            "Value 49",
            "Value 50",
            "Primary department label format name",
            "Secondary department label format name",
            "Date Pending",
            "Date Live",
            "Date Finished",
            "Label 2 X",
            "Label 2 Y",
            "Tile Textures",
            "Texture All Faces",
            "Transparency",
            "Tile X",
            "Tile Z",
            "Tile Y",
            "Part ID",
            "GLN"
        };
                #endregion

                public Department() {
                    Fields = new Dictionary<string, string>();
                    PointsInFile = new List<Point3D>();
                    Points = new List<PointF>();
                }


                public float Angle {
                    get { return float.Parse(Fields[FieldNames.Angle]); }
                    set {
                        float angle = value;

                        // normalize Angle between 0 and 360
                        while (angle >= 360) angle -= 360;
                        while (angle < 0) angle += 360;
                        Fields[FieldNames.Angle] = angle.ToString();
                    }
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


                /// <summary>
                /// Calculates the rotated points for the department.
                /// </summary>
                public void ComputeRotatedPoints() {
                    // rotate the points about the fixture origin
                    foreach (Point3D pt in this.PointsInFile) {
                        // translate pt about (0,0)
                        float x = pt.X - this.X;
                        float y = pt.Y - this.Y;
                        // Rotate the point
                        PointF rotPt = PFA.RotatePoint(x, y, this.Angle);
                        // translate rotPt back to fixture origin
                        rotPt.X += this.X;
                        rotPt.Y += this.Y;

                        Points.Add(rotPt);
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


                public string Name {
                    get { return Fields[FieldNames.Name]; }
                    set { Fields[FieldNames.Name] = value.Substring(0, Math.Min(100, value.Length)); }
                }


                public static Department ParsePFA(string line) {
                    string[] fields = PFA.SplitLine(line);
                    Department obj = new Department();

                    for (int i = 0; (i < _fieldNames.Length) && (i + 1 < fields.Length); i++) {
                        obj.Fields[_fieldNames[i]] = fields[i + 1];
                    }

                    obj.Angle = float.Parse(obj.Fields[FieldNames.Angle]);

                    return obj;
                }


                public override string ToString() {
                    return "Department " + Name;
                }


                public float Width {
                    get { return float.Parse(Fields[FieldNames.Width]); }
                    set { Fields[FieldNames.Width] = value.ToString(); }
                }


                /// <summary>
                /// Generates the PFA output string representation for a department.
                /// </summary>
                /// <returns>String</returns>
                public string WriteString() {
                    StringBuilder sb = new StringBuilder();
                    string replaceComma = "\\" + ",";

                    sb.Append("Department");
                    for (int i = 0; i < _fieldNames.Length; i++) {
                        sb.Append(",");
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
