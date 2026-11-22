using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
#if ENABLE_INPUT_SYSTEM && !ENABLE_LEGACY_INPUT_MANAGER
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
#endif

/// <summary>
/// Ghost Postmaster - Instructions screen.
///
/// Put this on an empty GameObject in an empty "Instructions" scene and press Play.
/// It builds the whole screen in code (canvas, background, title, instruction rows,
/// Back and Play buttons), so there is no manual UI layout to do.
///
/// Everything you might want to change - the wording, the controls, the icons,
/// the colors and the scene names - is editable in the Inspector.
///
/// Keyboard shortcuts: Enter/Space = Play, Esc = Back.
/// </summary>
public class InstructionsScreen : MonoBehaviour
{
    /// <summary>One line of instructions: an icon, a bold heading and a description.</summary>
    [System.Serializable]
    public class InstructionRow
    {
        public Sprite icon;
        public string heading;
        [TextArea(2, 4)] public string description;
    }

    [Header("Scene names (must match File > Build Settings)")]
    [Tooltip("Scene the Play button loads (your gameplay scene).")]
    public string playSceneName = "Game";
    [Tooltip("Scene the Back button loads (your title screen).")]
    public string backSceneName = "Title";

    [Header("Text")]
    public string title = "How to Play";
    public string subtitle = "You're the Ghost Postmaster! Deliver every letter to the right house.";

    [Tooltip("Edit these to match your game. Drag sprites from the art pack into the Icon slots.")]
    public InstructionRow[] rows =
    {
        new InstructionRow { heading = "Move",
            description = "Use WASD or the Arrow Keys to float around town." },
        new InstructionRow { heading = "Pick up your orders",
            description = "Each round you get 3 to 5 deliveries, in a random order." },
        new InstructionRow { heading = "Deliver to the right house",
            description = "Letters can only be delivered to the house on your current order." },
        new InstructionRow { heading = "Follow the glowing dots",
            description = "The path shows the shortest route to your next house." },
        new InstructionRow { heading = "Win",
            description = "Deliver every letter to complete your route!" },
    };

    [Header("Optional font (leave empty to use Unity's built-in font)")]
    public Font font;

    [Header("Colors (match the art pack)")]
    public Color backgroundColor = new Color32(0x1b, 0x17, 0x30, 0xff);
    public Color panelColor = new Color32(0x26, 0x20, 0x43, 0xff);
    public Color titleColor = new Color32(0xf2, 0xc9, 0x4c, 0xff);
    public Color textColor = new Color32(0xf4, 0xf2, 0xff, 0xff);
    public Color subtleTextColor = new Color32(0xc9, 0xc3, 0xe8, 0xff);
    public Color playButtonColor = new Color32(0x3f, 0x5f, 0xb5, 0xff);
    public Color backButtonColor = new Color32(0x4a, 0x3b, 0x5c, 0xff);

    void Start()
    {
        if (font == null)
        {
            font = LoadBuiltinFont();
        }
        EnsureEventSystem();
        BuildScreen();
    }

    void Update()
    {
        // Keyboard shortcuts so the screen also works without a mouse.
#if ENABLE_INPUT_SYSTEM && !ENABLE_LEGACY_INPUT_MANAGER
        Keyboard kb = Keyboard.current;
        if (kb == null) return;
        if (kb.enterKey.wasPressedThisFrame || kb.spaceKey.wasPressedThisFrame) Play();
        if (kb.escapeKey.wasPressedThisFrame) Back();
#else
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space)) Play();
        if (Input.GetKeyDown(KeyCode.Escape)) Back();
#endif
    }

    // ------------------------------------------------------------------
    // Button actions
    // ------------------------------------------------------------------

    public void Play() { LoadSceneSafe(playSceneName); }
    public void Back() { LoadSceneSafe(backSceneName); }

    static void LoadSceneSafe(string sceneName)
    {
        if (Application.CanStreamedLevelBeLoaded(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError("Scene '" + sceneName + "' can't be loaded. Check the spelling in the Inspector " +
                           "and add it under File > Build Settings > Scenes In Build.");
        }
    }

    // ------------------------------------------------------------------
    // Building the UI
    // ------------------------------------------------------------------

    void BuildScreen()
    {
        // Canvas that scales with the window (designed at 1920x1080).
        GameObject canvasObject = new GameObject("Instructions Canvas");
        Canvas canvas = canvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        CanvasScaler scaler = canvasObject.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        scaler.matchWidthOrHeight = 0.5f;
        canvasObject.AddComponent<GraphicRaycaster>();
        RectTransform root = canvasObject.GetComponent<RectTransform>();

        // Full-screen background.
        Image background = CreateImage("Background", root, backgroundColor);
        Stretch(background.rectTransform, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);

        // Title and subtitle.
        Text titleText = CreateText("Title", root, title, 96, titleColor, TextAnchor.MiddleCenter, FontStyle.Bold);
        Stretch(titleText.rectTransform, new Vector2(0, 1), new Vector2(1, 1), new Vector2(0, -170), new Vector2(0, -40));

        Text subtitleText = CreateText("Subtitle", root, subtitle, 36, subtleTextColor, TextAnchor.MiddleCenter, FontStyle.Normal);
        Stretch(subtitleText.rectTransform, new Vector2(0, 1), new Vector2(1, 1), new Vector2(0, -230), new Vector2(0, -170));

        // Panel that holds the instruction rows.
        Image panel = CreateImage("Panel", root, panelColor);
        RectTransform panelRect = panel.rectTransform;
        panelRect.anchorMin = panelRect.anchorMax = new Vector2(0.5f, 0.5f);
        panelRect.sizeDelta = new Vector2(1360, 640);
        panelRect.anchoredPosition = new Vector2(0, -20);

        VerticalLayoutGroup list = panel.gameObject.AddComponent<VerticalLayoutGroup>();
        list.padding = new RectOffset(56, 56, 40, 40);
        list.spacing = 18;
        list.childAlignment = TextAnchor.MiddleLeft;
        list.childControlWidth = true;
        list.childControlHeight = true;
        list.childForceExpandWidth = true;
        list.childForceExpandHeight = true;

        foreach (InstructionRow row in rows)
        {
            CreateRow(panelRect, row);
        }

        // Buttons along the bottom.
        Button back = CreateButton("Back Button", root, "Back", backButtonColor, Back);
        RectTransform backRect = back.GetComponent<RectTransform>();
        backRect.anchorMin = backRect.anchorMax = new Vector2(0.5f, 0f);
        backRect.sizeDelta = new Vector2(300, 90);
        backRect.anchoredPosition = new Vector2(-180, 100);

        Button play = CreateButton("Play Button", root, "Play", playButtonColor, Play);
        RectTransform playRect = play.GetComponent<RectTransform>();
        playRect.anchorMin = playRect.anchorMax = new Vector2(0.5f, 0f);
        playRect.sizeDelta = new Vector2(300, 90);
        playRect.anchoredPosition = new Vector2(180, 100);
    }

    /// <summary>One instruction line: [icon]  Heading - description.</summary>
    void CreateRow(RectTransform parent, InstructionRow row)
    {
        GameObject rowObject = new GameObject("Row - " + row.heading, typeof(RectTransform));
        rowObject.transform.SetParent(parent, false);

        HorizontalLayoutGroup layout = rowObject.AddComponent<HorizontalLayoutGroup>();
        layout.spacing = 28;
        layout.childAlignment = TextAnchor.MiddleLeft;
        layout.childControlWidth = true;
        layout.childControlHeight = true;
        layout.childForceExpandWidth = false;
        layout.childForceExpandHeight = false;

        // Icon: the sprite if one was assigned, otherwise a small gold square as a bullet.
        Image icon = CreateImage("Icon", rowObject.transform, row.icon != null ? Color.white : titleColor);
        if (row.icon != null)
        {
            icon.sprite = row.icon;
            icon.preserveAspect = true;
        }
        LayoutElement iconLayout = icon.gameObject.AddComponent<LayoutElement>();
        float iconSize = row.icon != null ? 72 : 20;
        iconLayout.preferredWidth = iconLayout.minWidth = 72;
        iconLayout.preferredHeight = iconLayout.minHeight = iconSize;
        if (row.icon == null)
        {
            // Keep the bullet small but leave the same column width so text lines up.
            iconLayout.preferredWidth = iconLayout.minWidth = 20;
            layout.padding = new RectOffset(26, 0, 0, 0);
        }

        // Gold bold heading on the first line, description underneath (one rich-text block).
        string hex = ColorUtility.ToHtmlStringRGB(titleColor);
        string content = "<color=#" + hex + "><b>" + row.heading + "</b></color>\n<size=31>" + row.description + "</size>";
        Text text = CreateText("Text", rowObject.transform, content, 38, textColor, TextAnchor.MiddleLeft, FontStyle.Normal);
        text.lineSpacing = 0.95f;
        LayoutElement textLayout = text.gameObject.AddComponent<LayoutElement>();
        textLayout.flexibleWidth = 1;
    }

    // ------------------------------------------------------------------
    // Small UI helpers
    // ------------------------------------------------------------------

    Image CreateImage(string objectName, Transform parent, Color color)
    {
        GameObject go = new GameObject(objectName, typeof(RectTransform));
        go.transform.SetParent(parent, false);
        Image image = go.AddComponent<Image>();
        image.color = color;
        return image;
    }

    Text CreateText(string objectName, Transform parent, string content, int size, Color color,
                    TextAnchor alignment, FontStyle style)
    {
        GameObject go = new GameObject(objectName, typeof(RectTransform));
        go.transform.SetParent(parent, false);
        Text text = go.AddComponent<Text>();
        text.font = font;
        text.text = content;
        text.fontSize = size;
        text.fontStyle = style;
        text.color = color;
        text.alignment = alignment;
        text.supportRichText = true;
        text.horizontalOverflow = HorizontalWrapMode.Wrap;
        text.verticalOverflow = VerticalWrapMode.Overflow;
        text.raycastTarget = false;
        return text;
    }

    Button CreateButton(string objectName, Transform parent, string label, Color color, UnityEngine.Events.UnityAction onClick)
    {
        Image image = CreateImage(objectName, parent, color);
        Button button = image.gameObject.AddComponent<Button>();

        // Lighter on hover, darker when pressed.
        ColorBlock colors = button.colors;
        colors.normalColor = Color.white;
        colors.highlightedColor = new Color(1.2f, 1.2f, 1.2f, 1f);
        colors.pressedColor = new Color(0.75f, 0.75f, 0.75f, 1f);
        colors.selectedColor = Color.white;
        button.colors = colors;
        button.onClick.AddListener(onClick);

        Text text = CreateText("Label", image.transform, label, 44, textColor, TextAnchor.MiddleCenter, FontStyle.Bold);
        Stretch(text.rectTransform, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
        return button;
    }

    static void Stretch(RectTransform rect, Vector2 anchorMin, Vector2 anchorMax, Vector2 offsetMin, Vector2 offsetMax)
    {
        rect.anchorMin = anchorMin;
        rect.anchorMax = anchorMax;
        rect.offsetMin = offsetMin;
        rect.offsetMax = offsetMax;
    }

    /// <summary>Unity 2022.2+ ships "LegacyRuntime.ttf"; older versions ship "Arial.ttf".</summary>
    static Font LoadBuiltinFont()
    {
        Font builtin = null;
        try { builtin = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf"); } catch { }
        if (builtin == null)
        {
            try { builtin = Resources.GetBuiltinResource<Font>("Arial.ttf"); } catch { }
        }
        return builtin;
    }

    /// <summary>Buttons need an EventSystem in the scene to receive clicks.</summary>
    static void EnsureEventSystem()
    {
        if (Object.FindObjectOfType<EventSystem>() != null)
        {
            return;
        }
        GameObject es = new GameObject("EventSystem");
        es.AddComponent<EventSystem>();
#if ENABLE_INPUT_SYSTEM && !ENABLE_LEGACY_INPUT_MANAGER
        es.AddComponent<InputSystemUIInputModule>();
#else
        es.AddComponent<StandaloneInputModule>();
#endif
    }
}
