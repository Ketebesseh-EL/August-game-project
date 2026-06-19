using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class PixelWaterEditor : ShaderGUI {
    private Gradient _gradient;

    private const string RenderingOptionsName = "Rendering Options";
    private GUIStyle _foldoutStyle;
    private static readonly Dictionary<string, bool> FoldoutStates =
        new Dictionary<string, bool> { { RenderingOptionsName, false } };

    private static readonly Dictionary<string, string> Tooltips = new Dictionary<string, string> {
        // Colors
        {"Shallow", "Color at the top of the water."},
        {"Deep", "Color below the surface."},
        {"Shallow depth", "The distance from surface where the transition from shallow to deep color starts."},
        {"Gradient size", "The height of the transition between the shallow and the deep colors."},
        {"Transparency", "How clear the color of the water is. " +
                         "The transparency doesn't affect other parameters like foam or refractions. " +
                         "This allows you to achieve awesome weird optical effects."},
        {"Shadow Strength", "How visible the shadow is."},

        // Crest
        {"Color{Crest}", "The color of the wave. It helps accentuate individual waves."},
        {"Size{Crest}", "How much of the wave is colored."},
        {"Sharp transition{Crest}", "How smoothly the accentuated wave blends into overall color of the water."},

        // Wave geometry
        {"Shape{Wave}", "The formula that determine how the waves are shaped and distributed across the mesh. " +
                  "Round is for concentric round-shaped ripples; Grid is more linear movement; " +
                  "Pointy is for more pronounced individual wave peaks."},
        {"Speed{Wave}", "How fast it moves along the `Direction` parameter."},
        {"Amplitude{Wave}", "Sets deviation amount, or, how high it is."},
        {"Frequency{Wave}", "Density of the effect."},
        {"Direction{Wave}", "Direction of the motion."},
        {"Noise{Wave}", "Adds randomness to the wave shape. Use it to add complexity to the surface."},

        // Foam
        {"Source{Foam}", "How the foam is being made — from texture or generated from noise."},
        {"Texture{Foam}", "A single channel texture map of foam."},
        {"Color{Foam}", "Color value. Can be opaque or transparent."},
        {"Shore Depth{Foam}", "Controls the amount of foam in regions where water intersects other scene objects."},
        {"Amount{Foam}", "How often 'grains' occur."},
        {"Sharpness{Foam}", "How smooth or sharp the foam is."},
        {"Scale{Foam}", "How big the foam 'chunks' are."},
        {"Stretch X{Foam}", "How stretched the foam is along X axis."},
        {"Stretch Y{Foam}", "How stretched the foam is along Y axis."},

        // Refraction

        // Specular
        {"Power", "Makes specular thin or thick. 'Power' value is a multiplier of 'Strength' parameter."},
        {"Strength", "How prominent the specular is."},
    };

    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties) {
        Material targetMaterial = materialEditor.target as Material;
        string[] keywords = targetMaterial.shaderKeywords;

        if (!targetMaterial.IsKeywordEnabled("_COLORMODE_LINEAR") &&
            !targetMaterial.IsKeywordEnabled("_COLORMODE_GRADIENT_TEXTURE")) {
            targetMaterial.EnableKeyword("_COLORMODE_LINEAR");
        }

        bool isColorModeGradient = targetMaterial.IsKeywordEnabled("_COLORMODE_GRADIENT_TEXTURE");

        int originalIntentLevel = EditorGUI.indentLevel;
        int foldoutRemainingItems = 0;
        bool latestFoldoutState = false;
        _foldoutStyle ??= new GUIStyle(EditorStyles.foldout) {
            fontStyle = FontStyle.Bold,
            margin = {
                left = -10
            },
            padding = {
                left = 20
            },
        };

        foreach (MaterialProperty property in properties) {
            bool skipProperty = false;

            if (isColorModeGradient) {
                skipProperty |= ShowColorGradientExportBox(materialEditor, property);
            }

            {
                var brackets = property.displayName.Split('[', ']');
                foreach (var bracket in brackets) {
                    if (bracket.Contains("FOLDOUT") || !property.displayName.Contains('[' + bracket + ']')) {
                        continue;
                    }

                    bool isNegative = bracket.StartsWith("!");
                    bool isPositive = !isNegative;
                    var param = bracket.TrimStart('!');
                    bool keywordOn = Array.IndexOf(keywords, param) != -1;

                    if (isPositive && !keywordOn) {
                        skipProperty = true;
                    }

                    if (isNegative && keywordOn) {
                        skipProperty = true;
                    }

                    if (skipProperty) {
                        break;
                    }
                }
            }

            // Foldouts.
            {
                string displayName = property.displayName;
                if (displayName.Contains("FOLDOUT")) {
                    string foldoutName = displayName.Split('(', ')')[1];
                    string foldoutItemCount = displayName.Split('{', '}')[1];
                    foldoutRemainingItems = Convert.ToInt32(foldoutItemCount);
                    FoldoutStates.TryAdd(property.name, false);
                    EditorGUILayout.Space();
                    FoldoutStates[property.name] =
                        EditorGUILayout.Foldout(FoldoutStates[property.name], foldoutName, true, _foldoutStyle);
                    latestFoldoutState = FoldoutStates[property.name];
                }

                if (foldoutRemainingItems > 0) {
                    skipProperty = skipProperty || !latestFoldoutState;
                    EditorGUI.indentLevel += 1;
                    --foldoutRemainingItems;
                }
            }

            bool hideInInspector = (property.flags & MaterialProperty.PropFlags.HideInInspector) != 0;
            if (!skipProperty && !hideInInspector) {
                DrawStandard(materialEditor, property);
            }

            if (targetMaterial.IsKeywordEnabled("_COLORMODE_GRADIENT_TEXTURE") &&
                property.type == MaterialProperty.PropType.Texture &&
                property.displayName.StartsWith("[_COLORMODE_GRADIENT_TEXTURE]") &&
                property.textureValue != null) {
                GUILayout.Space(-50);
                using (new EditorGUILayout.HorizontalScope()) {
                    GUILayout.Space(15);
                    if (GUILayout.Button("Reset", EditorStyles.miniButtonLeft,
                            GUILayout.Width(60f), GUILayout.ExpandWidth(false))) {
                        property.textureValue = null;
                    }

                    GUILayout.FlexibleSpace();
                }

                EditorGUILayout.Space(60);
            }

            EditorGUI.indentLevel = originalIntentLevel;
        }

        EditorGUILayout.Space();
        FoldoutStates[RenderingOptionsName] =
            EditorGUILayout.Foldout(FoldoutStates[RenderingOptionsName], RenderingOptionsName, true, _foldoutStyle);
        if (FoldoutStates[RenderingOptionsName]) {
            EditorGUI.indentLevel += 1;
            DrawOpaqueField(targetMaterial);
            DrawQueueOffsetField(properties, targetMaterial);
        }
    }

    private void DrawOpaqueField(Material material) {
        var opaque = EditorGUILayout.Toggle("Opaque", material.GetTag("RenderType", false) == "Opaque");
        if (opaque) {
            material.SetOverrideTag("RenderType", "Opaque");
            material.SetInt("_ZWrite", 1);
            material.renderQueue = (int)RenderQueue.Geometry;
        } else {
            material.SetOverrideTag("RenderType", "Transparent");
            material.SetInt("_ZWrite", 0);
            material.renderQueue = (int)RenderQueue.Transparent;
        }
    }

    private void DrawQueueOffsetField(MaterialProperty[] properties, Material material) {
        GUIContent queueSlider = new GUIContent("Priority",
            "Determines the chronological rendering order for a Material. High values are rendered first.");
        const int queueOffsetRange = 50;
        MaterialProperty queueOffsetProp = FindProperty("_QueueOffset", properties, false);
        if (queueOffsetProp == null) return;
        EditorGUI.BeginChangeCheck();
        EditorGUI.showMixedValue = queueOffsetProp.hasMixedValue;
        var queue = EditorGUILayout.IntSlider(queueSlider, (int)queueOffsetProp.floatValue, -queueOffsetRange,
            queueOffsetRange);
        if (EditorGUI.EndChangeCheck())
            queueOffsetProp.floatValue = queue;
        EditorGUI.showMixedValue = false;
        material.renderQueue += queue;
    }

    private void DrawStandard(MaterialEditor materialEditor, MaterialProperty property) {
        string displayName = property.displayName;

        // Remove everything in brackets.
        displayName = Regex.Replace(displayName, @" ?\[.*?\]", string.Empty);

        Tooltips.TryGetValue(displayName.Trim(), out string tooltip);
        displayName = Regex.Replace(displayName, @" ?\{.*?\}", string.Empty);

        var guiContent = new GUIContent(displayName, tooltip);

        if (property.type == MaterialProperty.PropType.Texture) {
            materialEditor.TexturePropertySingleLine(guiContent, property);
        } else {
            materialEditor.ShaderProperty(property, guiContent);
        }
    }

    private bool ShowColorGradientExportBox(MaterialEditor materialEditor, MaterialProperty property) {
        bool isGradientTexture = property.type == MaterialProperty.PropType.Texture &&
                                 property.displayName.StartsWith("[_COLORMODE_GRADIENT_TEXTURE]");
        if (isGradientTexture) {
            if (property.textureValue != null) {
                return false;
            }
        } else {
            return false;
        }

        var messageContent =
            EditorGUIUtility.TrTextContent(
                "Before the gradient can be used it needs to be exported as a texture.");
        var buttonContent = EditorGUIUtility.TrTextContent("Export");
        bool buttonPressed = GradientBoxWithButton(messageContent, buttonContent);
        if (buttonPressed) {
            var texture = GradientToTexture(_gradient);
            PromptTextureSave(materialEditor, texture, property.name);
        }

        return true;
    }

    private bool GradientBoxWithButton(GUIContent messageContent, GUIContent buttonContent) {
        float boxHeight = 40f;
        EditorGUILayout.Space(5);
        Rect rect = GUILayoutUtility.GetRect(messageContent, EditorStyles.helpBox);
        GUILayoutUtility.GetRect(0f, boxHeight);
        rect.height += boxHeight;
        var style = new GUIStyle(EditorStyles.helpBox);
        style.fontSize += 2;
        GUI.Label(rect, messageContent, style);
        float secondLineHeight = 20f;
        float secondLineY = rect.yMax - secondLineHeight - 15f;
        var buttonPosition = new Rect(rect.xMax - 60f - 4f, secondLineY, 60f, secondLineHeight);
        bool result = GUI.Button(buttonPosition, buttonContent);

        var gradientPosition = new Rect(rect.xMin + 8f, secondLineY, rect.width - 60f - 18f, secondLineHeight);
        if (_gradient == null) {
            _gradient = new Gradient();
        }

        _gradient = EditorGUI.GradientField(gradientPosition, _gradient);
        EditorGUILayout.Space(10);
        return result;
    }

    private Texture2D GradientToTexture(Gradient g) {
        const int width = 256;

        Texture2D texture = new Texture2D(width, 1, TextureFormat.RGBA32, /*mipChain=*/false) {
            name = "Pixel Water Color Gradient",
            wrapMode = TextureWrapMode.Clamp,
            hideFlags = HideFlags.HideAndDontSave,
            filterMode = FilterMode.Point
        };
        for (float x = 0;
             x < width;
             x++) {
            Color32 color = g.Evaluate(x / (width - 1));
            texture.SetPixel(Mathf.CeilToInt(x), 0, color);
        }

        texture.Apply();
        return texture;
    }

    private void PromptTextureSave(MaterialEditor materialEditor, Texture2D texture, string propertyName) {
        Material material = materialEditor.target as Material;
        if (material == null) {
            return;
        }

        var pngNameWithExtension = $"{materialEditor.target.name}{propertyName}.png";

        var fullPath =
            EditorUtility.SaveFilePanel("Save Gradient Texture", "Assets", pngNameWithExtension, "png");
        if (fullPath.Length > 0) {
            SaveTextureAsPng(texture, fullPath, FilterMode.Point);
            var loadedTexture = LoadTexture(fullPath);
            if (loadedTexture != null) {
                material.SetTexture(propertyName, loadedTexture);
            } else {
                Debug.LogWarning($"Could not load the texture from {fullPath}");
            }
        }
    }

    private void SaveTextureAsPng(Texture2D texture, string fullPath, FilterMode filterMode) {
        byte[] bytes = texture.EncodeToPNG();
        if (bytes == null) {
            Debug.LogError("Could not encode texture as PNG.");
            return;
        }

        File.WriteAllBytes(fullPath, bytes);
        AssetDatabase.Refresh();
        Debug.Log($"Texture saved as: {fullPath}");

        string pathRelativeToAssets = ConvertFullPathToAssetPath(fullPath);
        if (pathRelativeToAssets.Length == 0) {
            Debug.LogWarning($"Could not save the texture to {fullPath}.");
        }

        TextureImporter importer = (TextureImporter)TextureImporter.GetAtPath(pathRelativeToAssets);
        if (importer != null) {
            importer.filterMode = filterMode;
            importer.textureType = TextureImporterType.Default;
            importer.textureCompression = TextureImporterCompression.Uncompressed;
            importer.mipmapEnabled = false;
            var textureSettings = new TextureImporterPlatformSettings {
                format = TextureImporterFormat.RGBA32
            };
            importer.SetPlatformTextureSettings(textureSettings);
            EditorUtility.SetDirty(importer);
            importer.SaveAndReimport();
        }
    }

    private static Texture2D LoadTexture(string fullPath) {
        string pathRelativeToAssets = ConvertFullPathToAssetPath(fullPath);
        if (pathRelativeToAssets.Length == 0) {
            return null;
        }

        var loadedTexture = AssetDatabase.LoadAssetAtPath(pathRelativeToAssets, typeof(Texture2D)) as Texture2D;
        if (loadedTexture == null) {
            Debug.LogError($"Could not load texture from {fullPath} [{pathRelativeToAssets}].");
            return null;
        }

        loadedTexture.filterMode = FilterMode.Point;
        loadedTexture.wrapMode = TextureWrapMode.Clamp;
        return loadedTexture;
    }

    private static string ConvertFullPathToAssetPath(string fullPath) {
        int count = (Directory.GetCurrentDirectory() + System.IO.Path.DirectorySeparatorChar).Length;
        return fullPath.Remove(0, count);
    }
}
