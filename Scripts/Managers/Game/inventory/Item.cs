using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName ="collectable", menuName = "Item")]
public class Item : ScriptableObject
{
    public new string name;
    public int id;
    public string description;
    public string category;
    public Sprite icon;
    public GameObject prefab;

    public Item(string name, string category, string description)
    {
        this.name = name;
        this.category = category;
        this.description = description;
    }
    public Item(int id,string name, string category, string description)
    {
        this.name = name;
        this.category = category;
        this.description = description;
        this.id = id;
    }
    public Item()
    {
        name = "";
        description = "";
        category = "";
        icon = null;
        prefab = null;
        id = 0;
    }
}
