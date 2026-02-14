using Google.Apis.Auth;

namespace GraduationProject.Services
{
    public class GoogleAuthService
    {
        public async Task<GoogleJsonWebSignature.Payload> VerifyToken(string token)
        {
            return await GoogleJsonWebSignature.ValidateAsync(token);
        }
    }
}
