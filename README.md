# ⚡ Caching Proxy Server

```
    (            )     *             (              )
    )\ )      ( /(   (  `     (      )\ )   (    ( /(
   (()/(  (   )\())  )\))(    )\    (()/( ( )\   )\())
    /(_)) )\ ((_)\  ((_)()\((((_)(   /(_)))((_) ((_)\  
   (_))  ((_)  ((_) (_()((_))\ _ )\ (_)) ((_)_   _((_)
   | |   | __|/ _ \ |  \/  |(_)_\(_)| _ \ / _ \ |_  /
   | |__ | _|| (_) || |\/| | / _ \  |   /| (_) | / /
   |____||___|\___/ |_|  |_|/_/ \_\ |_|_\ \__\_\/___|
```

---

### 🚀 Descripción

**Caching Proxy Server** es una aplicación de consola en **C# (.NET 8)** que actúa como un proxy HTTP con almacenamiento en caché.  
Intercepta solicitudes, las reenvía al servidor de origen y guarda las respuestas en memoria durante un tiempo configurable (TTL).

Ideal para practicar desarrollo de red, manejo de caché y patrones asincrónicos en .NET.

---

### ⚙️ Características

- Caché en memoria con `MemoryCache`.
- TTL configurable (segundos, minutos, horas o días).
- Reenvío de solicitudes HTTP al origen.
- Cabecera personalizada:
  - `X-Cache: HIT` → respuesta servida desde la caché.
  - `X-Cache: MISS` → respuesta obtenida del servidor de origen.
- CLI desarrollada con **Spectre.Console.Cli**.
- Logging estructurado con colores y niveles de severidad.

---

### 🛠️ Requisitos

- **.NET SDK 8.0 o superior**  
  👉 [Descargar .NET](https://dotnet.microsoft.com/download)

---

### 🧑‍💻 Instalación y ejecución

Compilar el proyecto:

```bash
dotnet build
```

Ejecutar el proxy:

```bash
./bin/Debug/net8.0/caching-proxy.exe start --port 3000 --origin "https://github.com" --ttl "5m"
```

---

### 🧭 Uso

```
DESCRIPCIÓN:
    Inicia el servidor proxy con caché

USO:
    caching-proxy start [OPCIONES]

OPCIONES:
    -h, --help            Muestra la ayuda
    -p, --port <PUERTO>   Puerto del proxy (por defecto: 8080)
    -o, --origin <URL>    Servidor de origen
    -t, --ttl <TIEMPO>    Tiempo de vida en caché (5s, 5m, 5h, 5d)
```

Ejemplo:

```bash
./caching-proxy.exe start --port 8080 --origin "https://roadmap.sh" --ttl "5m"
```

---

### 🧩 Estructura del proyecto

```
caching-proxy/
 ├── cli/
 │   ├── Commands/
 │   │   └── ProxyCommand.cs
 │   └── Settings/
 │       └── ProxyCommandSettings.cs
 ├── core/
 │   ├── CacheManager.cs
 │   ├── CacheProxyConfiguration.cs
 │   ├── CacheResponseType.cs
 │   ├── ICacheManager.cs
 │   ├── ProxyServer.cs
 │   └── TTLParser.cs
 ├── extras/
 │   ├── Banner.cs
 │   ├── ILogC.cs
 │   └── LogC.cs
 ├── static/
 │   └── banner.txt
 ├── Program.cs
 ├── .gitignore
 ├── caching-proxy.csproj
 └── caching-proxy.sln
```

---

### 🧪 Ejemplo de respuesta

```
HTTP/1.1 200 OK
Content-Type: text/html; charset=utf-8
X-Cache: HIT
Server: CachingProxy/1.0
```

---

### 👤 Autor

**Leonel Marquez (::crack::night::)**  
Especialista en Cloud y Ciberseguridad | Desarrollador Backend .Net  
📧 leomarqz.main@gmail.com  
🐙 [github.com/leomarqz](https://github.com/leomarqz)

---

> *Caching Proxy Server — ligero, educativo y práctico.*
