using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialMenuSpawner : MonoBehaviour
{
    public RadialMenuScript menuPrefab;
    public static RadialMenuSpawner mSpawner;

    void Awake()
    {
        mSpawner = this;
    }
    public void SpawnMenu(ChessScript obj)
    {
        //Debug.Log("spawn menu");
        RadialMenuScript newMenu = Instantiate(menuPrefab) as RadialMenuScript;
        newMenu.transform.SetParent(transform, false);

        //Move menu to obj position
        newMenu.transform.position = Camera.main.WorldToScreenPoint(obj.transform.position);
        newMenu.SpawnButtons(obj);
        newMenu.label.text = obj.title.ToUpper();
    }
    public void SpawnSummonMenu(int ID)
    {
        RadialMenuScript newMenu = Instantiate(menuPrefab) as RadialMenuScript;
        newMenu.transform.SetParent(transform, false);

        //Move menu to ID position
        newMenu.transform.position = Camera.main.WorldToScreenPoint(GridManager.GetPosbyID(ID));
        newMenu.SpawnOption(ID);

}
}


