using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public Text chapterTitle;
    public Button[] levelButtons;
    public Button prevChapterButton;
    public Button nextChapterButton;

    private int currentChapter = 1;
    private const int TotalChapters = 3;
    private const int LevelsPerChapter = 5;

    void Start()
    {
        UpdateLevelPicker();
    }

    void UpdateLevelPicker()
    {
        chapterTitle.text = "Chapter " + currentChapter;

        for (int i = 0; i < levelButtons.Length; i++)
        {
            int levelNumber = i + 1;
            levelButtons[i].GetComponentInChildren<Text>().text = "Level " + levelNumber;
            
            bool isUnlocked = IsLevelUnlocked(currentChapter, levelNumber);
            levelButtons[i].interactable = isUnlocked;

            // Visual feedback for locked levels
            Color buttonColor = isUnlocked ? Color.white : Color.gray;
            levelButtons[i].GetComponent<Image>().color = buttonColor;

            int chapterCopy = currentChapter;
            int levelCopy = levelNumber;
            levelButtons[i].onClick.RemoveAllListeners();
            levelButtons[i].onClick.AddListener(() => LoadLevel(chapterCopy, levelCopy));
        }

        prevChapterButton.interactable = (currentChapter > 1);
        nextChapterButton.interactable = (currentChapter < TotalChapters);
    }

    public void NextChapter()
    {
        if (currentChapter < TotalChapters)
        {
            currentChapter++;
            UpdateLevelPicker();
        }
    }

    public void PreviousChapter()
    {
        if (currentChapter > 1)
        {
            currentChapter--;
            UpdateLevelPicker();
        }
    }

    void LoadLevel(int chapter, int level)
    {
        Debug.Log("Loading Chapter " + chapter + ", Level " + level);
        // Implement your level loading logic here
        // SceneManager.LoadScene("Level_" + chapter + "_" + level);
    }

    bool IsLevelUnlocked(int chapter, int level)
    {
        if (level == 1) return true; // First level is always unlocked

        // Check if previous level is completed
        return PlayerPrefs.GetInt($"Chapter{chapter}Level{level-1}Completed", 0) == 1;
    }

    // Call this method when a level is completed
    public void SetLevelCompleted(int chapter, int level)
    {
        PlayerPrefs.SetInt($"Chapter{chapter}Level{level}Completed", 1);
        PlayerPrefs.Save();
    }

    // Optional: Method to reset all progress
    public void ResetAllProgress()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        UpdateLevelPicker();
    }
}