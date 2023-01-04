using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalCommentController : MonoBehaviour
{
    private AnimalCommenter[] animalCommenters;

    private void Awake()
    {
        GameHandler.OnSceneUpdated += GetListOfAnimalCommenters;
        PlaythroughStats.OnAnimalKilled += OnAnimalKilled;
    }

    private void GetListOfAnimalCommenters()
    {
        animalCommenters = FindObjectsOfType<AnimalCommenter>();
    }

    private void OnAnimalKilled(int totalAnimalsKilled, Vector3 killEventPosition)
    {
        foreach (AnimalCommenter animalCommenter in animalCommenters)
        {
            if (animalCommenter.enabled)
            {
                animalCommenter.Comment("You're killing us all!");
            }
        }
    }
}
