using UnityEngine;



/// <summary>
/// シングルトン基底クラス(MonoBehaviour継承)
/// </summary>
public abstract class B_SingletonMonoBehaviour<T> : MonoBehaviour where T : B_SingletonMonoBehaviour<T>
{
    #region 定義
    [SerializeField, Header("シーンまたいでも削除しないか")]
    bool isDontDestroy;

    static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType<T>();
                if ((instance == null))
                {
                    //Debug.LogWarning(typeof(T) + "がありません");
                }
            }

            return instance;
        }
    }
    #endregion

    #region イベント関数
    protected virtual void Awake()
    {
        CheckInstance();
    }

    private void OnDestroy()
    {
        instance = null;
    }
    #endregion

    #region 非公開メソッド
    bool CheckInstance()
    {
        if (instance == null)
        {
            instance = (T)this;

            if (isDontDestroy)
                DontDestroyOnLoad(gameObject);

            return true;
        }
        else if (Instance == this)
        {
            return true;
        }

        Destroy(this.gameObject);

        return false;
    }
    #endregion
}



