using System;
using System.Collections.Generic;
using System.Text;

namespace JDA
{
    public partial class PSA
    {
        /// <summary>
        /// Planograms represent a group of merchandise on a fixture or collection of fixture elements.
        /// </summary>
        public class Planogram
        {

            # region Field Names
            public class FieldNames
            {
                public const string Name = "Name";
                public const string Key = "Key";
                public const string Width = "Width";
                public const string Height = "Height";
                public const string Depth = "Depth";
                public const string Color = "Color";
                public const string BackDepth = "Back depth";
                public const string DrawBack = "Draw back";
                public const string BaseWidth = "Base width";
                public const string BaseHeight = "Base height";
                public const string BaseDepth = "Base depth";
                public const string DrawBase = "Draw base";
                public const string BaseColor = "Base color";
                public const string DrawNotches = "Draw notches";
                public const string NotchOffset = "Notch offset";
                public const string NotchSpacing = "Notch spacing";
                public const string DoubleNotches = "Double notches";
                public const string NotchColor = "Notch color";
                public const string NotchMarks = "Notch marks";
                public const string DrawPegs = "Draw pegs";
                public const string DrawPegHoles = "Draw pegholes";
                public const string TrafficFlow = "Traffic flow";
                public const string AutoCreated = "Auto created";
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
                public const string CombinedPerformanceIndex = "Combined performance index";
                public const string NumberOfStores = "Number of Stores";
                public const string NotchWidth = "Notch width";
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
                public const string SegmentsToPrint = "Segments to print";
                public const string Filename = "File name";
                public const string Changed = "Changed";
                public const string LayoutFilename = "Layout file name";
                public const string Notes = "Notes";
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
                public const string SourceFileType = "Source file type";
                public const string Status1 = "Status 1";
                public const string Status2 = "Status 2";
                public const string Status3 = "Status 3";
                public const string DateCreated = "Date created";
                public const string DateModified = "Date modified";
                public const string DatePending = "Date pending";
                public const string DateEffective = "Date effective";
                public const string DateFinished = "Date finished";
                public const string Date1 = "Date 1";
                public const string Date2 = "Date 2";
                public const string Date3 = "Date 3";
                public const string CreatedBy = "Created by";
                public const string ModifiedBy = "Modified by";
                public const string FloorBitmapID = "Floor bitmap ID";
                public const string DoorTransparency = "Door transparency";
                public const string FloorTileWidth = "Floor tile width";
                public const string FloorTileDepth = "Floor tile depth";
                public const string InventoryModel_Manual = "Inventory model - Manual";
                public const string InventoryModel_CaseMultiple = "Inventory model - Case Multiple";
                public const string InventoryModel_DaysSupply = "Inventory model - Days Supply";
                public const string InventoryModel_Peak = "Inventory model - Peak";
                public const string InventoryModel_MinUnits = "Inventory model - Min units";
                public const string InventoryModel_MaxUnits = "Inventory model - Max units";
                public const string CaseMultiple = "Case multiple";
                public const string DaysSupply = "Days supply";
                public const string DemandCycleLength = "Demand cycle length";
                public const string PeakSafetyFactor = "Peak safety factor";
                public const string BackroomStock = "Backroom stock";
                public const string Demand1 = "Demand 1";
                public const string Demand2 = "Demand 2";
                public const string Demand3 = "Demand 3";
                public const string Demand4 = "Demand 4";
                public const string Demand5 = "Demand 5";
                public const string Demand6 = "Demand 6";
                public const string Demand7 = "Demand 7";
                public const string DeliverySchedule = "Delivery schedule";
                public const string ID = "ID";
                public const string Department = "Department";
                public const string PartID = "PartID";
                public const string GLN = "GLN";
                public const string CustomData = "Custom data";
                public const string PlanogramGUID = "Planogram GUID";
                public const string DBGUID = "DBGUID";
                public const string AbbrevName = "Abbrev name";
                public const string Category = "Category";
                public const string Subcategory = "Subcategory";
                public const string Source = "Source";
                public const string AllocationGroup = "Allocation Group";
                public const string AllocationSequence = "Allocation Sequence";
                public const string AllocationTargetMin = "Allocation Target Min";
                public const string AllocationTargetMax = "Allocation Target Max";
                public const string CanSegment = "Can Segment";
                public const string CanSplit = "Can Split";
                public const string PGStatus = "PG Status";
                public const string PGScorePercent = "PG Score Percent";
                public const string PGScoreNote = "PG Score Note";
                public const string PGWarningsCount = "PG Warnings Count";
                public const string PGErrorsCount = "PG Errors Count";
                public const string PGActionList = "PG Action List";
                public const string PGMaxStageReduce = "PG Max Stage Reduce";
                public const string PGMaxStageFillOut = "PG Max Stage Fill Out";
                public const string PGType = "PG Type";
                public const string ModelFilename = "Model Filename";
                public const string DBFamilyKey = "DBFamilyKey";
                public const string DBReplaceKey = "DBReplaceKey";
                public const string DBVersionKey = "DBVersionKey";
                public const string DBParentPGAuxTemplateKey = "DBParentPGAuxTemplateKey";
                public const string DBParentPGSourceKey = "DBParentPGSourceKey";
                public const string DBParentPGTemplateKey = "DBParentPGTemplateKey";
                public const string DBPGTimeDone = "DBPGTimeDone";
                public const string PGServerName = "PG Server Name";
                public const string PRStatus = "PR Status";
                public const string MovementPeriod = "Movement period";

            }
            # endregion


            public List<Drawing> Drawings;                          // Drawings associated with planogram
            public Dictionary<string, string> Fields;               // Data for each field read from the planogram line of the PSA file
            public Dictionary<string, Fixture> Fixtures;            // Dictionary mapping fixture names to Fixture objects associated with the planogram
            public Dictionary<string, Performance> Performances;    // Dictionary mapping UPC codes to Performance objects associated with the planogram
            public List<Position> Positions;                        // Positions not associated with a fixture
            public List<Segment> Segments;                          // Segments associated with planogram
            public Dictionary<string, Supplier> Suppliers;          // Suppliers associated with planogram
            public List<string> Unknowns;                           // Unrecognized lines during parsing

            #region data fields
            private static string[] _fieldNames = new string[]
        {
            "Name",
            "Key",
            "Width",
            "Height",
            "Depth",
            "Color",
            "Back depth",
            "Draw back",
            "Base width",
            "Base height",
            "Base depth",
            "Draw base",
            "Base color",
            "Draw notches",
            "Notch offset",
            "Notch spacing",
            "Double notches",
            "Notch color",
            "Notch marks",
            "Draw pegs",
            "Draw pegholes",
            "Traffic flow",
            "Auto created",
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
            "Combined performance index",
            "Number of Stores",
            "Notch width",
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
            "Segments to print",
            "File name",
            "Changed",
            "Layout file name",
            "Notes",
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
            "Source file type",
            "Status 1",
            "Status 2",
            "Status 3",
            "Date created",
            "Date modified",
            "Date pending",
            "Date effective",
            "Date finished",
            "Date 1",
            "Date 2",
            "Date 3",
            "Created by",
            "Modified by",
            "Floor bitmap ID",
            "Door transparency",
            "Floor tile width",
            "Floor tile depth",
            "Inventory model - Manual",
            "Inventory model - Case Multiple",
            "Inventory model - Days Supply",
            "Inventory model - Peak",
            "Inventory model - Min units",
            "Inventory model - Max units",
            "Case multiple",
            "Days supply",
            "Demand cycle length",
            "Peak safety factor",
            "Backroom stock",
            "Demand 1",
            "Demand 2",
            "Demand 3",
            "Demand 4",
            "Demand 5",
            "Demand 6",
            "Demand 7",
            "Delivery schedule",
            "ID",
            "Department",
            "PartID",
            "GLN",
        /*    "Custom data", */
            "Planogram GUID",
            "DBGUID",
            "Abbrev name",
            "Category",
            "Subcategory",
            "Source",
            "Allocation Group",
            "Allocation Sequence",
            "Allocation Target Min",
            "Allocation Target Max",
            "Can Segment",
            "Can Split",
            "PG Status",
            "PG Score Percent",
            "PG Score Note",
            "PG Warnings Count",
            "PG Errors Count",
            "PG Action List",
            "PG Max Stage Reduce",
            "PG Max Stage Fill Out",
            "PG Type",
            "Model Filename",
            "DBFamilyKey",
            "DBReplaceKey",
            "DBVersionKey",
            "DBParentPGAuxTemplateKey",
            "DBParentPGSourceKey",
            "DBParentPGTemplateKey",
            "DBPGTimeDone",
            "PG Server Name",
            "PR Status",
            "Movement period"
        };
            #endregion


            public Planogram() {
                Drawings = new List<Drawing>();
                Fields = new Dictionary<string, string>();
                Fixtures = new Dictionary<string, Fixture>();
                Performances = new Dictionary<string, Performance>();
                Positions = new List<Position>();
                Segments = new List<Segment>();
                Suppliers = new Dictionary<string, Supplier>();
                Unknowns = new List<string>();
            }


            /// <summary>
            /// Merges the lists of Positions for the Fixtures and Planogram.
            /// </summary>
            /// <returns></returns>
            public List<Position> AllPositions() {
                List<Position> answer = new List<Position>();
                answer.AddRange(Positions);
                foreach (KeyValuePair<string, Fixture> pair in Fixtures) {
                    answer.AddRange(pair.Value.Positions);
                }
                return answer;
            }


            public string Category {
                get { return Fields[FieldNames.Category]; }
                set { Fields[FieldNames.Category] = value.Substring(0, Math.Min(1000, value.Length)); }
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


            public string Department {
                get { return Fields[FieldNames.Department]; }
                set { Fields[FieldNames.Department] = value.Substring(0, Math.Min(1000, value.Length)); }
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


            public static Planogram ParsePSA(string line) {
                string[] fields = SplitLine(line);
                Planogram obj = new Planogram();

                for (int i = 0; (i < _fieldNames.Length) && (i + 1 < fields.Length); i++) {
                    obj.Fields[_fieldNames[i]] = fields[i + 1];
                }

                return obj;
            }


            public string Subcategory {
                get { return Fields[FieldNames.Subcategory]; }
                set { Fields[FieldNames.Subcategory] = value.Substring(0, Math.Min(1000, value.Length)); }
            }


            public override string ToString() {
                return "PSA Planogram " + Name;
            }


            /// <summary>
            /// Finds all of the products that are in a planogram, but are unused (not assigned to a Position).
            /// </summary>
            /// <returns>Returns a list of TheProject.Products dictionary keys for the unused products.</returns>
            public List<string> UnusedProducts() {
                List<Position> allPositions = AllPositions();
                List<string> answer = new List<string>();

                foreach (KeyValuePair<string, Performance> perf in Performances) {
                    // search for the product associated with perf in the allPositions list
                    Position pos = allPositions.Find((a) => a.UPC == perf.Value.UPC && a.ID == perf.Value.ID);
                    if (pos == null) answer.Add(Product.CreateDictionaryKey(perf.Value.ID, perf.Value.UPC));
                }

                return answer;
            }


            public float Width {
                get { return float.Parse(Fields[FieldNames.Width]); }
                set { Fields[FieldNames.Width] = value.ToString(); }
            }


            /// <summary>
            /// Generates the PFA output string representation for a planogram.
            /// </summary>
            /// <returns>String</returns>
            public string WriteString() {
                StringBuilder sb = new StringBuilder();
                string replaceComma = "\\" + ",";

                sb.Append("Planogram");
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
