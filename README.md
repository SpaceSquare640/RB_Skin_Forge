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

### Features (available now)
- **Asset Ingestion** — accepts PNG/JPG images and **OBJ / ASCII FBX** meshes, and validates the input.
- **Smart Template Generator** (2D) — fits your image onto the **585 × 559** Roblox clothing template and **fills the edges** so there are no transparent gaps.
- **Geometry Engine** (3D) — cleans the mesh, centers and scales it to Roblox bounds, generates missing normals, and exports a cleaned OBJ. Meshes over the **10,000-triangle limit are automatically decimated**.
- **Auto-Rigging** (3D) — implants Roblox **Attachment** nodes (head/neck/shoulders/root/waist/hips) and shows them on the preview.
- **Spec Validation** — checks output against Roblox limits (texture ≤ 1024 px, ≤ 10,000 triangles, ≤ 2,048 studs).
- **Batch processing** — drop several files and process them in one run, reviewing each result individually.
- **Console & Stats** — live log plus dimensions, triangles, vertices, materials, attachment count, bounds and file size, with a downloadable log.
- **10-language UI** — English, 繁體中文, 简体中文, 日本語, 한국어, Español, Português, Français, Deutsch, Русский — switchable in-app.
- **Light / Dark theme** — automatic (follows your system) or a manual Auto/Light/Dark toggle.

### Roadmap
- **Binary FBX** import (needs a different, non-WASM strategy) and richer material/texture handling.
- Quadric-error decimation for higher-fidelity reduction, and skinned-rig export.

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

### 功能（現已提供）
- **素材匯入** — 接受 PNG/JPG 影像與 **OBJ／ASCII FBX** 網格，並驗證輸入內容。
- **智慧範本產生器**（2D）— 將圖片貼合至 **585 × 559** 的 Roblox 服裝範本，並**自動填補邊緣**，避免出現透明縫隙。
- **幾何引擎**（3D）— 清理網格、置中並縮放至 Roblox 範圍、補上缺少的法線並匯出整理後的 OBJ。**超過 10,000 三角形上限的網格會自動精簡**。
- **自動綁定骨架**（3D）— 植入 Roblox **Attachment** 節點（頭／頸／肩／根部／腰／髖）並顯示於預覽。
- **規格驗證** — 依 Roblox 限制檢查輸出（材質 ≤ 1024 像素、≤ 10,000 三角形、≤ 2,048 studs）。
- **批次處理** — 一次拖入多個檔案並於單次執行中處理，逐檔檢視結果。
- **主控台與統計** — 即時紀錄，並顯示尺寸、三角形、頂點、材質、附著點數量、邊界與檔案大小，可下載紀錄。
- **10 種語言介面** — English、繁體中文、简体中文、日本語、한국어、Español、Português、Français、Deutsch、Русский，可於程式內切換。
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

### 功能（现已提供）
- **素材导入** — 接受 PNG/JPG 图像与 **OBJ／ASCII FBX** 网格，并验证输入内容。
- **智能模板生成器**（2D）— 将图片贴合至 **585 × 559** 的 Roblox 服装模板，并**自动填补边缘**，避免出现透明缝隙。
- **几何引擎**（3D）— 清理网格、居中并缩放至 Roblox 范围、补上缺少的法线并导出整理后的 OBJ。**超过 10,000 三角形上限的网格会自动精简**。
- **自动绑定骨架**（3D）— 植入 Roblox **Attachment** 节点（头／颈／肩／根部／腰／髋）并显示于预览。
- **规格验证** — 依 Roblox 限制检查输出（贴图 ≤ 1024 像素、≤ 10,000 三角形、≤ 2,048 studs）。
- **批量处理** — 一次拖入多个文件并于单次运行中处理，逐文件查看结果。
- **控制台与统计** — 实时日志，并显示尺寸、三角形、顶点、材质、附着点数量、边界与文件大小，可下载日志。
- **10 种语言界面** — English、繁體中文、简体中文、日本語、한국어、Español、Português、Français、Deutsch、Русский，可在程序内切换。
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

### 许可证
RB Skin Forge 采用专属许可证——请参阅 [LICENSE](LICENSE)。要点：可免费使用并分享原始发行版；
若要修改与再分发，**必须**完整保留 LICENSE 并以显著、逐字方式标注来源：
**“Creators: SpaceSquare & Claude Code | Owner: SpaceSquare”**。所有权由 SpaceSquare 保留。
本程序为非官方工具，与 Roblox Corporation 无任何隶属或背书关系。

第三方：图像处理采用 [SixLabors.ImageSharp](https://github.com/SixLabors/ImageSharp) 3.1.x（Six Labors Split License）。
