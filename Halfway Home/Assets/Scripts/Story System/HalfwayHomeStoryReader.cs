﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stratus.Modules.InkModule;
using Stratus;
using System;

namespace HalfwayHome
{
  /// <summary>
  /// The class that interacts with ink, managing the stories
  /// </summary>
  public class HalfwayHomeStoryReader : StoryReader<RegexParser>
  {
    //------------------------------------------------------------------------/
    // Fields
    //------------------------------------------------------------------------/
    public string saveFile = "HalfwayHomeStoryStates";
    private StorySave save = new StorySave();

    //------------------------------------------------------------------------/
    // Messages
    //------------------------------------------------------------------------/
    protected override void OnBindExternalFunctions(Story story)
    {
      story.runtime.BindExternalFunction(nameof(PlayMusic), new System.Action<string>(PlayMusic));
      story.runtime.BindExternalFunction(nameof(CharEnter), new System.Action<string, string>(CharEnter));
      story.runtime.BindExternalFunction(nameof(CharExit), new System.Action<string>(CharExit));
      story.runtime.BindExternalFunction(nameof(SetValue), new System.Action<string, bool>(SetValue));
      story.runtime.BindExternalFunction(nameof(AddSocialPoints), new System.Action<string, string>(AddSocialPoints));
      story.runtime.BindExternalFunction(nameof(AlterWellbeing), new System.Action<string, int>(AlterWellbeing));
      story.runtime.BindExternalFunction(nameof(AddSocialTier), new System.Action<string>(AddSocialTier));
      story.runtime.BindExternalFunction(nameof(GetValue), (string valueName) => { GetValue(valueName); });
      story.runtime.BindExternalFunction(nameof(GetStringValue), (string valueName) => { GetStringValue(valueName); });
      story.runtime.BindExternalFunction(nameof(SetTimeBlock), new System.Action<int>(SetTimeBlock));
      story.runtime.BindExternalFunction(nameof(CallSleep), new System.Action(CallSleep));
    }

    protected override void OnLoad(Dictionary<string, Story> stories)
    {
      if (StorySave.Exists(saveFile))
      {
        save = StorySave.Load(saveFile);

        // From list to dictionary!
        foreach (var story in save.stories)
        {
          Trace.Script($"Loaded {story.name}");
          stories.Add(story.name, story);
        }

        Trace.Script("Loaded story states!");
      }
    }

    protected override void OnSave(Dictionary<string, Story> stories)
    {
      // From dictionary to list
      List<Story> storyList = new List<Story>();
      foreach (var story in stories)
        storyList.Add(story.Value);

      // Now save it!
      save.stories = storyList;
      StorySave.Save(save, saveFile);

      Trace.Script("Saved story states!");
    }

    protected override void OnClear()
    {
      StorySave.Delete(saveFile);
    }

    protected override void OnConfigureParser(RegexParser parser)
    {
      // @TODO: Change these to use groups
      parser.AddPattern("Speaker", RegexParser.Presets.insideSquareBrackets, RegexParser.Target.Line, RegexParser.Scope.Default);
      parser.AddPattern("Message", RegexParser.Presets.insideDoubleQuotes, RegexParser.Target.Line, RegexParser.Scope.Default);
      
      // Poses
      string posePattern = RegexParser.Presets.ComposeAssignment("Person", "Pose", "=");
      parser.AddPattern("Pose", posePattern, RegexParser.Target.Tag, RegexParser.Scope.Group);

      // Stat increment
      string incrementStatPattern = RegexParser.Presets.ComposeUnaryOperation("Stat", '+');
      parser.AddPattern("StatUp", incrementStatPattern, RegexParser.Target.Tag, RegexParser.Scope.Group);

      // Add your others here based using Compose...
    }

    protected override void OnStoryLoaded(Story story)
    {
      
    }

    //------------------------------------------------------------------------/
    // Story
    //------------------------------------------------------------------------/

    //------------------------------------------------------------------------/
    // External Methods
    //------------------------------------------------------------------------/
    public void PlayMusic(string name)
    {

      Scene.Dispatch<PlayMusicEvent>(new PlayMusicEvent() { track = name });
    }

    public void CharEnter(string name, string _pose)
    {
      //Scene.Dispatch<CharacterChangeEvent>(new CharacterChangeEvent() { character = name, entering = true });
      Space.DispatchEvent(Events.CharacterCall, new StageDirectionEvent(name, _pose));
      Trace.Script("called char enter");
    }

    public void CharExit(string name)
    {
      //Scene.Dispatch<CharacterChangeEvent>(new CharacterChangeEvent() { character = name, entering = false });
      Space.DispatchEvent(Events.CharacterExit, new StageDirectionEvent(name, "Calm"));
    }

    public void SetValue(string ValueName, bool newValue)
    {
      Game.current.Progress.SetValue(ValueName, newValue);
    }

    public bool GetValue(string ValueName)
    {
      return Game.current.Progress.GetBoolValue(ValueName);
    }

    public string GetStringValue(string ValueName)
    {
      return Game.current.Progress.GetStringValue(ValueName);
    }

    public void AlterWellbeing(string WellnessStat, int Value)
    {
      var stat = Personality.Wellbeing.delusion;

      if (WellnessStat == "Stress")
        stat = Personality.Wellbeing.stress;
      if (WellnessStat == "Fatigue")
        stat = Personality.Wellbeing.fatigue;

      Space.DispatchEvent(Events.AddStat, new ChangeStatEvent(Value, stat));
    }

    public void AddSocialPoints(string SocialStat, string Value)
    {
      var stat = Personality.Social.awareness;


      if (SocialStat == "Grace")
        stat = Personality.Social.grace;
      if (SocialStat == "Expression")
        stat = Personality.Social.expression;

      Space.DispatchEvent(Events.AddStat, new ChangeStatEvent(Value, stat));
    }

    public void AddSocialTier(string SocialStat)
    {
      if (SocialStat == "Awareness")
        Game.current.Self.IncrementSocialTier(Personality.Social.awareness);
      if (SocialStat == "Grace")
        Game.current.Self.IncrementSocialTier(Personality.Social.grace);
      if (SocialStat == "Expression")
        Game.current.Self.IncrementSocialTier(Personality.Social.expression);


      Space.DispatchEvent(Events.StatChange);
    }

    public void SetTimeBlock(int time)
    {
      Game.current.SetTimeBlock(time);

    }

    public void CallSleep()
    {
      Game.current.Slept();
    }

  }

}