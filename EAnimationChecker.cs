using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace Editor
{
    public class EAnimationChecker : EditorWindow
    {
        [MenuItem("Escripts/EAnimationChecker/Check Animator Controllers")]
        private static void CheckAnimatorControllersMenu()
        {
            CheckAnimatorsForMissingClips("AnimatorController");
            Debug.Log("Completed checking Animator Controllers.");
        }

        [MenuItem("Escripts/EAnimationChecker/Check Animator Override Controllers")]
        private static void CheckAnimatorOverrideControllersMenu()
        {
            CheckAnimatorsForMissingClips("AnimatorOverrideController");
            Debug.Log("Completed checking Animator Override Controllers.");
        }

        [MenuItem("Escripts/EAnimationChecker/Check Scene Animators")]
        private static void CheckSceneAnimatorsMenu()
        {
            CheckSceneAnimators();
            Debug.Log("Completed checking scene Animators.");
        }

        private static void CheckAnimatorsForMissingClips(string type)
        {
            string[] guids = AssetDatabase.FindAssets($"t:{type}", new[] { "Assets" });
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                var animatorObject = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path);

                if (animatorObject is AnimatorController animatorController)
                {
                    CheckAnimatorController(animatorController);
                }
                else if (animatorObject is AnimatorOverrideController overrideController)
                {
                    CheckAnimatorOverrideController(overrideController);
                }
            }
        }

        private static void CheckAnimatorController(AnimatorController animatorController)
        {
            foreach (var layer in animatorController.layers)
            {
                CheckStateMachine(layer.stateMachine, animatorController.name);
            }
        }

        private static void CheckStateMachine(AnimatorStateMachine stateMachine, string animatorControllerName)
        {
            foreach (var state in stateMachine.states)
            {
                if (state.state.motion == null)
                {
                    Debug.LogWarning($"Animator State '{state.state.name}' in '{animatorControllerName}' has no motion assigned!");
                }
                else if (state.state.motion is BlendTree)
                {
                    CheckBlendTree(state.state.motion as BlendTree, animatorControllerName);
                }
            }

            foreach (var childStateMachine in stateMachine.stateMachines)
            {
                CheckStateMachine(childStateMachine.stateMachine, animatorControllerName);
            }
        }

        private static void CheckBlendTree(BlendTree blendTree, string animatorControllerName)
        {
            foreach (var child in blendTree.children)
            {
                if (child.motion == null)
                {
                    Debug.LogWarning($"BlendTree '{blendTree.name}' in '{animatorControllerName}' has a child with no motion assigned!");
                }
                else if (child.motion is BlendTree)
                {
                    CheckBlendTree(child.motion as BlendTree, animatorControllerName);
                }
            }
        }
    
        private static void CheckAnimatorOverrideController(AnimatorOverrideController overrideController)
        {
            bool missingClip = false;
            var overrides = new List<KeyValuePair<AnimationClip, AnimationClip>>();
            overrideController.GetOverrides(overrides);

            foreach (var overridePair in overrides)
            {
                if (overridePair.Value == null || overridePair.Value == overridePair.Key)
                {
                    Debug.LogWarning($"AnimatorOverrideController '{overrideController.name}' is using the original clip for '{overridePair.Key.name}'");
                    missingClip = true;
                }
            }

            if (!missingClip)
            {
                Debug.Log($"AnimatorOverrideController '{overrideController.name}' has all clips overridden.");
            }
        }

        private static void CheckSceneAnimators()
        {
            bool anyUnassigned = false;
            Animator[] animators = GameObject.FindObjectsOfType<Animator>(true); // true to include inactive objects

            foreach (Animator animator in animators)
            {
                if (animator.runtimeAnimatorController == null)
                {
                    Debug.LogWarning($"Animator on GameObject '{animator.gameObject.name}' in the scene has no Animator Controller assigned!");
                    anyUnassigned = true;
                }
            }

            if (!anyUnassigned)
            {
                Debug.Log("All scene animators have their Animator Controllers assigned.");
            }
        }
    }
}