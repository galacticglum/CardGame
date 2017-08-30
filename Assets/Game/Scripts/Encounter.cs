using System.Collections.Generic;
using Newtonsoft.Json;

public class Encounter
{
    public const string EnemiesJson = "enemies";

    [JsonProperty(EnemiesJson, Required = Required.Always)]
    public List<Enemy> Enemies { get; set; }
}