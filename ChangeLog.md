<!--
  RB Skin Forge — Change Log
  Languages: English · 繁體中文 · 简体中文
  Format based on Keep a Changelog (https://keepachangelog.com).
-->

# Change Log

**Languages:** [English](#english) · [繁體中文](#繁體中文) · [简体中文](#简体中文)

---

## English

### [Unreleased]
- Phase 3: FBX support, auto-rigging (Roblox Attachment nodes), and mesh decimation for over-limit models.

### [0.1.0] — 2026-06-13
First release — Phase 1 (2D images) and Phase 2 (OBJ 3D meshes) across all three platforms.

**Added**
- Shared `RB_Skin_Forge.Core` library with a single pipeline that routes images and meshes to the right engine.
- Phase 1 (images): asset ingestion, Roblox spec model, smart template generator (585 × 559 with edge-fill), spec validator.
- Phase 2 (OBJ meshes): OBJ/MTL parser, geometry engine (remove degenerate faces, center, scale-to-fit Roblox bounds, generate missing normals, export cleaned OBJ), wireframe preview, and triangle/bounds metrics. Over-limit meshes are flagged.
- Three front-ends sharing one Core — Blazor WebAssembly (HTML), .NET MAUI Windows (`.exe`), and Android (`.apk`) — each with Drop Zone / file picker, preview, Console & Stats, and download/save of output and log.
- Automatic light/dark theme, app icon and splash screen, and on-screen attribution.
- Packaged releases: self-contained Windows `.exe`, Android `.apk`, and an HTML launcher.

**Notes**
- Image processing uses SixLabors.ImageSharp **3.1.12** (last free 3.1.x line; 4.x requires a paid license — do not upgrade).
- Auto-rigging (`IAutoRigger`) is stubbed and arrives in Phase 3.

---

## 繁體中文

### [未發布]
- 第三階段：FBX 支援、自動綁定骨架（Roblox Attachment 節點），以及超出上限模型的網格精簡。

### [0.1.0] — 2026-06-13
首個版本——第一階段（2D 影像）與第二階段（OBJ 3D 網格），涵蓋三個平台。

**新增**
- 共用的 `RB_Skin_Forge.Core` 函式庫，單一流程會將影像與網格分派到對應的引擎。
- 第一階段（影像）：素材匯入、Roblox 規格模型、智慧範本產生器（585 × 559 並含邊緣填補）、規格驗證器。
- 第二階段（OBJ 網格）：OBJ/MTL 解析器、幾何引擎（移除退化面、置中、縮放至 Roblox 範圍、補上缺少的法線、匯出整理後的 OBJ）、線框預覽，以及三角形／邊界量測。超出上限的網格會被標示。
- 三種共用同一核心的前端——Blazor WebAssembly（HTML）、.NET MAUI Windows（`.exe`）與 Android（`.apk`）——皆具備拖放區／檔案選擇、預覽、主控台與統計，以及輸出與紀錄的下載／儲存。
- 自動淺色／深色佈景、應用程式圖示與啟動畫面，以及畫面上的署名。
- 打包發行：自包含的 Windows `.exe`、Android `.apk`，以及 HTML 啟動器。

**備註**
- 影像處理使用 SixLabors.ImageSharp **3.1.12**（最後的免費 3.1.x 版本；4.x 需付費授權——請勿升級）。
- 自動綁定骨架（`IAutoRigger`）目前為預留，將於第三階段提供。

---

## 简体中文

### [未发布]
- 第三阶段：FBX 支持、自动绑定骨架（Roblox Attachment 节点），以及超出上限模型的网格精简。

### [0.1.0] — 2026-06-13
首个版本——第一阶段（2D 图像）与第二阶段（OBJ 3D 网格），覆盖三个平台。

**新增**
- 共享的 `RB_Skin_Forge.Core` 库，单一流程会将图像与网格分派到对应的引擎。
- 第一阶段（图像）：素材导入、Roblox 规格模型、智能模板生成器（585 × 559 并含边缘填补）、规格验证器。
- 第二阶段（OBJ 网格）：OBJ/MTL 解析器、几何引擎（移除退化面、居中、缩放至 Roblox 范围、补上缺少的法线、导出整理后的 OBJ）、线框预览，以及三角形／边界测量。超出上限的网格会被标注。
- 三种共享同一核心的前端——Blazor WebAssembly（HTML）、.NET MAUI Windows（`.exe`）与 Android（`.apk`）——均具备拖放区／文件选择、预览、控制台与统计，以及输出与日志的下载／保存。
- 自动浅色／深色主题、应用图标与启动画面，以及屏幕上的署名。
- 打包发行：自包含的 Windows `.exe`、Android `.apk`，以及 HTML 启动器。

**备注**
- 图像处理使用 SixLabors.ImageSharp **3.1.12**（最后的免费 3.1.x 版本；4.x 需付费授权——请勿升级）。
- 自动绑定骨架（`IAutoRigger`）目前为预留，将于第三阶段提供。
