using System.IO;
using UnityEditor;
using UnityEngine;

namespace _Project._Scripts.Editor
{
    public class ClearGameProgress : MonoBehaviour
    {
        [MenuItem("Game/Clear game progress")]
        private static void ClearProgress()
        {
            if (EditorApplication.isPlaying || EditorApplication.isPaused)
            {
                return;
            }

            var path = $"{Application.persistentDataPath}";

            var di = new DirectoryInfo(path);

            foreach (var file in di.GetFiles())
            {
                file.Delete(); 
            }

            foreach (var dir in di.GetDirectories())
            {
                dir.Delete(true); 
            }
        }
    }
}

