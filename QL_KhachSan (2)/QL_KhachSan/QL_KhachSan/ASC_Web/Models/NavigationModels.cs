using System.Text.Json.Serialization;

namespace ASC_Web.Models
{
    public class NavigationMenuItem
    {
        [JsonPropertyName("DisplayName")] // Dùng cho System.Text.Json
        public string DisplayName { get; set; }

        [JsonPropertyName("MaterialIcon")]
        public string MaterialIcon { get; set; }

        [JsonPropertyName("Link")]
        public string Link { get; set; }

        [JsonPropertyName("IsNested")]
        public bool IsNested { get; set; }

        [JsonPropertyName("Sequence")]
        public int Sequence { get; set; }

        [JsonPropertyName("UserRoles")]
        public List<string> UserRoles { get; set; } = new List<string>();

        [JsonPropertyName("NestedItems")]
        public List<NavigationMenuItem> NestedItems { get; set; } = new List<NavigationMenuItem>();
    }

    public class NavigationMenu
    {
        [JsonPropertyName("MenuItems")]
        public List<NavigationMenuItem> MenuItems { get; set; } = new List<NavigationMenuItem>();
    }
}