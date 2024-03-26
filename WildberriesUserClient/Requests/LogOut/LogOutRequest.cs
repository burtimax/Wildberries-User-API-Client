using WildberriesUserClient.Wildberries;

namespace WildberriesUserClient.Requests.LogOut;

/// <summary>
/// Запрос выхода из аккаунта.
/// </summary>
public class LogOutRequest : RequestBase<LogOutResponseModel>
{
    public LogOutRequest(WildberriesClientOptions options) : base(options, options.AuthServiceHost, "/v2/auth/logoff")
    {
    }

    public override HttpRequestMessage GetHttpRequest(SessionData sessionData)
    {
        HttpRequestMessage httpRequest = base.GetHttpRequest(sessionData);
        httpRequest.Headers.Add("Deviceid", sessionData.DeviceId);
        httpRequest.Headers.Add("Host", ClientOptions.AuthServiceHost);
        return httpRequest;
    }
}