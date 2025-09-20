namespace AccountService.Service.DTO.Auth
{
    public class OtpVerifyRequest
    {
        public string? Email { get; set; }
        public string? OtpCode { get; set; }
    }
}