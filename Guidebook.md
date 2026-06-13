<!--
  RB Skin Forge — Guidebook (user guide)
  Languages: English · 繁體中文 · 简体中文
  Default language: English
-->

# RB Skin Forge — Guidebook

**Languages:** [English](#english) · [繁體中文](#繁體中文) · [简体中文](#简体中文)

---

## English

### 1. Which version should I use?
- **HTML (browser)** — nothing to install; open the [live demo](https://spacesquare640.github.io/RB_Skin_Forge/) or run the downloadable `HTML_Version` launcher.
- **Windows (.exe)** — desktop app; install with `RB_Skin_Forge_installer.exe` (or run the portable build).
- **Android (.apk)** — on-the-go.

All three behave the same way because they share one engine. Set the interface up with the **Language** selector and the **Auto / Light / Dark** theme buttons at the top right — both are remembered between visits.

### 2. Generating a clothing template (2D images)
1. **Pick a body part** — Torso, Arm, Leg, etc. (This labels the job; the layout is the standard template.)
2. **Choose an image** — drag a PNG/JPG into the Drop Zone (HTML) or click **Choose Files…** (Windows/Android).
3. **Click "Generate."**
4. **Check the Preview** — your image is fitted onto the **585 × 559** Roblox clothing template, and the edges are extended outward so there are no transparent gaps.
5. **Read Console & Stats** — dimensions, file size, and a pass/fail against Roblox limits. A green **Done** status means it's ready.
6. **Download / Save** — save the template image and the log.

### 3. Processing a 3D mesh (OBJ / ASCII FBX)
1. **Choose a mesh** — an `.obj` or **ASCII** `.fbx` file (export from Blender/Maya with the ASCII option; binary FBX is rejected with a note).
2. **Click "Generate."** The app cleans the mesh (removes degenerate faces), centers it, scales it to fit Roblox's stud bounds, and generates normals if they're missing.
3. **Auto-rigging** places Roblox **Attachment** nodes (head, neck, shoulders, root, waist, hips) and marks them in orange on the wireframe preview.
4. **Decimation** — if the mesh is over Roblox's 10,000-triangle limit it is automatically reduced; the stats show a **decimated** tag and the before/after counts appear in the log.
5. **Download / Save** the cleaned `.obj` (attachment positions are embedded as comments) and the log.

### 4. Batch processing
Select several files at once. Click **Generate** to process them in one run, then click any file in the list to review its preview, stats and log. A summary shows how many succeeded.

### 5. Using the output in Roblox
- **2D template** — upload the PNG to Roblox as a Classic Shirt/Pants/T-Shirt; the 585 × 559 canvas matches the classic clothing layout.
- **3D mesh** — import the cleaned `.obj` with the Roblox Studio mesh importer; it already fits the platform's size and triangle limits.

### 6. Tips
- Higher-resolution source images give crisper results, but the output is always within Roblox's 1024 px limit.
- If validation shows an **Error**, the Console explains why (e.g. an unsupported file type, or a binary FBX).
- The interface is available in 10 languages and remembers your language and theme between visits.

---

## 繁體中文

### 1. 我該用哪個版本？
- **HTML（瀏覽器）** — 免安裝；開啟[線上 Demo](https://spacesquare640.github.io/RB_Skin_Forge/)，或執行可下載的 `HTML_Version` 啟動器。
- **Windows（.exe）** — 桌面應用程式；以 `RB_Skin_Forge_installer.exe` 安裝（或執行免安裝版）。
- **Android（.apk）** — 隨身使用。

三種版本共用同一個引擎，操作方式完全相同。可用右上角的 **語言** 選擇器與 **自動／淺色／深色** 佈景按鈕設定介面——兩者都會在下次造訪時記住。

### 2. 產生服裝範本（2D 影像）
1. **選擇身體部位** — 軀幹、手臂、腿部等。（此為工作標示；版面採用標準範本。）
2. **選擇圖片** — 將 PNG/JPG 拖入拖放區（HTML），或點選 **Choose Files…**（Windows/Android）。
3. **點擊「Generate」。**
4. **檢視預覽** — 圖片會貼合到 **585 × 559** 的 Roblox 服裝範本，並向外延伸邊緣，避免透明縫隙。
5. **查看主控台與統計** — 顯示尺寸、檔案大小，以及是否符合 Roblox 限制。綠色的 **完成** 代表已就緒。
6. **下載／儲存** — 儲存範本圖片與紀錄。

### 3. 處理 3D 網格（OBJ／ASCII FBX）
1. **選擇網格** — `.obj` 或 **ASCII** `.fbx` 檔（請從 Blender/Maya 以 ASCII 選項匯出；二進位 FBX 會被拒絕並提示）。
2. **點擊「Generate」。** 程式會清理網格（移除退化面）、置中、縮放至 Roblox 的 stud 範圍，並在缺少法線時自動產生。
3. **自動綁定骨架** 會放置 Roblox **Attachment** 節點（頭、頸、肩、根部、腰、髖），並在線框預覽上以橙色標示。
4. **網格精簡** — 若網格超過 Roblox 的 10,000 三角形上限，將自動精簡；統計會顯示 **已減面** 標籤，紀錄中可見前後數量。
5. **下載／儲存** 整理後的 `.obj`（附著點位置會以註解嵌入）與紀錄。

### 4. 批次處理
一次選取多個檔案，點擊 **Generate** 於單次執行中處理，再點清單中的任一檔案即可檢視其預覽、統計與紀錄。摘要會顯示成功數量。

### 5. 在 Roblox 中使用輸出
- **2D 範本** — 將 PNG 以經典襯衫／褲子／T 恤上傳至 Roblox；585 × 559 的畫布符合經典服裝版面。
- **3D 網格** — 透過 Roblox Studio 的網格匯入工具匯入整理後的 `.obj`；其已符合平台的尺寸與三角形上限。

### 6. 小提示
- 來源圖片解析度越高，結果越清晰，但輸出一律不超過 Roblox 的 1024 像素限制。
- 若驗證顯示 **Error**，主控台會說明原因（例如不支援的檔案類型，或二進位 FBX）。
- 介面提供 10 種語言，並會在再次造訪時記住你的語言與佈景設定。

---

## 简体中文

### 1. 我该用哪个版本？
- **HTML（浏览器）** — 免安装；打开[在线 Demo](https://spacesquare640.github.io/RB_Skin_Forge/)，或运行可下载的 `HTML_Version` 启动器。
- **Windows（.exe）** — 桌面应用；用 `RB_Skin_Forge_installer.exe` 安装（或运行免安装版）。
- **Android（.apk）** — 随身使用。

三种版本共享同一个引擎，操作方式完全相同。可用右上角的 **语言** 选择器与 **自动／浅色／深色** 主题按钮设置界面——两者都会在下次访问时记住。

### 2. 生成服装模板（2D 图像）
1. **选择身体部位** — 躯干、手臂、腿部等。（此为任务标记；版面采用标准模板。）
2. **选择图片** — 将 PNG/JPG 拖入拖放区（HTML），或点击 **Choose Files…**（Windows/Android）。
3. **点击“Generate”。**
4. **查看预览** — 图片会贴合到 **585 × 559** 的 Roblox 服装模板，并向外延伸边缘，避免透明缝隙。
5. **查看控制台与统计** — 显示尺寸、文件大小，以及是否符合 Roblox 限制。绿色的 **完成** 代表已就绪。
6. **下载／保存** — 保存模板图片与日志。

### 3. 处理 3D 网格（OBJ／ASCII FBX）
1. **选择网格** — `.obj` 或 **ASCII** `.fbx` 文件（请从 Blender/Maya 以 ASCII 选项导出；二进制 FBX 会被拒绝并提示）。
2. **点击“Generate”。** 程序会清理网格（移除退化面）、居中、缩放至 Roblox 的 stud 范围，并在缺少法线时自动生成。
3. **自动绑定骨架** 会放置 Roblox **Attachment** 节点（头、颈、肩、根部、腰、髋），并在线框预览上以橙色标示。
4. **网格精简** — 若网格超过 Roblox 的 10,000 三角形上限，将自动精简；统计会显示 **已减面** 标签，日志中可见前后数量。
5. **下载／保存** 整理后的 `.obj`（附着点位置会以注释嵌入）与日志。

### 4. 批量处理
一次选取多个文件，点击 **Generate** 于单次运行中处理，再点列表中的任一文件即可查看其预览、统计与日志。摘要会显示成功数量。

### 5. 在 Roblox 中使用输出
- **2D 模板** — 将 PNG 以经典衬衫／裤子／T 恤上传至 Roblox；585 × 559 的画布符合经典服装版面。
- **3D 网格** — 通过 Roblox Studio 的网格导入工具导入整理后的 `.obj`；其已符合平台的尺寸与三角形上限。

### 6. 小提示
- 源图片分辨率越高，结果越清晰，但输出一律不超过 Roblox 的 1024 像素限制。
- 若验证显示 **Error**，控制台会说明原因（例如不支持的文件类型，或二进制 FBX）。
- 界面提供 10 种语言，并会在再次访问时记住你的语言与主题设置。
