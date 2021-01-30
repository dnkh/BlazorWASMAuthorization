using System;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace dnkh.AuthZ.WASM
{
    public class AuthZUserAccount : RemoteUserAccount
    {
        [JsonPropertyName("groups")]
        public string[] Groups { get; set; } = new string[] { };

        [JsonPropertyName("roles")]
        public string[] Roles { get; set; } = new string[] { };
    }
}
