using System;
using System.Collections.Generic;
using System.Text;

namespace JDA
{
    public partial class PFA
    {
        /// <summary>
        /// Fixtures are the physical structures on a floor plan.  Generally, they are gondolas, but they
        /// can also be obstructions.
        /// </summary>
        public class Fixture
        {

            # region Field Names
            public class FieldNames
            {
                public const string Type = "Type";
                public const string Name = "Name";
                public const string Key = "Key";
                public const string X = "X";
                public const string Width = "Width";
                public const string Z = "Z";
                public const string Height = "Height";
                public const string Y = "Y";
                public const string Depth = "Depth";
                public const string Angle = "Angle";
                public const string Color = "Color";
                public const string Assembly = "Assembly";
                public const string TileTextures = "Tile Textures";
                public const string Draw3DFrontOnly = "Draw 3D front only";
                public const string TextureAllFaces = "Texture All Faces";
                public const string LeftOverhang = "Left overhang";
                public const string RightOverhang = "Right overhang";
                public const string BackOverhang = "Back overhang";
                public const string FrontOverhang = "Front overhang";
                public const string CanCombine = "Can combine";
                public const string PrimaryFixtureLabelFormatName = "Primary fixture label format name";
                public const string SecondaryFixtureLabelFormatName = "Secondary fixture label format name";
                public const string ShapeID = "Shape ID";
                public const string BitmapID = "Bitmap ID";
                public const string SectionPlacement = "Section Placement";
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
                public const string LocationID = "Location ID";
                public const string FillPattern = "Fill pattern";
                public const string ModelFilename = "Model Filename";
                public const string Changed = "Changed";
                public const string AisleSpaceLeft = "Aisle space left";
                public const string AisleSpaceRight = "Aisle space right";
                public const string AisleSpaceBack = "Aisle space back";
                public const string AisleSpaceFront = "Aisle space front";
                public const string CanBeMoved = "Can Be Moved";
                public const string Transparency = "Transparency";
                public const string DatePending = "Date Pending";
                public const string DateLive = "Date Live";
                public const string DateFinished = "Date Finished";
                public const string TickWidth = "Tick Width";
                public const string TileX = "Tile X";
                public const string TileZ = "Tile Z";
                public const string TileY = "Tile Y";
                public const string TextureTransparency = "Texture Transparency";
                public const string TextureTransparentColor = "Texture Transparent color";
                public const string PartID = "Part ID";
                public const string CADBlockName = "CAD Block Name";
                public const string GLN = "GLN";
                public const string Merchandising = "Merchandising";
                public const string AllocationGroup = "Allocation Group";
                public const string AllocationSequence = "Allocation Sequence";
                public const string AllocationDirection = "Allocation Direction";
                public const string CanbeAllocated = "Can be Allocated";

            }
            # endregion


            public enum FixtureType { Regular = 0, Irregular = 1, Obstruction = 2 };
            public enum Placement { Normal = 0, Reversed = 1 };

            private static int counter = 0;                     // counter used to generate a unique ID for each fixture

            public int AngleR;                                  // regularized angle of fixture
            public Dictionary<string, string> Fields;           // Data for each field read from the fixture line of the PFA file
            public Floorplan Floorplan;                         // the floorplan object this fixture belongs to
            public List<Point3D> PointsFromFile;                // list of points describing the polygonal shape of non-regular fixture, as they were read from PFA file
            public List<PointF> Points;                         // list of rotated points describing actual shape of fixture (even for regular fixtures)
            public List<Section> Sections;                      // list of the section objects anchored by this fixture
            private string _uniqueID;                           // Unique identifier for this fixture

            #region data fields
            private static string[] _fieldNames = new string[]
        {
            "Type",
            "Name",
            "Key",
            "X",
            "Width",
            "Z",
            "Height",
            "Y",
            "Depth",
            "Angle",
            "Color",
            "Assembly",
            "Tile Textures",
            "Draw 3D front only",
            "Texture All Faces",
            "Left overhang",
            "Right overhang",
            "Back overhang",
            "Front overhang",
            "Can combine",
            "Primary fixture label format name",
            "Secondary fixture label format name",
            "Shape ID",
            "Bitmap ID",
            "Section Placement",
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
            "Location ID",
            "Fill pattern",
            "Model Filename",
            "Changed",
            "Aisle space left",
            "Aisle space right",
            "Aisle space back",
            "Aisle space front",
            "Can Be Moved",
            "Transparency",
            "Date Pending",
            "Date Live",
            "Date Finished",
            "Tick Width",
            "Tile X",
            "Tile Z",
            "Tile Y",
            "Texture Transparency",
            "Texture Transparent color",
            "Part ID",
            "CAD Block Name",
            "GLN",
            "Merchandising",
            "Allocation Group",
            "Allocation Sequence",
            "Allocation Direction",
            "Can be Allocated"
        };
            #endregion

            public Fixture() {
                Fields = new Dictionary<string, string>();
                PointsFromFile = new List<Point3D>();
                Points = new List<PointF>();
                Sections = new List<Section>();
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


            public bool CanCombine {
                get { return (Fields[FieldNames.CanCombine] == "1"); }
                set { Fields[FieldNames.CanCombine] = (value ? "1" : "0"); }
            }


            /// <summary>
            /// Computes the bounding rectangle for the rotated fixture.
            /// </summary>
            /// <returns>Bounding RectangleF</returns>
            public RectangleF BoundingRect() {
                float minX = float.MaxValue;
                float maxX = float.MinValue;
                float minY = float.MaxValue;
                float maxY = float.MinValue;

                foreach (PointF pt in Points) {
                    if (pt.X > maxX) maxX = pt.X;
                    if (pt.X < minX) minX = pt.X;
                    if (pt.Y > maxY) maxY = pt.Y;
                    if (pt.Y < minY) minY = pt.Y;
                }

                return new RectangleF(minX, minY, maxX - minX, maxY - minY);
            }


            /// <summary>
            /// Gets/Sets the Color assigned to this fixture
            /// </summary>
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
            /// Calculates the rotated points for the fixture.
            /// </summary>
            public void ComputeRotatedPoints() {

                if (Type == FixtureType.Regular) {
                    // rotate a rectangle the size of fixture about (0, 0)
                    PointF p1 = PFA.RotatePoint(Width, 0, Angle);
                    PointF p2 = PFA.RotatePoint(Width, Depth, Angle);
                    PointF p3 = PFA.RotatePoint(0, Depth, Angle);

                    // translate the coordinates to the origin of the rectangle
                    PointF p0 = new PointF(X, Y);
                    p1.X += X; p1.Y += Y;
                    p2.X += X; p2.Y += Y;
                    p3.X += X; p3.Y += Y;

                    // Add points to the RealPoints list
                    Points.Add(p0);
                    Points.Add(p1);
                    Points.Add(p2);
                    Points.Add(p3);

                } else {
                    // rotate the points about the fixture origin
                    foreach (Point3D pt in this.PointsFromFile) {
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
            }


            public float Depth {
                get { return float.Parse(Fields[FieldNames.Depth]); }
                set { Fields[FieldNames.Depth] = value.ToString("F2"); }
            }


            public float Height {
                get { return float.Parse(Fields[FieldNames.Height]); }
                set { Fields[FieldNames.Height] = value.ToString("F2"); }
            }


            public string Name {
                get { return Fields[FieldNames.Name]; }
                set { Fields[FieldNames.Name] = value.Substring(0, Math.Min(100, value.Length)); }
            }


            public static Fixture ParsePFA(string line) {
                string[] fields = PFA.SplitLine(line);
                Fixture obj = new Fixture();

                for (int i = 0; (i < _fieldNames.Length) && (i + 1 < fields.Length); i++) {
                    obj.Fields[_fieldNames[i]] = fields[i + 1];
                }

                obj.Angle = float.Parse(obj.Fields[FieldNames.Angle]);

                return obj;
            }


            /// <summary>
            /// Returns the unrotated bounding rectangle for the fixture
            /// </summary>
            public RectangleF Rectangle {
                get {
                    return new RectangleF(X, Y, Width, Depth);
                }
            }


            public Placement SectionPlacement {
                get { return (Fields[FieldNames.SectionPlacement] == "0" ? Placement.Normal : Placement.Reversed); }
                set { Fields[FieldNames.SectionPlacement] = (value == Placement.Normal ? "0" : "1"); }
            }


            public override string ToString() {
                return "Fixture " + Name;
            }


            public FixtureType Type {
                get {
                    return (Fields[FieldNames.Type] == "0" ?
                        FixtureType.Regular :
                        (Fields[FieldNames.Type] == "1" ? FixtureType.Irregular : FixtureType.Obstruction));
                }
                set { Fields[FieldNames.Type] = (value == FixtureType.Regular ? "0" : (value == FixtureType.Irregular ? "1" : "2")); }
            }


            public string UniqueID {
                get { return _uniqueID; }
            }


            public float Width {
                get { return float.Parse(Fields[FieldNames.Width]); }
                set { Fields[FieldNames.Width] = value.ToString("F2"); }
            }


            /// <summary>
            /// Generates the PFA output string representation for a fixture.
            /// </summary>
            /// <returns>String</returns>
            public string WriteString() {
                StringBuilder sb = new StringBuilder();
                string replaceComma = "\\" + ",";

                sb.Append("Fixture");
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
