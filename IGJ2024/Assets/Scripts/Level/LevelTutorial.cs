using UnityEngine;

public class LevelTutorial
{
    private const string Hint1Path = "Prefabs/Tutorial/Hint1";
    private const string Hint2Path = "Prefabs/Tutorial/Hint2";
    private readonly int _tutorialIndex;

    public LevelTutorial(int tutorialIndex)
    {
        _tutorialIndex = tutorialIndex;
        DisplayHints();
    }

    private void DisplayHints()
    {
        var path = GetHintPath(_tutorialIndex);
        var hints = Resources.Load(path);

        Object.Instantiate(hints, GameObject.FindGameObjectWithTag("Tutorial").transform.position, Quaternion.identity);
    }

    private string GetHintPath(int levelIndex)
    {
        switch (levelIndex)
        {
            case 0:
                return Hint1Path;
            case 1:
            default:
                return Hint2Path;

        }
    }
}
