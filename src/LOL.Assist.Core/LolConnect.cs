using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Net.WebSockets;
using System.Text;

namespace LOL.Assist.Core;

public class LolConnect
{
    private static readonly HttpClient _httpClient = new(new AuthHeaderHandler())
    {
        BaseAddress = new Uri("https://127.0.0.1")
    };

    private static ClientWebSocket _webSocket;
    private static bool _isConnectStatue;
    private static Task? _connectTask;
    private static readonly object ConnectLock = new();
    public static Action ConnectNotificationAction;
    public static Dictionary<string, Action<JToken?, string, string>> _eventAction = new();

    public static void ManageEventAction(bool isAdd, string eventUri, Action<JToken?, string, string> action)
    {
        bool have = _eventAction.ContainsKey(eventUri);
        if (!have)
        {
            if (!isAdd)
                return;
            _eventAction.Add(eventUri, action);
        }
        else
        {
            List<Delegate> invocations = _eventAction[eventUri].GetInvocationList().ToList();
            invocations.Remove(action);
            if (!invocations.Any())
            {
                _eventAction.Remove(eventUri);
            }
            else
            {
                invocations.ForEach(p => action += (Action<JToken?, string, string>)p);
                _eventAction[eventUri] = action;
            }
        }
    }
    public HttpClient GetHttpClient() => _httpClient;
    public LolConnect Connect()
    {
        if (_isConnectStatue || _connectTask != null)
            return this;
        lock (ConnectLock)
        {
            if (_connectTask != null)
                return this;
            _connectTask = Task.Factory.StartNew(() =>
            {
                do
                {
                    try
                    {
                        (string? password, int port) = LolAuthProvider.GetCommandLineText();
                        if (password == null)
                        {
                            Thread.Sleep(4000);
                            continue;
                        }
                        string baseUrl = string.Concat("127.0.0.1:", port);
                        AuthenticationHeaderValue authenticationHeader = new AuthenticationHeaderValue("Basic",
                            Convert.ToBase64String(Encoding.UTF8.GetBytes($"riot:{password}")));
                        AuthHeaderHandler.SetUriToken($"https://{baseUrl}", authenticationHeader);
                        _webSocket = new ClientWebSocket();
                        _webSocket.Options.RemoteCertificateValidationCallback = delegate { return true; };
                        _webSocket.Options.SetRequestHeader("Authorization", authenticationHeader.ToString());
                        _webSocket.ConnectAsync(new Uri($"wss://{baseUrl}/"), CancellationToken.None).Wait();
                        if (_webSocket.State != WebSocketState.Open)
                            throw new Exception("websocket 未打开");
                        _isConnectStatue = true;
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.ToString());
                        Thread.Sleep(6000);
                    }
                } while (!_isConnectStatue);
                ConnectNotificationAction?.Invoke();
                WebSocketConnectMessage();
            });
        }
        return this;
    }

    private LolConnect ReConnect()
    {
        _isConnectStatue = false;
        _connectTask = null;
        return Connect();
    }
    private void WebSocketConnectMessage()
    {
        if (_webSocket.State != WebSocketState.Open)
            return;
        //订阅所有事件
        _webSocket.SendAsync("[5, \"OnJsonApiEvent\"]"u8.ToArray(), WebSocketMessageType.Text, true, CancellationToken.None);
        //最终消息
        List<byte> allBytes = new List<byte>();
        //缓冲区
        ArraySegment<byte> arraySegment = new ArraySegment<byte>(new byte[1024 * 4]);
        WebSocketReceiveResult webSocketReceiveResult = _webSocket.ReceiveAsync(arraySegment, CancellationToken.None).Result;
        while (!webSocketReceiveResult.CloseStatus.HasValue)
        {
            try
            {
                allBytes.AddRange(arraySegment.Take(webSocketReceiveResult.Count));
                if (!webSocketReceiveResult.EndOfMessage)
                    continue;
                string message = Encoding.UTF8.GetString(allBytes.ToArray());
                if (!string.IsNullOrWhiteSpace(message))
                    OnMessage(message.Replace("\0", string.Empty));
                allBytes = new List<byte>();
            }
            finally
            {
                webSocketReceiveResult = _webSocket.ReceiveAsync(arraySegment, CancellationToken.None).Result;
            }
        }
        //重新连接
        ReConnect();
    }

    private void OnMessage(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
            return;
        JToken? data = JsonConvert.DeserializeObject<JToken>(message)?[2];
        if (data == null) return;
        string? uri = data["uri"]?.ToString();
        if (string.IsNullOrWhiteSpace(uri) ||
            uri.Contains("/data-store") ||
            uri.Contains("/riotclient") ||
            uri.Contains("/riot") ||
            uri.Contains("/lol-game-client-chat") ||
            uri.Contains("/lol-store") ||
            uri.Contains("/lol-clash") ||
            uri.Contains("/lol-login") ||
            uri.Contains("/lol-loadouts") ||
            uri.Contains("/lol-platform") ||
            uri.Contains("/lol-settings") ||
            uri.Contains("/lol-hovercard/") ||
            uri.Contains("/lol-chat/v1") && !uri.Contains("conversations") ||
            uri.Contains("/lol-client-config") ||
            uri.Contains("/lol-inventory") ||
            uri.Contains("/lol-challenges") ||
            uri.Contains("/lol-league-session") ||
            uri.Contains("/gcloud-voice-chat") ||
            uri.Contains("/lol-content-targeting"))
            return;
        Debug.WriteLine($"{uri}\r\n{message}");
        string? key = _eventAction.Keys.FirstOrDefault(p => p.Contains(uri) || uri.Contains(p));
        if (key == null)
            return;
        _eventAction[key].Invoke(data["data"], uri, data["eventType"]!.ToString());
    }
}
