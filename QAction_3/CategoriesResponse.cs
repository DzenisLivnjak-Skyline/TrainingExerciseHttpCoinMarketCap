using System;
using System.Collections.Generic;

public class CategoriesResponse
{
    public List<Category> Data { get; set; }
}

public class Category
{
    public string Id { get; set; }
    public string Name { get; set; }
    public double NumTokens { get; set; }
    public double AvgPriceChange { get; set; }
    public double MarketCap { get; set; }
    public double Volume { get; set; }
    public DateTime LastUpdated { get; set; }
}

public class CategoriestableQActionRow
{
    public string Categoriestableinstance_201 { get; set; }
    public string Categoriesname_202 { get; set; }
    public double Categoriesnumberoftokens_203 { get; set; }
    public double Categoriesavgpricechange_204 { get; set; }
    public double Categoriesmarketcap_205 { get; set; }
    public double Categoriesvolume_206 { get; set; }
    public double Categorieslastupdated_207 { get; set; }
}
