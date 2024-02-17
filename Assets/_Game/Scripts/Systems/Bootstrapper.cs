// From this video (Project Initialization - Unity Architecture - Tarodev): https://www.youtube.com/watch?v=zJOxWmVveXU
// This script automatically preloads `Systems` from the `Resources` folder (which is a `don't destroy on load` singleton).
// I don't love doing it this way (seems a little over complex?), but I think Unity is just shit in this regard,
// and maybe this is just the least annoying way to do it.



// NOTE: I think I don't need this anymore, since I moved back to just having one scene?

// using UnityEngine;

// public static class Bootstrapper {
//     [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
//     public static void Execute() => Object.DontDestroyOnLoad(Object.Instantiate(Resources.Load("Systems")));
// }