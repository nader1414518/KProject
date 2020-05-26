using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

/* This class is responsible for managing music and vfx stuff */
public class music_manager : MonoBehaviour
{
    public AudioMixer audio_mixer;
    public AudioSource audio_source;
    public List<AudioClip> tracks;

    public Text play_pause_btn_text;
    public Text music_log_text;

    bool is_player_paused = false;

    int track_counter = 0;

    #region callbacks
    public void set_volume_level(float value)
    {
        if (audio_mixer)
        {
            audio_mixer.SetFloat("MusicVolume", Mathf.Log10(value) * 20);
        }
    }
    public void play_pause_btn_callback()
    {
        if (is_player_paused)
        {
            resume_track();
        }
        else
        {
            pause_track();
        }
    }
    public void next_track_btn_callback()
    {
        play_next();
        if (music_log_text)
        {
            music_log_text.text = audio_source.clip.name;
        }
    }
    public void previous_track_btn_callback()
    {
        play_previous();
        if (music_log_text)
        {
            music_log_text.text = audio_source.clip.name;
        }
    }
    #endregion

    #region Auxilaries
    void play_track(AudioClip clip)
    {
        if (audio_source)
        {
            audio_source.clip = clip;
            if (play_pause_btn_text)
            {
                play_pause_btn_text.text = "Pause";
            }
            audio_source.Play();
        }
    }
    void pause_track()
    {
        if (audio_source)
        {
            audio_source.Pause();
            if (play_pause_btn_text)
            {
                play_pause_btn_text.text = "Play";
            }
            is_player_paused = true;
        }
    }
    void resume_track()
    {
        if (audio_source)
        {
            audio_source.Play();
            if (play_pause_btn_text)
            {
                play_pause_btn_text.text = "Pause";
            }
            is_player_paused = false;
        }
    }
    void play_next()
    {
        //if (audio_source)
        //{
        //    if (current_index+1 >= tracks.Count)
        //    {
        //        audio_source.clip = tracks[0];
        //    }
        //    else
        //    {
        //        current_index++;
        //        audio_source.clip = tracks[current_index];
        //    }
        //    audio_source.Play();
        //}
        if (track_counter < tracks.Count - 1)
        {
            track_counter++;
        }
        else
        {
            track_counter = 0;
        }
        play_track(tracks[track_counter]);
    }
    void play_previous()
    {
        //if (audio_source)
        //{
        //    if (current_index - 1 <= 0)
        //    {
        //        audio_source.clip = tracks[tracks.Count];
        //    }
        //    else
        //    {
        //        current_index--;
        //        audio_source.clip = tracks[current_index];
        //    }
        //    audio_source.Play();
        //}
        if (track_counter > 0)
        {
            track_counter--;
        }
        else
        {
            track_counter = tracks.Count;
        }
        play_track(tracks[track_counter]);
    }
    void display_track_name(AudioClip clip)
    {
        if (music_log_text)
        {
            music_log_text.text = clip.name;
        }
    }
    #endregion

    void Start()
    {
        // set_volume_level(0.1f);
    }

    void Update()
    {
        // play music from the playlist 
        if (audio_source)
        {
            if (!(is_player_paused) && !(audio_source.isPlaying))
            {
                play_track(tracks[track_counter]);
                display_track_name(audio_source.clip);
                if (track_counter < tracks.Count - 1)
                {
                    track_counter++;
                }
                else
                {
                    track_counter = 0;
                    is_player_paused = true;
                }
            }
        }
    }
}
