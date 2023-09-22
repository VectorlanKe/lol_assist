using System.Diagnostics;
using System.Net.Http;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;

namespace LOL.Assist.Core;

/// <summary>
/// lol认证信息获取
/// </summary>
public static class LolAuthProvider
{
    public static (string?, int) GetCommandLineText()
    {
        var processes = Process.GetProcessesByName("LeagueClientUx");
        if (processes.Length == 0)
            return (null, 0);
        Process process = new Process();
        process.StartInfo = new ProcessStartInfo("wmic", "process where name='LeagueClientUx.exe' GET commandline")
        {
            RedirectStandardOutput = true,
            CreateNoWindow = true,
        };
        process.Start();
        string result = process.StandardOutput.ReadToEnd();
        process.WaitForExit();
        if (process.ExitCode != 0)
            return (null, 0);
        string[] args = result.Split(" ");
        string? token = args.FirstOrDefault(p => p.Contains("--remoting-auth-token"))?.Split("=").Last().TrimEnd('"');
        int port = int.Parse(args.FirstOrDefault(p => p.Contains("--app-port"))?.Split("=").Last().TrimEnd('"') ?? string.Empty);
        return (token, port);
    }
}

/// <summary>
/// lol连接认证信息
/// </summary>
public class AuthHeaderHandler : HttpClientHandler
{

    private static Uri _clientUri = new("https://loclahost");
    private static AuthenticationHeaderValue _authenticationHeader;
    public AuthHeaderHandler()
    {

        ServerCertificateCustomValidationCallback = delegate { return true; };
    }

    public static void SetUriToken(string url, AuthenticationHeaderValue authenticationHeader)
    {
        _clientUri = new Uri(url);
        _authenticationHeader = authenticationHeader;
    }
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        request.RequestUri = new Uri(_clientUri, request.RequestUri?.PathAndQuery);
        request.Headers.Authorization = _authenticationHeader;
        string content = request.Content != null ? await request.Content.ReadAsStringAsync(cancellationToken) : string.Empty;
        request.Content = new StringContent(content, Encoding.UTF8, "application/json");
        HttpResponseMessage result = await base.SendAsync(request, cancellationToken);
        return result;
    }
}