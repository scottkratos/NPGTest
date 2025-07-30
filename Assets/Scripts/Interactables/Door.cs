using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : InteractiveBase
{
    [Header("Door setup")]
    [SerializeField] private string levelToLoad;
    [SerializeField] private string key;

    [Header("References")]
    [SerializeField] private Transform playerPosition;
    [SerializeField] private GameObject[] doorGameObjects;
    [SerializeField] private Vector3[] initialPositions;
    [SerializeField] private Vector3[] finalPositions;

    public override void Use(PlayerController player)
    {
        if (key != string.Empty) return;
        base.Use(player);
        player.isInLevelTransition = true;
        StartCoroutine(MoveDoor(true));
    }

    protected override void Start()
    {
        base.Start();
        if (GameManager.instance.doorUUID == UUID)
        {
            PlayerController.instance.gameObject.transform.position = playerPosition.position;
            PlayerController.instance.gameObject.transform.rotation = transform.rotation;
            PlayerController.instance.isInLevelTransition = false;
            StartCoroutine(MoveDoor(false));
        }
    }

    private IEnumerator MoveDoor(bool open)
    {
        float timer = .5f;
        float localTimer = 0;

        while (localTimer < timer)
        {
            for (int i = 0; i < doorGameObjects.Length; i++)
            {
                doorGameObjects[i].transform.localPosition = open ? Vector3.Lerp(initialPositions[i], finalPositions[i], localTimer / timer) : Vector3.Lerp(finalPositions[i], initialPositions[i], localTimer / timer);
            }
            localTimer += Time.deltaTime;
            yield return null;
        }

        for (int i = 0; i < doorGameObjects.Length; i++)
        {
            doorGameObjects[i].transform.localPosition = open ? finalPositions[i] : initialPositions[i];
        }

        if (open) GameManager.instance.LoadLevel(levelToLoad, UUID);
        else PlayerController.instance.isInLevelTransition = false;
    }
}
