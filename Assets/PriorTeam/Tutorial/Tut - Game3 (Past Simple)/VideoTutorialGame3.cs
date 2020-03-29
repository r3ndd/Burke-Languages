using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.EventSystems;

public class VideoTutorialGame3 : MonoBehaviour
{
    // This allows us to access the video player
    VideoPlayer video;
    float VideoProgress = 0;
    int flag = 0;
    // Start is called before the first frame update
    void Start()
    {
        // We get access to the video here
        video = GetComponent<VideoPlayer>();

        // changing playbackspeed of video to 75% of original speed
        video.playbackSpeed = (float)0.75;
    }

    // Update is called once per frame
    void Update()
    {
        VideoProgress = (float)video.frame / (float)video.frameCount;
        Debug.Log(VideoProgress);

        


        // checks if video reaches 27.75% then pause.
        if (VideoProgress > (float)0.2775 && VideoProgress < (float)0.285)
        {
            if (flag != 1)
            {
                flag = 1;
                video.Pause();
            } 
            
        }
        // reset flag
        if (VideoProgress > (float)0.285 && VideoProgress < (float)0.3)
        {
            flag = 0;
            Debug.Log("Flag reset");
        }


        // checks if video reaches 42.75% then pause.
        if (VideoProgress > (float)0.4275 && VideoProgress < (float)0.4325)
        {
            if (flag != 1)
            {
                flag = 1;
                video.Pause();
            }
        }
        // reset flag
        if (VideoProgress > (float)0.4325 && VideoProgress < (float)0.445)
        {
            flag = 0;
            Debug.Log("Flag reset");
        }

        // checks if video reaches 59.5% then pause.
        if (VideoProgress > (float)0.595 && VideoProgress < (float)0.6)
        {
            if (flag != 1)
            {
                flag = 1;
                video.Pause();
            }
        }
        // reset flag
        if (VideoProgress > (float)0.6 && VideoProgress < (float)0.65)
        {
            flag = 0;
            Debug.Log("Flag reset");
        }

        // checks if video reaches 69.75% then pause.
        if (VideoProgress > (float)0.6975 && VideoProgress < (float)0.71)
        {
            if (flag != 1)
            {
                flag = 1;
                video.Pause();
            }
        }
        // reset flag
        if (VideoProgress > (float)0.71 && VideoProgress < (float)0.75)
        {
            flag = 0;
            Debug.Log("Flag reset");
        }

        // checks if video reaches 78.25% then pause.
        if (VideoProgress > (float)0.7825 && VideoProgress < (float)0.788)
        {
            if (flag != 1)
            {
                flag = 1;
                video.Pause();
            }
        }
        // reset flag
        if (VideoProgress > (float)0.788 && VideoProgress < (float)0.81)
        {
            flag = 0;
            Debug.Log("Flag reset");
        }



    }

    // This method gets called when the image renderer is clicked
    public void OnPointerDown(PointerEventData eventData)
    {
        if (video.isPlaying)
            video.Pause();
        else
            video.Play();
    }

    public void playVideoRightOption()
    {
        if (video.isPlaying)
            Debug.Log("Video is already playing.");
        else if ((VideoProgress > (float)0.2775 && VideoProgress < (float)0.285)
            || (VideoProgress > (float)0.4275 && VideoProgress < (float)0.4325)
            || (VideoProgress > (float)0.7825 && VideoProgress < (float)0.788))
        {
            video.Play();
            Debug.Log("Playing video Now.");

            // Change playback speed to original speed x 1.2 at beginning of round 3
            if ((VideoProgress > (float)0.4275 && VideoProgress < (float)0.4325))
            {
                video.playbackSpeed = (float)1.2;
            }
        }
        else
        {
            Debug.Log("Incorrect Option. Option Selected: Right");
        }
    }

    public void playVideoLeftOption()
    {
        if (video.isPlaying)
            Debug.Log("Video is already playing.");
        else if ((VideoProgress > (float)0.595 && VideoProgress < (float)0.6)
            || (VideoProgress > (float)0.6975 && VideoProgress < (float)0.71))
        {
            video.Play();
            Debug.Log("Playing video Now.");
        }
        else
        {
            Debug.Log("Incorrect Option. Option Selected: Leftt");
        }
    }

    public void TestMethod()
    {
        // Print something in the console
        Debug.Log("test");
    }

}
