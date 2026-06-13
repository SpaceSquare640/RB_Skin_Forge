<!--
  RB Skin Forge — Change Log
  Languages: English · 繁體中文 · 简体中文
  Format based on Keep a Changelog (https://keepachangelog.com).
-->

# Change Log

**Languages:** [English](#english) · [繁體中文](#繁體中文) · [简体中文](#简体中文)

---

## English

### [0.2.0] — 2026-06-13
Phase 3 (3D) plus a multi-language interface and batch processing across all three platforms.

**Added**
- **Phase 3 — ASCII FBX input**: a pure-C# ASCII FBX reader. Binary FBX is detected and rejected with guidance, because it needs native libraries that can't run in the browser build.
- **Phase 3 — Auto-Rigging**: the geometry engine implants Roblox **Attachment** nodes (head / neck / shoulders / root / waist / hips) from the mesh bounds, embeds them as metadata in the exported OBJ, and draws them on the wireframe preview.
- **Phase 3 — Mesh decimation**: meshes over the 10,000-triangle limit are automatically reduced (vertex clustering, keeping the most detail that fits) instead of only being flagged.
- **10-language UI** with an in-app language selector — English, 繁體中文, 简体中文, 日本語, 한국어, Español, Português, Français, Deutsch, Русский — backed by a shared localization layer in Core. The choice is remembered.
- **Batch processing**: select multiple images/meshes at once, process them in one run, and review each result individually via a per-file status list.
- **Manual theme toggle** (Auto / Light / Dark) in addition to the automatic system-following theme; the choice is remembered.
- **Expanded Console & Stats**: vertices, materials, attachment count, bounding-box size, and a "decimated" indicator.
- Repository social-preview banner (`assets/social-preview.png`).

**Changed**
- `IAutoRigger` is now implemented (was a Phase 3 stub); the geometry engine accepts both OBJ and ASCII FBX.
- The HTML front-end uses a cleaner full-width layout.

### [0.1.0] — 2026-06-13
First release — Phase 1 (2D images) and Phase 2 (OBJ 3D meshes) across all three platforms.

**Added**
- Shared `RB_Skin_Forge.Core` library with a single pipeline that routes images and meshes to the right engine.
- Phase 1 (images): asset ingestion, Roblox spec model, smart template generator (585 × 559 with edge-fill), spec validator.
- Phase 2 (OBJ meshes): OBJ/MTL parser, geometry engine (remove degenerate faces, center, scale-to-fit Roblox bounds, generate missing normals, export cleaned OBJ), wireframe preview, and triangle/bounds metrics.
- Three front-ends sharing one Core — Blazor WebAssembly (HTML), .NET MAUI Windows (`.exe`), and Android (`.apk`) — each with Drop Zone / file picker, preview, Console & Stats, and download/save of output and log.
- Automatic light/dark theme, app icon and splash screen, and on-screen attribution.
- Packaged releases: self-contained Windows `.exe`, Android `.apk`, an HTML launcher, and a Windows installer.

**Notes**
- Image processing uses SixLabors.ImageSharp **3.1.12** (last free 3.1.x line; 4.x requires a paid license — do not upgrade).

---

## 繁體中文

### [0.2.0] — 2026-06-13
第三階段（3D），加上多語言介面與批次處理，涵蓋三個平台。

**新增**
- **第三階段 — ASCII FBX 匯入**：純 C# 的 ASCII FBX 讀取器。二進位 FBX 會被偵測並拒絕並給予指引，因為它需要無法在瀏覽器版執行的原生函式庫。
- **第三階段 — 自動綁定骨架**：幾何引擎依網格邊界植入 Roblox **Attachment** 節點（頭／頸／肩／根部／腰／髖），將其作為中繼資料嵌入匯出的 OBJ，並繪製在線框預覽上。
- **第三階段 — 網格精簡**：超過 10,000 三角形上限的網格會自動精簡（頂點叢集，盡量保留可容納的細節），而非僅被標示。
- **10 種語言介面**，內建語言選擇器——English、繁體中文、简体中文、日本語、한국어、Español、Português、Français、Deutsch、Русский——由 Core 中共用的在地化層支援，並會記住選擇。
- **批次處理**：一次選取多個影像／網格，於單次執行中處理，並透過逐檔狀態清單個別檢視結果。
- **手動佈景切換**（自動／淺色／深色），於自動跟隨系統之外另行提供；選擇會被記住。
- **擴充的主控台與統計**：頂點、材質、附著點數量、邊界框大小，以及「已減面」標示。
- 儲存庫社群預覽橫幅（`assets/social-preview.png`）。

**變更**
- `IAutoRigger` 現已實作（先前為第三階段預留）；幾何引擎同時接受 OBJ 與 ASCII FBX。
- HTML 前端改用更簡潔的全寬版面。

### [0.1.0] — 2026-06-13
首個版本——第一階段（2D 影像）與第二階段（OBJ 3D 網格），涵蓋三個平台。

**新增**
- 共用的 `RB_Skin_Forge.Core` 函式庫，單一流程會將影像與網格分派到對應的引擎。
- 第一階段（影像）：素材匯入、Roblox 規格模型、智慧範本產生器（585 × 559 並含邊緣填補）、規格驗證器。
- 第二階段（OBJ 網格）：OBJ/MTL 解析器、幾何引擎（移除退化面、置中、縮放至 Roblox 範圍、補上缺少的法線、匯出整理後的 OBJ）、線框預覽，以及三角形／邊界量測。
- 三種共用同一核心的前端——Blazor WebAssembly（HTML）、.NET MAUI Windows（`.exe`）與 Android（`.apk`）——皆具備拖放區／檔案選擇、預覽、主控台與統計，以及輸出與紀錄的下載／儲存。
- 自動淺色／深色佈景、應用程式圖示與啟動畫面，以及畫面上的署名。
- 打包發行：自包含的 Windows `.exe`、Android `.apk`、HTML 啟動器，以及 Windows 安裝程式。

**備註**
- 影像處理使用 SixLabors.ImageSharp **3.1.12**（最後的免費 3.1.x 版本；4.x 需付費授權——請勿升級）。

---

## 简体中文

### [0.2.0] — 2026-06-13
第三阶段（3D），加上多语言界面与批量处理，覆盖三个平台。

**新增**
- **第三阶段 — ASCII FBX 导入**：纯 C# 的 ASCII FBX 读取器。二进制 FBX 会被检测并拒绝并给予指引，因为它需要无法在浏览器版运行的原生库。
- **第三阶段 — 自动绑定骨架**：几何引擎依网格边界植入 Roblox **Attachment** 节点（头／颈／肩／根部／腰／髋），将其作为元数据嵌入导出的 OBJ，并绘制在线框预览上。
- **第三阶段 — 网格精简**：超过 10,000 三角形上限的网格会自动精简（顶点聚类，尽量保留可容纳的细节），而非仅被标注。
- **10 种语言界面**，内置语言选择器——English、繁體中文、简体中文、日本語、한국어、Español、Português、Français、Deutsch、Русский——由 Core 中共享的本地化层支持，并会记住选择。
- **批量处理**：一次选取多个图像／网格，于单次运行中处理，并通过逐文件状态列表单独查看结果。
- **手动主题切换**（自动／浅色／深色），在自动跟随系统之外另行提供；选择会被记住。
- **扩展的控制台与统计**：顶点、材质、附着点数量、边界框大小，以及“已减面”标识。
- 仓库社交预览横幅（`assets/social-preview.png`）。

**变更**
- `IAutoRigger` 现已实现（此前为第三阶段预留）；几何引擎同时接受 OBJ 与 ASCII FBX。
- HTML 前端改用更简洁的全宽布局。

### [0.1.0] — 2026-06-13
首个版本——第一阶段（2D 图像）与第二阶段（OBJ 3D 网格），覆盖三个平台。

**新增**
- 共享的 `RB_Skin_Forge.Core` 库，单一流程会将图像与网格分派到对应的引擎。
- 第一阶段（图像）：素材导入、Roblox 规格模型、智能模板生成器（585 × 559 并含边缘填补）、规格验证器。
- 第二阶段（OBJ 网格）：OBJ/MTL 解析器、几何引擎（移除退化面、居中、缩放至 Roblox 范围、补上缺少的法线、导出整理后的 OBJ）、线框预览，以及三角形／边界测量。
- 三种共享同一核心的前端——Blazor WebAssembly（HTML）、.NET MAUI Windows（`.exe`）与 Android（`.apk`）——均具备拖放区／文件选择、预览、控制台与统计，以及输出与日志的下载／保存。
- 自动浅色／深色主题、应用图标与启动画面，以及屏幕上的署名。
- 打包发行：自包含的 Windows `.exe`、Android `.apk`、HTML 启动器，以及 Windows 安装程序。

**备注**
- 图像处理使用 SixLabors.ImageSharp **3.1.12**（最后的免费 3.1.x 版本；4.x 需付费授权——请勿升级）。
