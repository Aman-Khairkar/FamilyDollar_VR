using System;
using System.Collections.Generic;
using System.Text;

namespace JDA
{
    public partial class PSA
    {

        /// <summary>
        /// Products represent merchandise that can be placed on a planogram.
        /// </summary>
        public class Product
        {

            # region Field Names
            public class FieldNames
            {
                public const string UPC = "UPC";
                public const string ID = "ID";
                public const string Name = "Name";
                public const string Key = "Key";
                public const string Width = "Width";
                public const string Height = "Height";
                public const string Depth = "Depth";
                public const string Color = "Color";
                public const string AbbrevName = "Abbrev name";
                public const string Size = "Size";
                public const string UOM = "UOM";
                public const string Manufacturer = "Manufacturer";
                public const string Category = "Category";
                public const string Supplier = "Supplier";
                public const string InnerPack = "Inner pack";
                public const string XNesting = "X nesting";
                public const string YNesting = "Y nesting";
                public const string ZNesting = "Z nesting";
                public const string PegHoles = "Pegholes";
                public const string PegHoleX = "Peghole X";
                public const string PegHoleY = "Peghole Y";
                public const string PegHoleWidth = "Peghole width";
                public const string PegHole2X = "Peghole 2 X";
                public const string PegHole2Y = "Peghole 2 Y";
                public const string PegHole2Width = "Peghole 2 width";
                public const string PegHole3X = "Peghole 3 X";
                public const string PegHole3Y = "Peghole 3 Y";
                public const string PegHole3Width = "Peghole 3 width";
                public const string PackageStyle = "Package style";
                public const string PegID = "Peg ID";
                public const string FingerSpaceY = "Finger space Y";
                public const string JumbleFactor = "Jumble factor";
                public const string Price = "Price";
                public const string CaseCost = "Case cost";
                public const string TaxCode = "Tax code";
                public const string UnitMovement = "Unit movement";
                public const string Share = "Share";
                public const string CaseMultiple = "Case multiple";
                public const string DaysSupply = "Days supply";
                public const string CombinedPerformanceIndex = "Combined performance index";
                public const string PegSpan = "Peg span";
                public const string MinimumUnits = "Minimum units";
                public const string MaximumUnits = "Maximum units";
                public const string ShapeID = "Shape ID";
                public const string BitmapIDOverride = "Bitmap ID Override";
                public const string TrayWidth = "Tray width";
                public const string TrayHeight = "Tray height";
                public const string TrayDepth = "Tray depth";
                public const string TrayNumberWide = "Tray number wide";
                public const string TrayNumberHigh = "Tray number high";
                public const string TrayNumberDeep = "Tray number deep";
                public const string TrayTotalNumber = "Tray total number";
                public const string TrayMaxHigh = "Tray max high";
                public const string CaseWidth = "Case width";
                public const string CaseHeight = "Case height";
                public const string CaseDepth = "Case depth";
                public const string CaseNumberWide = "Case number wide";
                public const string CaseNumberHigh = "Case number high";
                public const string CaseNumberDeep = "Case number deep";
                public const string CaseTotalNumber = "Case total number";
                public const string CaseMaxHigh = "Case max high";
                public const string DisplayWidth = "Display width";
                public const string DisplayHeight = "Display height";
                public const string DisplayDepth = "Display depth";
                public const string DisplayNumberWide = "Display number wide";
                public const string DisplayNumberHigh = "Display number high";
                public const string DisplayNumberDeep = "Display number deep";
                public const string DisplayTotalNumber = "Display total number";
                public const string DisplayMaxHigh = "Display max high";
                public const string AlternateWidth = "Alternate width";
                public const string AlternateHeight = "Alternate height";
                public const string AlternateDepth = "Alternate depth";
                public const string AlternateNumberWide = "Alternate number wide";
                public const string AlternateNumberHigh = "Alternate number high";
                public const string AlternateNumberDeep = "Alternate number deep";
                public const string AlternateTotalNumber = "Alternate total number";
                public const string AlternateMaxHigh = "Alternate max high";
                public const string LooseWidth = "Loose width";
                public const string LooseHeight = "Loose height";
                public const string LooseDepth = "Loose depth";
                public const string LooseNumberWide = "Loose number wide";
                public const string LooseNumberHigh = "Loose number high";
                public const string LooseNumberDeep = "Loose number deep";
                public const string LooseTotalNumber = "Loose total number";
                public const string LooseMaxHigh = "Loose max high";
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
                public const string NumberOfPositions = "Number of Positions";
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
                public const string MinimumSqueezeFactorX = "Minimum squeeze factor X";
                public const string MinimumSqueezeFactorY = "Minimum squeeze factor Y";
                public const string MinimumSqueezeFactorZ = "Minimum squeeze factor Z";
                public const string MaximumSqueezeFactorX = "Maximum squeeze factor X";
                public const string MaximumSqueezeFactorY = "Maximum squeeze factor Y";
                public const string MaximumSqueezeFactorZ = "Maximum squeeze factor Z";
                public const string FillPattern = "Fill pattern";
                public const string ModelFilename = "Model Filename";
                public const string Brand = "Brand";
                public const string Subcategory = "Subcategory";
                public const string Weight = "Weight";
                public const string PlanogramAlias = "Planogram alias";
                public const string Changed = "Changed";
                public const string FrontOverhang = "Front overhang";
                public const string FingerSpaceX = "Finger space X";
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
                public const string Status = "Status";
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
                public const string Transparency = "Transparency";
                public const string PeakSafetyFactor = "Peak safety factor";
                public const string BackroomStock = "Backroom stock";
                public const string DeliverySchedule = "Delivery schedule";
                public const string PartID = "PartID";
                public const string AuthorityLevel = "Authority Level";
                public const string BitmapIDOverrideUnit = "Bitmap ID Override Unit";
                public const string ModelFilenameLookup = "Model Filename Lookup";
                public const string DefaultMerchStyle = "Default merch style";
                public const string AutomaticModel = "Automatic model";
                public const string CustomData = "Custom data";
                public const string DBGUID = "DBGUID";
                public const string Source = "Source";
                public const string TechnicalKey = "TechnicalKey";
                public const string SqueezeExpandUnitsOnly = "Squeeze/expand units only";

            }
            # endregion


            public Dictionary<string, string> Fields;           // Data for each field read from the product line of the PSA file        

            #region data fields
            private static string[] _fieldNames = new string[]
        {
            "UPC",
            "ID",
            "Name",
            "Key",
            "Width",
            "Height",
            "Depth",
            "Color",
            "Abbrev name",
            "Size",
            "UOM",
            "Manufacturer",
            "Category",
            "Supplier",
            "Inner pack",
            "X nesting",
            "Y nesting",
            "Z nesting",
            "Pegholes",
            "Peghole X",
            "Peghole Y",
            "Peghole width",
            "Peghole 2 X",
            "Peghole 2 Y",
            "Peghole 2 width",
            "Peghole 3 X",
            "Peghole 3 Y",
            "Peghole 3 width",
            "Package style",
            "Peg ID",
            "Finger space Y",
            "Jumble factor",
            "Price",
            "Case cost",
            "Tax code",
            "Unit movement",
            "Share",
            "Case multiple",
            "Days supply",
            "Combined performance index",
            "Peg span",
            "Minimum units",
            "Maximum units",
            "Shape ID",
            "Bitmap ID Override",
            "Tray width",
            "Tray height",
            "Tray depth",
            "Tray number wide",
            "Tray number high",
            "Tray number deep",
            "Tray total number",
            "Tray max high",
            "Case width",
            "Case height",
            "Case depth",
            "Case number wide",
            "Case number high",
            "Case number deep",
            "Case total number",
            "Case max high",
            "Display width",
            "Display height",
            "Display depth",
            "Display number wide",
            "Display number high",
            "Display number deep",
            "Display total number",
            "Display max high",
            "Alternate width",
            "Alternate height",
            "Alternate depth",
            "Alternate number wide",
            "Alternate number high",
            "Alternate number deep",
            "Alternate total number",
            "Alternate max high",
            "Loose width",
            "Loose height",
            "Loose depth",
            "Loose number wide",
            "Loose number high",
            "Loose number deep",
            "Loose total number",
            "Loose max high",
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
            "Number of Positions",
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
            "Minimum squeeze factor X",
            "Minimum squeeze factor Y",
            "Minimum squeeze factor Z",
            "Maximum squeeze factor X",
            "Maximum squeeze factor Y",
            "Maximum squeeze factor Z",
            "Fill pattern",
            "Model Filename",
            "Brand",
            "Subcategory",
            "Weight",
            "Planogram alias",
            "Changed",
            "Front overhang",
            "Finger space X",
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
            "Status",
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
            "Transparency",
            "Peak safety factor",
            "Backroom stock",
            "Delivery schedule",
            "PartID",
            "Authority Level",
            "Bitmap ID Override Unit",
            "Model Filename Lookup",
            "Default merch style",
            "Automatic model",
            "Custom data",
            "DBGUID",
            "Source",
            "TechnicalKey",
            "Squeeze/expand units only"
        };
            #endregion

            public Product() {
                Fields = new Dictionary<string, string>();
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
            /// Creates the dictionary key used to identify this product in PSA.TheProject.Products dictionary.
            /// </summary>
            /// <param name="ID">ID field value for product</param>
            /// <param name="UPC">UPC field value for product</param>
            /// <returns>Unique key for product</returns>
            public static string CreateDictionaryKey(string ID, string UPC)
            {
                return ID + ":" + UPC;
            }


            public float Depth {
                get { return float.Parse(Fields[FieldNames.Depth]); }
                set { Fields[FieldNames.Depth] = value.ToString(); }
            }


            /// <summary>
            /// The key for this product in the project's Product dictionary.
            /// </summary>
            public string DictionaryKey
            {
                get { return CreateDictionaryKey(this.ID, this.UPC);  }
            }

            public float Height {
                get { return float.Parse(Fields[FieldNames.Height]); }
                set { Fields[FieldNames.Height] = value.ToString(); }
            }

            public string ID {
                get { return Fields[FieldNames.ID]; }
            }

            public string Key {
                get { return Fields[FieldNames.Key]; }
            }

            public string Name {
                get { return Fields[FieldNames.Name]; }
            }

            public static Product ParsePSA(string line) {
                string[] fields = SplitLine(line);
                Product obj = new Product();

                for (int i = 0; (i < _fieldNames.Length) && (i + 1 < fields.Length); i++) {
                    obj.Fields[_fieldNames[i]] = fields[i + 1];
                }

                return obj;
            }


            public override string ToString() {
                return "PSA Product " + UPC;
            }


            public string UPC {
                get { return Fields[FieldNames.UPC]; }
            }



            public float Width {
                get { return float.Parse(Fields[FieldNames.Width]); }
                set { Fields[FieldNames.Width] = value.ToString(); }
            }


            /// <summary>
            /// Generates the PSA output string representation for a product.
            /// </summary>
            /// <returns>String</returns>
            public string WriteString() {
                StringBuilder sb = new StringBuilder();
                string replaceComma = "\\" + ",";

                sb.Append("Product");
                for (int i = 0; i < _fieldNames.Length; i++)
                {
                    sb.Append(PSA.PsaDelimiters[0]);
                    if (Fields.ContainsKey(_fieldNames[i]))
                    {
                        string val = Fields[_fieldNames[i]];
                        if (val != null)
                        {
                            if (!val.Contains(replaceComma) && val.Contains(","))
                            {
                                val = val.Replace(",", replaceComma);
                            }
                            sb.Append(val);
                        }
                    }
                }
                return sb.ToString();
            }


        }
    }

}
