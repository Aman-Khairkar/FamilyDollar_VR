using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;


namespace JDA
{

    /// <summary>
    /// The PSA class provides the high-level interface for working with JDA Space Planning PSA files.  
    /// To read a PSA file, use the static method PSA.Read(string filename, bool silent), which returns a PSA object containing the contents of the PSA file.
    /// To write a PSA file, use the method PSA.Write(string filename).
    /// </summary>
    public partial class PSA
    {
        public static string[] PsaDelimiters = new string[] { ",", "\t"};    // delimiters used to separate values in PFA files
        private static string FileFormatVersion = ";V2008.0.0.14";      // version number of reference file format guiding this implementation

        public List<string> Headers;                        // list of header lines that appear before the project keyword
        public PSA.Project TheProject;                      // project object 
        private Version _version;                           // JDA version number included in PSA file
        public List<string> Warnings;                       // list of warnings generated while reading in the PSA file


        public PSA() {
            Headers = new List<string>();
            Warnings = new List<string>();
        }


        /// <summary>
        /// Adds the values for many of the JDA calculated Product fields to the supplied data.  The definitions of the calculated fields
        /// was taken from the ProSpace help file.  This function was designed to be called on the results of the GetMergedProductDataForPlanogram() function.
        /// </summary>
        /// <param name="data">Dictionary of dictionaries containing product field data.  The top-level Dictionary is indexed by product ID; 
        /// the inner Dictionary is indexed by field name.</param>
        /// <param name="psa">PSA object associated with the data parameter</param>
        /// <param name="planogram">Name of the planogram associated with the data parameter</param>
        private static void AddCalculatedProductFields(Dictionary<string, Dictionary<string, string>> data, PSA psa, string planogram) {
            double a, b, c;

            // Get commonly used values for Project and Planagram fields
            Planogram ThePlanogram = psa.TheProject.Planograms[planogram];
            double ProjectMovementPeriod = 0; double.TryParse(psa.TheProject.Fields[Project.FieldNames.MovementPeriod], out ProjectMovementPeriod);

            double PlanogramMovementPeriod = 0;
            if (ThePlanogram.Fields.ContainsKey(Planogram.FieldNames.MovementPeriod)) {
                double.TryParse(ThePlanogram.Fields[Planogram.FieldNames.MovementPeriod], out PlanogramMovementPeriod);
            }
            if (PlanogramMovementPeriod == 0) PlanogramMovementPeriod = ProjectMovementPeriod;

            double PlanogramNumberOfStores = 1; double.TryParse(ThePlanogram.Fields[Planogram.FieldNames.NumberOfStores], out PlanogramNumberOfStores);
            double PlanogramWidth = 0; double.TryParse(ThePlanogram.Fields[Planogram.FieldNames.Width], out PlanogramWidth);
            double PlanogramHeight = 0; double.TryParse(ThePlanogram.Fields[Planogram.FieldNames.Height], out PlanogramHeight);
            double PlanogramDepth = 0; double.TryParse(ThePlanogram.Fields[Planogram.FieldNames.Depth], out PlanogramDepth);

            double PlanogramLinear = 0;
            double PlanogramSquare = 0;
            foreach (KeyValuePair<string, Fixture> fixPair in ThePlanogram.Fixtures) {
                if (double.TryParse(fixPair.Value.Fields[Fixture.FieldNames.Width], out a)) PlanogramLinear += a;
                if (double.TryParse(fixPair.Value.Fields[Fixture.FieldNames.Width], out a) && double.TryParse(fixPair.Value.Fields[Fixture.FieldNames.Depth], out b)) PlanogramSquare += a * b;

            }

            List<Position> ThePositions = ThePlanogram.AllPositions();

            // Calculations for each product
            foreach (KeyValuePair<string, Dictionary<string, string>> productPair in data) {
                Dictionary<string, string> product = productPair.Value;

                // Calculate certain Position data fields
				// Start by finding all positions for the current Product.
				// We must use the unique dictionary key since ID or UPC might be empty.
				List<Position> positions = ThePositions.FindAll(
					p => Product.CreateDictionaryKey(p.ID, p.UPC) == Product.CreateDictionaryKey(product[Product.FieldNames.ID],product[Product.FieldNames.UPC]));

                double productCapacity = 0;
                foreach (Position p in positions) {
                    double positionCapacity = 0;
                    if (double.TryParse(p.Fields[Position.FieldNames.HFacings], out a) && double.TryParse(p.Fields[Position.FieldNames.VFacings], out b) && double.TryParse(p.Fields[Position.FieldNames.DFacings], out c)) {
                        positionCapacity = a * b * c;
                    }
                    switch (p.Fields["Merch style"]) {
                        case "1": // tray
                            positionCapacity *= double.Parse(product["Tray total number"]);
                            break;
                        case "2": // case
                            positionCapacity *= double.Parse(product["Case total number"]);
                            break;
                        case "3": // display
                            positionCapacity *= Math.Max(1.0, double.Parse(product["Display total number"]));
                            break;
                        case "4": // alternate
                            positionCapacity *= Math.Max(1.0, double.Parse(product["Alternate total number"]));
                            break;
                        case "5": // loose; this will only work if a Loose total number is supplied, otherwise the Capacity will be wrong
                            if (double.Parse(product["Loose total number"]) != 0) {
                                positionCapacity *= double.Parse(product["Loose total number"]);
                            } else {
                                throw new Exception("Loose merchandising style is not implemented by the export library");
                            }
                            break;
                        case "6": // log stacking is not supported; the Capacity will be wrong
                            throw new Exception("Log Stacking merchandising style is not implemented by the export library");
                        default:
                            break;
                    }
                    productCapacity += positionCapacity;
                }
                product["Capacity"] = productCapacity.ToString();

                if (positions.Count > 0) product[Position.FieldNames.HFacings] = positions.Max(p => double.Parse(p.Fields[Position.FieldNames.HFacings])).ToString();
                if (positions.Count > 0) product[Position.FieldNames.VFacings] = positions.Max(p => double.Parse(p.Fields[Position.FieldNames.VFacings])).ToString();
                if (positions.Count > 0) product[Position.FieldNames.DFacings] = positions.Max(p => double.Parse(p.Fields[Position.FieldNames.DFacings])).ToString();
                product["Linear"] = positions.Sum(p => double.Parse(p.Fields[Position.FieldNames.Width])).ToString();
                product["Cubic"] = positions.Sum(p => double.Parse(p.Fields[Position.FieldNames.Width]) * double.Parse(p.Fields[Position.FieldNames.Height]) * double.Parse(p.Fields[Position.FieldNames.Depth])).ToString();
                product["Square"] = positions.Sum(p => double.Parse(p.Fields[Position.FieldNames.Width]) * double.Parse(p.Fields[Position.FieldNames.Depth])).ToString();
                product["Facings"] = positions.Sum(p => double.Parse(p.Fields[Position.FieldNames.HFacings]) + double.Parse(p.Fields[Position.FieldNames.XCapNum])).ToString();

                // calculate the Product reporting fields that are available in JDA ProSpace.  The definitions of these values was taken from the ProSpace help file.
                if (double.TryParse(product[Performance.FieldNames.UnitMovement], out a) && double.TryParse(product[Performance.FieldNames.Price], out b)) 
                    product["Sales"] = (a * b).ToString();
                if (double.TryParse(product[Performance.FieldNames.CaseCost], out a) && double.TryParse(product[Product.FieldNames.CaseTotalNumber], out b))
                    product["UnitCost"] = (b == 0 ? 0 : (a / b)).ToString();
                if (double.TryParse(product["Sales"], out a) && double.TryParse(product["UnitCost"], out b) && double.TryParse(product[Performance.FieldNames.UnitMovement], out c)) 
                    product["Profit"] = (a - b * c).ToString();
                if (double.TryParse(product["Capacity"], out a) && double.TryParse(product[Product.FieldNames.CaseTotalNumber], out b)) 
                    product["ActualCaseMultiple"] = (b == 0 ? 0 : (a / b)).ToString();
                if (double.TryParse(product["Capacity"], out a) && double.TryParse(product[Product.FieldNames.UnitMovement], out b)) 
                    product["ActualDaysSupply"] = (b == 0 || PlanogramMovementPeriod == 0 ? 0 : (a / (b / PlanogramMovementPeriod))).ToString();
                if (double.TryParse(product[Product.FieldNames.UnitMovement], out a)) 
                    product["AnnualMovement"] = (PlanogramMovementPeriod == 0 ? 0 : (a / PlanogramMovementPeriod * 365.25)).ToString();
                if (double.TryParse(product["Profit"], out a)) 
                    product["AnnualProfit"] = (PlanogramMovementPeriod == 0 ? 0 : (a / PlanogramMovementPeriod * 365.25)).ToString();
                if (double.TryParse(product["UnitCost"], out a) && double.TryParse(product["Capacity"], out b)) 
                    product["CapacityCost"] = (a * b).ToString();
                if (double.TryParse(product["Price"], out a) && double.TryParse(product["Capacity"], out b)) 
                    product["CapacityRetail"] = (a * b).ToString();
                if (double.TryParse(product["Cubic"], out a)) 
                    product["Cubic%"] = (100 * a / (PlanogramWidth * PlanogramHeight * PlanogramDepth)).ToString();
                if (double.TryParse(product["Linear"], out a)) 
                    product["Linear%"] = (100 * a / PlanogramLinear).ToString();
                if (double.TryParse(product[Product.FieldNames.UnitMovement], out a) && double.TryParse(product["Cubic"], out b)) 
                    product["Movement/Cubic"] = (b == 0 ? 0 : (a / b)).ToString();
                if (double.TryParse(product[Product.FieldNames.UnitMovement], out a) && double.TryParse(product["Linear"], out b)) 
                    product["Movement/Linear"] = (b == 0 ? 0 : (a / b)).ToString();
                if (double.TryParse(product[Product.FieldNames.UnitMovement], out a) && double.TryParse(product["Square"], out b)) 
                    product["Movement/Square"] = (b == 0 ? 0 : (a / b)).ToString();
                if (double.TryParse(product["Profit"], out a) && double.TryParse(product["Cubic"], out b)) 
                    product["Profit/Cubic"] = (b == 0 ? 0 : (a / b)).ToString();
                if (double.TryParse(product["Profit"], out a) && double.TryParse(product["Linear"], out b)) 
                    product["Profit/Linear"] = (b == 0 ? 0 : (a / b)).ToString();
                if (double.TryParse(product["Profit"], out a) && double.TryParse(product["Square"], out b)) 
                    product["Profit/Square"] = (b == 0 ? 0 : (a / b)).ToString();
                if (double.TryParse(product["AnnualProfit"], out a) && double.TryParse(product["CapacityCost"], out b)) 
                    product["RoiiCost"] = (b == 0 ? 0 : (a / b)).ToString();
                if (double.TryParse(product["AnnualProfit"], out a) && double.TryParse(product["CapacityRetail"], out b)) 
                    product["RoiiRetail"] = (b == 0 ? 0 : (a / b)).ToString();
                if (double.TryParse(product["Sales"], out a) && double.TryParse(product["Cubic"], out b)) 
                    product["Sales/Cubic"] = (b == 0 ? 0 : (a / b)).ToString();
                if (double.TryParse(product["Sales"], out a) && double.TryParse(product["Linear"], out b)) 
                    product["Sales/Linear"] = (b == 0 ? 0 : (a / b)).ToString();
                if (double.TryParse(product["Sales"], out a) && double.TryParse(product["Square"], out b)) 
                    product["Sales/Square"] = (b == 0 ? 0 : (a / b)).ToString();
                if (double.TryParse(product["Square"], out a)) 
                    product["Square%"] = (PlanogramSquare == 0 ? 0 : (100 * a / PlanogramSquare)).ToString();
                if (double.TryParse(product["AnnualMovement"], out a) && double.TryParse(product["Capacity"], out b)) 
                    product["Turns"] = (b == 0 ? 0 : (a / b)).ToString();
                if (double.TryParse(product["Price"], out a) && double.TryParse(product["UnitCost"], out b)) 
                    product["UnitProfit"] = (a - b).ToString();
				if (double.TryParse(product["Capacity"], out a) && double.TryParse(product[Performance.FieldNames.UnitMovement], out b))
					product["Reach"] = (a / b).ToString();

            }
            
            // Percentage calculations, which requires computing the sum across all products
            double movementTotal = 0;
            double linearTotal = 0;
            double profitTotal = 0;
            double salesTotal = 0;
            double squareTotal = 0;			
			double facingsTotal = 0;
			double positionsTotal = 0; 

            foreach (KeyValuePair<string, Dictionary<string, string>> productPair in data) {
                //string productUPC = productPair.Key;
                Dictionary<string, string> product = productPair.Value;

                if (double.TryParse(product[Product.FieldNames.UnitMovement], out a)) movementTotal += a;
                if (double.TryParse(product["Linear"], out a)) linearTotal += a;
                if (double.TryParse(product["Profit"], out a)) profitTotal += a;
                if (double.TryParse(product["Sales"], out a)) salesTotal += a;
                if (double.TryParse(product["Square"], out a)) squareTotal += a;				
				if (double.TryParse(product["Facings"], out a)) facingsTotal += a;
				if (double.TryParse(product[Product.FieldNames.NumberOfPositions], out a)) positionsTotal += a;
            }
            foreach (KeyValuePair<string, Dictionary<string, string>> productPair in data) {
                //string productUPC = productPair.Key;
                Dictionary<string, string> product = productPair.Value;

                if (double.TryParse(product[Product.FieldNames.UnitMovement], out a)) 
                    product["Movement%"] = (movementTotal == 0 ? 0 : (100 * a / movementTotal)).ToString(); //volume share
                if (double.TryParse(product["Linear"], out a)) 
                    product["Linear%Used"] = (linearTotal == 0 ? 0 : (100 * a / linearTotal)).ToString(); //share of shelf
                if (double.TryParse(product["Profit"], out a)) 
                    product["Profit%"] = (profitTotal == 0 ? 0 : (100 * a / profitTotal)).ToString();
                if (double.TryParse(product["Sales"], out a)) 
                    product["Sales%"] = (salesTotal == 0 ? 0 : (100 * a / salesTotal)).ToString(); //turnover share
                if (double.TryParse(product["Square"], out a)) 
                    product["Square%Used"] = (squareTotal == 0 ? 0 : (100 * a / squareTotal)).ToString();				
				if (double.TryParse(product["Facings"], out a)) 
					product["Facings%"] = (facingsTotal == 0 ? 0 : (100 * a / facingsTotal)).ToString(); //share of facings
				if (double.TryParse(product[Product.FieldNames.NumberOfPositions], out a)) 
					product["Positions%"] = (positionsTotal == 0 ? 0 : (100 * a / positionsTotal)).ToString(); //share of skus
				//if (double.TryParse(product["Linear%Used"], out a) && double.TryParse(product["Movement%"], out b)) {
				//	product["Fair Share (lx volume)"] = (b == 0 ? 0 : (100 * a / b)).ToString(); 
				//}
            }

        }


        /// <summary>
        /// Gets the Planogram names from a PSA file.
        /// </summary>
        /// <param name="filename">PSA file name</param>
        /// <param name="errorMsg">Upon return, contains an error message if the action was unsuccessful.</param>
        /// <returns>List of planogram names</returns>
        public static List<string> GetPlanogramNames(string filename, out string errorMsg) {
            string[] rawData = File.ReadAllLines(filename);
            errorMsg = "";

            // make sure its a PSA file
            if (!rawData[0].StartsWith("PROSPACE SCHEMATIC FILE")) {
                errorMsg = "File is not a JDA Space Planning Schematic";
                return null;
            }

            // scan the lines, looking for planograms
            List<string> names = new List<string>();
            for (int i = 0; i < rawData.Length; i++) {
                string line = rawData[i];
                if (line.StartsWith("Planogram")) {
                    string[] fields = SplitLine(line);
                    names.Add(fields[1]);
                }
            }

            return names;
        }


        /// <summary>
        /// Returns a Dictionary containing data about each of the products that appear in the specified planogram.  The data is a merger of the Product
        /// Performance, and Position records.  The Dictionary is indexed by the ID of the product.
        /// </summary>
        /// <param name="psa">PSA object</param>
        /// <param name="planogram">Planogram name</param>
		/// <param name="filterField">Analytics Field to filter products out on. If null or empty, products are not filtered.</param>
		/// <param name="filterValue">Value of Analytics Field to filter products out on. If null or empty, products are not filtered.</param> 
        /// <returns>Dictionary of dictionaries containing product information.  The top-level dictionary is indexed by product IDs; the inner dictionary is
        /// indexed by field names.</returns>
        private static Dictionary<string, Dictionary<string, string>> GetMergedProductDataForPlanogram(PSA psa, string planogram, string filterField, string filterValue) {
            Planogram pog = psa.TheProject.Planograms[planogram];
            Dictionary<string, Dictionary<string, string>> data = new Dictionary<string, Dictionary<string, string>>();
            List<string> unusedProductsKeys = pog.UnusedProducts();

            // process all of the products that appear in the planogram
            foreach (KeyValuePair<string, Performance> performancePair in pog.Performances) {
                Product product = psa.TheProject.Products[performancePair.Key];

				// Filter out the data
				bool filterChecksOut = true;
				if(!string.IsNullOrEmpty(filterField) && !string.IsNullOrEmpty(filterValue)) {
					filterChecksOut = false;

					string productFilterValue = "";
					if(product.Fields.TryGetValue(filterField, out productFilterValue)) {
						if(filterValue.Equals(productFilterValue)) {
							filterChecksOut = true;
						}
					}
				}

                if (!unusedProductsKeys.Contains(product.DictionaryKey) && filterChecksOut) {
                    // make a copy of the product data fields
                    Dictionary<string, string> productData = new Dictionary<string, string>();
                    foreach (KeyValuePair<string, string> pair in product.Fields) {
                        productData[pair.Key] = pair.Value;
                    }

                    // merge in the performance data fields
                    foreach (KeyValuePair<string, string> pair in performancePair.Value.Fields) {
                        // if this Performance field has the same name as a Product field, then overwrite the Product data IF the Performance data is non-zero
                        if (productData.ContainsKey(pair.Key)) {
                            if (!string.IsNullOrEmpty(pair.Value)) {
                                double number;
                                if (double.TryParse(pair.Value, out number)) {
                                    if (number != 0) {
                                        productData[pair.Key] = pair.Value;
                                    }
                                } else {
                                    productData[pair.Key] = pair.Value;
                                }
                            }

                        } else {
                            // there is no matching Product field, so add this data
                            productData[pair.Key] = pair.Value;
                        }
                    }

                    string key = Product.CreateDictionaryKey(productData[Product.FieldNames.ID], productData[Product.FieldNames.UPC]);
                    if (data.ContainsKey(key)) {
                        throw new Exception("Product with ID:UPC of " + key + " appears more than once in planogram.");
                    } else {
                        data[key] = productData;
                    }
                }
            }

            return data;
        }


        /// <summary>
        /// Returns a matrix of product data. The first row of the matrix contains column headers.
        /// If the planogram is not successfully read from the PSA file, null is returned.
        /// </summary>
        /// <param name="filename">PSA file name</param>
        /// <param name="planogram">Name of the planogram</param>
        /// <param name="errorMsg">Upon return, contains an error message if the action was unsuccessful.</param>
        /// <returns>2D matrix of strings if successful; null, otherwise.</returns>
        public static string[,] GetProductDataMatrix(string filename, string planogram, out string errorMsg) {
            // read the PSA file and get the planogram
            PSA psa = Read(filename, out errorMsg);
            if (psa == null) return null;
            if (!psa.TheProject.Planograms.ContainsKey(planogram)) {
                errorMsg = "PSA file does not contain the requested planogram";
                return null;
            }

            try {
                Planogram pog = psa.TheProject.Planograms[planogram];
                List<string> unusedProductsKeys = pog.UnusedProducts();

                // get a list of the product fields
                List<string> productFields = psa.TheProject.Products.Values.First().Fields.Keys.ToList();

                // construct a products+1 X productFields matrix of data for all the products in the planogram
                string[,] matrix = new string[pog.Performances.Count+1, productFields.Count];

                // insert header row
                for (int col = 0; col < productFields.Count; col++) matrix[0, col] = productFields[col];

                // insert data rows
                int row = 1;
                foreach (KeyValuePair<string, Performance> performancePair in pog.Performances) {
                    Product product = psa.TheProject.Products[performancePair.Key];

                    if (!unusedProductsKeys.Contains(product.DictionaryKey)) {
                        for (int col = 0; col < productFields.Count; col++) {
                            string productValue = product.Fields[productFields[col]];

                            // copy the product data into the matrix first, to serve as default values
                            matrix[row, col] = productValue;

                            // if there is non-zero performance data for the same field, copy it 
                            if (performancePair.Value.Fields.ContainsKey(productFields[col])) {
                                string value = performancePair.Value.Fields[productFields[col]];
                                if (!string.IsNullOrEmpty(value)) {
                                    double number;
                                    if (double.TryParse(value, out number)) {
                                        if (number != 0) {
                                            matrix[row, col] = value;
                                        }
                                    } else {
                                        matrix[row, col] = value;
                                    }
                                }
                            }
                        }
                        row++;
                    }
                }

                return matrix;

            } catch (Exception ex) {
                errorMsg = ex.Message;
                return null;
            }
        }


        /// <summary>
        /// Returns a matrix of product data in the form of a tab-delimited string.  Each line contains the data for
        /// one product.  If the planogram is not successfully read from the PSA file, null is returned.
        /// </summary>
        /// <param name="filename">PSA file name</param>
        /// <param name="planogram">Name of the planogram</param>
        /// <param name="deleteEmptyColumns">Delete columns that are empty or contain all zeros</param>
        /// <param name="errorMsg">Upon return, contains an error message if the action was unsuccessful.</param>
        /// <returns>Tab-delimited string if read is successful; null, otherwise.</returns>
        public static string GetProductDataAsTabDelimitedString(string filename, string planogram, bool deleteEmptyColumns, out string errorMsg) {
            string[,] data = GetProductDataMatrix(filename, planogram, out errorMsg);
            if (data == null) return null;

            // scan through the columns of the matrix, finding columns with non-zero data
            List<int> keepers = new List<int>();
            if (deleteEmptyColumns) {
                for (int col = 0; col < data.GetLength(1); col++) {
                    for (int row = 1; row < data.GetLength(0); row++) {
                        if (!string.IsNullOrEmpty(data[row, col])) {
                            double number;
                            if (!double.TryParse(data[row, col], out number) || (number != 0)) {
                                keepers.Add(col);
                                break;
                            }
                        }
                    }
                }
            }

            // build the tab-delimited string representation of matrix
            StringBuilder sb = new StringBuilder();
            for (int row = 0; row < data.GetLength(0); row++) {
                for (int col = 0; col < data.GetLength(1); col++) {
                    if (!deleteEmptyColumns || keepers.Contains(col)) {
                        sb.Append(data[row, col]);
                        sb.Append("\t");
                    }
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }

		/// <summary>
		/// Returns a Dictionary of data about products appearing in a specified planogram to be used by TVS.  This data includes fields from
		/// the JDA Product, Performance, and Position records, and values that ProSpace normally computes for use in reporting tables.
		/// The top-level Dictionary is indexed by product ID; the inner Dictionary is indexed by field name.
		/// 
		/// If the planogram is not successfully read from the PSA file, null is returned.
		/// </summary>
		/// <param name="filename">PSA file name</param>
		/// <param name="planogram">Name of the planogram</param>
		/// <param name="errorMsg">Upon return, contains an error message if the action was unsuccessful.</param>
		/// <returns>Dictionary of dictionaries containing product data.  The top-level Dictionary is indexed by product ID; 
		/// the inner Dictionary is indexed by field name.</returns>
		public static Dictionary<string, Dictionary<string, string>> GetTvsAnalyticsData(string filename, string planogram,out string errorMsg) {             
			return GetTvsAnalyticsData(filename, planogram, null, null, out errorMsg);
		}
        /// <summary>
        /// Returns a Dictionary of data about products appearing in a specified planogram to be used by TVS.  This data includes fields from
        /// the JDA Product, Performance, and Position records, and values that ProSpace normally computes for use in reporting tables.
        /// The top-level Dictionary is indexed by product ID; the inner Dictionary is indexed by field name.
        /// 
        /// If the planogram is not successfully read from the PSA file, null is returned.
        /// </summary>
        /// <param name="filename">PSA file name</param>
        /// <param name="planogram">Name of the planogram</param>
		/// <param name="filterField">Analytics Field to filter products out on. If null or empty, products are not filtered.</param>
		/// <param name="filterValue">Value of Analytics Field to filter products out on. If null or empty, products are not filtered.</param> 
        /// <param name="errorMsg">Upon return, contains an error message if the action was unsuccessful.</param>
        /// <returns>Dictionary of dictionaries containing product data.  The top-level Dictionary is indexed by product ID; 
        /// the inner Dictionary is indexed by field name.</returns>
        public static Dictionary<string, Dictionary<string, string>> GetTvsAnalyticsData(string filename, string planogram, string filterField, string filterValue, out string errorMsg) {             
            // read the PSA file and get the planogram
            PSA psa = Read(filename, out errorMsg);

            if (psa == null) return null;
            if (!psa.TheProject.Planograms.ContainsKey(planogram)) {
                errorMsg = "PSA file does not contain the requested planogram";
                return null;
            }

            try {
                // get the raw data 
                Dictionary<string, Dictionary<string, string>> data = GetMergedProductDataForPlanogram(psa, planogram, filterField, filterValue);

				// add in the computed JDA analytics fields
                AddCalculatedProductFields(data, psa, planogram);
                return data;

            } catch (Exception ex) {
                errorMsg = ex.Message;

                return null;
            }

        }


        /// <summary>
        /// Returns a matrix of the TVS analytics data for the products in a planogram in the form of a tab-delimited string.  Each line contains the data for
        /// one product.  If the planogram is not successfully read from the PSA file, null is returned.
        /// </summary>
        /// <param name="filename">PSA file name</param>
        /// <param name="planogram">Name of the planogram</param>
        /// <param name="deleteEmptyColumns">Delete columns that are empty or contain all zeros</param>
        /// <param name="errorMsg">Upon return, contains an error message if the action was unsuccessful.</param>
        /// <returns>Tab-delimited string if read is successful; null, otherwise.</returns>
        public static string GetTvsAnalyticsDataAsTabDelimitedString(string filename, string planogram, bool deleteEmptyColumns, out string errorMsg) {
            string[,] data = GetTvsAnalyticsDataMatrix(filename, planogram, out errorMsg);
            if (data == null) return null;

            // scan through the columns of the matrix, finding columns with non-zero data
            List<int> keepers = new List<int>();
            if (deleteEmptyColumns) {
                for (int col = 0; col < data.GetLength(1); col++) {
                    for (int row = 1; row < data.GetLength(0); row++) {
                        if (!string.IsNullOrEmpty(data[row, col])) {
                            double number;
                            if (!double.TryParse(data[row, col], out number) || (number != 0)) {
                                keepers.Add(col);
                                break;
                            }
                        }
                    }
                }
            }

            // build the tab-delimited string representation of matrix
            StringBuilder sb = new StringBuilder();
            for (int row = 0; row < data.GetLength(0); row++) {
                for (int col = 0; col < data.GetLength(1); col++) {
                    if (!deleteEmptyColumns || keepers.Contains(col)) {
                        sb.Append(data[row, col]);
                        sb.Append("\t");
                    }
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }


        /// <summary>
        /// Returns a matrix of data about products appearing in a specified planogram to be used by TVS.  This data includes fields from
        /// the JDA Product, Performance, and Position records, and values that ProSpace normally computes for use in reporting tables.
        /// The first row of the matrix contains column headers.
        /// 
        /// If the planogram is not successfully read from the PSA file, null is returned.
        /// </summary>
        /// <param name="filename">PSA file name</param>
        /// <param name="planogram">Name of the planogram</param>
        /// <param name="errorMsg">Upon return, contains an error message if the action was unsuccessful.</param>
        /// <returns>2D matrix of strings if successful; null, otherwise.</returns>
        public static string[,] GetTvsAnalyticsDataMatrix(string filename, string planogram, out string errorMsg) {
            Dictionary<string, Dictionary<string, string>> data = GetTvsAnalyticsData(filename, planogram, out errorMsg);
            if (data == null) return null;

            // gather the names of the fields appearing in data
            List<string> fields = new List<string>();
            foreach (KeyValuePair<string, Dictionary<string, string>> productPair in data) {
                List<string> keys = productPair.Value.Keys.ToList();
                foreach (string key in keys) {
                    if (!fields.Contains(key)) fields.Add(key);
                }
            }


            // create the data matrix
            string[,] matrix = new string[ data.Count+1, fields.Count];
            // insert the header row
            for (int i = 0; i < fields.Count; i++) matrix[0, i] = fields[i];
            // insert the data
            int row = 1;
            foreach (KeyValuePair<string, Dictionary<string, string>> productPair in data) {
                foreach (KeyValuePair<string, string> fieldPair in productPair.Value) {
                    matrix[row, fields.IndexOf(fieldPair.Key)] = fieldPair.Value;
                }
                row += 1;
            }

            return matrix;
        }


        /// <summary>
        /// Attempts to read in a PSA file.  If this routine fails or the user cancels the operation, NULL is returned; otherwise, 
        /// a JDA.PSA object is returned.
        /// </summary>
        /// <param name="filename">Name of the file to read</param>
        /// <param name="errorMsg">string containing a list of errors and warnings for this JDA.PSA file</param>
        /// <returns>JDA.PSA object if the read is successful; NULL otherwise.</returns>
        public static PSA Read(string filename, out string errorMsg)
        {
            string[] rawData = File.ReadAllLines(filename);
            return Read(rawData, out errorMsg);
        }

        /// <summary>
        /// Attempts to read in a PSA file.  If this routine fails or the user cancels the operation, NULL is returned; otherwise, 
        /// a JDA.PSA object is returned.
        /// </summary>
        /// <param name="filename">Name of the file to read</param>
        /// <param name="errorMsg">string containing a list of errors and warnings for this JDA.PSA file</param>
        /// <returns>JDA.PSA object if the read is successful; NULL otherwise.</returns>
        public static PSA Read(string[] rawData, out string errorMsg)
        {
            PSA currentPsa = new PSA();
            bool inHeaders = true;
            Fixture currentFixture = null;
            Planogram currentPlanogram = null;
            Project currentProject = null;

            if (!rawData[0].StartsWith("PROSPACE SCHEMATIC FILE"))
            {
                currentPsa.Warnings.Add("File is not a JDA Space Planning Schematic");

            }
            else
            {
                if (rawData[1].StartsWith(";V")) currentPsa._version = new Version(rawData[1].Substring(2));

                for (int i = 0; i < rawData.Length; i++)
                {
                    string line = rawData[i];

                    if (line.StartsWith("Project,"))
                    {
                        currentProject = Project.ParsePSA(line);
                        inHeaders = false;

                    }
                    else if (inHeaders)
                    {
                        currentPsa.Headers.Add(line);


                    }
                    else if (line.StartsWith("Divider"))
                    {
                        if (currentFixture != null)
                        {
                            currentFixture.Dividers.Add(Divider.ParsePSA(line, currentFixture));
                        }
                        else
                        {
                            currentPsa.Warnings.Add("No current fixture for divider, line " + (i + 1));
                        }

                    }
                    else if (line.StartsWith("Drawing"))
                    {
                        if (currentPlanogram != null)
                        {
                            currentPlanogram.Drawings.Add(Drawing.ParsePSA(line));
                        }
                        else
                        {
                            currentPsa.Warnings.Add("No current planogram for drawing item, line " + (i + 1));
                        }

                    }
                    else if (line.StartsWith("Fixture"))
                    {
                        Fixture fix = Fixture.ParsePSA(line);
                        if (currentPlanogram != null)
                        {
                            currentPlanogram.Fixtures[fix.UniqueID] = fix;
                            fix.Planogram = currentPlanogram;
                        }
                        currentFixture = fix;

                        // if the fixture is a "polygonal shelf", it might be followed by 3D point lines
                        if (fix.Type == Fixture.FixtureType.PolygonalShelf)
                        {
                            // parse any 3D Point lines that directly follow the fixture
                            while ((i + 1 < rawData.Length) && (rawData[i + 1].StartsWith("3D point")))
                            {
                                line = rawData[++i];
                                fix.Points.Add(Point3D.ParsePSA(line));
                            }

                            // if there were no 3D Point lines, the fixture is a rectangle, so create points for it
                            if (fix.Points.Count == 0)
                            {
                                fix.Points.Add(new Point3D(fix.X, fix.Y, fix.Z, ""));
                                fix.Points.Add(new Point3D(fix.X + fix.Width, fix.Y, fix.Z, ""));
                                fix.Points.Add(new Point3D(fix.X + fix.Width, fix.Y + fix.Depth, fix.Z, ""));
                                fix.Points.Add(new Point3D(fix.X, fix.Y + fix.Depth, fix.Z, ""));
                            }
                        }

                    } else if (line.StartsWith("Peg")) {
                        if (currentProject != null) {
                            currentProject.Pegs.Add(Peg.ParsePSA(line));
                        } else {
                            currentPsa.Warnings.Add("No current project for peg item, line " + (i + 1));
                        }

                    } else if (line.StartsWith("Performance")) {
                        if (currentPlanogram != null) {
                            Performance p = Performance.ParsePSA(line);
                            currentPlanogram.Performances[p.DictionaryKey] = p;
                        } else {
                            currentPsa.Warnings.Add("No current planogram for performance item, line " + (i + 1));
                        }

                    } else if (line.StartsWith("Planogram")) {
                        Planogram plan = Planogram.ParsePSA(line);
                        string s = plan.Name;
                        if (currentProject.Planograms.ContainsKey(s)) {
                            currentPsa.Warnings.Add("There already exists a planogram named '" + s + "' in project, line " + (i + 1));
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
                        if ((plan.Category == null) || (plan.Category.Length == 0)) currentPsa.Warnings.Add("No category found for planogram item " + plan.Name + ", line " + (i + 1));

                    } else if (line.StartsWith("Position")) {
                        if (currentFixture != null) {
                            currentFixture.Positions.Add(Position.ParsePSA(line));
                        } else if (currentPlanogram != null) {
                            currentPlanogram.Positions.Add(Position.ParsePSA(line));
                        } else {
                            currentPsa.Warnings.Add("No current planogram for position item, line " + (i + 1));
                        }

                    } else if (line.StartsWith("Product")) {
                        if (currentProject != null) {
                            Product p = Product.ParsePSA(line);
                            if (currentProject.Products.ContainsKey(p.DictionaryKey)) {
                                currentPsa.Warnings.Add("A product with dictionary key = " + p.DictionaryKey + " already exists, line " + (i + 1));
                            } 
                            currentProject.Products[p.DictionaryKey] = p;
                        } else {
                            currentPsa.Warnings.Add("No current project for product, line " + (i + 1));
                        }

                    } else if (line.StartsWith("Segment")) {
                        Segment seg = Segment.ParsePSA(line);
                        if (currentPlanogram != null) currentPlanogram.Segments.Add(seg);
                        else currentPsa.Warnings.Add("No current planogram for segment item, line " + (i + 1));

                    } else if (line.StartsWith("Supplier")) {
                        if (currentPlanogram != null) {
                            Supplier sup = Supplier.ParsePSA(line);
                            currentPlanogram.Suppliers[sup.Key] = sup;
                        } else {
                            currentPsa.Warnings.Add("No current planogram for supplier, line " + (i + 1));
                        }

                    } else if (!inHeaders) {
                        // unclassified data line
                        if (currentPlanogram != null) currentPlanogram.Unknowns.Add(line);
                        else currentPsa.Warnings.Add("No current planogram for unrecognized item, line " + (i + 1));
                    }
                }
            }


            // deal with errors and warnings
            errorMsg = "";
            if (currentPsa.Warnings.Count > 0) {
                errorMsg = "The following problems were discovered while reading in a PSA file:\r\n\r\n";
                foreach (string s in currentPsa.Warnings) errorMsg += s + "\r\n";
            }

            // return the PSA object
            if (currentPsa != null) currentPsa.TheProject = currentProject;
            return currentPsa;
        }



        /// <summary>
        /// Splits a line from a PSA file into delimited parts.  A special function is needed to deal with slash-delimited commas embedded in the line.
        /// </summary>
        /// <param name="line">String to split</param>
        /// <returns>string[]</returns>
        public static string[] SplitLine(string line) {
            string[] parts = line.Replace(@"\,", "$comma$").Split(PsaDelimiters, StringSplitOptions.None);
            for (int i = 0; i < parts.Length; i++) {
                if (parts[i].Contains("$comma$")) parts[i] = parts[i].Replace("$comma$", @"\,");
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
        /// Writes a PSA structure to a file
        /// </summary>
        /// <param name="filename">Name of file to write</param>
        public void Write(string filename) {
            StreamWriter sw = new StreamWriter(filename, false, Encoding.ASCII);
            sw.NewLine = "\r\n";

            // headers
            sw.WriteLine("PROSPACE SCHEMATIC FILE");
            sw.WriteLine(FileFormatVersion);
            sw.WriteLine(TheProject.WriteString());

            // products
            foreach (KeyValuePair<string, Product> prod in TheProject.Products) {
                sw.WriteLine(prod.Value.WriteString());
            }

            // pegs
            foreach (Peg peg in TheProject.Pegs) {
                sw.WriteLine(peg.WriteString());
            }

            // planograms
            foreach (KeyValuePair<string, Planogram> plan in TheProject.Planograms) {
                string ss = plan.Value.WriteString();
                sw.WriteLine(ss);

                // performance
                foreach (KeyValuePair<string, Performance> perf in plan.Value.Performances) {
                    sw.WriteLine(perf.Value.WriteString());
                }

                // segments
                foreach (Segment s in plan.Value.Segments) {
                    sw.WriteLine(s.WriteString());
                }

                // positions
                foreach (Position pos in plan.Value.Positions) {
                    sw.WriteLine(pos.WriteString());
                }

                // fixtures
                foreach (KeyValuePair<string, Fixture> fix in plan.Value.Fixtures) {
                    sw.WriteLine(fix.Value.WriteString());
                }

                // suppliers
                foreach (KeyValuePair<string, Supplier> supplier in plan.Value.Suppliers) {
                    sw.WriteLine(supplier.Value.WriteString());
                }

                // drawings
                foreach (Drawing d in plan.Value.Drawings) {
                    sw.WriteLine(d.WriteString());
                }
                // unknowns
                foreach (string u in plan.Value.Unknowns) {
                    sw.WriteLine(u);
                }
            }

            sw.Close();
        }

        /// <summary>
        /// Writes a PSA structure to a string and returns it
        /// </summary>
        /// <returns> PSA represented in a string </returns>
        public string WriteString()
        {
            string psaResult = "";
            //StreamWriter sw = new StreamWriter(filename, false, Encoding.ASCII);
            string newLine = "\r\n";

            // headers
            psaResult += "PROSPACE SCHEMATIC FILE" + newLine;
            psaResult += FileFormatVersion + newLine;
            psaResult += TheProject.WriteString() + newLine;

            // products
            foreach (KeyValuePair<string, Product> prod in TheProject.Products)
            {
                psaResult += prod.Value.WriteString() + newLine;
            }

            // pegs
            foreach (Peg peg in TheProject.Pegs)
            {
                psaResult += peg.WriteString() + newLine;
            }

            // planograms
            foreach (KeyValuePair<string, Planogram> plan in TheProject.Planograms)
            {
                string ss = plan.Value.WriteString();
                psaResult += ss + newLine;

                // performance
                foreach (KeyValuePair<string, Performance> perf in plan.Value.Performances)
                {
                    psaResult += perf.Value.WriteString() + newLine;
                }

                // segments
                foreach (Segment s in plan.Value.Segments)
                {
                    psaResult += s.WriteString() + newLine;
                }

                // positions
                foreach (Position pos in plan.Value.Positions)
                {
                    psaResult += pos.WriteString() + newLine;
                }

                // fixtures
                foreach (KeyValuePair<string, Fixture> fix in plan.Value.Fixtures)
                {
                    psaResult += fix.Value.WriteString() + newLine;
                }

                // suppliers
                foreach (KeyValuePair<string, Supplier> supplier in plan.Value.Suppliers)
                {
                    psaResult += supplier.Value.WriteString() + newLine;
                }

                // drawings
                foreach (Drawing d in plan.Value.Drawings)
                {
                    psaResult += d.WriteString() + newLine;
                }
                // unknowns
                foreach (string u in plan.Value.Unknowns)
                {
                    psaResult += u + newLine;
                }
            }

            return psaResult;
        }
    }
}
