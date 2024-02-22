using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ItemChoice : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _nameItem;
    [SerializeField]
    private TextMeshProUGUI _textItem;
    [SerializeField]
    private Image _imageItem;
    [SerializeField] 
    private Button _buttonItem;

    public void SetupItem(string name, string text, Sprite sprite)
    {
        _nameItem.text = name;
        _textItem.text = text;
        _imageItem.sprite = sprite;
    }

    public void AddListenerButton(UnityAction callback)
    {
        _buttonItem.onClick.RemoveAllListeners();
        _buttonItem.onClick.AddListener(callback);
    }
}
