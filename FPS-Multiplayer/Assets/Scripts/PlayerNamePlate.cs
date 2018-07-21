using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNamePlate : MonoBehaviour {

    [SerializeField]
    private RectTransform healthbarfill;

    [SerializeField]
    private Text playername;

    [SerializeField]
    private Player player;

    private void Start()
    {
    }

    private void Update()
    {
        healthbarfill.localScale = new Vector3(1f, player.GetHealthPct(), 1f);
    }
}
