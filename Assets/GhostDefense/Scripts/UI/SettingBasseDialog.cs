using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UDEV.GhostDefense
{
    public class SettingBasseDialog : Dialog
    {
        public Slider musicSlider;
        public Slider soundSlider;

        public override void Show(bool isShow)
        {
            base.Show(isShow);

            if (musicSlider)
            {
                musicSlider.value = GameData.Ins.musicVol;
                AudioController.Ins.SetMusicVolume(GameData.Ins.musicVol);
            }

            if (soundSlider)
            {
                soundSlider.value = GameData.Ins.soundVol;
                AudioController.Ins.SetSoundVolume(GameData.Ins.soundVol);
            }
        }

        public void OnMusicChange(float value)
        {
            AudioController.Ins.SetMusicVolume(value);
        }

        public void OnSoundChange(float value)
        {
            AudioController.Ins.SetSoundVolume(value);
        }

        public void Save()
        {
            GameData.Ins.musicVol = AudioController.Ins.musicVolume;
            GameData.Ins.soundVol = AudioController.Ins.sfxVolume;
            GameData.Ins.SaveData();
            Close();
        }

    }

}