using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GunManager : MonoBehaviour
{
    public Gun equippedGun;
    public Camera cam;

    float currentTime;
    float lineLifetime;
    float delayBetweenShots;
    Animator animator;
    LineRenderer line;
    GameObject instantiatedGun;
    bool isReloading;

    public Transform gunHolder;

    public TextMeshProUGUI ammoCounter;
    int currentAmmo;

    public float swayAmount;
    public float smooth;

    Vector3 initialPosition;
    public float bobIntensity;
    public float bobSpeed;
    float sinAngle;

    void Start()
    {
        EquipGun();
    }

    private void EquipGun()
    {
        if (equippedGun != null)
        {
            // Convert default position to world position
            Quaternion fixedQuaternion = cam.transform.rotation * Quaternion.Euler(equippedGun.defaultRotation);
            Vector3 fixedPosition = cam.transform.TransformPoint(equippedGun.defaultPosition);


            // Get gun holder transform and attach gun to it
            instantiatedGun = Instantiate(equippedGun.gunPrefab, fixedPosition, fixedQuaternion, gunHolder);

            // Set the Initial Time and Current Time equal
            delayBetweenShots = equippedGun.fireRate;
            currentTime = delayBetweenShots;
            animator = instantiatedGun.GetComponent<Animator>();
            line = instantiatedGun.GetComponent<LineRenderer>();

            // Ammo
            currentAmmo = equippedGun.magazineSize;
            ammoCounter.enabled = true;
            UpdateAmmoCounter();

            // Initialize viewbob
            initialPosition = gunHolder.transform.localPosition;
        }
    }

    private void UpdateAmmoCounter()
    {
        if (equippedGun != null)
        {
            ammoCounter.text = $"{currentAmmo}/{equippedGun.magazineSize}";
        }
        else
        {
            ammoCounter.enabled = false;
        }

    }

    void Update()
    {
        // None of this matters if no gun
        if (equippedGun == null) return;

        // Increase current time per frame
        if (currentTime < delayBetweenShots)
        {
            currentTime += Time.deltaTime;
        }
        // Disable Line After a while

        if (lineLifetime > 0)
        {
            lineLifetime -= Time.deltaTime;
        }
        else
        {
            line.enabled = false;
        }

        // Check if currently reloading
        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Reload"))
        {
            isReloading = true;

        }
        if (isReloading)
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsTag("Reload"))
            {
                currentAmmo = equippedGun.magazineSize;
                UpdateAmmoCounter();
                isReloading = false;
            }
        }

    }

    public void BobWeapon(Vector2 input)
    {
        if (equippedGun == null)
            return;

        if (input == Vector2.zero)
        {
            sinAngle = 0;
            gunHolder.transform.localPosition = initialPosition;
            return;
        }


        sinAngle += Time.deltaTime * bobSpeed;
        gunHolder.transform.localPosition = new Vector3(
            gunHolder.transform.localPosition.x,
            initialPosition.y + bobIntensity * Mathf.Abs(Mathf.Sin(sinAngle)),
            initialPosition.z + bobIntensity * Mathf.Cos(sinAngle)
            );
    }

    public void FireGun(bool wasPressedThisFrame)
    {
        // Accepts input from inputmanager and shoots held gun

        // Check if gun is equipped:
        if (equippedGun == null)
            return;

        // Check if reloading
        if (isReloading)
            return;

        // Check if semiAutomatic and pressed this frame else return
        if (equippedGun.fireMode == Gun.FireModes.semiAutomatic)
        {
            if (!wasPressedThisFrame)
                return;
        }

        // Check if sufficient ammo
        if (currentAmmo <= 0)
            return;

        // Fire Ray from camera if its time
        if (currentTime >= delayBetweenShots)
        {
            // Pre-Shot: All this matters even if nothing is hit

            // Shoot Ray
            currentTime = 0;
            Ray gunRay = new Ray(cam.transform.position, cam.transform.forward);
            Physics.Raycast(gunRay, out RaycastHit hitInfo, equippedGun.range);

            // Subtract ammunition
            currentAmmo -= 1;
            UpdateAmmoCounter();

            // Visuals
            animator.SetTrigger("Shoot");
            instantiatedGun.GetComponent<ParticleSystem>().Play();
            Vector3 particlePos = instantiatedGun.GetComponent<ParticleSystem>().shape.position;
            // Line
            if (hitInfo.point != Vector3.zero)
            {
                Vector3 lineOrigin = instantiatedGun.transform.position;
                line.enabled = true;
                line.startColor = new Color(1, 1, 1, 0.05f);
                lineLifetime = 0.02f;
                line.widthMultiplier = 0.05f;
                line.SetPosition(0, lineOrigin);
                line.SetPosition(1, hitInfo.point);
                //Debug.Log("lineOrigin: " + lineOrigin.ToString() + "hitPos: " + hitInfo.point.ToString());
            }

            //Post-Shot: All this only matters if something is hit
            if (hitInfo.collider == null)
                return;

            // Do Damage if Damageable
            Damageable damageable = hitInfo.collider.GetComponent<Damageable>();
            if (damageable != null && damageable.isDamageable)
            {
                damageable.DoDamage(equippedGun.damage);
            }

        }

    }
    public void Reload()
    {
        if (equippedGun == null)
            return;

        if (isReloading) return;
        if (currentAmmo == equippedGun.magazineSize) return;

        if (equippedGun != null)
        {
            animator.SetTrigger("Reload");
        }
    }
    public void WeaponSway(Vector2 input)
    {
        if (equippedGun == null)
            return;

        float mouseY = input.y * swayAmount;
        float mouseX = input.x * swayAmount;

        Quaternion rotationX = Quaternion.AngleAxis(-mouseY, Vector3.right);
        Quaternion rotationY = Quaternion.AngleAxis(mouseX, Vector3.up);

        Quaternion targetRotation = rotationX * rotationY;
        //Fix to default rotation
        targetRotation.eulerAngles = new Vector3(
            targetRotation.eulerAngles.x + equippedGun.defaultRotation.x,
            targetRotation.eulerAngles.y + equippedGun.defaultRotation.y,
            targetRotation.eulerAngles.z + equippedGun.defaultRotation.z);

        instantiatedGun.transform.localRotation = Quaternion.Slerp(instantiatedGun.transform.localRotation, targetRotation, smooth * Time.deltaTime);
    }

}

