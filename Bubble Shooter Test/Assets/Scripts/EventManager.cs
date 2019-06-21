using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    private static EventManager instance = null;
    public static EventManager Instance
    {
        get
        {
            if (!instance)
            {
                instance = FindObjectOfType<EventManager>();
                if (!instance)
                {
                    GameObject newInstance = new GameObject("Event Manager");
                    instance = newInstance.AddComponent<EventManager>();
                }
            }

            return instance;
        }
    }

    public Action<GameObject> OnBubbleHit;

    // Start is called before the first frame update
    private void Awake()
    {
        //if (instance != this)
        //{
        //    Destroy(gameObject);
        //}
    }

    // Update is called once per frame
    void Update()
    {

    }
}
