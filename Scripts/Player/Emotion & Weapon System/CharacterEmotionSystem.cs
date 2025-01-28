using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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

    public void ChangeEmotion(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (context.control.name == "rightShoulder" || context.control.name == "q")
            {
                currentEmotionIndex++;
                if (currentEmotionIndex >= emotions.Count) currentEmotionIndex = 0;
            }
            else if (context.control.name == "leftShoulder" || context.control.name == "e")
            {
                currentEmotionIndex--;
                if (currentEmotionIndex < 0) currentEmotionIndex = emotions.Count - 1;
            }

            currentEmotion = emotions[currentEmotionIndex];
        }
    }
}
