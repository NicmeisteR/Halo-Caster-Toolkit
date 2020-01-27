using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Halo_Streamer_Tools
{
  public partial class Stats
  {
    [JsonProperty("redteam")]
    public BlueteamClass Redteam { get; set; }

    [JsonProperty("blueteam")]
    public BlueteamClass Blueteam { get; set; }

    [JsonProperty("mvp")]
    public string Mvp { get; set; }
  }

  public partial class BlueteamClass
  {
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("standing")]
    public Standing Standing { get; set; }

    [JsonProperty("team")]
    public TeamElement Team { get; set; }

    [JsonProperty("players")]
    public TeamElement[] Players { get; set; }
  }

  public partial class TeamElement
  {
    [JsonProperty("gamertag", NullValueHandling = NullValueHandling.Ignore)]
    public string Gamertag { get; set; }

    [JsonProperty("kills")]
    public long Kills { get; set; }

    [JsonProperty("deaths")]
    public long Deaths { get; set; }

    [JsonProperty("assists")]
    public long Assists { get; set; }

    [JsonProperty("kd")]
    public double Kd { get; set; }

    [JsonProperty("damage")]
    public double Damage { get; set; }
  }

  public partial class Standing
  {
    [JsonProperty("wins")]
    public long Wins { get; set; }

    [JsonProperty("losses")]
    public long Losses { get; set; }
  }

  public partial class Stats
  {
    public static Stats FromJson(string json) => JsonConvert.DeserializeObject<Stats>(json, Converter.Settings);
  }

  public static class Serialize
  {
    public static string ToJson(this Stats self) => JsonConvert.SerializeObject(self, Converter.Settings);
  }

  internal static class Converter
  {
    public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
    {
      MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
      DateParseHandling = DateParseHandling.None,
      Converters = {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
    };
  }
}
