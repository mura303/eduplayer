using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using System.IO;

public class NewBehaviourScript : MonoBehaviour {

    List<string> mURLs = new List<string>();
    VideoPlayer mPlayer;
    int mPlayingIndex;

	void Start () {

        var datapath = Application.dataPath;

        DirectoryInfo dir = new DirectoryInfo( datapath + "/movie");
        foreach ( DirectoryInfo d in dir.GetDirectories() ){
            FileInfo[] info = d.GetFiles("*.mp4");
            foreach (FileInfo f in info){
                mURLs.Add("file://" + f.FullName);
            } 
        } 


        mPlayer = gameObject.GetComponent<VideoPlayer>();

        mPlayingIndex = PlayerPrefs.GetInt("CurrentIndex", 0);
        if( mPlayingIndex >= mURLs.Count ){ // 動画をアプデしたときの数の修正
            mPlayingIndex = 0;
        }

        mPlayer.url = mURLs[mPlayingIndex];
        mPlayer.Play();
        mPlayer.time = PlayerPrefs.GetInt("CurrentSeconds", 0);

        InvokeRepeating("SavePeriodically", 1, 1);
        mPlayer.loopPointReached += EndReached;
	}

    void SavePeriodically()
    {
        PlayerPrefs.SetInt("CurrentIndex", mPlayingIndex);
        PlayerPrefs.SetInt("CurrentSeconds", (int) mPlayer.time );
        PlayerPrefs.Save();
    }

    void EndReached(UnityEngine.Video.VideoPlayer vp)
    {
        mPlayingIndex++;
        if (mPlayingIndex >= mURLs.Count)
        {
            mPlayingIndex = 0;
        }
        vp.url = mURLs[mPlayingIndex];
        vp.Play();
    }

	void Update () {		
    }
}
