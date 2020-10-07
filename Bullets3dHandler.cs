using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;
using ItemAPI;

namespace PandaMod
{
    class Bullets3dHandler : MonoBehaviour
    {

        private void Start()
        {
            BaseObject = base.gameObject;
            proj = base.gameObject.GetComponent<Projectile>();

            if (proj)
            {

                obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                obj.transform.position = BaseObject.transform.position;
                obj.transform.SetParent(BaseObject.transform);
                obj.transform.localScale *= 0.6f;
                obj.GetComponent<Renderer>().material = EnemyDatabase.GetOrLoadByName("GunNut").sprite.renderer.material;
                obj.GetComponent<Renderer>().material.SetFloat("_EmissivePower", 0f);
                obj.GetComponent<Renderer>().material.mainTexture = ItemAPI.ResourceExtractor.GetTextureFromResource("PandaMod/Resources/bullet.png");

            }


            if (obj.layer != LayerMask.NameToLayer("Unpixelated"))
            {
                obj.SetLayerRecursively(LayerMask.NameToLayer("Unpixelated"));
            }

        }

        private void Update()
        {
            if (proj != null)
            {
                Vector2 vec = proj.Direction;
                obj.transform.TweenRotateTo(proj.Direction);
                obj.transform.Rotate(new Vector2(vec.y, vec.x * -1), 30f, Space.World);
                if (obj.layer != LayerMask.NameToLayer("Unpixelated"))
                {
                    obj.SetLayerRecursively(LayerMask.NameToLayer("Unpixelated"));
                }

            }
            else if (GameManager.Instance.PrimaryPlayer != null)
            {
                base.transform.Rotate(new Vector3(0f, 0.4f, 1f), 4f, Space.Self);
            }
        }

        Projectile proj;
        private GameObject BaseObject;
        private GameObject obj;

    }
}
