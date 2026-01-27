using UnityEngine;

[System.Serializable]
public struct TweetData
{
    public string userName;
    [TextArea]
    public string message;
}
