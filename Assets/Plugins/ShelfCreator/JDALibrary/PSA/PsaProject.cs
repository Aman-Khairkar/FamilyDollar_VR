using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace JDA
{
    public partial class PSA
    {
        /// <summary>
        /// PSA project container object.  The primary attribute of this class that will be of interest to the programmer is Planograms,
        /// which is a dictionary mapping planogram names to Planogram objects.
        /// </summary>
        public class Project
        {

            # region Field Names
            public class FieldNames
            {
                public const string Name = "Name";
                public const string Key = "Key";
                public const string PrimaryKey = "Primary key";
                public const string LayoutFile = "Layout file";
                public const string MovementPeriod = "Movement period";
                public const string CaseMultiple = "Case multiple";
                public const string DaysSupply = "Days supply";
                public const string DemandCycleLength = "Demand cycle length";
                public const string PeakSafetyFactor = "Peak safety factor";
                public const string BackroomStock = "Backroom stock";
                public const string PegID = "Peg ID";
                public const string Measurement = "Measurement";
                public const string NumberOfStores = "Number of Stores";
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
                public const string Demand1 = "Demand 1";
                public const string Demand2 = "Demand 2";
                public const string Demand3 = "Demand 3";
                public const string Demand4 = "Demand 4";
                public const string Demand5 = "Demand 5";
                public const string Demand6 = "Demand 6";
                public const string Demand7 = "Demand 7";
                public const string Demand8 = "Demand 8";
                public const string Demand9 = "Demand 9";
                public const string Demand10 = "Demand 10";
                public const string Demand11 = "Demand 11";
                public const string Demand12 = "Demand 12";
                public const string Demand13 = "Demand 13";
                public const string Demand14 = "Demand 14";
                public const string Demand15 = "Demand 15";
                public const string Demand16 = "Demand 16";
                public const string Demand17 = "Demand 17";
                public const string Demand18 = "Demand 18";
                public const string Demand19 = "Demand 19";
                public const string Demand20 = "Demand 20";
                public const string Demand21 = "Demand 21";
                public const string Demand22 = "Demand 22";
                public const string Demand23 = "Demand 23";
                public const string Demand24 = "Demand 24";
                public const string Demand25 = "Demand 25";
                public const string Demand26 = "Demand 26";
                public const string Demand27 = "Demand 27";
                public const string Demand28 = "Demand 28";
                public const string InventoryModel_Manual = "Inventory model - Manual";
                public const string InventoryModel_CaseMultiple = "Inventory model - Case Multiple";
                public const string InventoryModel_DaysSupply = "Inventory model - Days Supply";
                public const string InventoryModel_Peak = "Inventory model - Peak";
                public const string InventoryModel_MinUnits = "Inventory model - Min units";
                public const string InventoryModel_MaxUnits = "Inventory model - Max units";
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
                public const string Notes = "Notes";
                public const string Changed = "Changed";
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
                public const string UsePerformancePrice = "Use performance price";
                public const string UsePerformanceCost = "Use performance cost";
                public const string UsePerformanceTaxCode = "Use performance tax code";
                public const string UsePerformanceMovement = "Use performance movement";
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
                public const string PlanogramSpecificInventory = "Planogram specific inventory";
                public const string DeliverySchedule = "Delivery schedule";
                public const string CustomData = "Custom data";
                public const string DBFamilyKey = "DBFamilyKey";
                public const string DBReplaceKey = "DBReplaceKey";

            }
            # endregion


            public enum MeasurementSystem { Imperial = 0, Metric = 1 }

            public Dictionary<string, string> Fields;           // Data for each field read from the project line of the PSA file
            public List<Peg> Pegs;                              // Peg objects
            public Dictionary<string, Planogram> Planograms;    // Dictionary mapping names to planograms present in PSA file
            public Dictionary<string, Product> Products;        // Dictionary mapping product ID:UPC to products present in PSA file

            #region field names
            private static string[] _fieldNames = new string[]
        {
            "Name",
            "Key",
            "Primary key",
            "Layout file",
            "Movement period",
            "Case multiple",
            "Days supply",
            "Demand cycle length",
            "Peak safety factor",
            "Backroom stock",
            "Peg ID",
            "Measurement",
            "Number of Stores",
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
            "Demand 1",
            "Demand 2",
            "Demand 3",
            "Demand 4",
            "Demand 5",
            "Demand 6",
            "Demand 7",
            "Demand 8",
            "Demand 9",
            "Demand 10",
            "Demand 11",
            "Demand 12",
            "Demand 13",
            "Demand 14",
            "Demand 15",
            "Demand 16",
            "Demand 17",
            "Demand 18",
            "Demand 19",
            "Demand 20",
            "Demand 21",
            "Demand 22",
            "Demand 23",
            "Demand 24",
            "Demand 25",
            "Demand 26",
            "Demand 27",
            "Demand 28",
            "Inventory model - Manual",
            "Inventory model - Case Multiple",
            "Inventory model - Days Supply",
            "Inventory model - Peak",
            "Inventory model - Min units",
            "Inventory model - Max units",
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
            "Notes",
            "Changed",
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
            "Use performance price",
            "Use performance cost",
            "Use performance tax code",
            "Use performance movement",
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
            "Planogram specific inventory",
            "Delivery schedule",
            "Custom data",
            "DBFamilyKey",
            "DBReplaceKey"
        };
            #endregion

            public Project() {
                Fields = new Dictionary<string, string>();
                Pegs = new List<Peg>();
                Planograms = new Dictionary<string, Planogram>();
                Products = new Dictionary<string, Product>();
            }


            public MeasurementSystem Measurement {
                get { return (Fields[FieldNames.Measurement] == "0" || Fields[FieldNames.Measurement] == "Imperial" ? MeasurementSystem.Imperial : MeasurementSystem.Metric); }
                set { Fields[FieldNames.Measurement] = value.ToString(); }
            }


            public string Name {
                get { return Fields[FieldNames.Name]; }
                set { Fields[FieldNames.Name] = value.Substring(0, Math.Min(100, value.Length)); }
            }


            public static Project ParsePSA(string line) {
                string[] fields = SplitLine(line);
                Project obj = new Project();

                for (int i = 0; (i < _fieldNames.Length) && (i + 1 < fields.Length); i++) {
                    obj.Fields[_fieldNames[i]] = fields[i + 1];
                }

                return obj;
            }


            public override string ToString() {
                return "PSA Project " + Name;
            }


            /// <summary>
            /// Generates the PSA output string representation for a project.
            /// </summary>
            /// <returns>String</returns>
            public string WriteString() {
                StringBuilder sb = new StringBuilder();

                sb.Append("Project");
                for (int i = 0; i < _fieldNames.Length; i++) {
                    sb.Append(PSA.PsaDelimiters[0]);
                    if (Fields.ContainsKey(_fieldNames[i])) sb.Append(Fields[_fieldNames[i]].Replace(",", "\\,"));
                }

                return sb.ToString();
            }

        }
    }
}
