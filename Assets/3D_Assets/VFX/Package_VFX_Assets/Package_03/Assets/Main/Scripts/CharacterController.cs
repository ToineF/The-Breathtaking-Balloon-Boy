using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace VFX
{
    public class CharacterController : MonoBehaviour
    {
        [SerializeField] Animator anim;
        [SerializeField] VisualEffect[] effect;

        [SerializeField] VisualEffect swordSlashVFX;
        [SerializeField] VisualEffect swordHitVFX;

        [SerializeField] VisualEffect bowChargeVFX;

        bool isCastingAbility = false;

        [SerializeField] GameObject arrow;
        [SerializeField] GameObject arrowShootPos;
        [SerializeField] float arrowSpeed = 10f;

        [SerializeField] VisualEffect shieldChargeVFX;

        [SerializeField] VisualEffect[] mageChargeVFX;
        [SerializeField] VisualEffect mageFireVFX;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (anim != null)
            {
                if (!isCastingAbility)
                {
                    ButtonPress(KeyCode.Space, "levelUp", 0);
                    ButtonPress(KeyCode.Q, "qCast", 1);
                    ButtonPress(KeyCode.W, "wCast", 2);
                    ButtonPress(KeyCode.E, "eCast", 3);
                    ButtonPress(KeyCode.R, "rCast", 4);
                }
            }
        }

        void ButtonPress(KeyCode button, string trigger, int effectNum)
        {
            if (Input.GetKeyDown(button))
            {
                anim.SetTrigger(trigger);
                if (effect[effectNum] != null)
                    effect[effectNum].Play();

                if (trigger.Equals("wCast") && shieldChargeVFX != null)
                    shieldChargeVFX.Play();

                isCastingAbility = true;
                StartCoroutine(AbilityCooldown());
            }
        }

        IEnumerator AbilityCooldown()
        {
            yield return new WaitForSeconds(2f);
            isCastingAbility = false;
        }

        public void SwordEffect()
        {
            if (swordSlashVFX != null)
                swordSlashVFX.Play();
        }

        public void SwordHitEffect()
        {
            if (swordHitVFX != null)
                swordHitVFX.Play();
        }

        public void BowChargeEffect()
        {
            if (bowChargeVFX != null)
                bowChargeVFX.Play();
        }

        public void ShootArrow()
        {
            GameObject go;
            go = Instantiate(arrow, arrowShootPos.transform.position, transform.rotation);
            go.GetComponent<Rigidbody>().AddForce(transform.forward * arrowSpeed, ForceMode.Impulse);
        }

        public void MageChargeEffect()
        {
            foreach (VisualEffect visual in mageChargeVFX)
            {
                if (visual != null)
                    visual.Play();
            }
        }

        public void MageFireEffect()
        {
            if (mageFireVFX != null)
            {
                mageFireVFX.Play();
            }
        }
    }
}