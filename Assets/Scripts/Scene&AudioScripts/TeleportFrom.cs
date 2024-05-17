using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TeleportFrom : MonoBehaviour {

    [HideInInspector]
    public string teleportName;
    public UnityEvent onSpawn;

    void Start () {
		if(teleportName == PlayerController.instance.areaTransitionName)
        {
            Debug.Log("teleported");
            PlayerController.instance.transform.position = transform.position;
        }

        ScreenFade.instance.FadeFromBlack();
        GameManager.instance.fadingBetweenAreas = false;

        onSpawn?.Invoke();
	}
}
