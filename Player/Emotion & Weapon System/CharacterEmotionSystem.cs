using System.Collections.Generic;
using UnityEngine;

public class CharacterEmotionSystem : MonoBehaviour
{
    [Header("Emotions")]
    [SerializeField] List<CharacterEmotion> emotions;
    public CharacterEmotion currentEmotion; 
    private int currentEmotionIndex = 0; 

    void Start()
    {
        if (emotions.Count > 0)
        {
            currentEmotion = emotions[currentEmotionIndex];
        }
    }

    void Update()
    {
        ChangeEmotion();
    }

    private void ChangeEmotion()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            currentEmotionIndex--;

            if (currentEmotionIndex < 0)
            {
                currentEmotionIndex = emotions.Count - 1; 
            }

            currentEmotion = emotions[currentEmotionIndex];
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            currentEmotionIndex++;

            if (currentEmotionIndex >= emotions.Count)
            {
                currentEmotionIndex = 0; 
            }

            currentEmotion = emotions[currentEmotionIndex];
        }
    }
}
