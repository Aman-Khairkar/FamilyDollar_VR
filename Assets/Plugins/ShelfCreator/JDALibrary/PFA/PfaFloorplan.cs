using System;
using System.Collections.Generic;
using System.Text;

namespace JDA
{
    public partial class PFA
    {
        /// <summary>
        /// Floorplan represents the floor space of a store.  A PFA file may contain several Floorplan objects, which usually represent different designs
        /// for the same store, such as the current and proposed designs.
        /// </summary>
        public class Floorplan
        {

            # region Field Names
            public class FieldNames
            {
                public const string Name = "Name";
                public const string Key = "Key";
                public const string Width = "Width";
                public const string Depth = "Depth";
                public const string CeilingColor = "Ceiling Color";
                public const string DrawFloor = "Draw floor";
                public const string FloorColor = "Floor color";
                public const string DrawGrid = "Draw grid";
                public const string VerticalSpacing = "Vertical spacing";
                public const string LineStyle = "Line style";
                public const string GridColor = "Grid color";
                public const string AutoCreated = "Auto created";
                public const string ShapeID = "Shape ID";
                public const string BitmapID = "Bitmap ID";
                public const string HorizontalSpacing = "Horizontal spacing";
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
                public const string FillPattern = "Fill pattern";
                public const string Filename = "File name";
                public const string Changed = "Changed";
                public const string LayoutFilename = "Layout file name";
                public const string DBKey1 = "DBKey1";
                public const string DBKey2 = "DBKey2";
                public const string DBKey3 = "DBKey3";
                public const string DBKey4 = "DBKey4";
                public const string DBKey5 = "DBKey5";
                public const string DBKey6 = "DBKey6";
                public const string DBKey7 = "DBKey7";
                public const string DBKey8 = "DBKey8";
                public const string DBKey9 = "DBKey9";
                public const string DBKey10 = "DBKey10";
                public const string Notes = "Notes";
                public const string DatePending = "Date Pending";
                public const string DateLive = "Date Live";
                public const string DateFinished = "Date Finished";
                public const string ThreeDFloorImage = "Three D Floor Image";
                public const string ThreeDCeilingImage = "Three D Ceiling Image";
                public const string CeilingHeight = "Ceiling Height";
                public const string CeilingTileX = "Ceiling Tile X";
                public const string CeilingTileY = "Ceiling Tile Y";
                public const string FloorTileX = "Floor Tile X";
                public const string FloorTileY = "Floor Tile Y";
                public const string GLN = "GLN";
                public const string DBGUID = "DBGUID";
                public const string Source = "Source";
                public const string Status1 = "Status 1";
                public const string TexturePaths = "Texture Paths";
                public const string PlanogramSplitDefault = "Planogram Split Default";
                public const string DBFamilyKey = "DBFamilyKey";
                public const string DBReplaceKey = "DBReplaceKey";
                public const string DBVersionKey = "DBVersionKey";

            }
            # endregion


            public Dictionary<string, Department> Departments;      // Dictionary mapping strings (department name) to deparments present in PFA file
            public List<Drawing> Drawings;                          // List of drawings associated with floorplan
            public Dictionary<string, string> Fields;               // Data for each field read from the floorplan line of the PFA file
            public Dictionary<string, Fixture> Fixtures;            // Dictionary mapping strings (uniqueID) to fixtures present in PFA file
            public Dictionary<string, Performance> Performances;    // Dictionary mapping strings (planogram key) to performance object associated with that planogram 
            public Dictionary<string, Section> Sections;            // Dictionary mapping names to sections present in PFA file
            public List<string> Unknowns;                           // list of unclassified lines associated with floorplan

            #region field names
            private static string[] _fieldNames = new string[]
        {
            "Name",
            "Key",
            "Width",
            "Depth",
            "Ceiling Color",
            "Draw floor",
            "Floor color",
            "Draw grid",
            "Vertical spacing",
            "Line style",
            "Grid color",
            "Auto created",
            "Shape ID",
            "Bitmap ID",
            "Horizontal spacing",
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
            "Fill pattern",
            "File name",
            "Changed",
            "Layout file name",
            "DBKey1",
            "DBKey2",
            "DBKey3",
            "DBKey4",
            "DBKey5",
            "DBKey6",
            "DBKey7",
            "DBKey8",
            "DBKey9",
            "DBKey10",
            "Notes",
            "Date Pending",
            "Date Live",
            "Date Finished",
            "Three D Floor Image",
            "Three D Ceiling Image",
            "Ceiling Height",
            "Ceiling Tile X",
            "Ceiling Tile Y",
            "Floor Tile X",
            "Floor Tile Y",
            "GLN",
            "DBGUID",
            "Source",
            "Status 1",
            "Texture Paths",
            "Planogram Split Default",
            "DBFamilyKey",
            "DBReplaceKey",
            "DBVersionKey"
        };
            #endregion


            public Floorplan() {
                Departments = new Dictionary<string, Department>();
                Drawings = new List<Drawing>();
                Fields = new Dictionary<string, string>();
                Fixtures = new Dictionary<string, Fixture>();
                Performances = new Dictionary<string, Performance>();
                Sections = new Dictionary<string, Section>();
                Unknowns = new List<string>();
            }


            public float Depth {
                get { return float.Parse(Fields[FieldNames.Depth]); }
                set { Fields[FieldNames.Depth] = value.ToString(); }
            }


            /// <summary>
            /// Draws the store to a Bitmap.
            /// </summary>
            /// <param name="scaleInchesPerPixel">Number of inches per pixel used as drawing scale</param>
            /// <returns>Bitmap containing drawing of store</returns>
            //public Bitmap Draw(float scaleInchesPerPixel) {
            //    int width = (int)(Width / scaleInchesPerPixel);
            //    int height = (int)(Depth / scaleInchesPerPixel);
            //    Bitmap bmp = new Bitmap(width, height);
            //    Graphics dc = Graphics.FromImage(bmp);

            //    // create a white background
            //    dc.FillRectangle(new SolidBrush(Color.White), 0, 0, width, height);

            //    // draw each of the departments
            //    foreach (KeyValuePair<string, Department> dept in Departments) {
            //        // scale the rotated points of the department
            //        System.Drawing.PointF[] pts = new System.Drawing.PointF[dept.Value.Points.Count];
            //        for (int i = 0; i < dept.Value.Points.Count; i++) {
            //            pts[i].X = dept.Value.Points[i].X / scaleInchesPerPixel;
            //            pts[i].Y = dept.Value.Points[i].Y / scaleInchesPerPixel;
            //        }

            //        dc.FillPolygon(new SolidBrush(Color.FromArgb(255, dept.Value.Color.R, dept.Value.Color.G, dept.Value.Color.B)), pts);
            //        dc.DrawPolygon(Pens.Black, pts);
            //    }

            //    // draw each of the fixtures
            //    foreach (KeyValuePair<string, Fixture> fix in Fixtures) {
            //        // scale the rotated points of fixture
            //        System.Drawing.PointF[] pts = new System.Drawing.PointF[fix.Value.Points.Count];
            //        for (int i = 0; i < fix.Value.Points.Count; i++) {
            //            pts[i].X = fix.Value.Points[i].X / scaleInchesPerPixel;
            //            pts[i].Y = fix.Value.Points[i].Y / scaleInchesPerPixel;
            //        }

            //        dc.FillPolygon(new SolidBrush(Color.FromArgb(255, fix.Value.Color.R, fix.Value.Color.G, fix.Value.Color.B)), pts);
            //        dc.DrawPolygon(Pens.Black, pts);
            //    }

            //    // draw each of the sections
            //    foreach (KeyValuePair<string, Section> sec in Sections) {
            //        // scale the rotated points of section
            //        System.Drawing.PointF[] pts = new System.Drawing.PointF[sec.Value.Points.Count];
            //        for (int i = 0; i < sec.Value.Points.Count; i++) {
            //            pts[i].X = sec.Value.Points[i].X / scaleInchesPerPixel;
            //            pts[i].Y = sec.Value.Points[i].Y / scaleInchesPerPixel;
            //        }

            //        dc.FillPolygon(new SolidBrush(Color.FromArgb(255, sec.Value.Color.R, sec.Value.Color.G, sec.Value.Color.B)), pts);
            //        dc.DrawPolygon(Pens.Black, pts);
            //    }

            //    // Flip the image vertically because the JDA coordinate system puts (0,0) at the bottom left, while Windows puts it at the top left.
            //    bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);
            //    return bmp;
            //}


            public string Name {
                get { return Fields[FieldNames.Name]; }
                set { Fields[FieldNames.Name] = value.Substring(0, Math.Min(100, value.Length)); }
            }


            public static Floorplan ParsePFA(string line) {
                string[] fields = PFA.SplitLine(line);
                Floorplan obj = new Floorplan();

                for (int i = 0; (i < _fieldNames.Length) && (i + 1 < fields.Length); i++) {
                    obj.Fields[_fieldNames[i]] = fields[i + 1];
                }

                return obj;
            }


            public override string ToString() {
                return "Floorplan " + Name;
            }


            public float Width {
                get { return float.Parse(Fields[FieldNames.Width]); }
                set { Fields[FieldNames.Width] = value.ToString(); }
            }


            /// <summary>
            /// Generates the PFA output string representation for a floorplan.
            /// </summary>
            /// <returns>String</returns>
            public string WriteString() {
                StringBuilder sb = new StringBuilder();
                string replaceComma = "\\" + ",";

                sb.Append("Floorplan");
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

        }
    }

}
