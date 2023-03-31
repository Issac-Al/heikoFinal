using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCDialogue : MonoBehaviour
{
    public GameObject containerGameObject, dialogueCamera, playerCamera;
    public Text dialogueText;
    public string[] dialogue;
    private int index;
    public float wordSpeed;
    public bool playerIsClose;
    public GameObject playerUI;
    private Transform playerTransform;
    private bool close = false, speaking = false;
    public GameEvent talk, doneTalking;
    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerClose();
        if (Input.GetKeyDown(KeyCode.E) && close && !speaking)
        {
            if(containerGameObject.activeInHierarchy)
            {
                zeroText();
            }else
            {
                //GameObject.FindGameObjectWithTag("Player").GetComponent<Ayako>().enabled = false;
                dialogueCamera.SetActive(true);
                containerGameObject.SetActive(true);
                playerUI.SetActive(false);
                playerCamera.SetActive(false);
                StartCoroutine(Typing());
                talk.Raise();
                speaking = true;

            }
        }
        if (dialogueText.text == dialogue[index])
        {
            if(Input.GetKeyDown(KeyCode.Mouse0))
            {
                NextLine();
            }
        }
    }

    public void zeroText()
    {
        dialogueText.text = "";
        index = 0;
        containerGameObject.SetActive(false);
        playerUI.SetActive(true);
        dialogueCamera.SetActive(false);
        playerCamera.SetActive(true);
        doneTalking.Raise();
        //GameObject.FindGameObjectWithTag("Player").GetComponent<Ayako>().enabled = true;

    }

    public void NextLine()
    {
        if(index < dialogue.Length-1)
        {
            index++;
            dialogueText.text = "";
            StartCoroutine(Typing());
        }else
        {
            zeroText();
        }

    }

    IEnumerator Typing()
    {
        foreach(char letter in dialogue[index].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(wordSpeed);
        }   
    }

    public void Show()
    {
        containerGameObject.SetActive(true);
    }

    public void Hide()
    {
        containerGameObject.SetActive(false);
    }

    public void PlayerClose()
    {
        float distance = Mathf.Abs(Vector3.Distance(gameObject.transform.position, playerTransform.position));
        if (distance < 4)
            close = true;
        else close = false;
    }
}
