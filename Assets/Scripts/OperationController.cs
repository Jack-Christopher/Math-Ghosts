using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class OperationController : MonoBehaviour
{
    [Header("General")]
    public TMP_Text operationContent;
    public TMP_Text scoreContent;
    public enum OperationDifficulty { Easy, Medium, Hard, Impossible };
    public OperationDifficulty difficulty;
    public int Result;

    public List<GameObject> ghosts;
    public GameObject ghostPrefab;

    public static int score = 0;
    public static int remainingGhosts = 11;


    public bool IsCorrectAnswer() 
    {
        score++;
        Debug.Log("Correcto, Score: " + score);
        scoreContent.text = "Puntaje: " + score.ToString();
        return true;        
    }

    private void GenerateOperation()
    {
        // list of operands
        List<int> operands = new List<int>();
        List<string> operators = new List<string>();

        // change the dificulty of the operation based on the remaining ghosts
        // if (difficulty == OperationDifficulty.Easy)
        if (remainingGhosts > 5)
        {
            operands.Add(Random.Range(1, 10));
            operands.Add(Random.Range(1, 10));
            
            operators.Add(" + ");
            Debug.Log("Difficulty is Easy");
        }
        // else if (difficulty == OperationDifficulty.Medium)
        else if (remainingGhosts > 3)
        {
            operands.Add(Random.Range(2, 10));
            operands.Add(Random.Range(2, 5));

            operators.Add(Random.Range(0, 2) == 0 ? " * " : " - ");
            Debug.Log("Difficulty is Medium");
        }
        // else if (difficulty == OperationDifficulty.Hard)
        else if (remainingGhosts > 1)
        {
            operands.Add(Random.Range(2, 5));
            operands.Add(Random.Range(1, 10));
            operands.Add(Random.Range(1, 10));
            
            operators.Add(" * ");
            operators.Add(Random.Range(0, 2) == 0 ? " + " : " - ");
            Debug.Log("Difficulty is Hard");
        }
        // else if (difficulty == OperationDifficulty.Impossible)
        else if (remainingGhosts > 0)
        {
            operands.Add(Random.Range(1, 10));
            operands.Add(Random.Range(1, 10));
            operands.Add(Random.Range(1, 10));
            operands.Add(Random.Range(1, 10));
            
            operators.Add(Random.Range(0, 2) == 0 ? " * " : " * ");
            operators.Add(Random.Range(0, 2) == 0 ? " + " : " + ");
            operators.Add(Random.Range(0, 2) == 0 ? " + " : " - ");
            Debug.Log("Difficulty is Impossible");
        }

        string operation = "";
        for (int i = 0; i < operands.Count; i++)
        {
            operation += operands[i].ToString();
            if (i < operators.Count)
            {
                operation += operators[i];
            }
        }

        operationContent.text = operation + "= ?";
        Debug.Log("Operation Generated: " + operation);

        // set Result
        Result = 0;
        for (int i = 0; i < operands.Count; i++)
        {
            if (i == 0)
            {
                Result = operands[i];
            }
            else
            {
                if (operators[i - 1] == " + ")
                {
                    Result += operands[i];
                }
                else if (operators[i - 1] == " - ")
                {
                    Result -= operands[i];
                }
                else if (operators[i - 1] == " * ")
                {
                    Result *= operands[i];
                }
                else if (operators[i - 1] == " / ")
                {
                    Result /= operands[i];
                }
            }
        }
    }

    void Start()
    {
        TMP_Text operationContent = GetComponent<TMP_Text>();
        TMP_Text scoreContent = GetComponent<TMP_Text>();
        scoreContent.text = "Puntaje: " + score.ToString();
        GenerateOperation();
        GenerateGhosts();
    }


    void GenerateGhosts()
    {
        remainingGhosts--;
        Debug.Log("N° of ghosts: " + remainingGhosts);
        ghosts = new List<GameObject>();
        // Debug.Log("Generating ghosts");

        for (int i = 0; i < remainingGhosts; i++)
        {
            int x = Random.Range(-40, 40);
            int z = Random.Range(-40, 40);
            GameObject obj = Instantiate(ghostPrefab) as GameObject;
            obj.transform.position = new Vector3(x, 0, z);
            obj.transform.localScale = new Vector3(3, 3, 3);
            // obj.name = i.ToString();
            if (i == 0) 
            {   
                obj.GetComponent<GhostController>().SetText(Result.ToString());
                obj.GetComponent<GhostController>().SetIsCorrect(true);
            }
            else 
            {  
                int otherResult = Result + Random.Range(-15, 15);
                if (otherResult == Result) { otherResult += 1; }
                obj.GetComponent<GhostController>().SetText( otherResult.ToString() );
                obj.GetComponent<GhostController>().SetIsCorrect(false);
            }
            obj.name = obj.GetComponent<GhostController>().GetText();
            obj.GetComponent<GhostController>().IsCorrect = IsCorrectAnswer;
            obj.GetComponent<GhostController>().correctAnswer = Result;

            // Debug.Log("'" + obj.name + "' created at " + obj.transform.position);
            ghosts.Add(obj);
        }

        // if no remaining ghosts, load menu scene
        if (remainingGhosts == 0)
        {
            Debug.Log("No more ghosts");
            SceneManager.LoadScene("Menu");
        }
    }

    public void Restart()
    {
        Debug.Log("Restarting game");
        GenerateOperation();

        // destroy ghosts in 2 seconds
        for( int i = 0; i < ghosts.Count; i++)
        {
            Destroy(ghosts[i], 2);
            // pop ghost from list
            ghosts.RemoveAt(i);          
        }
        GenerateGhosts();
    }

    void Update()
    {
        // Debug.Log("score: " + score);
    }
}
