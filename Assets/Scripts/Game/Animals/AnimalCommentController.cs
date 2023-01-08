using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalCommentController : MonoBehaviour
{
    public static List<AnimalCommenter> animalCommenters = new List<AnimalCommenter>();

    private string[] animalKilledComments = {"No!", "Don't kill us!", "Please stop!", "Ahhh!"};
    private string[] enemyKilledComments = {"Yes!"};
 
    private void Awake()
    {
        PlaythroughStats.OnAnimalKilled += OnAnimalKilled;
        PlaythroughStats.OnEnemyKilled += OnEnemyKilled;
    }

    private void OnAnimalKilled(int totalAnimalsKilled, Vector3 killEventPosition)
    {
        // Remove where animal commenter is > 15 away
        List<AnimalCommenter> closeAnimalCommenters = animalCommenters.FindAll(
            animalCommenter => Vector3.Magnitude(animalCommenter.gameObject.transform.position - GameObject.FindGameObjectWithTag("Player").transform.position) < 15);

        foreach (AnimalCommenter animalCommenter in closeAnimalCommenters) {
            // Pick random comment
            animalCommenter.Comment(animalKilledComments[Random.Range(0, animalKilledComments.Length)]);
        }
    }

    private void OnEnemyKilled(int totalEnemiesKilled, Vector3 killEventPosition)
    {
        // Remove where animal commenter is > 15 away
        List<AnimalCommenter> closeAnimalCommenters = animalCommenters.FindAll(
            animalCommenter => Vector3.Magnitude(animalCommenter.gameObject.transform.position - GameObject.FindGameObjectWithTag("Player").transform.position) < 15);

        foreach (AnimalCommenter animalCommenter in closeAnimalCommenters) {
            // Pick random comment
            animalCommenter.Comment(enemyKilledComments[Random.Range(0, enemyKilledComments.Length)]);
        }
    }
}
