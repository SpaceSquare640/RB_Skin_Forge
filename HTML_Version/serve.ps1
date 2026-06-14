# RB Skin Forge - minimal static web server for the HTML (Blazor WASM) version.
# Uses a raw TcpListener on localhost so it needs NO admin rights and NO extra installs.
param([int]$Port = 5500)

$ErrorActionPreference = 'Stop'
$root = Join-Path $PSScriptRoot 'app'

if (-not (Test-Path $root)) {
    Write-Host "ERROR: '$root' not found. The published site should live in HTML_Version\app." -ForegroundColor Red
    exit 1
}

$mime = @{
    '.html'='text/html; charset=utf-8'; '.htm'='text/html; charset=utf-8';
    '.js'='text/javascript'; '.mjs'='text/javascript'; '.css'='text/css';
    '.wasm'='application/wasm'; '.json'='application/json';
    '.png'='image/png'; '.jpg'='image/jpeg'; '.jpeg'='image/jpeg'; '.gif'='image/gif';
    '.svg'='image/svg+xml'; '.ico'='image/x-icon';
    '.woff'='font/woff'; '.woff2'='font/woff2'; '.ttf'='font/ttf';
    '.dll'='application/octet-stream'; '.dat'='application/octet-stream';
    '.blat'='application/octet-stream'; '.bin'='application/octet-stream';
    '.map'='application/json'; '.txt'='text/plain; charset=utf-8';
    '.webmanifest'='application/manifest+json'
}

try {
    $listener = [System.Net.Sockets.TcpListener]::new([System.Net.IPAddress]::Loopback, $Port)
    $listener.Start()
} catch {
    Write-Host "ERROR: could not bind to port $Port. Is it already in use? Try: serve.ps1 -Port 5600" -ForegroundColor Red
    exit 1
}

$url = "http://localhost:$Port/"
Write-Host "RB Skin Forge (HTML) is running at $url" -ForegroundColor Green
Write-Host "Close this window to stop the server."
Start-Process $url

while ($true) {
    $client = $listener.AcceptTcpClient()
    try {
        $stream = $client.GetStream()
        $reader = [System.IO.StreamReader]::new($stream)
        $requestLine = $reader.ReadLine()
        if (-not $requestLine) { continue }
        while (($line = $reader.ReadLine()) -ne $null -and $line -ne '') { }

        $target = ($requestLine -split ' ')[1]
        $path = [System.Uri]::UnescapeDataString(($target -split '\?')[0])
        if ($path -eq '/') { $path = '/index.html' }

        $file = Join-Path $root ($path.TrimStart('/') -replace '/', '\')
        if (-not (Test-Path $file -PathType Leaf)) {
            $file = Join-Path $root 'index.html'   # SPA fallback for client-side routes
        }

        $ext = [System.IO.Path]::GetExtension($file).ToLowerInvariant()
        $ct = if ($mime.ContainsKey($ext)) { $mime[$ext] } else { 'application/octet-stream' }
        $bytes = [System.IO.File]::ReadAllBytes($file)

        $header = "HTTP/1.1 200 OK`r`nContent-Type: $ct`r`nContent-Length: $($bytes.Length)`r`nCache-Control: no-cache`r`nConnection: close`r`n`r`n"
        $hb = [System.Text.Encoding]::ASCII.GetBytes($header)
        $stream.Write($hb, 0, $hb.Length)
        $stream.Write($bytes, 0, $bytes.Length)
        $stream.Flush()
    } catch {
    } finally {
        $client.Close()
    }
}
