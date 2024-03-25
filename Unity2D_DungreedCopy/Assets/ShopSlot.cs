using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ShopSlot : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler, IPointerClickHandler
{
    [Header("������SO")]
    [SerializeField]
    private ItemSO              item;
    [SerializeField]
    private InventorySO         inventory;

    [Header("UI���")]
    private Image               baseImage;
    [SerializeField]
    private Sprite[]            baseSprites;
    [SerializeField]
    private Image               itemImage;
    [SerializeField]
    private TextMeshProUGUI     textName;
    [SerializeField]
    private TextMeshProUGUI     textGold;

    private GameObject          discriptionUI;

    private void Awake()
    {
        baseImage   = GetComponent<Image>();

        discriptionUI = GameObject.Find("Canvas/InventoryUI/InventoryDescription");
    }
    private void Start()
    {
        itemImage.sprite = item.ItemImage;
        itemImage.SetNativeSize();
        textName.text = item.Name;
        textGold.text = item.Gold.ToString();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        discriptionUI.SetActive(true);
        baseImage.sprite = baseSprites[1];
        discriptionUI.GetComponent<UIInventoryDescription>().SetDescription(itemImage.sprite, textName.text, item.Description);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        discriptionUI.SetActive(false);
        baseImage.sprite = baseSprites[0];
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // ���콺 ��Ŭ����
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            // ���� ����
            if(PlayerStats.instance.GOLD >= item.Gold)
            {
                Debug.Log("����!");
                inventory.AddItem(item, 1);
                PlayerStats.instance.gold -= item.Gold;
                this.gameObject.SetActive(false);
            }
            // ��� ����
            else
            {
                Debug.Log("���� �Ұ�");
                StartCoroutine(DeactivateTextNoGold());
            }
        }
    }

    private IEnumerator DeactivateTextNoGold()
    {
        UIManager.instance.UpdateTextNoGold(true);
        yield return new WaitForSeconds(1.5f);
        UIManager.instance.UpdateTextNoGold(false);
    }
}