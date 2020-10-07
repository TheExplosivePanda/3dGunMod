using System;
using System.Collections;
using Gungeon;
using MonoMod;
using UnityEngine;
using ItemAPI;
using BasicGun;
namespace PandaMod
{
    public class Gun3d : GunBehaviour
    {
        public static void Add()
        {
            // Get yourself a new gun "base" first.
            // Let's just call it "Basic Gun", and use "jpxfrd" for all sprites and as "codename" All sprites must begin with the same word as the codename. For example, your firing sprite would be named "jpxfrd_fire_001".
            Gun gun = ETGMod.Databases.Items.NewGun("3d Gun", "jpxfrd");
            // "kp:basic_gun determines how you spawn in your gun through the console. You can change this command to whatever you want, as long as it follows the "name:itemname" template.
            Game.Items.Rename("outdated_gun_mods:3d_gun", "pb:gun_3d");
            gun.gameObject.AddComponent<Gun3d>();
            //These two lines determines the description of your gun, ".SetShortDescription" being the description that appears when you pick up the gun and ".SetLongDescription" being the description in the Ammonomicon entry. 
            gun.SetShortDescription("Wait, its all 3d?");
            gun.SetLongDescription("Always has been.");
            gun.SetupSprite(null, "jpxfrd_glitch", 8);
            // ETGMod automatically checks which animations are available.
            // The numbers next to "shootAnimation" determine the animation fps. You can also tweak the animation fps of the reload animation and idle animation using this method.

            // Every modded gun has base projectile it works with that is borrowed from other guns in the game. 
            // The gun names are the names from the JSON dump! While most are the same, some guns named completely different things. If you need help finding gun names, ask a modder on the Gungeon discord

            // Here we just take the default projectile module and change its settings how we want it to be.
            gun.AddProjectileModuleFrom("ak-47", true, false);
            gun.gameObject.GetOrAddComponent<Gun3dMoveAndControl>();
            // Here we just take the default projectile module and change its settings how we want it to be.
            gun.DefaultModule.projectiles[0].gameObject.AddComponent<Bullets3dHandler>();
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Automatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1.1f;
            gun.DefaultModule.cooldownTime = 0.1f;
            gun.DefaultModule.numberOfShotsInClip = 25;
            gun.SetBaseMaxAmmo(400);
            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.CUSTOM;
            gun.DefaultModule.customAmmoType = "FakePrefabDfSpriteTest";
            gun.DefaultModule.finalCustomAmmoType = "FakePrefabDfSpriteTest";
            gun.sprite.renderer.enabled = false;
            gun.gameObject.GetComponent<Renderer>().enabled = false;
            gun.PreventOutlines = true;

            // Here we just set the quality of the gun and the "EncounterGuid", which is used by Gungeon to identify the gun.
            gun.quality = PickupObject.ItemQuality.B;

            ETGMod.Databases.Items.Add(gun, null, "ANY");

        }

        public override void OnReloadPressed(PlayerController player, Gun gun, bool ManualReload)
        {


        }

        // This determines what the projectile does when it fires.
        public override void PostProcessProjectile(Projectile projectile)
        {
            PlayerController playerController = this.gun.CurrentOwner as PlayerController;
            if (playerController == null)
                this.gun.ammo = this.gun.GetBaseMaxAmmo();
            //projectile.baseData allows you to modify the base properties of your projectile module.
            //In our case, our gun uses modified projectiles from the ak-47.
            //Setting static values for a custom gun's projectile stats prevents them from scaling with player stats and bullet modifiers (damage, shotspeed, knockback)
            //You have to multiply the value of the original projectile you're using instead so they scale accordingly. For example if the projectile you're using as a base has 10 damage and you want it to be 6 you use this
            //In our case, our projectile has a base damage of 5.5, so we multiply it by 1.1 so it does 10% more damage from the ak-47.
            projectile.baseData.damage *= 1.10f;
            projectile.baseData.speed *= 1f;
            this.gun.DefaultModule.ammoCost = 1;
            base.PostProcessProjectile(projectile);

            //Camera.Camera.transform.Rotate(new Vector3(0f, 0f, 1f), 90f, Space.Self);

            //This is for when you want to change the sprite of your projectile and want to do other magic fancy stuff. But for now let's just change the sprite. 
            //Refer to BasicGunProjectile.cs for changing the sprite.


        }

        public override void OnPostFired(PlayerController player, Gun gun)
        {
            //This determines what sound you want to play when you fire a gun.
            //Sounds names are based on the Gungeon sound dump, which can be found at EnterTheGungeon/Etg_Data/StreamingAssets/Audio/GeneratedSoundBanks/Windows/sfx.txt
            gun.PreventNormalFireAudio = true;
            AkSoundEngine.PostEvent("Play_WPN_smileyrevolver_shot_01", gameObject);
        }

        public Gun3d()
        {
        }

        internal class BasicGunProjectile : MonoBehaviour
        {

            public void Start()
            {
                this.projectile = base.GetComponent<Projectile>();


                this.player = (this.projectile.Owner as PlayerController);
                Projectile proj = this.projectile;
                //This determines what sprite you want your projectile to use.
                projectile.sprite.GetSpriteIdByName("jpxfrd_projectile_022");



            }


            private Projectile projectile;

            private PlayerController player;
        }
    }

}