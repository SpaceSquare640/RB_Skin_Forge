<!--
  RB Skin Forge — README
  Languages: English · 繁體中文 · 简体中文
  Default language: English
-->

# RB Skin Forge

> **Creators: SpaceSquare & Claude Code | Owner: SpaceSquare**

**▶ Live web demo: https://spacesquare640.github.io/RB_Skin_Forge/**
**· Latest release: [v0.2.0](https://github.com/SpaceSquare640/RB_Skin_Forge/releases/latest)**

**Languages:** [English](#english) · [繁體中文](#繁體中文) · [简体中文](#简体中文)

---

## English

**RB Skin Forge** turns the assets you already have into Roblox-ready content:
upload an **image** and it produces a standard **Roblox clothing template**; upload an
**OBJ or ASCII FBX mesh** and it cleans, scales, **auto-rigs**, validates and (if
needed) **decimates** the model into a Roblox-ready mesh — all from one shared C# core.

It runs everywhere from a single codebase:

| Version | Tech | Output |
|---------|------|--------|
| HTML    | Blazor WebAssembly | Runs in any modern browser (also hosted as the live demo) |
| Windows | .NET MAUI | `RB_Skin_Forge.exe` (+ installer) |
| Android | .NET MAUI | `RB_Skin_Forge.apk` |

The image/mesh processing runs entirely on your device (in the browser via WebAssembly
for the HTML version) — nothing is uploaded to a server.

### Features
- **Asset ingestion** — accepts PNG/JPG images and **OBJ / ASCII FBX** meshes, and validates the input.
- **Smart template generator** (2D) — fits your image onto the **585 × 559** Roblox clothing template and **fills the edges** so there are no transparent gaps.
- **Geometry engine** (3D) — removes degenerate faces, centers and scales the mesh to Roblox's stud bounds, generates missing normals, and exports a cleaned OBJ.
- **Mesh decimation** (3D) — meshes over the **10,000-triangle** limit are automatically reduced (vertex clustering) instead of just being flagged.
- **Auto-rigging** (3D) — implants Roblox **Attachment** nodes (head / neck / shoulders / root / waist / hips) from the mesh bounds, embeds them in the exported OBJ, and marks them on the preview.
- **Spec validation** — checks output against Roblox limits (texture ≤ 1024 px, ≤ 10,000 triangles, ≤ 2,048 studs).
- **Batch processing** — drop several files at once, process them in one run, and review each result individually.
- **Console & Stats** — a live log plus dimensions, triangles, vertices, materials, attachment count, bounds and file size, all downloadable.
- **10-language interface** — English, 繁體中文, 简体中文, 日本語, 한국어, Español, Português, Français, Deutsch, Русский — switchable in-app and remembered between visits.
- **Light / Dark theme** — automatic (follows your system) or a manual **Auto / Light / Dark** toggle.

### Roadmap
- **Binary FBX** import (needs a non-WASM strategy) and richer material/texture handling.
- Quadric-error decimation for higher-fidelity reduction, and skinned-rig export.

### Build from source
```bash
# Requires the .NET 9 SDK. For the Windows/Android apps you also need: dotnet workload install maui
cd Source_Code

# HTML version (Blazor)
dotnet run --project RB_Skin_Forge.Web

# Windows version
dotnet build RB_Skin_Forge.Maui -f net9.0-windows10.0.19041.0
```

### Repository layout
```
Source_Code/                      Shared core + the three front-ends (branch: main)
  RB_Skin_Forge.Core/             Shared C# logic — pipeline, parsers, engines, localization
  RB_Skin_Forge.Web/              Blazor WebAssembly (HTML)
  RB_Skin_Forge.Maui/             .exe (Windows) + .apk (Android)
Installer/                        Inno Setup script for the Windows installer
assets/                           Repository art (social preview, etc.)
```
Built downloads live on the **Releases** page and on the artifact branches
`Package_Program_EXE`, `Package_Program_APK` and `HTML_Version`.

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

**RB Skin Forge** 能把你手邊既有的素材轉換成可直接用於 Roblox 的內容：
上傳**圖片**即可產生標準的 **Roblox 服裝範本**；上傳 **OBJ 或 ASCII FBX 網格**，
則會清理、縮放、**自動綁定骨架**、驗證，並在需要時**精簡（減面）**，把模型整理成
可直接用於 Roblox 的網格——全部由單一共用的 C# 核心完成。

本程式由單一程式碼庫衍生出三種版本：

| 版本 | 技術 | 產出 |
|------|------|------|
| HTML   | Blazor WebAssembly | 可在任何現代瀏覽器執行（亦提供線上 Demo） |
| Windows | .NET MAUI | `RB_Skin_Forge.exe`（含安裝程式） |
| Android | .NET MAUI | `RB_Skin_Forge.apk` |

圖片／網格的處理全部在你的裝置上進行（HTML 版透過 WebAssembly 在瀏覽器中執行）——
不會上傳到任何伺服器。

### 功能
- **素材匯入** — 接受 PNG/JPG 影像與 **OBJ／ASCII FBX** 網格，並驗證輸入內容。
- **智慧範本產生器**（2D）— 將圖片貼合至 **585 × 559** 的 Roblox 服裝範本，並**自動填補邊緣**，避免透明縫隙。
- **幾何引擎**（3D）— 移除退化面、置中並縮放至 Roblox 的 stud 範圍、補上缺少的法線，並匯出整理後的 OBJ。
- **網格精簡**（3D）— 超過 **10,000 三角形**上限的網格會自動精簡（頂點叢集），而非僅被標示。
- **自動綁定骨架**（3D）— 依網格邊界植入 Roblox **Attachment** 節點（頭／頸／肩／根部／腰／髖），嵌入匯出的 OBJ，並在預覽上標示。
- **規格驗證** — 依 Roblox 限制檢查輸出（材質 ≤ 1024 像素、≤ 10,000 三角形、≤ 2,048 studs）。
- **批次處理** — 一次拖入多個檔案，於單次執行中處理，並逐檔檢視結果。
- **主控台與統計** — 即時紀錄，並顯示尺寸、三角形、頂點、材質、附著點數量、邊界與檔案大小，皆可下載。
- **10 種語言介面** — English、繁體中文、简体中文、日本語、한국어、Español、Português、Français、Deutsch、Русский——可於程式內切換，並會記住設定。
- **淺色 / 深色佈景** — 自動跟隨系統，或手動切換（自動／淺色／深色）。

### 開發藍圖
- **二進位 FBX** 匯入（需採用非 WASM 的另一種策略）與更完整的材質／貼圖處理。
- 二次誤差（quadric-error）精簡以獲得更高保真度，以及具蒙皮骨架的匯出。

### 從原始碼建置
```bash
# 需要 .NET 9 SDK。若要建置 Windows/Android 版本，另需：dotnet workload install maui
cd Source_Code

# HTML 版本（Blazor）
dotnet run --project RB_Skin_Forge.Web

# Windows 版本
dotnet build RB_Skin_Forge.Maui -f net9.0-windows10.0.19041.0
```

### 儲存庫結構
```
Source_Code/                      共用核心 + 三種前端（分支：main）
  RB_Skin_Forge.Core/             共用 C# 邏輯——流程、解析器、引擎、在地化
  RB_Skin_Forge.Web/              Blazor WebAssembly（HTML）
  RB_Skin_Forge.Maui/             .exe（Windows）+ .apk（Android）
Installer/                        Windows 安裝程式的 Inno Setup 腳本
assets/                           儲存庫美術素材（社群預覽圖等）
```
打包好的下載檔位於 **Releases** 頁面，以及成品分支
`Package_Program_EXE`、`Package_Program_APK` 與 `HTML_Version`。

### 授權
RB Skin Forge 採用專屬授權——請參閱 [LICENSE](LICENSE)。重點：可免費使用並分享原始發行版；
若要修改與再散布，**必須**完整保留 LICENSE 並以顯著、逐字方式標示來源：
**「Creators: SpaceSquare & Claude Code | Owner: SpaceSquare」**。所有權由 SpaceSquare 保留。
本程式為非官方工具，與 Roblox Corporation 無任何隸屬或背書關係。

第三方：影像處理採用 [SixLabors.ImageSharp](https://github.com/SixLabors/ImageSharp) 3.1.x（Six Labors Split License）。

---

## 简体中文

**RB Skin Forge** 能把你手边现有的素材转换成可直接用于 Roblox 的内容：
上传**图片**即可生成标准的 **Roblox 服装模板**；上传 **OBJ 或 ASCII FBX 网格**，
则会清理、缩放、**自动绑定骨架**、验证，并在需要时**精简（减面）**，把模型整理成
可直接用于 Roblox 的网格——全部由单一共享的 C# 核心完成。

本程序由单一代码库衍生出三种版本：

| 版本 | 技术 | 产物 |
|------|------|------|
| HTML   | Blazor WebAssembly | 可在任何现代浏览器运行（亦提供在线 Demo） |
| Windows | .NET MAUI | `RB_Skin_Forge.exe`（含安装程序） |
| Android | .NET MAUI | `RB_Skin_Forge.apk` |

图片／网格的处理全部在你的设备上进行（HTML 版通过 WebAssembly 在浏览器中运行）——
不会上传到任何服务器。

### 功能
- **素材导入** — 接受 PNG/JPG 图像与 **OBJ／ASCII FBX** 网格，并验证输入内容。
- **智能模板生成器**（2D）— 将图片贴合至 **585 × 559** 的 Roblox 服装模板，并**自动填补边缘**，避免透明缝隙。
- **几何引擎**（3D）— 移除退化面、居中并缩放至 Roblox 的 stud 范围、补上缺少的法线，并导出整理后的 OBJ。
- **网格精简**（3D）— 超过 **10,000 三角形**上限的网格会自动精简（顶点聚类），而非仅被标注。
- **自动绑定骨架**（3D）— 依网格边界植入 Roblox **Attachment** 节点（头／颈／肩／根部／腰／髋），嵌入导出的 OBJ，并在预览上标示。
- **规格验证** — 依 Roblox 限制检查输出（贴图 ≤ 1024 像素、≤ 10,000 三角形、≤ 2,048 studs）。
- **批量处理** — 一次拖入多个文件，于单次运行中处理，并逐文件查看结果。
- **控制台与统计** — 实时日志，并显示尺寸、三角形、顶点、材质、附着点数量、边界与文件大小，均可下载。
- **10 种语言界面** — English、繁體中文、简体中文、日本語、한국어、Español、Português、Français、Deutsch、Русский——可在程序内切换，并会记住设置。
- **浅色 / 深色主题** — 自动跟随系统，或手动切换（自动／浅色／深色）。

### 开发路线
- **二进制 FBX** 导入（需采用非 WASM 的另一种策略）与更完整的材质／贴图处理。
- 二次误差（quadric-error）精简以获得更高保真度，以及带蒙皮骨架的导出。

### 从源码构建
```bash
# 需要 .NET 9 SDK。若要构建 Windows/Android 版本，另需：dotnet workload install maui
cd Source_Code

# HTML 版本（Blazor）
dotnet run --project RB_Skin_Forge.Web

# Windows 版本
dotnet build RB_Skin_Forge.Maui -f net9.0-windows10.0.19041.0
```

### 仓库结构
```
Source_Code/                      共享核心 + 三种前端（分支：main）
  RB_Skin_Forge.Core/             共享 C# 逻辑——流程、解析器、引擎、本地化
  RB_Skin_Forge.Web/              Blazor WebAssembly（HTML）
  RB_Skin_Forge.Maui/             .exe（Windows）+ .apk（Android）
Installer/                        Windows 安装程序的 Inno Setup 脚本
assets/                           仓库美术素材（社交预览图等）
```
打包好的下载位于 **Releases** 页面，以及成品分支
`Package_Program_EXE`、`Package_Program_APK` 与 `HTML_Version`。

### 许可证
RB Skin Forge 采用专属许可证——请参阅 [LICENSE](LICENSE)。要点：可免费使用并分享原始发行版；
若要修改与再分发，**必须**完整保留 LICENSE 并以显著、逐字方式标注来源：
**“Creators: SpaceSquare & Claude Code | Owner: SpaceSquare”**。所有权由 SpaceSquare 保留。
本程序为非官方工具，与 Roblox Corporation 无任何隶属或背书关系。

第三方：图像处理采用 [SixLabors.ImageSharp](https://github.com/SixLabors/ImageSharp) 3.1.x（Six Labors Split License）。
