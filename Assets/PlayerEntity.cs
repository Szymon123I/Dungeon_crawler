using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerEntity : Entity
{
    public Canvas UIcanvas;
    public static PlayerEntity Instance {get; private set;}
    [HideInInspector]
    public HealthScript healthScript;
    [HideInInspector]
    public InputScript inputScript;
    [HideInInspector]
    public MovementScript movementScript;
    private Animator animator;
    private Slider healthSlider;
    void Awake()
    {
        animator = GetComponent<Animator>();
        Instance = this;
    }

    void onHit(){
        CinemachineShake.Instance.ShakeCamera(2,0.1f);
        UpdateUI();
    }

    void Start(){
        healthScript = GetComponent<HealthScript>();
        inputScript = GetComponent<InputScript>();
        movementScript = GetComponent<MovementScript>(); 
        healthScript.onHit += onHit;
        healthSlider = UIcanvas.transform.Find("HealthBar").GetComponent<Slider>();
    }
    private TMP_Text textMesh;
    void UpdateUI(){
        
        healthSlider.value = Mathf.Clamp01(healthScript.health/healthScript.maxHealth);
        if (textMesh == null) textMesh = healthSlider.transform.Find("HealthText").GetComponent<TMP_Text>();
        textMesh.text = $"{Mathf.Round(healthScript.health)} HP";
    }

    void Update()
    {
        animator.SetBool("walking", inputScript.isWalking);
        UpdateUI();
    }
}
