using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


namespace PandaMod.SpapiTools
{
    public class DetectMissingDefinitions : MonoBehaviour
    {
        private void Update()
        {
            if (GameUIRoot.HasInstance)
            {
                foreach (GameUIAmmoController uiammocontroller in GameUIRoot.Instance.ammoControllers)
                {
                    foreach (GameUIAmmoType uiammotype in SpecialItemModule.addedAmmoTypes)
                    {
                        if (!uiammocontroller.ammoTypes.Contains(uiammotype))
                        {
                            StaticToolClass.Add(ref uiammocontroller.ammoTypes, uiammotype);
                        }
                    }
                }
            }
        }
    }
}
