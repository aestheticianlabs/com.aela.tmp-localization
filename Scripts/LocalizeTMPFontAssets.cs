using UnityEngine;
using TMPro;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace AeLa.TMPLocalization
{
    [ExecuteAlways]
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class LocalizeTMPFontAssets : LocalizedMonoBehaviour
    {
        [Tooltip("Reference to the localization asset table containing TMP_FontAssets.")]
        public LocalizedAsset<TMP_FontAsset> FontReference;

        [Tooltip("Reference to the localization asset table containing TMP materials.")]
        public LocalizedAsset<Material> MaterialReference;

        private TextMeshProUGUI text;

        protected void OnEnable()
        {
            if (!text)
                text = GetComponent<TextMeshProUGUI>();

            // Subscribe
            if (FontReference != null)
                FontReference.AssetChanged += OnFontChanged;

            if (MaterialReference != null)
                MaterialReference.AssetChanged += OnMaterialChanged;

            // Apply current values immediately
            if (FontReference != null)
            {
                FontReference.LoadAssetAsync().Completed += FontLoaded;
            }

            if (MaterialReference != null)
            {
                MaterialReference.LoadAssetAsync().Completed += MaterialLoaded;
            }
        }

        protected void OnDisable()
        {
            if (FontReference != null)
                FontReference.AssetChanged -= OnFontChanged;

            if (MaterialReference != null)
                MaterialReference.AssetChanged -= OnMaterialChanged;
        }

        private void FontLoaded(AsyncOperationHandle<TMP_FontAsset> op)
        {
            OnFontChanged(op.Result);
        }

        private void MaterialLoaded(AsyncOperationHandle<Material> op)
        {
            OnMaterialChanged(op.Result);
        }

        private void OnFontChanged(TMP_FontAsset asset)
        {
            if (!text)
                return;

            if (!asset)
            {
                Debug.LogWarning(
                    $"Localized font asset not found for {FontReference.TableReference}/{FontReference.TableEntryReference}",
                    this
                );
                return;
            }

            text.font = asset;
        }

        private void OnMaterialChanged(Material asset)
        {
            if (!text)
                return;

            if (!asset)
            {
                Debug.LogWarning(
                    $"Localized material asset not found for {MaterialReference.TableReference}/{MaterialReference.TableEntryReference}",
                    this
                );
                return;
            }

            text.fontSharedMaterial = asset;
        }
    }
}