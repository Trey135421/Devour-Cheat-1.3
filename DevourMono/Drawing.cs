using System.Collections.Generic;
using System.Reflection;

using UnityEngine;

namespace DevourMono
{
    public class Drawing : Main
    {
        // I did not make the drawing stuff, found it in a source and used it, i'd give credit if I knew who made it

        public static Texture2D lineTex;
        public static Material blitMat;
        public static Rect lineRect = new Rect(0f, 0f, 1f, 1f);
        
        public static void InitDrawing()
        {
            lineTex = new Texture2D(1, 1, TextureFormat.ARGB32, false);
            lineTex.SetPixel(0, 1, Color.white);
            lineTex.Apply();

            blitMat = (Material)typeof(GUI).GetMethod("get_blitMaterial", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, null);
        }

        public static void DrawLine(Vector2 start, Vector2 end, Color color, float width)
        {
            float num = end.x - start.x;
            float num2 = end.y - start.y;
            float num3 = Mathf.Sqrt(num * num + num2 * num2);
            if (num3 < 0.001f)
                return;
            Texture2D text = lineTex;
            Material mat = blitMat;
            float num4 = width * num2 / num3;
            float num5 = width * num / num3;
            Matrix4x4 identity = Matrix4x4.identity;
            identity.m00 = num;
            identity.m01 = -num4;
            identity.m03 = start.x + 0.5f * num4;
            identity.m10 = num2;
            identity.m11 = num5;
            identity.m13 = start.y - 0.5f * num5;
            GL.PushMatrix();
            GL.MultMatrix(identity);
            GUI.color = color;
            GUI.DrawTexture(lineRect, text);
            GL.PopMatrix();
        }

        public static void RectFilled(float x, float y, float width, float height, Texture2D text)
        {
            GUI.DrawTexture(new Rect(x, y, width, height), text);
        }
        public static void RectOutlined(float x, float y, float width, float height, Texture2D text, float thickness = 1f)
        {
            RectFilled(x, y, thickness, height, text);
            RectFilled(x + width - thickness, y, thickness, height, text);
            RectFilled(x + thickness, y, width - thickness * 2f, thickness, text);
            RectFilled(x + thickness, y + height - thickness, width - thickness * 2f, thickness, text);
        }
        public static void DrawBox(float x, float y, float width, float height, Texture2D text, float thickness = 1f)
        {
            RectOutlined(x - width / 2f, y - height, width, height, text, thickness);
        }

        public static void RenderObj(GameObject g, Color color, string name = "", bool drawLine = false, bool drawBox = false, Transform footBone = null, Transform headBone = null)
        {
            GUI.color = color;

            #region Vars
            Vector3 footPos = Cam.WorldToScreenPoint(g.transform.position);
            Vector3 headPos = Vector3.zero;
            if (footBone != null)
            {
                footPos = Cam.WorldToScreenPoint(new Vector3(headBone.position.x, footBone.position.y, headBone.position.z));
            }
            if (headBone != null)
            {
                headPos = Cam.WorldToScreenPoint(headBone.position);
            }

            float dist = Vector3.Distance(Cam.transform.position, g.transform.position);
            string fDist = $"[{Mathf.Round(dist)}]";
            #endregion

            if (footPos.z > 0)
            {
                string n = g.name;
                n = n.Replace("(Clone)", "");
                n = n.Replace("Survival", "");

                if (drawLine) DrawLine(new Vector2(Screen.width / 2, Screen.height / 2), new Vector2(footPos.x, Screen.height - footPos.y), color, 2f);

                float num = Mathf.Abs(footPos.y - headPos.y);
                if (drawBox) DrawBox(footPos.x, Screen.height - footPos.y, num / 1.8f, num, Utility.CreateTexture2D(color), 1f);

                if (name == "" || name == null) GUI.Label(new Rect(footPos.x, Screen.height - footPos.y, 200, 200), $"{n} {fDist}");
                else GUI.Label(new Rect(footPos.x, Screen.height - footPos.y, 200, 200), $"{name} {fDist}");
            }
        }
        public static void RenderBone(Transform bone1, Transform bone2, Color color)
        {
            Vector3 w2s1 = Cam.WorldToScreenPoint(bone1.position);
            Vector3 w2s2 = Cam.WorldToScreenPoint(bone2.position);

            if (w2s1.z > 0)
            {
                DrawLine(new Vector2(w2s1.x, Screen.height - w2s1.y), new Vector2(w2s2.x, Screen.height - w2s2.y), color, 2f);
            }
        }
        public static void DrawBones(GameObject g, List<Transform> bones, Color color)
        {
            if (g.name == "SurvivalKai(Clone)")
            {
                RenderBone(bones[0], bones[1], color); // Head to Neck
                RenderBone(bones[1], bones[2], color); // Neck to Spine
                RenderBone(bones[2], bones[11], color); // Spine to Hips

                RenderBone(bones[2], bones[3], color); // Spine to Left Shoulder
                RenderBone(bones[3], bones[4], color); // Left Shoulder to Left Upper Arm
                RenderBone(bones[4], bones[5], color); // Left Upper Arm to Left Lower Arm
                RenderBone(bones[5], bones[6], color); // Left Lower Arm to Left Hand

                RenderBone(bones[2], bones[7], color); // Spine to Right Shoulder
                RenderBone(bones[7], bones[8], color); // Right Shoulder to Right Upper Arm
                RenderBone(bones[8], bones[9], color); // Right Upper Arm to Right Lower Arm
                RenderBone(bones[9], bones[10], color); // Right Lower Arm to Right Hand

                if (Map == "Farm")
                    RenderBone(bones[11], bones[12], color); // Hips to Left Upper Leg
                else if (Map == "Asylum")
                    RenderBone(bones[2], bones[12], color); // Spine to Left Upper Leg
                RenderBone(bones[12], bones[13], color); // Left Upper Leg to Left Lower Leg
                RenderBone(bones[13], bones[14], color); // Left Lower Leg to Left Foot

                if (Map == "Farm")
                    RenderBone(bones[11], bones[15], color); // Hips to Right Upper Leg
                else if (Map == "Asylum")
                    RenderBone(bones[2], bones[15], color); // Spine to Right Upper Leg
                RenderBone(bones[15], bones[16], color); // Right Upper Leg to Right Lower Leg
                RenderBone(bones[16], bones[17], color); // Right Lower Leg to Right Foot*
            }
            else
            {
                RenderBone(bones[0], bones[1], color); // Head to Neck
                RenderBone(bones[1], bones[4], color); // Neck to Upper Chest

                RenderBone(bones[2], bones[3], color); // Spine to Chest
                RenderBone(bones[3], bones[4], color); // Chest to Upper Chest
                if (Map == "Farm" || Players.Contains(g))
                    RenderBone(bones[2], bones[13], color); // Spine to Hips

                RenderBone(bones[4], bones[5], color); // Upper Chest to Left Shoulder
                RenderBone(bones[5], bones[6], color); // Left Shoulder to Left Upper Arm
                RenderBone(bones[6], bones[7], color); // Left Upper Arm to Left Lower Arm
                RenderBone(bones[7], bones[8], color); // Left Lower Arm to Left Hand

                RenderBone(bones[4], bones[9], color); // Upper Chest to Right Shoulder
                RenderBone(bones[9], bones[10], color); // Right Shoulder to Right Upper Arm
                RenderBone(bones[10], bones[11], color); // Right Upper Arm to Right Lower Arm
                RenderBone(bones[11], bones[12], color); // Right Lower Arm to Right Hand

                if (Map == "Farm" || Players.Contains(g))
                    RenderBone(bones[13], bones[14], color); // Hips to Left Upper Leg
                else if (Map == "Asylum")
                    RenderBone(bones[2], bones[14], color); // Spine to Left Upper Leg
                RenderBone(bones[14], bones[15], color); // Left Upper Leg to Left Lower Leg

                if (Map == "Farm" || Players.Contains(g))
                    RenderBone(bones[13], bones[17], color); // Hips to Right Upper Leg
                else if (Map == "Asylum")
                    RenderBone(bones[2], bones[17], color); // Spine to Right Upper Leg
                RenderBone(bones[17], bones[18], color); // Right Upper Leg to Right Lower Leg
                RenderBone(bones[18], bones[19], color); // Right Lower Leg to Right Foot
            }
        }
    }
}