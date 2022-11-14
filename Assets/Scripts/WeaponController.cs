using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class WeaponController : MonoBehaviour
{
    [Header("References")]
    public Transform weaponMuzzle;

    [Header("Info")]
    public string weaponName;
    public Sprite icon;

    [Header("General")]
    public LayerMask hittableLayers;
    public GameObject bulletHolePrefab;

    [Header("Shoot Paramaters")]
    public float fireRange = 200;
    public float recoilForce = 4f; //Fuerza de retroceso del arma
    public float fireRate = 0.2f;
    public int maxAmmo = 8;

    [Header("Reload Parameters")]
    public float reloadTime = 1.5f;
    public int currentAmmo { get; private set; }
    private float lastTimeShoot = Mathf.NegativeInfinity;

    [Header("Sounds & Visuals")]
    public GameObject flashEffect;
    private AudioSource audioSource;
    public AudioClip shoot;
    public AudioClip ghost;

    // public GameObject owner { get; set; }
    public GameObject owner;
    public GameObject CorrectPanel;
    public GameObject WrongPanel;

    private void Awake()
    {
        currentAmmo = maxAmmo;
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();        
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            TryShoot();
        }

        // if (Input.GetKeyDown(KeyCode.R))
        // {
        //     StartCoroutine(Reload());
        // }

        transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, Time.deltaTime * 5f);
    }

    private bool TryShoot()
    {
        if (lastTimeShoot + fireRate < Time.time)
        {
            if (currentAmmo >= 1)
            {
                HandleShoot();
                // currentAmmo -= 1;
                return true;
            }
        }

        return false;
    }

    private void HandleShoot()
    {
        audioSource.PlayOneShot(shoot);

        GameObject flashClone = Instantiate(flashEffect, weaponMuzzle.position, Quaternion.Euler(weaponMuzzle.forward), transform);
        Destroy(flashClone, 1f);

        AddRecoil();

        RaycastHit[] hits;
        //hits = Physics.RaycastAll(owner.GetComponent<PlayerController>().playerCamera.transform.position, owner.GetComponent<PlayerController>().playerCamera.transform.forward, fireRange, hittableLayers);
        hits = Physics.RaycastAll(
            // owner.GetComponent<PlayerController>().playerCamera.transform.position,
            gameObject.transform.position,
            // owner.GetComponent<PlayerController>().playerCamera.transform.forward, 
            gameObject.transform.forward,
            fireRange,
            hittableLayers
        );
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.gameObject == owner)
            {
                Debug.Log("Don't try suicide");
                Invoke("ReloadScene", 2f);
            }

            if (hit.collider.gameObject != owner && hit.collider.gameObject.name != "Dashboard")
            {
                string hitName = hit.collider.gameObject.name;
                // if it's a ghost
                if (hitName.All(char.IsDigit))
                {
                    audioSource.PlayOneShot(ghost);
                    Debug.Log("Ghost: " + hitName);
                    // Destroy(hit.collider.gameObject, 0.5f);
                    bool IsCorrectAnswer = hit.collider.gameObject.GetComponent<GhostController>().CheckAnswer();
                    
                    if (IsCorrectAnswer)
                    {
                        // active correct panel
                        CorrectPanel.gameObject.SetActive(true);
                    }
                    else
                    {
                        WrongPanel.gameObject.SetActive(true);
                    }
                    
                    // 
                    // delay of 2 seconds
                    Invoke("ReloadScene", 2f);
                }

                GameObject bulletHoleClone = Instantiate(bulletHolePrefab, hit.point + hit.normal * 0.001f, Quaternion.LookRotation(hit.normal));
                Destroy(bulletHoleClone, 10f);
            }
        }

        lastTimeShoot = Time.time;
    }

    private void AddRecoil()
    {
        transform.Rotate(-recoilForce, 0f, 0f);
        transform.position = transform.position - transform.forward * (recoilForce/30f);
    }

    IEnumerator Reload()
    {
        //TODO emepezar animacion de recarga
        Debug.Log("Recargando...");
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = maxAmmo;
        Debug.Log("Recargada");
        //TODO terminar la animacion
    }
}
