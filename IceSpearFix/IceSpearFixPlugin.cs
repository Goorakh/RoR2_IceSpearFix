using BepInEx;
using RoR2;
using RoR2.ContentManagement;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace IceSpearFix
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    [BepInDependency(R2API.R2API.PluginGUID)]
    public class IceSpearFixPlugin : BaseUnityPlugin
    {
        public const string PluginGUID = $"com.{PluginAuthor}.{PluginName}";
        public const string PluginAuthor = "Gorakh";
        public const string PluginName = "IceSpearFix";
        public const string PluginVersion = "1.0.0";

        static IceSpearFixPlugin _instance;
        internal static IceSpearFixPlugin Instance => _instance;

        void Awake()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            SingletonHelper.Assign(ref _instance, this);

            Log.Init(Logger);

            AssetReferenceGameObject iceBombProjectileRef = new AssetReferenceGameObject(AddressableGuids.RoR2_Base_Mage_MageIceBombProjectile_prefab);
            AssetAsyncReferenceManager<GameObject>.LoadAsset(iceBombProjectileRef, AsyncReferenceHandleUnloadType.Preload).Completed += (handle) =>
            {
                GameObject iceBombProjectilePrefab = handle.Result;
                if (iceBombProjectilePrefab)
                {
                    iceBombProjectilePrefab.layer = LayerIndex.projectile.intVal;
                }
                else
                {
                    Log.Error($"Failed to load ice spear projectile: {handle.OperationException}");
                }

                AssetAsyncReferenceManager<GameObject>.UnloadAsset(iceBombProjectileRef);
            };

            stopwatch.Stop();
            Log.Message_NoCallerPrefix($"Initialized in {stopwatch.Elapsed.TotalMilliseconds:F0}ms");
        }

        void OnDestroy()
        {
            SingletonHelper.Unassign(ref _instance, this);
        }
    }
}
