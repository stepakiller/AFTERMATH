using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
public class DictafonController : MonoBehaviour, Equippable
{
    [SerializeField] AudioSource audioSource;
    public AudioClip audioClip;
    [SerializeField] Material targetMaterial;
    [SerializeField] Animator playButtonAnimator;
    [SerializeField] Animator pauseButtonAnimator;
    [SerializeField] Animator[] GearsAnimator;
    [SerializeField] Door targetDoor;
    float onIntensity = 20f;
    float offIntensity = 0f;
    int emissiveColorID;
    bool isListening;
    bool isPaused;
    bool isEquipped;
    [HideInInspector] public bool canOpenDoor = false;
    Color capturedBaseColor = new Color(0.502f, 0.000f, 0.000f, 1.000f);

    void Start()
    {
        GearsAnimator[0].speed = 0;
        GearsAnimator[1].speed = 0;
        Renderer rend = GetComponent<Renderer>();
        if (rend != null)
        {
            emissiveColorID = Shader.PropertyToID("_EmissiveColor");
            targetMaterial.EnableKeyword("_EMISSION");

            UpdateEmissionColor(false);
        }
    }

    void UpdateEmissionColor(bool isActive)
    {
        float intensity = isActive ? onIntensity : offIntensity;
        targetMaterial.SetColor(emissiveColorID, capturedBaseColor * intensity);
    }

    void OnDictaphonePlay()
    {
        if (!isListening)
        {
            OnListen();
            if(canOpenDoor) targetDoor.OpenDoor();
        }
        else if (isPaused) TogglePause();
    }

    void OnDictaphonePause()
    {
        if (isListening) TogglePause();
    }

    void OnListen()
    {
        playButtonAnimator.SetBool("IsPressed", true);
        GearsAnimator[0].speed = 1;
        GearsAnimator[1].speed = 1;
        audioSource.clip = audioClip;
        audioSource.Play();
        isListening = true;
        UpdateEmissionColor(true);
        StartCoroutine(WaitForSoundEnd());
    }

    void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            pauseButtonAnimator.SetBool("IsPressed", true);
            audioSource.Pause();
            SetGearsSpeed(0);
        }
        else
        {
            pauseButtonAnimator.SetBool("IsPressed", false);
            audioSource.UnPause();
            SetGearsSpeed(1);
        }
    }

    IEnumerator WaitForSoundEnd()
    {
        while (audioSource.time < audioSource.clip.length)
        {
            if (!audioSource.isPlaying && !isPaused) break;
            yield return null;
        }

        playButtonAnimator.SetBool("IsPressed", false);
        SetGearsSpeed(0);
        isListening = false;
        isPaused = false;

        UpdateEmissionColor(false);
    }

    void SetGearsSpeed(float speed)
    {
        foreach (var anim in GearsAnimator)
        {
            if (anim != null) anim.speed = speed;
        }
    }

    public void Equip() 
    {
        Debug.Log("игарет");
        isEquipped = true;
        InputManager.Instance.OnDictaphonePlayUsePressed += OnDictaphonePlay;
        InputManager.Instance.OnDictaphonePauseUsePressed += OnDictaphonePause;
    } 

    public void Unequip() 
    {
        Debug.Log("игарет");
        isEquipped = false;
        InputManager.Instance.OnDictaphonePlayUsePressed -= OnDictaphonePlay;
        InputManager.Instance.OnDictaphonePauseUsePressed -= OnDictaphonePause;
    }

    void OnDisable()
    {
        if (isEquipped && InputManager.Instance != null)
        {
            InputManager.Instance.OnDictaphonePlayUsePressed -= OnDictaphonePlay;
            InputManager.Instance.OnDictaphonePauseUsePressed -= OnDictaphonePause;
        }
    }
}