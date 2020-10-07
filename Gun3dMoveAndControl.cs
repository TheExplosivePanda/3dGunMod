using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Gungeon;
using ItemAPI;
using UnityEngine.Scripting;
using UnityEngineInternal;
namespace PandaMod
{
    class Gun3dMoveAndControl : MonoBehaviour
    {
        private void Start()
        {
            gun = base.GetComponent<Gun>();
            Gun3dMoveAndControl previousMaybe = gun.GetComponent<Gun3dMoveAndControl>();

            if (previousMaybe != null)
            {
                Destroy(previousMaybe.obj);
            }

            obj = Instantiate<GameObject>(Module.pandaBundle.LoadAsset<GameObject>("MachinePistol"), gun.transform.position + new Vector3(0.5f, 0, 0), Quaternion.identity, gun.transform);

            obj.transform.Rotate(new Vector3(0f, 1f, 0f), 90f, Space.Self);
            obj.transform.Rotate(new Vector3(-1f, 0f, 0f), 90f, Space.Self);
            obj.transform.localScale *= 0.8f;

            gun.sprite.renderer.enabled = false;

            if (gun.HasBeenPickedUp)
            {
                if (!UIgunObject)
                {
                    Add3dUIgun();
                    Hide2dGunUI();
                    Create3dClip();
                }

            }
            gun.OnPostFired += Fire;

            flip = gun.CurrentOwner.SpriteFlipped;
            if (flip)
            {
                Flip();
            }


        }



        private void Fire(PlayerController player, Gun gun)
        {
            StartCoroutine(Shoot());
        }
        private void Update()
        {
            if (gun != null)
            {
                if (gun.CurrentOwner != null)
                {
                    if (gun.CurrentOwner.SpriteFlipped != flip)
                    {
                        Flip();
                        flip = gun.CurrentOwner.SpriteFlipped;
                    }
                }
                gun.sprite.renderer.enabled = false;
            }




            base.gameObject.transform.Rotate(new Vector3(0.8f, 0f, 0.2f), 4f, Space.Self);

            if (UIgunObject != null && obj != null)
            {
                UIgunObject.transform.rotation = obj.transform.rotation;
            }
            else if (gun.CurrentOwner != null && UIgunObject != null)
            {
                Add3dUIgun();
            }
            if (base.gameObject.layer != LayerMask.NameToLayer("Unpixelated"))
            {
                base.gameObject.SetLayerRecursively(LayerMask.NameToLayer("Unpixelated"));
            }
            if (gun.CurrentOwner != null)
            {
                Hide2dGunUI();
            }
            else if (clip.Capacity > 0)
            {
                Destroy3dClip();
            }
            if ((gun.ClipShotsRemaining > clip.Count && gun.CurrentOwner != null) || clip[0] == null)
            {
                Create3dClip();
            }
            if (gun.ClipShotsRemaining < clip.Count)
            {
                Destroy(clip[clip.Count - 1]);
                clip.RemoveAt(clip.Count - 1);
                clip.TrimExcess();
            }
        }

        private IEnumerator Shoot()
        {
            obj.transform.Rotate(new Vector3(-1, 0, 0), 15);
            yield return new WaitForSeconds(0.09f);
            obj.transform.Rotate(new Vector3(1, 0, 0), 15);


        }

        private IEnumerator EnterNewFloor()
        {
            yield return new WaitForFixedUpdate();
            Hide2dGunUI();
        }
        private void OnEnable()
        {
            if (gun == null)
            {
                gun = base.GetComponent<Gun>();
            }
            gun.sprite.renderer.enabled = false;
            if (!UIgunObject)
            {

                Add3dUIgun();
                Hide2dGunUI();
            }


        }

        private void OnDisable()
        {
            Destroy3dClip();
            Show2dGunUI();
            Destroy3dUI();
        }

        private void Show2dGunUI()
        {
            for (int i = 0; i < GameUIRoot.Instance.ammoControllers.Count; i++)
            {
                for (int j = 0; j < GameUIRoot.Instance.ammoControllers[i].gunSprites.Length; j++)
                {
                    GameUIRoot.Instance.ammoControllers[i].gunSprites[j].color = new Color(1, 1, 1, 1);

                    SpriteOutlineManager.ToggleOutlineRenderers(GameUIRoot.Instance.ammoControllers[i].gunSprites[j], true);


                }
            }
        }
        private void Hide2dGunUI()
        {
            for (int i = 0; i < GameUIRoot.Instance.ammoControllers.Count; i++)
            {
                for (int j = 0; j < GameUIRoot.Instance.ammoControllers[i].gunSprites.Length; j++)
                {
                    GameUIRoot.Instance.ammoControllers[i].gunSprites[j].color = new Color(1, 1, 1, 0);

                    SpriteOutlineManager.ToggleOutlineRenderers(GameUIRoot.Instance.ammoControllers[i].gunSprites[j], false);
                }
            }
        }


        private void Add3dUIgun()
        {
            Camera camera = GameUIRoot.Instance.gunNameLabels[0].GetManager().RenderCamera;
            float distanceFromCamera = camera.farClipPlane - 13;
            Vector3 bottomRight = camera.ViewportToWorldPoint(new Vector3(0.92f, 0.07f, distanceFromCamera));
            UIgunObject = Instantiate<GameObject>(Module.pandaBundle.LoadAsset<GameObject>("MachinePistol"), bottomRight, Quaternion.identity, camera.transform);
            UIgunObject.layer = GameUIRoot.Instance.gunNameLabels[0].gameObject.layer;
            UIgunObject.transform.localScale *= 0.15f;

        }

        private void Destroy3dUI()
        {
            Destroy(UIgunObject);
        }

        private void Flip()
        {
            obj.transform.localScale = new Vector3(obj.transform.localScale.x, obj.transform.localScale.y, -obj.transform.localScale.z);
        }


        private void Create3dClip()
        {
            Destroy3dClip();
            float localscale = 0.45f;
            Camera camera = GameUIRoot.Instance.gunNameLabels[0].GetManager().RenderCamera;
            float distanceFromCamera = camera.farClipPlane - 13;
            for (int i = 0; i < gun.ClipShotsRemaining; i++)
            {
                Vector3 bottomRight = camera.ViewportToWorldPoint(new Vector3(0.985f, 0.05f + (i * 0.04f * localscale), distanceFromCamera));
                clip.Add(Instantiate<GameObject>(Module.pandaBundle.LoadAsset<GameObject>("MachinePistol_Shell"), bottomRight, Quaternion.identity, camera.transform));
                clip[i].transform.Rotate(new Vector3(1, 0, 0), 90, Space.Self);
                clip[i].transform.Rotate(new Vector3(0, 1, 0), 90, Space.Self);
                clip[i].transform.localScale *= localscale;
                clip[i].AddComponent<Bullets3dHandler>();
                clip[i].layer = GameUIRoot.Instance.gunNameLabels[0].gameObject.layer;
                clip[i].transform.localScale *= 0.15f;
            }
        }
        public void Destroy3dClip()
        {
            for (int i = clip.Count - 1; i >= 0; i--)
            {
                Destroy(clip[i]);
            }
            clip.Clear();
            clip.TrimExcess();
        }

        List<GameObject> clip = new List<GameObject>();

        private Gun gun;

        private GameObject UIgunObject;

        public GameObject obj;

        bool flip;
    }
}

