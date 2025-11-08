
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using caching_proxy.extras;

namespace caching_proxy.core;
public class ProxyServer
{
    private int _port;
    private string _origin;
    private TimeSpan _ttl;
    private ICacheManager _cacheManager;
    private HttpListener _httpListener;
    private CancellationTokenSource _cts;
    private HttpClient _httpClient;
    private LogC _logc;

    public ProxyServer(CacheProxyConfiguration configuration, ICacheManager cacheManager)
    {
        _logc = new LogC("ProxyServer.cs");
        _port = configuration.Port;
        _origin = configuration.Origin;
        _ttl = configuration.TTL;
        _cacheManager = cacheManager;
        _httpClient = new HttpClient();

        try
        {
            _cacheManager.Clear(); // Limpiar caché al iniciar el servidor
        }
        catch (Exception ex)
        {
            _logc.Error($"Error clearing cache on startup: {ex.Message}");
        }

    }

    public void Start()
    {
        try
        {
            _cts = new CancellationTokenSource();
            _httpListener = new HttpListener();
            _httpListener.Prefixes.Add($"http://127.0.0.1:{_port}/");
            _httpListener.Start();

            _logc.Info($"Proxy server started on: http://127.0.0.1:{_port}/, forwarding to {_origin} with TTL {_ttl}.");

            Task.Run(() => ListenAsync(_cts.Token));

        }
        catch (Exception ex)
        {
            _logc.Error($"Exception: {ex.Message}");
        }
    }

    public void Stop()
    {
        try
        {
            _logc.Info("Stopping proxy server...");
            _cts?.Cancel();
            _httpListener?.Stop();
            _httpClient.Dispose();
            _logc.Info("Proxy server stopped successfully.");
        }
        catch (Exception ex)
        {
            _logc.Error($"Error stopping server: {ex.Message}");
        }
    }

    private async Task ListenAsync(CancellationToken token)
    {
        if (_httpListener == null)
        {
            _logc.Error("HttpListener is not initialized.");
            return;
        }

        while (!token.IsCancellationRequested)
        {
            try
            {
                var context = await _httpListener.GetContextAsync();
                _ = Task.Run(() => HandleRequestAsync(context), token);
            }
            catch (HttpListenerException) when (token.IsCancellationRequested)
            {
                break; // Stop() fue llamado
            }
            catch (Exception ex)
            {
                _logc.Error("Listener error: " + ex.Message);
            }
        }
    }
    
    private async Task HandleRequestAsync(HttpListenerContext context)
    {
        try
        {
            // Obtenemos la ruta y query de la solicitud
            // Ejemplo de path: /api/v1/products/1001?includeDetails=true
            string path = context.Request.Url!.PathAndQuery;

            //Ejemplo de key: GET:/api/v1/products/1001
            string cacheKey = $"{context.Request.HttpMethod}:{path}";

            // _logc.Info($"Incoming request → {cacheKey}");

            // Verificar si la respuesta está en caché
            if (_cacheManager.TryGetValue(cacheKey, out byte[] cachedData))
            {
                _logc.Info($"Cache HIT → {cacheKey}");
                context.Response.Headers.Add("X-Cache", "HIT");
                context.Response.ContentLength64 = cachedData.Length;

                await using var output = context.Response.OutputStream;
                await output.WriteAsync(cachedData, 0, cachedData.Length);
                await output.FlushAsync();

                _logc.Ok($"Response served from cache → {cacheKey}");
                return;
            }

            // ! Esta etapa se ejecuta si no está en caché

            _logc.Ok($"Cache MISS → {cacheKey}");

            // No está en caché, obtener del origen
            var originUrl = _origin.TrimEnd('/') + path;

            // Realizar la solicitud al servidor de origen
            using var originResponse = await _httpClient.GetAsync(originUrl, HttpCompletionOption.ResponseHeadersRead);

            // Leer el contenido de la respuesta
            byte[] body = await originResponse.Content.ReadAsByteArrayAsync();

            // Guardamos la respuesta en cache antes de enviarla al cliente
            try
            {
                _cacheManager.Set(cacheKey, body, _ttl);
            }
            catch (Exception ex)
            {
                _logc.Error($"Error caching response for key: {cacheKey}. Error: {ex.Message}");
            }

            // Copiar headers del servidor origen
            foreach (var header in originResponse.Headers)
                context.Response.Headers[header.Key] = string.Join(", ", header.Value);

            foreach (var header in originResponse.Content.Headers)
                context.Response.Headers[header.Key] = string.Join(", ", header.Value);

            // Aplicar ContentType explícitamente
            if (originResponse.Content.Headers.ContentType != null)
                context.Response.ContentType = originResponse.Content.Headers.ContentType.ToString();

            /// Asegurar cabecera X-Cache
            context.Response.Headers["X-Cache"] = "MISS";

            // Solo asignar ContentLength64 si no hay Transfer-Encoding
            if (!originResponse.Headers.Contains("Transfer-Encoding"))
            {
                context.Response.ContentLength64 = body.Length;
            }

            // Escribir respuesta
            await using var stream = context.Response.OutputStream;
            await stream.WriteAsync(body, 0, body.Length);
            await stream.FlushAsync();

            _logc.Ok($"Response from origin and cached → {cacheKey}");
        }
        catch (Exception ex)
        {
            _logc.Error("Request handling error: " + ex.Message);
        }
    }
    
}