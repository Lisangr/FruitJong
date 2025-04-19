using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
//using TMPro;

public class ReplaceTMPWithLegacyText : EditorWindow
{
    [MenuItem("Tools/Replace TMP → Legacy Text")]
    public static void ShowWindow()
    {
        GetWindow<ReplaceTMPWithLegacyText>("Replace TMP");
    }

    private void OnGUI()
    {
        GUILayout.Label("Заменить все TextMeshProUGUI на Legacy Text в текущих сценах", EditorStyles.wordWrappedLabel);
        GUILayout.Space(10);
        if (GUILayout.Button("Запустить замену"))
        {
            ReplaceAllTMP();
        }
    }

    private static void ReplaceAllTMP()
    {
        // Находим все TMP-компоненты, включая неактивные объекты
        //когда поставишь ТМПро раскоменнтируй этой
        //var allTMP = Resources.FindObjectsOfTypeAll<TextMeshProUGUI>();
       
        var allTMP = Resources.FindObjectsOfTypeAll<Text>();
        int count = 0;

        foreach (var tmp in allTMP)
        {
            // Убедимся, что это не prefab из-пакета и находится в сцене
            if (EditorUtility.IsPersistent(tmp) || tmp.gameObject.scene.name == null)
                continue;

            GameObject go = tmp.gameObject;
            string txt = tmp.text;

            // Удаляем Layout- и прочие привязанные к TMP компоненты, если нужно
            // (по желанию можно расширить логику копирования других свойств)

            // Удаляем сам TMP-компонент
            Undo.RecordObject(go, "Remove TMP");
            Object.DestroyImmediate(tmp, true);

            // Добавляем Legacy Text
            Undo.AddComponent<Text>(go);
            var legacyText = go.GetComponent<Text>();
            legacyText.text = txt;

            // По желанию: задать шрифт по умолчанию
            Font defaultFont = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            legacyText.font = defaultFont;

            count++;
        }

        Debug.Log($"Заменено компонентов TextMeshProUGUI: {count}");
        EditorUtility.DisplayDialog("Готово", $"Заменено TextMeshProUGUI → Legacy Text: {count}", "OK");
    }
}
