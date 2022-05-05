using System.Collections.Generic;
using UnityEngine;

public class LoadPresetPanel : MonoBehaviour
{
    [SerializeField] private GameObject presetItemPrefab;
    [SerializeField] private Transform contentPanel;
    [SerializeField] private float spacing;
    private float _itemHeight;
    public Preset SelectedPreset;

    public void SetPresets(List<Preset> presets)
    {
        ClearContentPanel();
        _itemHeight = presetItemPrefab.GetComponent<RectTransform>().rect.height;

        contentPanel.GetComponent<RectTransform>().sizeDelta =
            new Vector2(0, _itemHeight * presets.Count + spacing * (presets.Count - 1));
        foreach (var p in presets)
        {
            var item = Instantiate(presetItemPrefab, contentPanel);
            item.GetComponent<PresetItem>().SetData(p);
            item.GetComponent<PresetItem>().loadPresetPanel = this;
        }
    }

    private void ClearContentPanel()
    {
        var childCount = contentPanel.childCount;
        for (var i = 0; i < childCount; i++)
        {
            Destroy(contentPanel.GetChild(i).gameObject);
        }
    }

    public void LoadPreset()
    {
        if (SelectedPreset == null)
            return;
        GameManager.instance.LoadPreset(SelectedPreset);
    }

    public void GoBack()
    {
        GameManager.instance.SetConfigForm(true);
        gameObject.SetActive(false);
    }
}