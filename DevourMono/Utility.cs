using System.Collections.Generic;
using System.Reflection;

using UnityEngine;

namespace DevourMono
{
    public class Utility : Main
    {
        #region Reflection Stuff
        public static void ReplaceMultipleFields(object obj, string[] names, object[] values, BindingFlags bf)
        {
            for (int i = 0; i < names.Length; i++)
            {
                FieldInfo fi = GetField(obj, names[i], bf);
                SetValue(fi, obj, values[i]);
            }
        }
        public static void ReplaceField(object obj, string name, object value, BindingFlags bf)
        {
            FieldInfo fi = GetField(obj, name, bf);
            SetValue(fi, obj, value);
        }

        public static FieldInfo GetField(object obj, string fieldName, BindingFlags bf)
        {
            return obj.GetType().GetField(fieldName, bf);
        }

        public static MethodInfo GetMethod(object obj, string methodName, BindingFlags bf)
        {
            return obj.GetType().GetMethod(methodName, bf);
        }

        public static object GetValue(FieldInfo fi, object obj)
        {
            return fi.GetValue(obj);
        }
        public static void SetValue(FieldInfo fi, object obj, object value)
        {
            fi.SetValue(obj, value);
        }
        #endregion
        #region Bone Stuff
        public static Transform GetBone(GameObject g, HumanBodyBones bone)
        {
            return g.GetComponent<Animator>().GetBoneTransform(bone);
        }
        public static List<Transform> GetBones(GameObject g)
        {
            List<Transform> bones = new List<Transform>();

            if (g.name == "SurvivalKai(Clone)")
            {
                bones.Add(GetBone(g, HumanBodyBones.Head)); // 0
                bones.Add(GetBone(g, HumanBodyBones.Neck));
                bones.Add(GetBone(g, HumanBodyBones.Spine));
                bones.Add(GetBone(g, HumanBodyBones.LeftShoulder));
                bones.Add(GetBone(g, HumanBodyBones.LeftUpperArm));
                bones.Add(GetBone(g, HumanBodyBones.LeftLowerArm)); // 5
                bones.Add(GetBone(g, HumanBodyBones.LeftHand));
                bones.Add(GetBone(g, HumanBodyBones.RightShoulder));
                bones.Add(GetBone(g, HumanBodyBones.RightUpperArm));
                bones.Add(GetBone(g, HumanBodyBones.RightLowerArm));
                bones.Add(GetBone(g, HumanBodyBones.RightHand)); // 10
                bones.Add(GetBone(g, HumanBodyBones.Hips));
                bones.Add(GetBone(g, HumanBodyBones.LeftUpperLeg));
                bones.Add(GetBone(g, HumanBodyBones.LeftLowerLeg));
                bones.Add(GetBone(g, HumanBodyBones.LeftFoot));
                bones.Add(GetBone(g, HumanBodyBones.RightUpperLeg)); // 15
                bones.Add(GetBone(g, HumanBodyBones.RightLowerLeg));
                bones.Add(GetBone(g, HumanBodyBones.RightFoot)); // 17
                return bones;
            }
            else
            {
                bones.Add(GetBone(g, HumanBodyBones.Head)); // 0
                bones.Add(GetBone(g, HumanBodyBones.Neck));
                bones.Add(GetBone(g, HumanBodyBones.Spine));
                bones.Add(GetBone(g, HumanBodyBones.Chest));
                bones.Add(GetBone(g, HumanBodyBones.UpperChest));
                bones.Add(GetBone(g, HumanBodyBones.LeftShoulder)); // 5
                bones.Add(GetBone(g, HumanBodyBones.LeftUpperArm));
                bones.Add(GetBone(g, HumanBodyBones.LeftLowerArm));
                bones.Add(GetBone(g, HumanBodyBones.LeftHand));
                bones.Add(GetBone(g, HumanBodyBones.RightShoulder));
                bones.Add(GetBone(g, HumanBodyBones.RightUpperArm)); // 10
                bones.Add(GetBone(g, HumanBodyBones.RightLowerArm));
                bones.Add(GetBone(g, HumanBodyBones.RightHand));
                bones.Add(GetBone(g, HumanBodyBones.Hips));
                bones.Add(GetBone(g, HumanBodyBones.LeftUpperLeg));
                bones.Add(GetBone(g, HumanBodyBones.LeftFoot)); // 15
                bones.Add(GetBone(g, HumanBodyBones.LeftLowerLeg));
                bones.Add(GetBone(g, HumanBodyBones.RightUpperLeg));
                bones.Add(GetBone(g, HumanBodyBones.RightLowerLeg));
                bones.Add(GetBone(g, HumanBodyBones.RightFoot)); // 19
                return bones;
            }
        }
        #endregion

        public static void UnlockAll()
        {
			AchievementHelpers ah = FindObjectOfType<AchievementHelpers>();

			string[] names = { "hasAchievedFusesUsed", "hasAchievedGasolineUsed", "hasAchievedNoKnockout", "hasCollectedAllPatches", "hasCollectedAllRoses",
				"hasCompletedHardAsylumGame", "hasCompletedHardGame", "hasCompletedNightmareAsylumGame", "hasCompletedNightmareGame", "hasCompletedNormalGame",
				"isStatsValid", "isStatsFetched"};
			object[] values = { true, true, true, true, true, true, true, true, true, true, true, true };
			ReplaceMultipleFields(ah, names, values, BindingFlags.Instance | BindingFlags.NonPublic);

			string[] achievments = { "ACH_ALL_ROSES", "ACH_BURNT_GOAT", "ACH_SURVIVED_TO_3_GOATS", "ACH_SURVIVED_TO_5_GOATS", "ACH_SURVIVED_TO_7_GOATS", "ACH_WON_SP", "ACH_WON_COOP",
				"ACH_LOST", "ACH_LURED_20_GOATS", "ACH_REVIVED_20_PLAYERS", "ACH_ALL_NOTES_READ", "ACH_KNOCKED_OUT_BY_ANNA", "ACH_KNOCKOUT_OUT_BY_DEMON", "ACH_KNOCKED_OUT_20_TIMES",
				"ACH_NEVER_KNOCKED_OUT", "ACH_ONLY_ONE_KNOCKED_OUT", "ACH_UNLOCKED_CAGE", "ACH_UNLOCKED_ATTIC_CAGE", "ACH_BEAT_GAME_5_TIMES", "ACH_100_GASOLINE_USED",
				"ACH_FRIED_20_DEMONS", "ACH_STAGGERED_ANNA_20_TIMES", "ACH_CALMED_ANNA_10_TIMES", "ACH_CALMED_ANNA", "ACH_WIN_NIGHTMARE", "ACH_BEAT_GAME_5_TIMES_IN_NIGHTMARE_MODE",
				"ACH_WON_NO_KNOCKOUT_COOP", "ACH_WIN_NIGHTMARE_SP", "ACH_WON_HARD", "ACH_WON_HARD_SP", "ACH_100_FUSES_USED", "ACH_ALL_CLIPBOARDS_READ", "ACH_ALL_PATCHES",
				"ACH_FRIED_RAT", "ACH_FRIED_100_INMATES", "ACH_LURED_20_RATS", "ACH_STAGGERED_MOLLY_20_TIMES", "ACH_WON_MOLLY_SP", "ACH_WON_MOLLY_HARD_SP", "ACH_WON_MOLLY_NIGHTMARE_SP",
				"ACH_WON_MOLLY_COOP", "ACH_WON_MOLLY_HARD", "ACH_WON_MOLLY_NIGHTMARE", "ACH_20_TRASH_CANS_KICKED", "ACH_CALM_MOLLY_10_TIMES"
			};
			for (int i = 0; i < achievments.Length; i++)
			{
				ah.Unlock(achievments[i]);
			}
		}

        public static Texture2D CreateTexture2D(Color color)
        {
            Texture2D text = new Texture2D(1, 1, TextureFormat.ARGB32, false);
            text.SetPixel(1, 0, color);
            text.SetPixel(0, 1, color);
            text.SetPixel(1, 1, color);
            text.Apply();
            return text;
        }
        public static float GetDistance(GameObject obj1, GameObject obj2)
        {
            return Vector3.Distance(obj1.transform.position, obj2.transform.position);
        }
    }
}