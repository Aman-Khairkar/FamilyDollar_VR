using System;
using System.Collections.Generic;
using System.Text;

namespace JDA
{
    public partial class PFA
    {
        /// <summary>
        /// Sections are the physical representations of planogram segments.  A planogram may contain multiple sections.
        /// </summary>
        public class Section
        {

            # region Field Names
            public class FieldNames
            {
                public const string Name = "Name";
                public const string ID = "ID";
                public const string Key = "Key";
                public const string X = "X";
                public const string Width = "Width";
                public const string Z = "Z";
                public const string Height = "Height";
                public const string Y = "Y";
                public const string Depth = "Depth";
                public const string RankX = "Rank X";
                public const string RankZ = "Rank Z";
                public const string RankY = "Rank Y";
                public const string PrimarySectionLabelFormatName = "Primary section label format name";
                public const string SecondarySectionLabelFormatName = "Secondary section label format name";
                public const string MerchXSize = "MerchXSize";
                public const string MerchDepth = "Merch Depth";
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
                public const string WidthOverride = "Width override";
                public const string LocationID = "Location ID";
                public const string Changed = "Changed";
                public const string DepartmentOverride = "Department Override";
                public const string DepthOverride = "Depth override";
                public const string DatePending = "Date Pending";
                public const string DateLive = "Date Live";
                public const string DateFinished = "Date Finished";
                public const string HeightOverride = "Height override";
                public const string TextureOverride = "Texture Override";
                public const string PartID = "Part ID";
                public const string GLN = "GLN";
                public const string Angle = "Angle";
                public const string SegmentStart = "Segment Start";
                public const string SegmentEnd = "Segment End";

            }
            # endregion


            private static int counter = 0;                     // used in the generation of unique IDs

            public Fixture AnchoringFixture;                    // fixture where the origin of section is located
            public int AngleR;                                  // regularized angle of section; cleans up some of the slop in angles from JDA software
            public string Category;                             // name of category associated with this section
            public Dictionary<string, string> Fields;           // Data for each field read from the section line of the PFA file  
            public Planogram Planogram;                         // planogram associated with this section
            private List<PointF> _points;                       // list of rotated points describing shape of section
            private string _uniqueID;                           // unique ID for this section

            #region data fields
            private static string[] _fieldNames = new string[]
        {
            "Name",
            "ID",
            "Key",
            "X",
            "Width",
            "Z",
            "Height",
            "Y",
            "Depth",
            "Rank X",
            "Rank Z",
            "Rank Y",
            "Primary section label format name",
            "Secondary section label format name",
            "MerchXSize",
            "Merch Depth",
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
            "Width override",
            "Location ID",
            "Changed",
            "Department Override",
            "Depth override",
            "Date Pending",
            "Date Live",
            "Date Finished",
            "Height override",
            "Texture Override",
            "Part ID",
            "GLN",
            "Angle",
            "Segment Start",
            "Segment End"
        };
            #endregion

            public Section() {
                Fields = new Dictionary<string, string>();
                _points = new List<PointF>();
                _uniqueID = (++counter).ToString();
            }


            public float Angle {
                get { return float.Parse(Fields[FieldNames.Angle]); }
                set {
                    float angle = value;

                    // normalize Angle between 0 and 360
                    while (angle >= 360) angle -= 360;
                    while (angle < 0) angle += 360;
                    Fields[FieldNames.Angle] = angle.ToString();

                    // regularize the angle
                    AngleR = (int)angle;
                    if ((-10 <= Angle) && (Angle <= 10)) AngleR = 0;
                    else if ((80 <= Angle) && (Angle <= 100)) AngleR = 90;
                    else if ((170 <= Angle) && (Angle <= 190)) AngleR = 180;
                    else if ((260 <= Angle) && (Angle <= 280)) AngleR = 270;
                }
            }


            /// <summary>
            /// Computes the bounding rectangle for the rotated section.
            /// </summary>
            /// <returns>Bounding RectangleF</returns>
            public RectangleF BoundingRect() {
                PointF p0 = _points[0];
                PointF p1 = _points[1];
                PointF p2 = _points[2];
                PointF p3 = _points[3];

                // Find the bounds
                float minX = Math.Min(p0.X, Math.Min(p1.X, Math.Min(p2.X, p3.X)));
                float minY = Math.Min(p0.Y, Math.Min(p1.Y, Math.Min(p2.Y, p3.Y)));
                float maxX = Math.Max(p0.X, Math.Max(p1.X, Math.Max(p2.X, p3.X)));
                float maxY = Math.Max(p0.Y, Math.Max(p1.Y, Math.Max(p2.Y, p3.Y)));

                return new RectangleF(minX, minY, maxX - minX, maxY - minY);
            }


            public PfaColor Color {
                get { return Planogram.Color; }
            }


            /// <summary>
            /// Computes the rotated points for the section
            /// </summary>
            public void ComputeRotatedPoints() {
                ComputeRotatedPointsAboutOrigin();
                ComputeRotatedPointsAboutAnchor();
            }


            /// <summary>
            /// Computes the rotated points about the anchoring fixture.  Assumes the section has already
            /// been rotated by ComputeRotatedPointsAboutOrigin().
            /// </summary>
            public void ComputeRotatedPointsAboutAnchor() {
                List<PointF> rotPts = new List<PointF>();

                foreach (PointF pt in _points) {
                    rotPts.Add(PFA.RotateAboutPoint(pt.X, pt.Y, AnchoringFixture.AngleR, AnchoringFixture.X, AnchoringFixture.Y));
                }

                _points = rotPts;
            }


            /// <summary>
            /// Computes the rotated points about the origin for the section.
            /// </summary>
            public void ComputeRotatedPointsAboutOrigin() {
                // rotate a rectangle the size of section about (0, 0)
                PointF p1 = PFA.RotatePoint(Width, 0, AngleR);
                PointF p2 = PFA.RotatePoint(Width, Depth, AngleR);
                PointF p3 = PFA.RotatePoint(0, Depth, AngleR);

                // translate the coordinates to the origin of the section
                PointF p0 = new PointF(X, Y);
                p1.X += X; p1.Y += Y;
                p2.X += X; p2.Y += Y;
                p3.X += X; p3.Y += Y;

                // Add points to the _RotatedPoints list
                _points.Clear();
                _points.Add(p0);
                _points.Add(p1);
                _points.Add(p2);
                _points.Add(p3);
            }


            public float Depth {
                get { return float.Parse(Fields[FieldNames.Depth]); }
                set { Fields[FieldNames.Depth] = value.ToString("F2"); }
            }


            public float Height {
                get { return float.Parse(Fields[FieldNames.Height]); }
                set { Fields[FieldNames.Height] = value.ToString("F2"); }
            }


            public string Key {
                get { return Fields[FieldNames.Key]; }
            }


            public string Name {
                get { return Fields[FieldNames.Name]; }
                set { Fields[FieldNames.Name] = value.Substring(0, Math.Min(100, value.Length)); }
            }


            public static Section ParsePFA(string line) {
                string[] fields = PFA.SplitLine(line);
                Section obj = new Section();

                for (int i = 0; (i < _fieldNames.Length) && (i + 1 < fields.Length); i++) {
                    obj.Fields[_fieldNames[i]] = fields[i + 1];
                }

                // normalize Angle between 0 and 360
                obj.Angle = float.Parse(obj.Fields[FieldNames.Angle]);
                while (obj.Angle >= 360) obj.Angle -= 360;
                while (obj.Angle < 0) obj.Angle += 360;

                // regularize the angle
                obj.AngleR = (int)obj.Angle;
                if ((-10 <= obj.Angle) && (obj.Angle <= 10)) obj.AngleR = 0;
                else if ((80 <= obj.Angle) && (obj.Angle <= 100)) obj.AngleR = 90;
                else if ((170 <= obj.Angle) && (obj.Angle <= 190)) obj.AngleR = 180;
                else if ((260 <= obj.Angle) && (obj.Angle <= 280)) obj.AngleR = 270;

                return obj;
            }


            /// <summary>
            /// Returns the list of points for the polygon representing of the section.
            /// </summary>
            public List<PointF> Points {
                get {
                    if ((AnchoringFixture != null) && (AnchoringFixture.Type == Fixture.FixtureType.Irregular)) return AnchoringFixture.Points;
                    else return _points;
                }
            }


            public float SegmentEnd {
                get { return int.Parse(Fields[FieldNames.SegmentEnd]); }
                set { Fields[FieldNames.SegmentEnd] = value.ToString(); }
            }


            public float SegmentStart {
                get { return int.Parse(Fields[FieldNames.SegmentStart]); }
                set { Fields[FieldNames.SegmentStart] = value.ToString(); }
            }


            public override string ToString() {
                return "Section " + Name;
            }


            public string UniqueID {
                get { return _uniqueID; }
            }


            public float Width {
                get { return float.Parse(Fields[FieldNames.Width]); }
                set { Fields[FieldNames.Width] = value.ToString("F2"); }
            }


            /// <summary>
            /// Generates the PFA output string representation for a section.
            /// </summary>
            /// <returns>String</returns>
            public string WriteString() {
                StringBuilder sb = new StringBuilder();
                string replaceComma = "\\" + ",";

                sb.Append("Section");
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
                set { Fields[FieldNames.X] = value.ToString("F2"); }
            }


            public float Y {
                get { return float.Parse(Fields[FieldNames.Y]); }
                set { Fields[FieldNames.Y] = value.ToString("F2"); }
            }


            public float Z {
                get { return float.Parse(Fields[FieldNames.Z]); }
                set { Fields[FieldNames.Z] = value.ToString("F2"); }
            }

        }
    }
}
