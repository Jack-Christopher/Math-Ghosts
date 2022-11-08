using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GhostController : MonoBehaviour
{
    private CharacterController ghost;
    public TMP_Text resultContent;
    private GameObject player;
    // public IEnumerator<GameObject> enumerator;
    public int correctAnswer;
    public Func<bool> IsCorrect;

    public void CheckAnswer()
    {
        int result = int.Parse(resultContent.text);
        if (result == correctAnswer)
        {
            IsCorrect();
            Debug.Log("CA: Correct");
            // Destroy(gameObject);
        }
        else
        {
            Debug.Log("CA: Incorrect");
        }        
    }

    void Start()
    {
        ghost = GetComponent<CharacterController>();        
        player = GameObject.Find("Player");

        if (ghost == null)
        {
            Debug.LogError("GhostController: No se ha encontrado el CharacterController del fantasma");
        }
        if (player == null)
        {
            Debug.LogError("GhostController: No se ha encontrado el CharacterController del jugador");
        }

    }

    // Update is called once per frame
    void Update()
    {
        // move towards player
        Vector3 direction = player.transform.position - transform.position;
        Vector3 velocity = direction * 5f;
        // velocity.y = 0f;
        velocity.Normalize();

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(velocity), 0.1f);

        ghost.Move(velocity * Time.deltaTime); 

        // keep y in 3
        transform.position = new Vector3(transform.position.x, 3, transform.position.z);
    }

    public void SetText(string text)
    {
        resultContent.text = text;
    }

    public string GetText()
    {
        return resultContent.text;
    }

    public void shout()
    {

    }
}
