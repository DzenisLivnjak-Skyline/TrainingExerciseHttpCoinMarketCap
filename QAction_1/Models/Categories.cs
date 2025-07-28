﻿// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

public class Datum
{
    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("title")]
    public string Title { get; set; }

    [JsonProperty("description")]
    public string Description { get; set; }

    [JsonProperty("num_tokens")]
    public int NumTokens { get; set; }

    [JsonProperty("avg_price_change")]
    public double AvgPriceChange { get; set; }

    [JsonProperty("market_cap")]
    public double MarketCap { get; set; }

    [JsonProperty("market_cap_change")]
    public double MarketCapChange { get; set; }

    [JsonProperty("volume")]
    public double Volume { get; set; }

    [JsonProperty("volume_change")]
    public double VolumeChange { get; set; }

    [JsonProperty("last_updated")]
    public DateTime LastUpdated { get; set; }
}

public class Categories
{
    [JsonProperty("status")]
    public CategoryStatus Status { get; set; }

    [JsonProperty("data")]
    public List<Datum> Data { get; set; }
}

public class CategoryStatus
{
    [JsonProperty("timestamp")]
    public DateTime Timestamp { get; set; }

    [JsonProperty("error_code")]
    public int ErrorCode { get; set; }

    [JsonProperty("error_message")]
    public object ErrorMessage { get; set; }

    [JsonProperty("elapsed")]
    public int Elapsed { get; set; }

    [JsonProperty("credit_count")]
    public int CreditCount { get; set; }

    [JsonProperty("notice")]
    public object Notice { get; set; }
}