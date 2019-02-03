using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MalbersAnimations;
using UnityEngine.UI;

public class ConversationScript : MonoBehaviour
{
    public conversation[] conversations; // 전체 대화내용
    public bool isConverseDisposable = false; // 한번 대화가 진행되면 또 대화할 수 있는지;

    private SphereCollider sphereCollider; // 대화 대상감지
    private MalbersInput minput; // 대화중 입력제어
    private Animal animal; // 대화중 감정표현
    private Text text; // 대화 내용
    private bool isOnConver = false; // 대화중인지 나타내는 bool변수
    private GameObject textBox; // 대화창 부모 오브젝트


    [System.Serializable]
    public class conversation
    {
        public string speaker;
        [TextArea]
        public string content;
        public string GetSentence()
        {
            return speaker +" : " + content;
        }
    }
    private void Start()
    {
        sphereCollider = GetComponent<SphereCollider>();
        minput = GameObject.FindGameObjectWithTag("Player").GetComponent<MalbersInput>();
        animal = GameObject.FindGameObjectWithTag("Player").GetComponent<Animal>();
        text = GameObject.Find("Conversation Box").GetComponentInChildren<Text>();
        textBox = GameObject.Find("Conversation Box");

        textBox.SetActive(false);
    }
    IEnumerator Conversation()
    {
        isOnConver = true;
        EnableControlDuringConver(false);
        animal.Move(new Vector3(0f, 0f, 0f));
        int i = 0;
        text.text = conversations[i].GetSentence();
        i++;
        while (i != conversations.GetLength(0) + 1)
        {
            if(Input.GetMouseButtonDown(0))
            {
                if (i == conversations.GetLength(0))
                {
                    i++;
                }
                else
                {
                    text.text = conversations[i].GetSentence();
                    i++;
                }
            }
            
            yield return null;
        }
        EnableControlDuringConver(true);

    }
    private void EnableControlDuringConver(bool active)
    {
        minput.EnableInput("Jump", active);
        minput.EnableInput("Shift", active);
        minput.EnableInput("Attack1", active);
        minput.EnableInput("Action", active);
        minput.EnableMovement(active);
        textBox.SetActive(!active);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(!isOnConver) StartCoroutine("Conversation");
    }
    private void OnTriggerExit(Collider other)
    {
        
        isOnConver = false;
        if(isConverseDisposable)
        {
            print("checktrigger");
            Destroy(sphereCollider);
        }
    }
}
