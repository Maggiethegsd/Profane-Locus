using UnityEngine;
using UnityEditor;

public class Flashlight : MonoBehaviour
{
    [Header("Flashlight Properties")]
    //flashlight properties
    [Range(.5f, 25f), SerializeField, Tooltip("Intensity of flashlight.")] float intensity;
    [Range(.25f, 25f), SerializeField,Tooltip("Range of flashlight.")] float range;
    [Range(1f, 165f), SerializeField, Tooltip("Angle of light.")] float spotAngle;  

    [SerializeField, Tooltip("Color of light")] Color lightColor;
    [SerializeField, Tooltip("Cookie texture for light, not mandatory")] Texture2D cookieTexture; 

    [Header("Audio")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip turnOnSound;

    [Header("Input Properties")]
    [SerializeField, Range(0, 2)] int toggleMouseKey;

    Light spotLight;
    Equipment equipment;
    new Renderer renderer;

    [SerializeField] bool isOn = true;

    string materialEmPropertyRef = "EmissionColor";

    void Start()
    {
        GetVars();
    }

    void Update()
    {
        if (!Input.GetMouseButtonDown(toggleMouseKey))
            return;

        if (!equipment.equipped)
            return;

        Toggle();
    }

    void TurnOn()
    {
        isOn = true;

        UpdateLightProperties();
    }

    void TurnOff()
    {
        isOn = false;

        UpdateLightProperties();
    }

    void Toggle()
    {
        isOn = !isOn;
        
        UpdateLightProperties();
    }

    void GetVars()
    {
        //set up un-serialized vars 

        materialEmPropertyRef = "EmissionColor";
        
        if (spotLight == null)
            spotLight = GetComponentInChildren<Light>();

        if (renderer == null)    
            renderer = GetComponentInChildren<Renderer>();
        
        if (equipment == null)
            equipment = this.GetComponent<Equipment>();

        if (audioSource == null)
            audioSource = this.GetComponent<AudioSource>();
    }

    void OnValidate() //whenever a value is changed 
    {
        GetVars();
        SetFlashlightProperties();
        UpdateLightProperties();
    }

    ///<summary>
    /// Expensive method so calling it while the game is playing is not a good idea.
    ///</summary>
    void SetFlashlightProperties() 
    {
        spotLight.intensity = intensity * (isOn ? 1 : 0);
        spotLight.range = range;
        spotLight.spotAngle = spotAngle;

        spotLight.color = lightColor;

        spotLight.cookie = cookieTexture;

        renderer.material.SetColor(materialEmPropertyRef, lightColor * intensity); //set material emmission property
    }

    void UpdateLightProperties()
    {
        renderer.material.SetFloat("_Emission", isOn ? 1 : 0);
        spotLight.gameObject.SetActive(isOn);
    }

    void PlayAudio()
    {
        audioSource.clip = turnOnSound;
        audioSource.Play();
    }
}
