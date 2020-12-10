using System;

namespace TimeTracker
{
  public class SpeechSynthesizer
  {
    public int Rate { get; set; }
    public int Volume { get; set; }

    public void SpeakAsyncCancelAll() => throw new NotImplementedException();
    public void Speak(string msg) => throw new NotImplementedException();
    public void SpeakAsync(string msg) => throw new NotImplementedException();
    public void SelectVoiceByHints(object gender) => throw new NotImplementedException();
  }
}