namespace backend.Model.DTO
{
    // Generelles DTO für alle Tokenbasierten Anfragen mit einem Token und bis zu drei Werten
    public class TokenDTO
    {
        public required string Token { get; set; }
        public required string Value1 { get; set; }
        public string? Value2 { get; set; }
        public string? Value3 { get; set; }
    }
}
