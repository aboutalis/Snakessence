using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    public float DestroyTime = 0.3f;
    private Vector3 offset = new Vector3(0, 2, 0);

    public string[] words = {};

    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Animator>().enabled = true;
        Destroy(gameObject,DestroyTime);

        transform.localPosition += offset;

        int randomIndex = Random.Range(0, words.Length);

        //Generate random tetx from the string[]
        string randomWord = words[randomIndex];
        TextMesh textMesh = gameObject.GetComponent<TextMesh>();
        textMesh.text = randomWord;
    }

}
