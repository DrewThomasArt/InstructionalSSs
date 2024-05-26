using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public static Shop instance;
    public Item selectedItem;
    // Start is called before the first frame update
    void Start()
    {
    instance = this;   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
