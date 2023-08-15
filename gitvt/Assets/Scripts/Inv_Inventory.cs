using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Inv_Inventory : MonoBehaviour
{       
    //��� ������ UI ���������
    [SerializeField] List<Button> buttons = new List<Button>();   
    //��� ������� �� ����� Resources
    [SerializeField] List<GameObject> resourceItems = new List<GameObject>();
    [SerializeField] GameObject buttonsPath;
    //����� ��������, ������� �� �������
    List<string> inventoryItems = new List<string>();
    //�� ��� � ��� � ����
    GameObject itemInArm;
    //����� � ������� ��������� ������� �� ���������
    [SerializeField]Transform itemPoint;
    //��������� ���������(Text)
    [SerializeField] TMP_Text warning;
    //�������� ������ �� �����
    [SerializeField] List<GameObject> playerItems = new List<GameObject>();
    private void Start()
    {
        //�������� ��� ��������� ������� ���������, ������� ����� � ����� Resources
        GameObject[] objArr = Resources.LoadAll<GameObject>("");
        //��������� ������ ��������� ��������� ���������
        resourceItems.AddRange(objArr);
        //���������� ��� ������ ��������� �� ����� � ����� �� � ������
        foreach(Transform child in buttonsPath.transform)
        {
            buttons.Add(child.GetComponent<Button>());
        }
    }
    
    public void AddItem(Sprite img, string itemName, GameObject obj)
    {        
        //���� � ��� ������ ���������, �� ������� �� ���� ��������� � ��������� ������
        if (inventoryItems.Count >= buttons.Count)
        {
            warning.text = "Full Inventory!";
            Invoke("WarningUpdate", 1f);
            return;
        }
        //���� � ��������� ��� ���� ����� �������, �� ������� �� ���� ��������� � ��������� ������
        if (inventoryItems.Contains(itemName))
        {
            warning.text = "You already have " + itemName;
            Invoke("WarningUpdate", 1f);
            return;
        }
        //��������� ���� �������� � ���������
        inventoryItems.Add(itemName);
        //�������� ��������� ��������� ������ � � ��������� Image
        var buttonImage = buttons[inventoryItems.Count - 1].GetComponent<Image>();
        //���������� � ������ �������� ��������, ������� �������
        buttonImage.sprite = img;
        //���������� ������, ������� ���������
        Destroy(obj);
    }
    //�����, ������� ������� ��� ���������
    void WarningUpdate()
    {
        warning.text = "";
    }
    //���� ����� ���������� �� ������� ������
    public void UseItem(int itemPos)
    {           
        //���� �� ������ ������, � ������� ������ ���, �� ��������� ������
        if (inventoryItems.Count <= itemPos) return;
        //���������� ��� �������, ������� �������� ���� ������ � ����������
        string item = inventoryItems[itemPos];
        //�������� ����� ������ ������� �� ��������� � �������� ��� �������, ������� ����� �����
        GetItemFromInventory(item);
    }
    //����� ������ �������
    public void GetItemFromInventory(string itemName)
    {
        //���������� ��� ������� � ����� Resources
        foreach (var resourceItem in resourceItems)
        {
            //���� ��� ������� ������� � ���, ������� �� ����� �����,��
            if (resourceItem.name == itemName)
            {
                //������� ���������� � ������ � �� ������ ��� �������� ��������� � ���, ��� ��� �����. (���� ������ ������� ���, �� ���������� ������ �������� null)
                GameObject putFind = playerItems.Find(x => x.name == itemName);
                //���� �� ����� ������ ��� �� ��������� �� ���������, ��
                if (putFind == null)
                {
                    if(itemInArm != null)
                    {
                        itemInArm.SetActive(false);
                    }
                    //������� ���� ������ �� ����� Resources �� ����� � ����� itemPoint(� ������ ������ ��� ����� �� ������ ����)
                    var newItem = Instantiate(resourceItem.gameObject, itemPoint);
                    //���������� ���� ������ � �������� ������
                    newItem.transform.parent = itemPoint;
                    //���� ��� ��� 
                    newItem.name = itemName;
                    //��������� � ������ ��������� ������ ���� ������
                    playerItems.Add(newItem);
                    //������ ������� �����, ��� ���������� itemInArm = ����� �������(�� ���� ���������� ��, ��� � ��� ������ ����� �� ���������)
                    itemInArm = newItem;
                }
                //����� ���� �������, ������� �� ����� ������� �� �������� ��� ���� �������, ��..
                else if (putFind.name == itemInArm.name)
                {
                    //�������� ��� ��� ���������
                    putFind.SetActive(!putFind.activeSelf);
                }
                //� ��������� �������, ���� �������, ������� �� ����� ������� ��� ��� ��������� �� ����� � ���� �� �� ��������� � ���, ��� � ��� ����� ������, ��..
                else
                {
                    //��������� �������, ������� � ��� ������ � �����
                    itemInArm.SetActive(false);
                    //�������� �������, ������� ����� �����
                    putFind.SetActive(true);
                    //������ ������� �����, ��� ���������� itemInArm = ����� �������(�� ���� ���������� ��, ��� � ��� ������ ����� �� ���������)
                    itemInArm = putFind;
                }
            }
        }
    }
}
