using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace JDA
{
    /// <summary>
    /// The PFA class provides the high-level interface for working with JDA Floor Planning PFA files.  
    /// To read a PFA file, use the static method PFA.Read(string filename, bool silent), which returns a PFA object containing the contents of the PFA file.
    /// To write a PFA file, use the method PFA.Write(string filename).
    /// </summary>
    public partial class PFA
    {
        public static string[] PfaDelimiters = new string[] { "," };    // delimiters used to separate values in PFA files
        private static string FileFormatVersion = ";V2008.0.0.17";      // version number of reference file format guiding this implementation

        public List<string> Headers;                        // list of header lines that appear before the project keyword
        public Project TheProject;                             // project object containing floor planning schematic info
        private Version _version;                           // JDA version number included in PFA file
        public List<string> Warnings;                       // list of warnings generated while reading in the PFA file


        public PFA() {
            Headers = new List<string>();
            Warnings = new List<string>();
        }


        /// <summary>
        /// Calculates the Euclidean distance between two points.
        /// </summary>
        /// <param name="a">Point A</param>
        /// <param name="b">Point B</param>
        /// <returns>Euclidean distance between A and B</returns>
        public static double Distance(PointF a, PointF b) {
            return Math.Sqrt(Math.Pow(a.X - b.X, 2.0) + Math.Pow(a.Y - b.Y, 2.0));
        }


        /// <summary>
        /// Attempts to read in a PFA file.  If this routine fails or the user cancels the operation, NULL is returned; otherwise, 
        /// a JDA.PFA object is returned.
        /// </summary>
        /// <param name="filename">Name of the file to read</param>
        /// <param name="errorMsg">Error and warning messages for the PFA file</param>
        /// <returns>JDA.PFA object if the read is successful; NULL otherwise.</returns>
        public static PFA Read(string filename, out string errorMsg) {
            string[] rawData = File.ReadAllLines(filename);

            PFA currentPfa = new PFA();
            bool inHeaders = true;
            Fixture currentFixture = null;
            Floorplan currentFloorplan = null;
            Planogram currentPlanogram = null;
            Project currentProject = null;

            if (!rawData[0].StartsWith("PROFLOOR SCHEMATIC FILE")) {
                currentPfa.Warnings.Add("File is not a JDA Floor Planning Schematic");

            } else {
                if (rawData[1].StartsWith(";V")) currentPfa._version = new Version(rawData[1].Substring(2));

                for (int i = 0; i < rawData.Length; i++) {
                    string line = rawData[i];

                    if (line.StartsWith("Project")) {
                        currentProject = Project.ParsePFA(line);
                        inHeaders = false;

                    } else if (inHeaders) {
                        currentPfa.Headers.Add(line);

                    } else if (line.StartsWith("Department")) {
                        Department dept = Department.ParsePFA(line);
                        if (currentFloorplan != null) {
                            currentFloorplan.Departments[dept.Name] = dept;
                        } else {
                            currentPfa.Warnings.Add("No current floorplan for department item, line " + (i + 1));
                        }

                        // parse any 3D Point lines that directly follow the department
                        while ((i + 1 < rawData.Length) && (rawData[i+1].StartsWith("3D point"))) {
                            line = rawData[++i];
                            dept.PointsInFile.Add(Point3D.ParsePFA(line));
                        }

                        dept.ComputeRotatedPoints();

                    } else if (line.StartsWith("Drawing")) {
                        if (currentFloorplan != null) {
                            currentFloorplan.Drawings.Add(Drawing.ParsePFA(line));
                        } else {
                            currentPfa.Warnings.Add("No current floorplan for drawing item, line " + (i + 1));
                        }

                    } else if (line.StartsWith("Fixture")) {
                        Fixture fix = Fixture.ParsePFA(line);
                        if (currentFloorplan != null) {
                            currentFloorplan.Fixtures[fix.UniqueID] = fix;
                            fix.Floorplan = currentFloorplan;
                        }
                        currentFixture = fix;

                        // if the fixture is not a "regular fixture", it might be followed by 3D point lines
                        if (fix.Type != Fixture.FixtureType.Regular) {
                            // parse any 3D Point lines that directly follow the fixture
                            while ((i + 1 < rawData.Length) && (rawData[i + 1].StartsWith("3D point"))) {
                                line = rawData[++i];
                                fix.PointsFromFile.Add(Point3D.ParsePFA(line));
                            }

                            // if there were no 3D Point lines, the fixture is a rectangle, so create points for it
                            if (fix.PointsFromFile.Count == 0) {
                                fix.PointsFromFile.Add(new Point3D(fix.X, fix.Y, fix.Z));
                                fix.PointsFromFile.Add(new Point3D(fix.X + fix.Width, fix.Y, fix.Z));
                                fix.PointsFromFile.Add(new Point3D(fix.X + fix.Width, fix.Y + fix.Depth, fix.Z));
                                fix.PointsFromFile.Add(new Point3D(fix.X, fix.Y + fix.Depth, fix.Z));
                            }
                        } 

                        fix.ComputeRotatedPoints();


                    } else if (line.StartsWith("Floorplan")) {
                        Floorplan fp = Floorplan.ParsePFA(line);
                        string s = fp.Name;
                        if (currentProject.Floorplans.ContainsKey(s)) {
                            currentPfa.Warnings.Add("There already exists a floorplan named '" + s + "' in project, line " + (i + 1));
                            do {
                                s += "_";
                            } while (currentProject.Floorplans.ContainsKey(s));
                        }
                        currentProject.Floorplans[s] = fp;
                        currentFloorplan = fp;

                    } else if (line.StartsWith("Performance")) {
                        if (currentFloorplan != null) {
                            Performance p = Performance.ParsePFA(line);
                            currentFloorplan.Performances[p.Name] = p;
                        } else {
                            currentPfa.Warnings.Add("No current floorplan for performance item, line " + (i + 1));
                        }

                    } else if (line.StartsWith("Planogram")) {
                        Planogram plan = Planogram.ParsePFA(line);
                        string s = plan.Name;
                        if (currentProject.Planograms.ContainsKey(s)) {
                            currentPfa.Warnings.Add("There already exists a planogram named '" + s + "' in project, line " + (i + 1));
                            do {
                                s += "_";
                            } while (currentProject.Planograms.ContainsKey(s));
                        }
                        currentProject.Planograms[s] = plan;
                        currentPlanogram = plan;

                        // assign the category for the planogram
                        if (plan.Fields[Planogram.FieldNames.Category].Length != 0) {
                            plan.Category = plan.Fields[Planogram.FieldNames.Category];
                        } else {
                            // try to tease out the category this planogram belongs to from P&G specific adjacency metadata
                            string adjacencyLeft = plan.Fields[Planogram.FieldNames.Desc4];
                            string adjacencyRight = plan.Fields[Planogram.FieldNames.Desc6];
                            if (adjacencyLeft.Contains(" - ")) {
                                plan.Category = adjacencyLeft.Substring(adjacencyLeft.IndexOf(" - ") + 3);
                            } else if (adjacencyLeft.Length > 0) {
                                plan.Category = adjacencyLeft;
                            } else if (adjacencyRight.Contains(" - ")) {
                                plan.Category = adjacencyRight.Substring(0, adjacencyRight.IndexOf(" - "));
                            } else {
                                plan.Category = "";
                            }
                        }
                        if ((plan.Category == null) || (plan.Category.Length == 0)) currentPfa.Warnings.Add("No category found for planogram item " + plan.Name + ", line " + (i + 1));

                    } else if (line.StartsWith("Section")) {
                        Section sec = Section.ParsePFA(line);

                        // find the planogram associated with this section
                        if (currentProject.Planograms.ContainsKey(sec.Name)) {
                            sec.Planogram = currentProject.Planograms[sec.Name];
                            sec.Category = sec.Planogram.Category;
                        } else {
                            // try to tease out the category this section belongs to from P&G specific adjacency metadata
                            string adjacencyLeft = sec.Fields[Section.FieldNames.Desc4];
                            string adjacencyRight = sec.Fields[Section.FieldNames.Desc6];
                            if (adjacencyLeft.Contains(" - ")) {
                                sec.Category = adjacencyLeft.Substring(adjacencyLeft.IndexOf(" - ") + 3);
                            } else if (adjacencyRight.Contains(" - ")) {
                                sec.Category = adjacencyRight.Substring(0, adjacencyRight.IndexOf(" - "));
                            } else {
                                sec.Category = "";
                            }
                        }

                        if (currentFloorplan != null) {
                            currentFloorplan.Sections[sec.UniqueID] = sec;
                        } else {
                            currentPfa.Warnings.Add("No current floorplan for section item " + sec.Name + "," + sec.Key + ", line " + (i + 1));
                        }
                        if ((sec.Category == null) || (sec.Category.Length == 0)) {
                            currentPfa.Warnings.Add("No category found for section item " + sec.Name + "," + sec.Key + ", line " + (i + 1));
                        }

                        if (currentFixture != null) {
                            sec.AnchoringFixture = currentFixture;
                            currentFixture.Sections.Add(sec);
                            sec.ComputeRotatedPoints();
                        } else {
                            currentPfa.Warnings.Add("No fixture found for section item " + sec.Name + "," + sec.Key + ", line " + (i + 1));
                        }


                    } else if (line.StartsWith("Segment")) {
                        Segment seg = Segment.ParsePFA(line);
                        if (currentPlanogram != null) currentPlanogram.Segments.Add(seg);
                        else currentPfa.Warnings.Add("No current planogram for segment item, line " + (i + 1));

                    } else if (line.StartsWith("Tracefile")) {
                        if (currentFloorplan != null) currentFloorplan.Unknowns.Add(line);
                        else currentPfa.Warnings.Add("No current floorplan for tracefile item, line " + (i + 1));

                    } else if (!inHeaders) {
                        // unclassified data line
                        if (currentFloorplan != null) currentFloorplan.Unknowns.Add(line);
                        else currentPfa.Warnings.Add("No current floorplan for unrecognized item, line " + (i + 1));
                    }
                }
            }

            // Build the CombinedFixtures that combine adjacent regular fixtures 
            List<CombinedFixture> combinedFixtures = CombinedFixture.BuildCombinedFixtures(currentFloorplan.Fixtures);

            // Position the sections within the CombinedFixtures, then rotate sections to their true position
            foreach (CombinedFixture sf in combinedFixtures) sf.PositionSections();

            // Error and warning messages
            errorMsg = "";
            if (currentPfa.Warnings.Count > 0) {
                errorMsg = "The following problems were discovered while reading in a PFA file:\r\n\r\n";
                foreach (string s in currentPfa.Warnings) errorMsg += s + "\r\n";
            }

            // return the PFA object
            if (currentPfa != null) currentPfa.TheProject = currentProject;
            return currentPfa;
        }


        /// <summary>
        /// Rotates point A around point B by the specified angle
        /// </summary>
        /// <param name="x">x coordinate for Point A</param>
        /// <param name="y">y coordinate for Point A</param>
        /// <param name="angle">Angle to rotate, in degrees</param>
        /// <param name="aboutX">x coordinate for Point B</param>
        /// <param name="aboutY">y coordinate for Point B</param>
        /// <returns>PointF containing rotated point</returns>
        public static PointF RotateAboutPoint(double x, double y, double angle, double aboutX, double aboutY) {
            double x1 = x - aboutX;
            double y1 = y - aboutY;
            PointF pt1 = RotatePoint(x1, y1, angle);
            return new PointF((float)(pt1.X + aboutX), (float)(pt1.Y + aboutY));
        }


        /// <summary>
        /// Rotates a point around the origin by the specified angle
        /// </summary>
        /// <param name="x">x coordinate</param>
        /// <param name="y">y coordinate</param>
        /// <param name="angle">Angle to rotate, in degrees</param>
        /// <returns>PointF containing rotated point</returns>
        public static PointF RotatePoint(double x, double y, double angle) {
            double rad = Math.PI / 180.0 * angle;
            double c = Math.Cos(rad);
            double s = Math.Sin(rad);

            PointF answer = new PointF();
            answer.X = (float)(x * c - y * s);
            answer.Y = (float)(x * s + y * c);
            return answer;
        }


        /// <summary>
        /// Splits a line from a PFA file into delimited parts.  A special function is needed to deal with slash-delimited commas embedded in the line.
        /// </summary>
        /// <param name="line">String to split</param>
        /// <returns>string[]</returns>
        public static string[] SplitLine(string line) {
            string[] parts = line.Replace(@"\,", "$comma$").Split(PfaDelimiters, StringSplitOptions.None);
            for (int i = 0; i < parts.Length; i++) {
                if (parts[i].Contains("$comma$")) parts[i] = parts[i].Replace("$comma$", ",");
            }
            return parts;
        }


        /// <summary>
        /// Returns the version number included in the FPA file.
        /// </summary>
        public Version Version {
            get { return _version; }
        }


        /// <summary>
        /// Writes a PFA structure to a file
        /// </summary>
        /// <param name="filename">Name of file to write</param>
        public void Write(string filename) {
            StreamWriter sw = new StreamWriter(filename, false, Encoding.ASCII);
            sw.NewLine = "\r\n";

            // headers
            sw.WriteLine("PROFLOOR SCHEMATIC FILE");
            sw.WriteLine(FileFormatVersion);
            sw.WriteLine(TheProject.WriteString());

            // planogram and segments
            foreach (KeyValuePair<string,Planogram> p in TheProject.Planograms) {
                sw.WriteLine(p.Value.WriteString());
                foreach (PFA.Segment s in p.Value.Segments) {
                    sw.WriteLine(s.WriteString());
                }
            }

            // floorplans
            foreach (KeyValuePair<string, Floorplan> fp in TheProject.Floorplans) {
                sw.WriteLine(fp.Value.WriteString());
                // performance
                foreach (KeyValuePair<string, Performance> perf in fp.Value.Performances) {
                    sw.WriteLine(perf.Value.WriteString());
                }
                // departments
                foreach (KeyValuePair<string, Department> dept in fp.Value.Departments) {
                    sw.WriteLine(dept.Value.WriteString());
                    foreach (Point3D point in dept.Value.PointsInFile) sw.WriteLine(point.WriteString());
                }
                // fixtures 
                foreach (KeyValuePair<string, Fixture> fix in fp.Value.Fixtures) {
                    sw.WriteLine(fix.Value.WriteString());
                    foreach (Point3D point in fix.Value.PointsFromFile) sw.WriteLine(point.WriteString());
                    // sections
                    foreach (Section sec in fix.Value.Sections) {
                        sw.WriteLine(sec.WriteString());
                    }
                }

                // drawings
                foreach (Drawing d in fp.Value.Drawings) {
                    sw.WriteLine(d.WriteString());
                }
                // unknowns
                foreach (string u in fp.Value.Unknowns) {
                    sw.WriteLine(u);
                }
            }

            sw.Close();
        }

    }
}
