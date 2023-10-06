using System;
using System.Collections.Generic;
using System.Text;

namespace JDA
{
    public partial class PSA
    {
        /// <summary>
        /// Drawing objects are graphics drawn on the planogram, including text.  This library does not support Drawing objects, beyond 
        /// the ability to get/set properties via the Fields dictionary.
        /// </summary>
        public class Drawing
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
                public const string Color = "Color";
                public const string BackFill = "Back fill";
                public const string BackColor = "Back color";
                public const string ViewCreatedIn = "View created in";
                public const string ShowInAllViews = "Show in all views";
                public const string WordWrap = "Word wrap";
                public const string Circular = "Circular";
                public const string StartX = "Start X";
                public const string StartY = "Start Y";
                public const string StartZ = "Start Z";
                public const string EndX = "End X";
                public const string EndY = "End Y";
                public const string EndZ = "End Z";
                public const string String = "String";
                public const string Scale = "Scale";
                public const string Outline = "Outline";
                public const string Callout = "Callout";
                public const string FontHeight = "Font height";
                public const string FontWidth = "Font width";
                public const string FontEscapement = "Font escapement";
                public const string FontOrientation = "Font orientation";
                public const string FontWeight = "Font weight";
                public const string FontItalic = "Font italic";
                public const string FontUnderline = "Font underline";
                public const string FontStrikeout = "Font strike out";
                public const string FontCharset = "Font char set";
                public const string FontOutPrecision = "Font out precision";
                public const string FontClipPrecision = "Font clip precision";
                public const string FontQuality = "Font quality";
                public const string FontPitchAndFamily = "Font pitch and family";
                public const string FontFaceName = "Font face name";
                public const string CalloutX = "Callout X";
                public const string CalloutY = "Callout Y";
                public const string CalloutZ = "Callout Z";
                public const string CenterText = "Center text";
                public const string Changed = "Changed";
                public const string HideIfPrinting = "Hide if printing";
                public const string CustomData = "Custom data";
                public const string AdjustFontColor = "Adjust font color";

            }
            # endregion


            public enum DrawingType { Arc = 0, Ellipse = 1, Line = 2, Polygon = 3, Rectangle = 4, Text = 5 };

            public Dictionary<string, string> Fields;           // Data for each field read from the drawing line of the PSA file        
            public List<Point3D> Points;                        // Points that make up certain kinds of drawing

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
            "Color",
            "Back fill",
            "Back color",
            "View created in",
            "Show in all views",
            "Word wrap",
            "Circular",
            "Start X",
            "Start Y",
            "Start Z",
            "End X",
            "End Y",
            "End Z",
            "String",
            "Scale",
            "Outline",
            "Callout",
            "Font height",
            "Font width",
            "Font escapement",
            "Font orientation",
            "Font weight",
            "Font italic",
            "Font underline",
            "Font strike out",
            "Font char set",
            "Font out precision",
            "Font clip precision",
            "Font quality",
            "Font pitch and family",
            "Font face name",
            "Callout X",
            "Callout Y",
            "Callout Z",
            "Center text",
            "Changed",
            "Hide if printing",
            "Custom data",
            "Adjust font color"
        };
            #endregion

            public Drawing() {
                Fields = new Dictionary<string, string>();
                Points = new List<Point3D>();
            }


            public static Drawing ParsePSA(string line) {
                string[] fields = SplitLine(line);
                Drawing obj = new Drawing();

                for (int i = 0; (i < _fieldNames.Length) && (i + 1 < fields.Length); i++) {
                    obj.Fields[_fieldNames[i]] = fields[i + 1];
                }

                return obj;
            }


            /// <summary>
            /// Gets or Sets the type of drawing.
            /// </summary>
            public DrawingType Type {
                get {
                    switch (Fields[FieldNames.Type]) {
                        case "0": return DrawingType.Arc;
                        case "1": return DrawingType.Ellipse;
                        case "2": return DrawingType.Line;
                        case "3": return DrawingType.Polygon;
                        case "4": return DrawingType.Rectangle;
                        case "5": return DrawingType.Text;
                        default: return 0;
                    }
                }
                set {
                    switch (value) {
                        case DrawingType.Arc: Fields[FieldNames.Type] = "0"; break;
                        case DrawingType.Ellipse: Fields[FieldNames.Type] = "1"; break;
                        case DrawingType.Line: Fields[FieldNames.Type] = "2"; break;
                        case DrawingType.Polygon: Fields[FieldNames.Type] = "3"; break;
                        case DrawingType.Rectangle: Fields[FieldNames.Type] = "4"; break;
                        case DrawingType.Text: Fields[FieldNames.Type] = "5"; break;
                    }
                }
            }


            /// <summary>
            /// Generates the PSA output string representation for a drawing.
            /// </summary>
            /// <returns>String</returns>
            public string WriteString() {
                StringBuilder sb = new StringBuilder();
                string replaceComma = "\\" + ",";

                sb.Append("Drawing");
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

                return sb.ToString();
            }
        }
    }
}
