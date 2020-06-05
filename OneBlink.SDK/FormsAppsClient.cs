using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using OneBlink.SDK.Model;

namespace OneBlink.SDK
{
    public class FormsAppsClient
    {
        OneBlinkApiClient oneBlinkApiClient;

        public FormsAppsClient(string accessKey, string secretKey, TenantName tenantName = TenantName.ONEBLINK)
        {
            this.oneBlinkApiClient = new OneBlinkApiClient(
                accessKey,
                secretKey,
                tenant: new Tenant(tenantName)
            );
        }

        public async Task<JWTPayload> VerifyJWT(string token)
        {
            try {
                if (token.Contains("Bearer ")) {
                    token = token.Split(' ')[1];
                }
                JwtSecurityToken jwt = new JwtSecurityToken(token);
                JsonWebKey jwk = await Token.GetJsonWebKey(
                    iss: this.oneBlinkApiClient.tenant.jwtIssuer,
                    kid: jwt.Header.Kid
                );
                string verifiedToken = Token.VerifyJWT(token, jwk);
                return JsonConvert.DeserializeObject<JWTPayload>(verifiedToken);
            } catch(Exception err) {
                throw new Exception("The supplied JWT was invalid.");
            }
        }

    }
}