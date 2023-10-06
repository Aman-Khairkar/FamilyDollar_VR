using System;
using System.Collections.Generic;


namespace JDA
{
    public partial class PFA
    {
        /// <summary>
        /// CombinedFixture is not explicitly part of the PFA file definition.  CombinedFixtures combine co-linear regular fixtures that can share
        /// merchandising space into one long fixture.  The main purpose for creating CombinedFixtures is to mimic how JDA Floor Planning deals
        /// with Sections - it pushes them towards the right or left of the CombinedFixtures depending on the section placement setting of the fixtures, 
        /// regardless of which Fixture they were anchored to in the PFA file.
        /// </summary>
        class CombinedFixture
        {
            int _angle;                         // fixture angle
            RectangleF _bounds;                 // boundary of the super fixture block
            Fixture.Placement _sectionPlacement;    // merchandise combining direction; Normal = right-to-left, Reversed=left-to-right
            List<Fixture> _fixtures;            // list of Fixtures belonging to this super fixture
            List<Section> _sections;            // list of Sections belonging to this super fixture
            float _width;                       // width of CombinedFixture
            float _X;                           // X origin of unrotated super fixture
            float _Y;                           // X origin of unrotated super fixture

            public CombinedFixture(int angle) {
                _angle = angle;
                _bounds = new RectangleF();
                _fixtures = new List<Fixture>();
                _sections = new List<Section>();
                _width = 0;
            }


            /// <summary>
            /// Builds all the CombinedFixtures found in a dictionary of fixtures.
            /// </summary>
            /// <param name="fixtures">Dictionary of fixtures</param>
            /// <returns>List of CombinedFixtures</returns>
            public static List<CombinedFixture> BuildCombinedFixtures(Dictionary<string, Fixture> fixtures) {
                Dictionary<int, List<Fixture>> angles = new Dictionary<int, List<Fixture>>();

                // segregate the regular fixtures by AngleR
                foreach (KeyValuePair<string, Fixture> pair in fixtures) {
                    Fixture f = pair.Value;
                    if (f.Type == Fixture.FixtureType.Regular) {
                        if (!angles.ContainsKey(f.AngleR)) angles[f.AngleR] = new List<Fixture>();
                        angles[f.AngleR].Add(f);
                    }
                }

                // process each of the angle groups, looking for connected fixtures
                List<CombinedFixture> answer = new List<CombinedFixture>();
                foreach (KeyValuePair<int, List<Fixture>> anglePair in angles) {
                    // sort the fixtures left-to-right 
                    List<Fixture> sortedFixtures = anglePair.Value;
                    if ((anglePair.Key < 90) || (anglePair.Key > 270)) sortedFixtures.Sort((a, b) => a.X.CompareTo(b.X));
                    else if (anglePair.Key == 90) sortedFixtures.Sort((a, b) => a.Y.CompareTo(b.Y));
                    else if (anglePair.Key == 270) sortedFixtures.Sort((a, b) => b.Y.CompareTo(a.Y));
                    else sortedFixtures.Sort((a, b) => b.X.CompareTo(a.X));
                    //if (anglePair.Key == 90 || anglePair.Key == 270) sortedFixtures.Sort((a, b) => b.Points[0].Y.CompareTo(a.Points[0].Y));
                    //else sortedFixtures.Sort((a, b) => a.Points[0].X.CompareTo(b.Points[0].X));

                    // if two fixtures are linked, then the rear corners will touch and A.Corners[2] ~ B.Corners[3]
                    //List<List<Fixture>> connectedFixtures = new List<List<Fixture>>();
                    foreach (Fixture f in sortedFixtures) {
                        // if the fixture's merchandising space can not combined with neighbors, do not consider it for inclusion in a CombinedFixture
                        if (f.CanCombine) {
                            // compare corners of f with each list of connectedFixtures, looking for a match
                            bool found = false;
                            foreach (CombinedFixture sf in answer) {
                                if ((sf._angle == f.Angle) && (sf._sectionPlacement == f.SectionPlacement)) {
                                    Fixture last = sf._fixtures[sf._fixtures.Count - 1];
                                    if ((PFA.Distance(last.Points[2], f.Points[3]) <= 1.0) || (PFA.Distance(last.Points[3], f.Points[2]) <= 1.0)) {
                                        sf._fixtures.Add(f);
                                        if (f.Sections.Count > 0) sf._sections.AddRange(f.Sections);
                                        found = true;
                                        break;
                                    }
                                }
                            }
                            if (!found) {
                                // this fixture is the origin of a new CombinedFixture
                                CombinedFixture sf = new CombinedFixture(anglePair.Key);
                                sf._fixtures.Add(f);
                                if (f.Sections.Count > 0) sf._sections.AddRange(f.Sections);
                                sf._sectionPlacement = f.SectionPlacement;
                                sf._X = f.X;
                                sf._Y = f.Y;
                                answer.Add(sf);
                            }
                        }
                    }
                }

                // determine the width of each CombinedFixture
                foreach (CombinedFixture sf in answer) {
                    sf._width = 0;
                    foreach (Fixture f in sf._fixtures) sf._width += f.Width;
                }


                // determine the boundaries of each CombinedFixture
                foreach (CombinedFixture sf in answer) {
                    float minX = float.MaxValue;
                    float maxX = float.MinValue;
                    float minY = float.MaxValue;
                    float maxY = float.MinValue;

                    foreach (Fixture f in sf._fixtures) {
                        RectangleF box = f.BoundingRect();

                        if (box.Left < minX) minX = box.Left;
                        if (box.Right > maxX) maxX = box.Right;
                        if (box.Top < minY) minY = box.Top;
                        if (box.Bottom > maxY) maxY = box.Bottom;
                    }

                    sf._bounds = new RectangleF(minX, minY, maxX - minX, maxY - minY);
                }

                return answer;
            }


            /// <summary>
            /// Positions the Sections within a CombinedFixture.  Depending on the SectionPlacement setting of the CombinedFixture, the sections are pushed 
            /// towards either the left or right.
            /// </summary>
            public void PositionSections() {
                if ((_fixtures.Count == 1) || (_sections.Count == 0)) return;

                Section[] arySections = _sections.ToArray();

                // Sort the sections according to _sectionPlacement direction
                if (_sectionPlacement == 0) {
                    // arrange the sections from left to right in the CombinedFixture
                    if ((_angle < 90) || (_angle > 270)) Array.Sort(arySections, (a, b) => (a.AnchoringFixture == b.AnchoringFixture ? a.X.CompareTo(b.X) : a.AnchoringFixture.X.CompareTo(b.AnchoringFixture.X)));
                    else if (_angle == 90) Array.Sort(arySections, (a, b) => (a.AnchoringFixture == b.AnchoringFixture ? a.X.CompareTo(b.X) : a.AnchoringFixture.Y.CompareTo(b.AnchoringFixture.Y)));
                    else if (_angle == 270) Array.Sort(arySections, (a, b) => (a.AnchoringFixture == b.AnchoringFixture ? a.X.CompareTo(b.X) : b.AnchoringFixture.Y.CompareTo(a.AnchoringFixture.Y)));
                    else Array.Sort(arySections, (a, b) => (a.AnchoringFixture == b.AnchoringFixture ? a.X.CompareTo(b.X) : b.AnchoringFixture.X.CompareTo(a.AnchoringFixture.X)));

                    // push sections to the left (towards the CombinedFixture origin)
                    float x = _X;
                    for (int i = 0; i < arySections.Length; i++) {
                        arySections[i].Fields[Section.FieldNames.X] = x.ToString();
                        arySections[i].Fields[Section.FieldNames.Y] = (_Y + (arySections[i].AnchoringFixture.Depth - arySections[i].Depth)).ToString();
                        x += arySections[i].Width;
                    }

                } else {
                    // arrange the sections from right to left in CombinedFixture
                    if ((_angle < 90) || (_angle > 270)) Array.Sort(arySections, (a, b) => (b.AnchoringFixture == a.AnchoringFixture ? b.X.CompareTo(a.X) : b.AnchoringFixture.X.CompareTo(a.AnchoringFixture.X)));
                    else if (_angle == 90) Array.Sort(arySections, (a, b) => (b.AnchoringFixture == a.AnchoringFixture ? b.X.CompareTo(a.X) : b.AnchoringFixture.Y.CompareTo(a.AnchoringFixture.Y)));
                    else if (_angle == 270) Array.Sort(arySections, (a, b) => (b.AnchoringFixture == a.AnchoringFixture ? b.X.CompareTo(a.X) : a.AnchoringFixture.Y.CompareTo(b.AnchoringFixture.Y)));
                    else Array.Sort(arySections, (a, b) => (b.AnchoringFixture == a.AnchoringFixture ? b.X.CompareTo(a.X) : a.AnchoringFixture.X.CompareTo(b.AnchoringFixture.X)));

                    // push sections to the right (away from the CombinedFixture origin)
                    float x = _X + _width;
                    for (int i = 0; i < arySections.Length; i++) {
                        x -= arySections[i].Width;
                        arySections[i].Fields[Section.FieldNames.X] = x.ToString();
                        arySections[i].Fields[Section.FieldNames.Y] = (_Y + (arySections[i].AnchoringFixture.Depth - arySections[i].Depth)).ToString();
                    }
                }

                // set the AnchoringFixture for all Sections to be the first Fixture in CombinedFixture
                for (int i = 0; i < arySections.Length; i++) {
                    // sever connection with current AnchoringFixture
                    arySections[i].AnchoringFixture.Sections.Remove(arySections[i]);

                    // reset the AnchoringFixture of this section to be the starting fixture of this CombinedFixture
                    Fixture newAnchor = this._fixtures[0];
                    arySections[i].AnchoringFixture = newAnchor;
                    newAnchor.Sections.Add(arySections[i]);

                    // rotate using the new anchor
                    arySections[i].ComputeRotatedPoints();
                }
            }


            public override string ToString() {
                Fixture f = null;
                if (_fixtures.Count > 0) f = _fixtures[0];

                return String.Format("{0}, ({1}), {2}", f, _fixtures.Count, _bounds);
            }
        }
    }
}
