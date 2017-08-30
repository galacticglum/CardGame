using Newtonsoft.Json;
using UnityEngine;

public abstract class Entity
{
    public const string NamePropertyJson = "name";
    public const string SpriteNamePropertyJson = "sprite_name";
    public const string DescriptionPropertyJson = "description";

    [JsonProperty(NamePropertyJson, Required = Required.Always)]
    public string Name { get; set; }

    [JsonProperty(SpriteNamePropertyJson, Required = Required.Always)]
    public string SpriteName { get; set; }

    [JsonProperty(DescriptionPropertyJson, DefaultValueHandling = DefaultValueHandling.Populate)]
    public string Description { get; set; }

    [JsonIgnore]
    public abstract Sprite Sprite { get; }
}
