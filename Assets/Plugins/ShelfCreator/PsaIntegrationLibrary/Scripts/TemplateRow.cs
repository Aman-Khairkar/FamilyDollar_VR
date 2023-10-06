using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class TemplateRow {
    public string Category;
    public string UPC;
    public string PlanogramName;
    public string X;
    public string Y;
    public string Height;
    public string Depth;
    public string Width;
    public string XFacings;
    public string YFacings;
    public string ZFacings;
    public string ShareOfXFacings;
    public string ShareOfLinear;
    public string EyeLevelRange;
    public string PrePost;
    public string Description1;
    public string Description2;
    public string Description3;
    public string Description4;
    public string StoreNumber;
    public string Form;
    public string Manufacturer;
    public string Brand;
    public string Description;
    public string GTIN;
    public string WeekEndDate;
    public string POSDollarSales;
    public string POSUnitSales;
    public string UnitPrice;
    public string OOS;
    public string MarketSales;

    public TemplateRow(string rowString)
    {
        Regex CSVParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
        string[] stat = CSVParser.Split(rowString);
        int i = 0;

        Category = stat[i++];
        UPC = stat[i++];
        PlanogramName = stat[i++];
        X = stat[i++];
        Y = stat[i++];
        Height = stat[i++];
        Depth = stat[i++];
        Width = stat[i++];
        XFacings = stat[i++];
        YFacings = stat[i++];
        ZFacings = stat[i++];
        ShareOfXFacings = stat[i++];
        ShareOfLinear = stat[i++];
        EyeLevelRange = stat[i++];
        PrePost = stat[i++];
        Description1 = stat[i++];
        Description2 = stat[i++];
        Description3 = stat[i++];
        Description4 = stat[i++];
        StoreNumber = stat[i++];
        Form = stat[i++];
        Manufacturer = stat[i++];
        Brand = stat[i++];
        Description = stat[i++];
        GTIN = stat[i++];
        WeekEndDate = stat[i++];
        POSDollarSales = stat[i++];
        POSUnitSales = stat[i++];
        UnitPrice = stat[i++];
        OOS = stat[i++];
        MarketSales = stat[i++];
    }
}
