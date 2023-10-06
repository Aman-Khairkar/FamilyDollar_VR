using System;
using System.Collections.Generic;
using System.Text;

namespace JDA
{
    public partial class PSA
    {

        /// <summary>
        /// Positions are the physical representation of a merchandised product, either floating
        /// freely on a planogram or placed on a fixture in a planogram.
        /// </summary>
        public class Position
        {

            # region Field Names
            public class FieldNames
            {
                public const string UPC = "UPC";
                public const string ID = "ID";
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
                public const string MerchStyle = "Merch style";
                public const string HFacings = "HFacings";
                public const string VFacings = "VFacings";
                public const string DFacings = "DFacings";
                public const string XCapNum = "X cap num";
                public const string XCapNested = "X cap nested";
                public const string XCapReversed = "X cap reversed";
                public const string XCapOrientation = "X cap orientation";
                public const string YCapNum = "Y cap num";
                public const string YCapNested = "Y cap nested";
                public const string YCapReversed = "Y cap reversed";
                public const string YCapOrientation = "Y cap orientation";
                public const string ZCapNum = "Z cap num";
                public const string ZCapNested = "Z cap nested";
                public const string ZCapReversed = "Z cap reversed";
                public const string ZCapOrientation = "Z cap orientation";
                public const string Orientation = "Orientation";
                public const string JumbleWidth = "Jumble width";
                public const string JumbleHeight = "Jumble height";
                public const string JumbleDepth = "Jumble depth";
                public const string MerchStyleWidth = "Merch style width";
                public const string MerchStyleHeight = "Merch style height";
                public const string MerchStyleDepth = "Merch style depth";
                public const string FullWidth = "Full width";
                public const string FullHeight = "Full height";
                public const string FullDepth = "Full depth";
                public const string XSubUnits = "X sub units";
                public const string YSubUnits = "Y sub units";
                public const string ZSubUnits = "Z sub units";
                public const string PegID = "Peg ID";
                public const string ManualUnits = "Manual units";
                public const string RankX = "Rank X";
                public const string RankY = "Rank Y";
                public const string RankZ = "Rank Z";
                public const string PegSpan = "Peg span";
                public const string AlwaysFloat = "Always float";
                public const string PrimaryPositionLabelFormatName = "Primary position label format name";
                public const string SecondaryPositionLabelFormatName = "Secondary position label format name";
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
                public const string UsTtargetSpaceX = "Use target space X";
                public const string UseTargetSpaceY = "Use target space Y";
                public const string UseTargetSpaceZ = "Use target space Z";
                public const string TargetSpaceX = "Target space X";
                public const string TargetSpaceY = "Target space Y";
                public const string TargetSpaceZ = "Target space Z";
                public const string LocationID = "Location ID";
                public const string Changed = "Changed";
                public const string ReplenishmentMin = "Replenishment min";
                public const string ReplenishmentMax = "Replenishment max";
                public const string ShapeID = "Shape ID";
                public const string BitmapIDOverride = "Bitmap ID Override";
                public const string HideIfPrinting = "Hide if printing";
                public const string PartID = "PartID";
                public const string BitmapIDOverrideUnit = "Bitmap ID Override Unit";
                public const string AutomaticModel = "Automatic model";
                public const string CustomData = "Custom data";
                public const string XCapWithUnits = "X cap with units";
                public const string YCapWithUnits = "Y cap with units";
                public const string ZCapWithUnits = "Z cap with units";

            }
            # endregion


            public Dictionary<string, string> Fields;           // Data for each field read from the position line of the PSA file        

            #region data fields
            private static string[] _fieldNames = new string[]
    {
        "UPC",
        "ID",
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
        "Merch style",
        "HFacings",
        "VFacings",
        "DFacings",
        "X cap num",
        "X cap nested",
        "X cap reversed",
        "X cap orientation",
        "Y cap num",
        "Y cap nested",
        "Y cap reversed",
        "Y cap orientation",
        "Z cap num",
        "Z cap nested",
        "Z cap reversed",
        "Z cap orientation",
        "Orientation",
        "Jumble width",
        "Jumble height",
        "Jumble depth",
        "Merch style width",
        "Merch style height",
        "Merch style depth",
        "Full width",
        "Full height",
        "Full depth",
        "X sub units",
        "Y sub units",
        "Z sub units",
        "Peg ID",
        "Manual units",
        "Rank X",
        "Rank Y",
        "Rank Z",
        "Peg span",
        "Always float",
        "Primary position label format name",
        "Secondary position label format name",
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
        "Use target space X",
        "Use target space Y",
        "Use target space Z",
        "Target space X",
        "Target space Y",
        "Target space Z",
        "Location ID",
        "Changed",
        "Replenishment min",
        "Replenishment max",
        "Shape ID",
        "Bitmap ID Override",
        "Hide if printing",
        "PartID",
        "Bitmap ID Override Unit",
        "Automatic model",
        "Custom data",
        "X cap with units",
        "Y cap with units",
        "Z cap with units"
    };
            #endregion

            public Position() {
                Fields = new Dictionary<string, string>();
            }

            public float Depth {
                get { return float.Parse(Fields[FieldNames.Depth]); }
                set { Fields[FieldNames.Depth] = value.ToString(); }
            }

            public int DFacings {
                get { return int.Parse(Fields[FieldNames.DFacings]); }
                set { Fields[FieldNames.DFacings] = value.ToString(); }
            }

            public float Height {
                get { return float.Parse(Fields[FieldNames.Height]); }
                set { Fields[FieldNames.Height] = value.ToString(); }
            }

            public int HFacings {
                get { return int.Parse(Fields[FieldNames.HFacings]); }
                set { Fields[FieldNames.HFacings] = value.ToString(); }
            }

            public string ID {
                get { return Fields[FieldNames.ID]; }
            }

            public string Key {
                get { return Fields[FieldNames.Key]; }
            }


            public static Position ParsePSA(string line) {
                string[] fields = SplitLine(line);
                Position obj = new Position();

                for (int i = 0; (i < _fieldNames.Length) && (i + 1 < fields.Length); i++) {
                    obj.Fields[_fieldNames[i]] = fields[i + 1];
                }

                return obj;
            }


            public override string ToString() {
                return "PSA Position " + UPC;
            }


            public string UPC {
                get { return Fields[FieldNames.UPC]; }
            }


            public int VFacings {
                get { return int.Parse(Fields[FieldNames.VFacings]); }
                set { Fields[FieldNames.VFacings] = value.ToString(); }
            }

            public float Width {
                get { return float.Parse(Fields[FieldNames.Width]); }
                set { Fields[FieldNames.Width] = value.ToString(); }
            }
      

            /// <summary>
            /// Generates the PSA output string representation for a Position.
            /// </summary>
            /// <returns>String</returns>
            public string WriteString() {
                StringBuilder sb = new StringBuilder();
                string replaceComma = "\\" + ",";

                sb.Append("Position");
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

            /*
            public const string XCapNum = "X cap num";
            public const string XCapNested = "X cap nested";
            public const string XCapReversed = "X cap reversed";
            public const string XCapOrientation = "X cap orientation";
            public const string YCapNum = "Y cap num";
            public const string YCapNested = "Y cap nested";
            public const string YCapReversed = "Y cap reversed";
            public const string YCapOrientation = "Y cap orientation";
            public const string ZCapNum = "Z cap num";
            public const string ZCapNested = "Z cap nested";
            public const string ZCapReversed = "Z cap reversed";
            public const string ZCapOrientation = "Z cap orientation";
            */

            public float XCapNum {
                get { return float.Parse(Fields[FieldNames.XCapNum]); }
                set { Fields[FieldNames.XCapNum] = value.ToString(); }
            }

            public float XCapNested
            {
                get { return float.Parse(Fields[FieldNames.XCapNested]); }
                set { Fields[FieldNames.XCapNested] = value.ToString(); }
            }

            public int XCapReversed
            {
                get { return int.Parse(Fields[FieldNames.XCapReversed]); }
                set { Fields[FieldNames.XCapReversed] = value.ToString(); }
            }

            public int XCapOrientation
            {
                get { return int.Parse(Fields[FieldNames.XCapOrientation]); }
                set { Fields[FieldNames.XCapOrientation] = value.ToString(); }
            }

            public float YCapNum
            {
                get { return float.Parse(Fields[FieldNames.YCapNum]); }
                set { Fields[FieldNames.YCapNum] = value.ToString(); }
            }

            public float YCapNested
            {
                get { return float.Parse(Fields[FieldNames.YCapNested]); }
                set { Fields[FieldNames.YCapNested] = value.ToString(); }
            }

            public float YCapReversed
            {
                get { return float.Parse(Fields[FieldNames.YCapReversed]); }
                set { Fields[FieldNames.YCapReversed] = value.ToString(); }
            }

            public int YCapOrientation
            {
                get { return int.Parse(Fields[FieldNames.YCapOrientation]); }
                set { Fields[FieldNames.YCapOrientation] = value.ToString(); }
            }

            public float ZCapNum
            {
                get { return float.Parse(Fields[FieldNames.ZCapNum]); }
                set { Fields[FieldNames.ZCapNum] = value.ToString(); }
            }

            public float ZCapNested
            {
                get { return float.Parse(Fields[FieldNames.ZCapNested]); }
                set { Fields[FieldNames.ZCapNested] = value.ToString(); }
            }

            public float ZCapReversed
            {
                get { return float.Parse(Fields[FieldNames.ZCapReversed]); }
                set { Fields[FieldNames.ZCapReversed] = value.ToString(); }
            }

            public int ZCapOrientation
            {
                get { return int.Parse(Fields[FieldNames.ZCapOrientation]); }
                set { Fields[FieldNames.ZCapOrientation] = value.ToString(); }
            }
            /*
            public const string Slope = "Slope";
            public const string Angle = "Angle";
            public const string Roll = "Roll";
            */

            public int Orientation
            {
                get { return int.Parse(Fields[FieldNames.Orientation]); }               
            }
            
            

        }
    }
}

