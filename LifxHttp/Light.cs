﻿using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifxHttp
{
    /// <summary>
    /// Model object for a Light
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public sealed class Light
    {
        [JsonObject(MemberSerialization.Fields)]
        internal class CollectionSpec
        {
            public string id;
            public string name;
            public override bool Equals(object obj)
            {
                CollectionSpec spec = obj as CollectionSpec;
                return spec != null && spec.id == id && spec.name == name;
            }

            public override int GetHashCode()
            {
                return (id.GetHashCode() * 77) + name.GetHashCode();
            }
        }

        /// <summary>
        /// Serial number of the light
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; private set; }

        [JsonProperty("uuid")]
        public string UUID { get; private set; }

        [JsonProperty("label")]
        public string Label { get; private set; }

        [JsonProperty("connected")]
        public bool IsConnected { get; private set; }

        [JsonProperty("power")]
        public PowerState PowerState { get; private set; }

        [JsonProperty("color")]
        public LifxColor.HSBK Color { get; private set; }

        [JsonProperty("brightness")]
        public float Brightness { get; private set; }

        [JsonProperty("group")]
        internal CollectionSpec group = new CollectionSpec();
        public string GroupId { get { return group.id; } }
        public string GroupName { get { return group.name; } }

        [JsonProperty("location")]
        internal CollectionSpec location = new CollectionSpec();
        public string LocationId { get { return location.id; } }
        public string LocationName { get { return location.name; } }
        
        [JsonProperty("last_seen")]
        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTime LastSeen { get; private set; }

        [JsonProperty("seconds_since_seen")]
        public float SecondsSinceSeen { get; private set; }

        [JsonProperty("product_name")]
        public string ProductName { get; private set; }

        [JsonProperty]
        private Dictionary<string, bool> capabilties = new Dictionary<string, bool>();

        public IEnumerable<string> Capabilities
        {
            get
            {
                foreach (var entry in capabilties)
                {
                    if (entry.Value)
                    {
                        yield return entry.Key;
                    }
                }
            }
        }

        internal Light() { }

        public bool HasCapability(string capabilitity)
        {
            return capabilties.ContainsKey(capabilitity) && capabilties[capabilitity];
        }

        public override string ToString()
        {
            return Label;
        }

        public static implicit operator Selector(Light light)
        {
            return new Selector.LightId(light.Id);
        }
    }
}
