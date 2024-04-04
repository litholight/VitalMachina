using System.Collections.Generic;

namespace Mario.Common.Scenes
{
    public class SceneManager
    {
        private readonly Dictionary<string, Scene> scenes = new Dictionary<string, Scene>();
        public Scene CurrentScene { get; private set; }

        public void AddScene(string name, Scene scene)
        {
            scenes[name] = scene;
        }

        public bool SwitchToScene(string name)
        {
            if (scenes.TryGetValue(name, out var scene))
            {
                CurrentScene = scene;
                return true; // Successfully switched scenes
            }
            return false; // Scene not found
        }

        // Consider adding functionality to remove scenes, check for the existence of a scene, etc.
    }
}
