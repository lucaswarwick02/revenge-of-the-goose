using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalCommentController : MonoBehaviour
{
    private AnimalCommenter[] animalCommenters;

    private Dictionary<int, List<string>> commentsDict = new Dictionary<int, List<string>>{
        {1, new List<string>{"Thank you!", "Our Saviour!"}}
    };
    private List<string> currentComments;

    private void Awake()
    {
        GameHandler.OnMapAreaChanged += GetListOfAnimalCommenters;
        PlaythroughStats.OnAnimalKilled += OnAnimalKilled;
        PlaythroughStats.OnEnemyKilled += OnEnemyKilled;
    }

    private void GetListOfAnimalCommenters()
    {
        animalCommenters = FindObjectsOfType<AnimalCommenter>();
    }

    private void OnAnimalKilled(int totalAnimalsKilled, Vector3 killEventPosition)
    {
        List<string> attemptedComments;
        if (commentsDict.TryGetValue(totalAnimalsKilled, out attemptedComments)) {
            currentComments = attemptedComments;
        }

        foreach (AnimalCommenter animalCommenter in animalCommenters)
        {
            if (animalCommenter.enabled)
            {
                string randomComment = currentComments[Random.Range(0, currentComments.Count)];
                animalCommenter.Comment(randomComment);
            }
        }
    }

    private void OnEnemyKilled(int totalEnemiesKilled, Vector3 killEventPosition)
    {
        List<string> attemptedComments;
        if (commentsDict.TryGetValue(totalEnemiesKilled, out attemptedComments))
        {
            currentComments = attemptedComments;
        }

        foreach (AnimalCommenter animalCommenter in animalCommenters)
        {
            if (animalCommenter.enabled)
            {
                string randomComment = currentComments[Random.Range(0, currentComments.Count)];
                animalCommenter.Comment(randomComment);
            }
        }
    }
}
