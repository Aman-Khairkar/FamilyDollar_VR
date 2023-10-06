using System;
using System.Collections.Generic;
using System.Text;

namespace JDA
{
    public partial class PSA
    {
        /// <summary>
        /// Fixtures are the physical structures on a planogram.  Most fixtures can contain positions; however,
        /// Sign and obstruction fixtures cannot contain positions. 
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
                public const string Y = "Y";
                public const string Height = "Height";
                public const string Z = "Z";
                public const string Depth = "Depth";
                public const string Slope = "Slope";
                public const string Angle = "Angle";
                public const string Roll = "Roll";
                public const string Color = "Color";
                public const string Assembly = "Assembly";
                public const string XSpacing = "X spacing";
                public const string YSpacing = "Y spacing";
                public const string XStart = "X start";
                public const string YStart = "Y start";
                public const string WallWidth = "Wall width";
                public const string WallHeight = "Wall height";
                public const string WallDepth = "Wall depth";
                public const string Curve = "Curve";
                public const string Merch = "Merch";
                public const string CheckOtherFixtures = "Check other fixtures";
                public const string CheckOtherPositions = "Check other positions";
                public const string CanObstruct = "Can obstruct";
                public const string LeftOverhang = "Left overhang";
                public const string RightOverhang = "Right overhang";
                public const string LowerOverhang = "Lower overhang";
                public const string UpperOverhang = "Upper overhang";
                public const string BackOverhang = "Back overhang";
                public const string FrontOverhang = "Front overhang";
                public const string DefaultMerchStyle = "Default merch style";
                public const string DividerWidth = "Divider width";
                public const string DividerHeight = "Divider height";
                public const string DividerDepth = "Divider depth";
                public const string CanCombine = "Can combine";
                public const string GrilleHeight = "Grille Height";
                public const string NotchOffset = "Notch Offset";
                public const string XSpacing2 = "X spacing 2";
                public const string XStart2 = "X start 2";
                public const string PegDrop = "Peg drop";
                public const string PegGapX = "Peg gap X";
                public const string PegGapY = "Peg gap Y";
                public const string PrimaryFixtureLabelFormatName = "Primary fixture label format name";
                public const string SecondaryFixtureLabelFormatName = "Secondary fixture label format name";
                public const string ShapeID = "Shape ID";
                public const string BitmapID = "Bitmap ID";
                public const string MerchXMin = "MerchXMin";
                public const string MerchXMax = "MerchXMax";
                public const string MerchXUprights = "MerchXUprights";
                public const string MerchXCaps = "MerchXCaps";
                public const string MerchXPlacement = "MerchXPlacement";
                public const string MerchXNumber = "MerchXNumber";
                public const string MerchXSize = "MerchXSize";
                public const string MerchXDirection = "MerchXDirection";
                public const string MerchXSqueeze = "MerchXSqueeze";
                public const string MerchYMin = "MerchYMin";
                public const string MerchYMax = "MerchYMax";
                public const string MerchYUprights = "MerchYUprights";
                public const string MerchYCaps = "MerchYCaps";
                public const string MerchYPlacement = "MerchYPlacement";
                public const string MerchYNumber = "MerchYNumber";
                public const string MerchYSize = "MerchYSize";
                public const string MerchYDirection = "MerchYDirection";
                public const string MerchYSqueeze = "MerchYSqueeze";
                public const string MerchZMin = "MerchZMin";
                public const string MerchZMax = "MerchZMax";
                public const string MerchZUprights = "MerchZUprights";
                public const string MerchZCaps = "MerchZCaps";
                public const string MerchZPlacement = "MerchZPlacement";
                public const string MerchZNumber = "MerchZNumber";
                public const string MerchZSize = "MerchZSize";
                public const string MerchZDirection = "MerchZDirection";
                public const string MerchZSqueeze = "MerchZSqueeze";
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
                public const string WeightCapacity = "Weight Capacity";
                public const string Changed = "Changed";
                public const string DividerAtStart = "Divider at start";
                public const string DividerAtEnd = "Divider at end";
                public const string DividersBetweenFacings = "Dividers between facings";
                public const string Transparency = "Transparency";
                public const string HideIfPrinting = "Hide if printing";
                public const string ProductAssociation = "Product association";
                public const string PartID = "PartID";
                public const string HideViewDimensions = "Hide view dimensions";
                public const string GLN = "GLN";
                public const string CustomData = "Custom data";

            }
            # endregion


            public enum FixtureType { Shelf	= 0, Chest = 1, Bin	= 2, PolygonalShelf = 3, Rod = 4, LateralRod = 5, Bar = 6, Pegboard = 7, MultiRowPegboard = 8, CurvedRod = 9, Obstruction = 10, Sign = 11, GravityFeed = 12 };

            private static int counter = 0;                     // counter used to generate a unique ID for each fixture

            public List<Divider> Dividers;                      // list of the dividers that are part of this fixture
            public Dictionary<string, string> Fields;           // Data for each field read from the fixture line of the PFA file
            public Planogram Planogram;                         // the planogram object this fixture belongs to
            public List<Point3D> Points;                        // list of points describing the polygonal shape of PolygonalShelf fixture, as they were read from PSA file
            public List<Position> Positions;                    // Positions associated with this fixture
            private string _uniqueID;                           // Unique identifier for this fixture

            #region data fields
            private static string[] _fieldNames = new string[]
        {
            "Type",
            "Name",
            "Key",
            "X",
            "Width",
            "Y",
            "Height",
            "Z",
            "Depth",
            "Slope",
            "Angle",
            "Roll",
            "Color",
            "Assembly",
            "X spacing",
            "Y spacing",
            "X start",
            "Y start",
            "Wall width",
            "Wall height",
            "Wall depth",
            "Curve",
            "Merch",
            "Check other fixtures",
            "Check other positions",
            "Can obstruct",
            "Left overhang",
            "Right overhang",
            "Lower overhang",
            "Upper overhang",
            "Back overhang",
            "Front overhang",
            "Default merch style",
            "Divider width",
            "Divider height",
            "Divider depth",
            "Can combine",
            "Grille Height",
            "Notch Offset",
            "X spacing 2",
            "X start 2",
            "Peg drop",
            "Peg gap X",
            "Peg gap Y",
            "Primary fixture label format name",
            "Secondary fixture label format name",
            "Shape ID",
            "Bitmap ID",
            "MerchXMin",
            "MerchXMax",
            "MerchXUprights",
            "MerchXCaps",
            "MerchXPlacement",
            "MerchXNumber",
            "MerchXSize",
            "MerchXDirection",
            "MerchXSqueeze",
            "MerchYMin",
            "MerchYMax",
            "MerchYUprights",
            "MerchYCaps",
            "MerchYPlacement",
            "MerchYNumber",
            "MerchYSize",
            "MerchYDirection",
            "MerchYSqueeze",
            "MerchZMin",
            "MerchZMax",
            "MerchZUprights",
            "MerchZCaps",
            "MerchZPlacement",
            "MerchZNumber",
            "MerchZSize",
            "MerchZDirection",
            "MerchZSqueeze",
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
            "Weight Capacity",
            "Changed",
            "Divider at start",
            "Divider at end",
            "Dividers between facings",
            "Transparency",
            "Hide if printing",
            "Product association",
            "PartID",
            "Hide view dimensions",
            "GLN",
            "Custom data"
        };
            #endregion

            public Fixture() {
                Dividers = new List<Divider>();
                Fields = new Dictionary<string, string>();
                Points = new List<Point3D>();
                Positions = new List<Position>();
                _uniqueID = (++counter).ToString();
            }


            public bool CanCombine {
                get { return (Fields[FieldNames.CanCombine] == "1"); }
                set { Fields[FieldNames.CanCombine] = (value ? "1" : "0"); }
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


            public static Fixture ParsePSA(string line) {
                string[] fields = SplitLine(line);
                Fixture obj = new Fixture();

                for (int i = 0; (i < _fieldNames.Length) && (i + 1 < fields.Length); i++) {
                    obj.Fields[_fieldNames[i]] = fields[i + 1];
                }

                return obj;
            }


            public override string ToString() {
                return "PSA Fixture " + Name;
            }


            public FixtureType Type {
                get {
                    switch (Fields[FieldNames.Type]) {
                        case "0": return FixtureType.Shelf;
                        case "1": return FixtureType.Chest;
                        case "2": return FixtureType.Bin;
                        case "3": return FixtureType.PolygonalShelf;
                        case "4": return FixtureType.Rod;
                        case "5": return FixtureType.LateralRod;
                        case "6": return FixtureType.Bar;
                        case "7": return FixtureType.Pegboard;
                        case "8": return FixtureType.MultiRowPegboard;
                        case "9": return FixtureType.CurvedRod;
                        case "10": return FixtureType.Obstruction;
                        case "11": return FixtureType.Sign;
                        default: return FixtureType.GravityFeed;
                    }

                }            
                set { Fields[FieldNames.Type] = value.ToString(); }
            }


            public string UniqueID {
                get { return _uniqueID; }
            }


            public float Width {
                get { return float.Parse(Fields[FieldNames.Width]); }
                set { Fields[FieldNames.Width] = value.ToString(); }
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
                    sb.Append(PSA.PsaDelimiters[0]);
                    if (Fields.ContainsKey(_fieldNames[i])) {
                        string val = Fields[_fieldNames[i]];
                        if (!val.Contains(replaceComma) && val.Contains(",")) {
                            val = val.Replace(",", replaceComma);
                        }
                        sb.Append(val);
                    }
                }

                if ((Points != null) && (Points.Count > 0)) {
                    sb.AppendLine();
                    foreach (Point3D pt in Points) sb.AppendLine(pt.WriteString());
                }

                if ((Dividers != null) && (Dividers.Count > 0)) {
                    sb.AppendLine();
                    foreach (Divider d in Dividers) sb.AppendLine(d.WriteString());
                }

                if ((Positions != null) && (Positions.Count > 0)) {
                    sb.AppendLine();
                    foreach (Position p in Positions) sb.AppendLine(p.WriteString());
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
