namespace RB_Skin_Forge.Core.Localization;

/// <summary>
/// The UI translation table. Each key maps to one value per language, in the exact
/// order of <see cref="Languages.All"/>:
/// 0 en · 1 zh-Hant · 2 zh-Hans · 3 ja · 4 ko · 5 es · 6 pt · 7 fr · 8 de · 9 ru
/// </summary>
internal static class Strings
{
    public static readonly IReadOnlyDictionary<string, string[]> Table = new Dictionary<string, string[]>
    {
        ["app.tagline"] = new[]
        {
            "Images → Roblox clothing templates. OBJ/FBX meshes → cleaned, auto-rigged, validated Roblox meshes.",
            "圖片 → Roblox 服裝範本。OBJ/FBX 網格 → 清理、自動綁定、驗證後的 Roblox 網格。",
            "图片 → Roblox 服装模板。OBJ/FBX 网格 → 清理、自动绑定、验证后的 Roblox 网格。",
            "画像 → Roblox 服テンプレート。OBJ/FBX メッシュ → 整理・自動リグ・検証済みの Roblox メッシュ。",
            "이미지 → Roblox 의상 템플릿. OBJ/FBX 메시 → 정리·자동 리깅·검증된 Roblox 메시.",
            "Imágenes → plantillas de ropa de Roblox. Mallas OBJ/FBX → mallas de Roblox limpias, con rig automático y validadas.",
            "Imagens → modelos de roupa do Roblox. Malhas OBJ/FBX → malhas do Roblox limpas, com rig automático e validadas.",
            "Images → modèles de vêtements Roblox. Maillages OBJ/FBX → maillages Roblox nettoyés, riggés automatiquement et validés.",
            "Bilder → Roblox-Kleidungsvorlagen. OBJ/FBX-Meshes → bereinigte, automatisch geriggte, validierte Roblox-Meshes.",
            "Изображения → шаблоны одежды Roblox. Меши OBJ/FBX → очищенные, авторигованные и проверенные меши Roblox.",
        },
        ["nav.language"] = new[]
        { "Language", "語言", "语言", "言語", "언어", "Idioma", "Idioma", "Langue", "Sprache", "Язык" },
        ["nav.theme"] = new[]
        { "Theme", "佈景主題", "主题", "テーマ", "테마", "Tema", "Tema", "Thème", "Design", "Тема" },
        ["theme.auto"] = new[]
        { "Auto", "自動", "自动", "自動", "자동", "Auto", "Auto", "Auto", "Auto", "Авто" },
        ["theme.light"] = new[]
        { "Light", "淺色", "浅色", "ライト", "라이트", "Claro", "Claro", "Clair", "Hell", "Светлая" },
        ["theme.dark"] = new[]
        { "Dark", "深色", "深色", "ダーク", "다크", "Oscuro", "Escuro", "Sombre", "Dunkel", "Тёмная" },
        ["panel.dropzone"] = new[]
        { "Drop Zone", "拖放區", "拖放区", "ドロップゾーン", "드롭 존", "Zona de arrastre", "Zona de soltar", "Zone de dépôt", "Ablagebereich", "Зона загрузки" },
        ["field.bodypart"] = new[]
        { "Body part", "身體部位", "身体部位", "ボディ部位", "신체 부위", "Parte del cuerpo", "Parte do corpo", "Partie du corps", "Körperteil", "Часть тела" },
        ["dropzone.hint"] = new[]
        {
            "Choose or drop images (PNG, JPG…) or meshes (OBJ, FBX)",
            "選擇或拖入圖片（PNG、JPG…）或網格（OBJ、FBX）",
            "选择或拖入图片（PNG、JPG…）或网格（OBJ、FBX）",
            "画像（PNG、JPG…）またはメッシュ（OBJ、FBX）を選択／ドロップ",
            "이미지(PNG, JPG…) 또는 메시(OBJ, FBX)를 선택하거나 끌어다 놓기",
            "Elige o arrastra imágenes (PNG, JPG…) o mallas (OBJ, FBX)",
            "Escolha ou arraste imagens (PNG, JPG…) ou malhas (OBJ, FBX)",
            "Choisissez ou déposez des images (PNG, JPG…) ou des maillages (OBJ, FBX)",
            "Bilder (PNG, JPG…) oder Meshes (OBJ, FBX) wählen oder ablegen",
            "Выберите или перетащите изображения (PNG, JPG…) или меши (OBJ, FBX)",
        },
        ["btn.generate"] = new[]
        { "Generate", "產生", "生成", "生成", "생성", "Generar", "Gerar", "Générer", "Erzeugen", "Создать" },
        ["btn.processing"] = new[]
        { "Processing…", "處理中…", "处理中…", "処理中…", "처리 중…", "Procesando…", "Processando…", "Traitement…", "Verarbeite…", "Обработка…" },
        ["panel.preview"] = new[]
        { "Preview", "預覽", "预览", "プレビュー", "미리보기", "Vista previa", "Pré-visualização", "Aperçu", "Vorschau", "Предпросмотр" },
        ["preview.empty"] = new[]
        { "No output yet", "尚無輸出", "尚无输出", "出力はまだありません", "아직 출력 없음", "Aún no hay resultado", "Ainda sem resultado", "Pas encore de résultat", "Noch keine Ausgabe", "Пока нет результата" },
        ["panel.console"] = new[]
        { "Console & Stats", "主控台與統計", "控制台与统计", "コンソールと統計", "콘솔 및 통계", "Consola y estadísticas", "Console e estatísticas", "Console et statistiques", "Konsole & Statistik", "Консоль и статистика" },
        ["stats.dimensions"] = new[]
        { "Dimensions", "尺寸", "尺寸", "サイズ", "크기", "Dimensiones", "Dimensões", "Dimensions", "Abmessungen", "Размеры" },
        ["stats.triangles"] = new[]
        { "Triangles", "三角形", "三角形", "三角形", "삼각형", "Triángulos", "Triângulos", "Triangles", "Dreiecke", "Треугольники" },
        ["stats.vertices"] = new[]
        { "Vertices", "頂點", "顶点", "頂点", "정점", "Vértices", "Vértices", "Sommets", "Eckpunkte", "Вершины" },
        ["stats.materials"] = new[]
        { "Materials", "材質", "材质", "マテリアル", "재질", "Materiales", "Materiais", "Matériaux", "Materialien", "Материалы" },
        ["stats.attachments"] = new[]
        { "Attachments", "附著點", "附着点", "アタッチメント", "어태치먼트", "Puntos de anclaje", "Pontos de fixação", "Points d'attache", "Attachments", "Точки крепления" },
        ["stats.bounds"] = new[]
        { "Bounds (studs)", "邊界（studs）", "边界（studs）", "境界（studs）", "경계 (studs)", "Límites (studs)", "Limites (studs)", "Limites (studs)", "Grenzen (Studs)", "Габариты (studs)" },
        ["stats.outputsize"] = new[]
        { "Output size", "輸出大小", "输出大小", "出力サイズ", "출력 크기", "Tamaño de salida", "Tamanho da saída", "Taille de sortie", "Ausgabegröße", "Размер вывода" },
        ["stats.decimated"] = new[]
        { "decimated", "已減面", "已减面", "減面済み", "감면됨", "decimado", "decimado", "décimé", "reduziert", "упрощено" },
        ["btn.downloadoutput"] = new[]
        { "Download output", "下載輸出", "下载输出", "出力をダウンロード", "출력 다운로드", "Descargar resultado", "Baixar resultado", "Télécharger le résultat", "Ausgabe herunterladen", "Скачать результат" },
        ["btn.downloadlog"] = new[]
        { "Download log", "下載紀錄", "下载日志", "ログをダウンロード", "로그 다운로드", "Descargar registro", "Baixar registro", "Télécharger le journal", "Protokoll herunterladen", "Скачать журнал" },
        ["btn.saveoutput"] = new[]
        { "Save Output", "儲存輸出", "保存输出", "出力を保存", "출력 저장", "Guardar resultado", "Salvar resultado", "Enregistrer le résultat", "Ausgabe speichern", "Сохранить результат" },
        ["btn.savelog"] = new[]
        { "Save Log", "儲存紀錄", "保存日志", "ログを保存", "로그 저장", "Guardar registro", "Salvar registro", "Enregistrer le journal", "Protokoll speichern", "Сохранить журнал" },
        ["btn.choose"] = new[]
        { "Choose Files…", "選擇檔案…", "选择文件…", "ファイルを選択…", "파일 선택…", "Elegir archivos…", "Escolher arquivos…", "Choisir des fichiers…", "Dateien wählen…", "Выбрать файлы…" },
        ["file.none"] = new[]
        { "No file selected", "未選擇檔案", "未选择文件", "ファイル未選択", "선택된 파일 없음", "Ningún archivo seleccionado", "Nenhum arquivo selecionado", "Aucun fichier sélectionné", "Keine Datei ausgewählt", "Файл не выбран" },
        ["status.ready"] = new[]
        { "Ready.", "就緒。", "就绪。", "準備完了。", "준비 완료.", "Listo.", "Pronto.", "Prêt.", "Bereit.", "Готово." },
        ["batch.summary"] = new[]
        { "{0} of {1} succeeded", "{0}/{1} 成功", "{0}/{1} 成功", "{0}/{1} 成功", "{0}/{1} 성공", "{0} de {1} con éxito", "{0} de {1} com sucesso", "{0} sur {1} réussis", "{0} von {1} erfolgreich", "{0} из {1} успешно" },
        ["state.Queued"] = new[]
        { "Queued", "排隊中", "排队中", "待機中", "대기 중", "En cola", "Na fila", "En file", "In Warteschlange", "В очереди" },
        ["state.Processing"] = new[]
        { "Processing", "處理中", "处理中", "処理中", "처리 중", "Procesando", "Processando", "En cours", "Verarbeitung", "Обработка" },
        ["state.Synchronized"] = new[]
        { "Done", "已完成", "已完成", "完了", "완료", "Listo", "Concluído", "Terminé", "Fertig", "Готово" },
        ["state.Failed"] = new[]
        { "Failed", "失敗", "失败", "失敗", "실패", "Fallido", "Falhou", "Échec", "Fehlgeschlagen", "Ошибка" },
        ["bodypart.Unknown"] = new[]
        { "Unknown", "未知", "未知", "不明", "알 수 없음", "Desconocido", "Desconhecido", "Inconnu", "Unbekannt", "Неизвестно" },
        ["bodypart.Torso"] = new[]
        { "Torso", "軀幹", "躯干", "胴体", "몸통", "Torso", "Torso", "Torse", "Torso", "Торс" },
        ["bodypart.LeftArm"] = new[]
        { "Left Arm", "左臂", "左臂", "左腕", "왼팔", "Brazo izquierdo", "Braço esquerdo", "Bras gauche", "Linker Arm", "Левая рука" },
        ["bodypart.RightArm"] = new[]
        { "Right Arm", "右臂", "右臂", "右腕", "오른팔", "Brazo derecho", "Braço direito", "Bras droit", "Rechter Arm", "Правая рука" },
        ["bodypart.LeftLeg"] = new[]
        { "Left Leg", "左腿", "左腿", "左脚", "왼다리", "Pierna izquierda", "Perna esquerda", "Jambe gauche", "Linkes Bein", "Левая нога" },
        ["bodypart.RightLeg"] = new[]
        { "Right Leg", "右腿", "右腿", "右脚", "오른다리", "Pierna derecha", "Perna direita", "Jambe droite", "Rechtes Bein", "Правая нога" },
        ["bodypart.Head"] = new[]
        { "Head", "頭部", "头部", "頭", "머리", "Cabeza", "Cabeça", "Tête", "Kopf", "Голова" },
        ["bodypart.Accessory"] = new[]
        { "Accessory", "配件", "配件", "アクセサリー", "액세서리", "Accesorio", "Acessório", "Accessoire", "Zubehör", "Аксессуар" },
    };
}
