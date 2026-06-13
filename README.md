<!--
  RB Skin Forge — README
  Languages: English · 繁體中文 · 简体中文
  Default language: English
-->

# RB Skin Forge

> **Creators: SpaceSquare & Claude Code | Owner: SpaceSquare**

**▶ Live web demo: https://spacesquare640.github.io/RB_Skin_Forge/**

**Languages:** [English](#english) · [繁體中文](#繁體中文) · [简体中文](#简体中文)

---

## English

**RB Skin Forge** converts user-uploaded images into standard **Roblox clothing templates**, and (in later phases) processes 3D models into Roblox-ready assets with auto-rigging and quality checks.

It ships in three forms from a single shared C# core:

| Version | Tech | Output |
|---------|------|--------|
| HTML    | Blazor WebAssembly | Runs in any modern browser |
| Windows | .NET MAUI | `RB_Skin_Forge.exe` |
| Android | .NET MAUI | `RB_Skin_Forge.apk` |

### Features (Phase 1 — available now)
- **Asset Ingestion** — accepts PNG/JPG and validates the input.
- **Smart Template Generator** — fits your image onto the **585 × 559** Roblox clothing template and **fills the edges** so there are no transparent gaps.
- **Spec Validation** — checks output against Roblox limits (texture ≤ 1024 px, etc.).
- **Console & Stats** — live processing log plus output dimensions and texture size, with a downloadable log.
- **Light / Dark theme** — follows your system preference.

### Roadmap
- **Phase 2** — OBJ 3D mesh support (optimization, scale, attachment nodes).
- **Phase 3** — FBX support + **Auto-Rigging** (head/body/accessory detection, Roblox Attachment implantation) and full quality metrics (triangle count, material size).

### Build from source
```bash
# Requires .NET 9 SDK. For the Windows/Android app you also need: dotnet workload install maui
cd Source_Code

# HTML version (Blazor)
dotnet run --project RB_Skin_Forge.Web

# Windows version
dotnet build RB_Skin_Forge.Maui -f net9.0-windows10.0.19041.0
```

### Repository layout
```
Source_Code/            Shared core + the three front-ends
  RB_Skin_Forge.Core/   Shared C# logic (pipeline, Roblox specs)
  RB_Skin_Forge.Web/    Blazor WebAssembly (HTML)
  RB_Skin_Forge.Maui/   .exe (Windows) + .apk (Android)
```

### License
RB Skin Forge is released under its own license — see [LICENSE](LICENSE). In short:
free to use and share the original release; you may modify and redistribute **only**
if you keep the LICENSE intact and give conspicuous, verbatim credit:
**"Creators: SpaceSquare & Claude Code | Owner: SpaceSquare"**. Ownership is retained
by SpaceSquare. This is an unofficial tool and is not affiliated with or endorsed by
Roblox Corporation.

Third-party: image processing by [SixLabors.ImageSharp](https://github.com/SixLabors/ImageSharp) 3.1.x (Six Labors Split License).

---

## 繁體中文

**RB Skin Forge** 能將使用者上傳的圖片轉換為標準的 **Roblox 服裝範本**，並在後續階段把 3D 模型處理成可直接用於 Roblox 的素材，包含自動綁定骨架與品質檢查。

本程式由單一共用的 C# 核心衍生出三種版本：

| 版本 | 技術 | 產出 |
|------|------|------|
| HTML   | Blazor WebAssembly | 可在任何現代瀏覽器執行 |
| Windows | .NET MAUI | `RB_Skin_Forge.exe` |
| Android | .NET MAUI | `RB_Skin_Forge.apk` |

### 功能（第一階段 — 現已提供）
- **素材匯入** — 接受 PNG/JPG 並驗證輸入內容。
- **智慧範本產生器** — 將圖片貼合至 **585 × 559** 的 Roblox 服裝範本，並**自動填補邊緣**，避免出現透明縫隙。
- **規格驗證** — 依 Roblox 限制檢查輸出（材質 ≤ 1024 像素等）。
- **主控台與統計** — 即時處理紀錄，顯示輸出尺寸與材質大小，並可下載紀錄。
- **淺色 / 深色佈景** — 自動跟隨系統設定。

### 開發藍圖
- **第二階段** — 支援 OBJ 3D 網格（最佳化、縮放、附著節點）。
- **第三階段** — 支援 FBX 並提供**自動綁定骨架**（偵測頭部/身體/配件、植入 Roblox Attachment）與完整品質指標（三角形數量、材質大小）。

### 從原始碼建置
```bash
# 需要 .NET 9 SDK。若要建置 Windows/Android 版本，另需：dotnet workload install maui
cd Source_Code

# HTML 版本（Blazor）
dotnet run --project RB_Skin_Forge.Web

# Windows 版本
dotnet build RB_Skin_Forge.Maui -f net9.0-windows10.0.19041.0
```

### 授權
RB Skin Forge 採用專屬授權——請參閱 [LICENSE](LICENSE)。重點：可免費使用並分享原始發行版；
若要修改與再散布，**必須**完整保留 LICENSE 並以顯著、逐字方式標示來源：
**「Creators: SpaceSquare & Claude Code | Owner: SpaceSquare」**。所有權由 SpaceSquare 保留。
本程式為非官方工具，與 Roblox Corporation 無任何隸屬或背書關係。

第三方：影像處理採用 [SixLabors.ImageSharp](https://github.com/SixLabors/ImageSharp) 3.1.x（Six Labors Split License）。

---

## 简体中文

**RB Skin Forge** 能将用户上传的图片转换为标准的 **Roblox 服装模板**，并在后续阶段把 3D 模型处理成可直接用于 Roblox 的素材，包含自动绑定骨架与质量检查。

本程序由单一共享的 C# 核心衍生出三种版本：

| 版本 | 技术 | 产物 |
|------|------|------|
| HTML   | Blazor WebAssembly | 可在任何现代浏览器运行 |
| Windows | .NET MAUI | `RB_Skin_Forge.exe` |
| Android | .NET MAUI | `RB_Skin_Forge.apk` |

### 功能（第一阶段 — 现已提供）
- **素材导入** — 接受 PNG/JPG 并验证输入内容。
- **智能模板生成器** — 将图片贴合至 **585 × 559** 的 Roblox 服装模板，并**自动填补边缘**，避免出现透明缝隙。
- **规格验证** — 依 Roblox 限制检查输出（贴图 ≤ 1024 像素等）。
- **控制台与统计** — 实时处理日志，显示输出尺寸与贴图大小，并可下载日志。
- **浅色 / 深色主题** — 自动跟随系统设置。

### 开发路线
- **第二阶段** — 支持 OBJ 3D 网格（优化、缩放、附着节点）。
- **第三阶段** — 支持 FBX 并提供**自动绑定骨架**（检测头部/身体/配件、植入 Roblox Attachment）与完整质量指标（三角形数量、材质大小）。

### 从源码构建
```bash
# 需要 .NET 9 SDK。若要构建 Windows/Android 版本，另需：dotnet workload install maui
cd Source_Code

# HTML 版本（Blazor）
dotnet run --project RB_Skin_Forge.Web

# Windows 版本
dotnet build RB_Skin_Forge.Maui -f net9.0-windows10.0.19041.0
```

### 许可证
RB Skin Forge 采用专属许可证——请参阅 [LICENSE](LICENSE)。要点：可免费使用并分享原始发行版；
若要修改与再分发，**必须**完整保留 LICENSE 并以显著、逐字方式标注来源：
**“Creators: SpaceSquare & Claude Code | Owner: SpaceSquare”**。所有权由 SpaceSquare 保留。
本程序为非官方工具，与 Roblox Corporation 无任何隶属或背书关系。

第三方：图像处理采用 [SixLabors.ImageSharp](https://github.com/SixLabors/ImageSharp) 3.1.x（Six Labors Split License）。
