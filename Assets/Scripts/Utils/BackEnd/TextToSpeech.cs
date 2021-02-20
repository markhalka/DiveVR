using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;

public class TextToSpeech : MonoBehaviour
{
    // Start is called before the first frame update
    SpeechConfig config;
    SpeechSynthesizer synthesizer;


    void Start()
    {
        config = SpeechConfig.FromSubscription("b28be6e3166e4026810578159dd8c5af", "eastus");
        synthesizer = new SpeechSynthesizer(config);

        //SynthesizeAudioAsync("Hello world");
        
    }

    public async Task SynthesizeAudioAsync(string text)
    {
        await synthesizer.SpeakTextAsync(text);
    }

}
