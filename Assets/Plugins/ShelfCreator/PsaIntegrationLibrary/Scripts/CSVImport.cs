using System.Collections.Generic;
using System.IO;
using UnityEngine;
using JDA;


public class CSVImport : MonoBehaviour
{
    static PSA currentPsa = new PSA();
    string readErrors;
    //PSA string representation of the color white
    public const string PsaColorWhite = "16777215";

    // Use this for initialization
    void Start()
    {
             
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <param name="filename">Name of the file to read</param>
    /// <param name="errorMsg">string containing a list of errors and warnings for this JDA.PSA file</param>
    public static PSA Read(string filename, out string errorMsg)
    {
        //create a string of the file
        string[] rawData = File.ReadAllLines(filename);
        //create a PSA Project
        currentPsa.TheProject = new PSA.Project();
        currentPsa.TheProject.Measurement = PSA.Project.MeasurementSystem.Imperial;
        currentPsa.TheProject.Name = "FastCatPSA";
        //create a product dictionary
        currentPsa.TheProject.Products = new Dictionary<string, PSA.Product>();
        //create a planogram dictionary
        currentPsa.TheProject.Planograms = new Dictionary<string, PSA.Planogram>();

        //go through our file and fill our psa with the planogram, products, and positions.
        //we start i at 1 in order to skip the header row of the csv file.
        for (int i = 1; i < rawData.Length - 1; i++)
        {
            // The planogram this position is on
            PSA.Planogram plano;
            // The position corresponding to the current row
            PSA.Position pos;
            // The product this position is an instance of
            PSA.Product prod;

            // Parse the current row into a template object
            TemplateRow templateRow = new TemplateRow(rawData[i]);
            
            // If the current planogram is already listed in the psa
            if (currentPsa.TheProject.Planograms.ContainsKey(templateRow.PlanogramName))
            {
                // get a reference to the planogram
                plano = currentPsa.TheProject.Planograms[templateRow.PlanogramName];
            }
            else
            {
                // Create a new planogram
                plano = CreatePlanograms(templateRow.PlanogramName, templateRow.Category);
                // Add it to the PSA
                currentPsa.TheProject.Planograms.Add(templateRow.PlanogramName, plano);
            }

            //If the current product is already listed in the psa
            if (currentPsa.TheProject.Products.ContainsKey(templateRow.UPC + ":" + templateRow.UPC))
            {
                // Get a reference to the product
                prod = currentPsa.TheProject.Products[templateRow.UPC + ":" + templateRow.UPC];
            }
            else
            {
                // Create the product
                prod = CreateProduct(templateRow.Category, templateRow.UPC,
                                        templateRow.Height, templateRow.Depth, templateRow.Width,
                                        templateRow.Description1, templateRow.Description3, templateRow.Description4,
                                        templateRow.Manufacturer, templateRow.Brand,
                                        templateRow.UnitPrice, templateRow.POSDollarSales, templateRow.StoreNumber);
                // Add it to the PSA
                currentPsa.TheProject.Products.Add(prod.Key, prod);
            }

            // Create the position object
            pos = CreatePosition(templateRow.UPC,
                                    templateRow.X, templateRow.Y,
                                    templateRow.XFacings, templateRow.YFacings, templateRow.ZFacings,
                                    prod);
            // Add it to the PSA
            plano.Positions.Add(pos);

            /*
             * Size the planogram based on the products' positions and dimensions 
             * so that the planogram's back and base look approximately correct.
             */
            // If this position reaches higher than the current planogram height
            if (pos.Height + pos.Y > plano.Height)
            {
                // Set the top of the position as the new planogram height
                plano.Height = pos.Height + pos.Y;
            }

            // If this position reaches farther right than current planogram width
            if (pos.Width + pos.X > plano.Width)
            {
                // Set the right side of the position as the new planogram width
                plano.Width = pos.Width + pos.X;
                plano.Fields[PSA.Planogram.FieldNames.BaseWidth] = plano.Width + "";
            }

            // If this position reaches farther forward than current planogram depth
            if (pos.Depth + pos.Z > plano.Depth)
            {
                // Set the front side of the position as the new planogram depth
                plano.Depth = pos.Depth + pos.Z;
                // Set the base depth to match the planogram depth
                plano.Fields[PSA.Planogram.FieldNames.BaseDepth] = plano.Depth + "";
            }

            // If this position sits lower than the current top of the base
            if(pos.Y < float.Parse(plano.Fields[PSA.Planogram.FieldNames.BaseHeight]))
            {
                // Set the base height to match the bottom side of the position
                plano.Fields[PSA.Planogram.FieldNames.BaseHeight] = pos.Y.ToString();
            }      
        }
                
        errorMsg = "";
        
        return currentPsa;
    }
    /// <summary>
    /// creates a planogram with the info from the file
    /// </summary>
    /// <param name="name">the name of the planogram</param>
    /// <param name="category">the category of the planogram</param>
    /// <returns>returns a psa.planogram object with the planogram information</returns>
    public static PSA.Planogram CreatePlanograms(string name, string category)
    {
        PSA.Planogram temp = new PSA.Planogram();
        temp.Name = name;
        temp.Fields[PSA.Planogram.FieldNames.Key] = name;
        temp.Fields[PSA.Planogram.FieldNames.Category] = category;
        temp.Width = 1;
        temp.Height = 1;
        temp.Depth = 1;
        temp.Fields[PSA.Planogram.FieldNames.BaseHeight] = "400";
        temp.Fields[PSA.Planogram.FieldNames.BaseWidth] = "0";
        temp.Fields[PSA.Planogram.FieldNames.BaseDepth] = "0";
        temp.Fields[PSA.Planogram.FieldNames.BackDepth] = "1";
        temp.Fields[PSA.Planogram.FieldNames.DrawBase] = "1";
        temp.Fields[PSA.Planogram.FieldNames.DrawBack] = "1";
        temp.Fields[PSA.Planogram.FieldNames.BaseColor] = PsaColorWhite;
        temp.Fields[PSA.Planogram.FieldNames.Color] = PsaColorWhite;
        return temp;
    }
    /// <summary>
    /// creates a product based on the file information we were given
    /// </summary>
    /// <param name="category">the category of the product</param>
    /// <param name="upc">the upc of the product</param>
    /// <param name="height">the height of the product</param>
    /// <param name="depth">the depth of the product</param>
    /// <param name="width">the width of the product</param>
    /// <param name="name">the name of the product</param>
    /// <param name="desc3">the value for description 3 of the product</param>
    /// <param name="desc4">the value for description 4 of the product</param>
    /// <param name="manuf">the manufacturer of product</param>
    /// <param name="brand">the brand of the product</param>
    /// <param name="unitPrice">the unit price of the product</param>
    /// <param name="POS">the POS $ sales of the product</param>
    /// <returns>returns a psa.product object containing the products information</returns>
    public static PSA.Product CreateProduct(string category, string upc, string height, string depth, string width, string name, string desc3, string desc4,string manuf, string brand, string unitPrice, string POS, string StoreNumber)
    {
        PSA.Product temp = new PSA.Product();                
        temp.Fields[PSA.Product.FieldNames.Key] = upc + ":" + upc;
        temp.Fields[PSA.Product.FieldNames.Category] = category;
        temp.Fields[PSA.Product.FieldNames.UPC] = upc;
        temp.Fields[PSA.Product.FieldNames.ID] = upc;
        temp.Fields[PSA.Product.FieldNames.Name] = name;
        temp.Height = float.Parse(height);
        temp.Width = float.Parse(width);
        temp.Depth = float.Parse(depth);
        temp.Fields[PSA.Product.FieldNames.Brand] = brand;
        temp.Fields[PSA.Product.FieldNames.Desc3] = desc3;
        temp.Fields[PSA.Product.FieldNames.Desc4] = desc4;
        temp.Fields[PSA.Product.FieldNames.Desc10] = POS;
        temp.Fields[PSA.Product.FieldNames.Price] = unitPrice;
        temp.Fields[PSA.Product.FieldNames.Desc11] = StoreNumber;
        temp.Fields[PSA.Product.FieldNames.Manufacturer] = manuf;
        temp.Fields[PSA.Product.FieldNames.Color] = PsaColorWhite;
        
        return temp;  
    }
    /// <summary>
    /// creates positions in the psa 
    /// </summary>
    /// <param name="upc">the upc of the product</param>
    /// <param name="x">the x position of the bottom left back corner of the product</param>
    /// <param name="y">the y position of the bottom left back corner of the product</param>
    /// <param name="xfacings">the number of horizontal facings that this position contains</param>
    /// <param name="yfacings">the number of vertical facings that this position contains</param>
    /// <param name="zfacings">the number of depth facings that this position contains</param>
    /// <param name="prod">the product info for the orientation of the product</param>
    /// <returns>returns a psa.position object containing the information for this position</returns>
    public static PSA.Position CreatePosition(string upc, string x, string y, string xfacings, string yfacings, string zfacings, PSA.Product prod)
    {
        PSA.Position temp = new PSA.Position();
        temp.Fields[PSA.Position.FieldNames.ID] = upc;
        temp.Fields[PSA.Position.FieldNames.UPC] = upc;
        temp.X = float.Parse(x);
        temp.Y = float.Parse(y);
        temp.Z = 0;
        temp.Fields[PSA.Position.FieldNames.Orientation] = "0";
        //caps are not in the csv files, but you need to fill the values as if there were no caps still.
        temp.Fields[PSA.Position.FieldNames.XCapOrientation] = "0";
        temp.Fields[PSA.Position.FieldNames.YCapOrientation] = "0";
        temp.Fields[PSA.Position.FieldNames.ZCapOrientation] = "0";
        temp.Fields[PSA.Position.FieldNames.XCapNum] = "0";
        temp.Fields[PSA.Position.FieldNames.YCapNum] = "0";
        temp.Fields[PSA.Position.FieldNames.ZCapNum] = "0";
        temp.Fields[PSA.Position.FieldNames.XCapReversed] = "0";
        temp.Fields[PSA.Position.FieldNames.YCapReversed] = "0";
        temp.Fields[PSA.Position.FieldNames.ZCapReversed] = "0";
        temp.Fields[PSA.Position.FieldNames.HFacings] = xfacings;
        temp.Fields[PSA.Position.FieldNames.VFacings] = yfacings;
        temp.Fields[PSA.Position.FieldNames.DFacings] = zfacings;
        temp.Width = PsaProductPostionBuilder.GetIndividualRealWidth(prod, temp.Orientation) * temp.HFacings;
        temp.Height = PsaProductPostionBuilder.GetIndividualRealHeight(prod, temp.Orientation) * temp.VFacings;
        temp.Depth = PsaProductPostionBuilder.GetIndividualRealDepth(prod, temp.Orientation) * temp.DFacings;
        return temp;
    }

}
    
