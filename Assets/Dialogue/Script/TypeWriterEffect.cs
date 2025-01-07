using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TypeWriterEffect : MonoBehaviour
{
    [SerializeField] private float typeWriterSpeed = 50f;
    
    public bool isRunning { get; private set; }

    private readonly List<Punctuation> punctuations = new ()
    {
        new Punctuation(new HashSet<char>(){'.', '!', '&'}, 0.6f),
        new Punctuation(new HashSet<char>(){',', ';', ':'}, 0.3f)
    };

    public Coroutine typingCorutine;
    
    public Coroutine Run(string textToType, TMP_Text textLabel)
    {
        typingCorutine = StartCoroutine(TypeText(textToType, textLabel));
        return typingCorutine;
    }

    public void Stop()
    {
        StopCoroutine(typingCorutine);
        isRunning = false;
    }

    private IEnumerator TypeText(string textToType, TMP_Text textLabel)
    {
        isRunning = true;
        textLabel.text = string.Empty;

        float t = 0;
        int charIndex = 0;
        while (charIndex < textToType.Length)
        {
            int lastCharIndex = charIndex;
            
            t += Time.deltaTime * typeWriterSpeed;
            
            charIndex = Mathf.FloorToInt(t);
            charIndex = Mathf.Clamp(charIndex, 0, textToType.Length);

            for (int i = lastCharIndex; i < charIndex; i++)
            {
                bool isLast = i >= textToType.Length - 1;
                textLabel.text = textToType.Substring(0, i + 1);
                if (isPunct(textToType[i], out float waitTime) && !isLast && !isPunct(textToType[i+1], out _))
                {
                    yield return new WaitForSeconds(waitTime);
                }
            }
            
            yield return null;
            if (Input.GetKeyDown(KeyCode.Space)) //!!!!!!!!!!!!!!!!!!!!
            {
                yield break;
            }
        }
        isRunning = false;
    }

    private bool isPunct(char character, out float waitTime)
    {
        foreach (Punctuation punctCategory in punctuations)
        {
            if (punctCategory.Punctuations.Contains(character))
            {
                waitTime = punctCategory.WaitTime;
                return true;   
            }
        }

        waitTime = default;
        return false;
    }

    private readonly struct Punctuation
    {
        public readonly HashSet<char> Punctuations;
        public readonly float WaitTime;

        public Punctuation(HashSet<char> punctuations, float waitTime)
        {
            Punctuations = punctuations;
            WaitTime = waitTime;
        }
    }
}
