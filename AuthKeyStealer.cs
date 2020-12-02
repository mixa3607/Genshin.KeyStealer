using System;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using Titanium.Web.Proxy;
using Titanium.Web.Proxy.EventArguments;
using Titanium.Web.Proxy.Models;

namespace Genshin.KeyStealer
{
    public class AuthKeyStealer
    {
        private readonly string _genshinHost;
        private readonly ProxyServer _proxyServer;
        public AuthKeyStealer()
        {
            _proxyServer = new ProxyServer();
            _genshinHost = "hk4e-api-os.mihoyo.com";
        }

        public void Start(string host, int port)
        {
            _proxyServer.BeforeRequest += OnBeforeRequest;
            var proxySettings = new ExplicitProxyEndPoint(IPAddress.Parse(host), port);
            _proxyServer.AddEndPoint(proxySettings);
            _proxyServer.Start();
            _proxyServer.SetAsSystemProxy(proxySettings, ProxyProtocolType.AllHttp);
        }

        public void Stop()
        {
            _proxyServer.RestoreOriginalProxySettings();
            _proxyServer.BeforeRequest -= OnBeforeRequest;
            _proxyServer.Stop();
        }

        private Task OnBeforeRequest(object sender, SessionEventArgs e)
        {
            var request = e.HttpClient.Request;
            if (request.Host == _genshinHost)
            {
                var qPaths = request.RequestUri.Query.Split('?', '&');
                foreach (var qPath in qPaths)
                {
                    if (qPath.StartsWith("authkey", StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine(HttpUtility.UrlDecode(qPath.Split('=')[1]));
                        Console.WriteLine("Press any key");
                    }
                }
            }

            return Task.CompletedTask;
        }
    }
}