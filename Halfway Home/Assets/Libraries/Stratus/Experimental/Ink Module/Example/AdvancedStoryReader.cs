using System.Collections;
using System.Collections.Generic;
using Ink.Runtime;
using UnityEngine;
using System.Text.RegularExpressions;

namespace Stratus.Modules.InkModule
{
  public class AdvancedStoryReader : StoryReader<RegexParser>
  { 
    protected override void OnBindExternalFunctions(Story story)
    {
    }

    protected override void OnConfigureParser(RegexParser parser)
    {
      parser.AddPattern("Speaker", RegexParser.Presets.insideSquareBrackets, RegexParser.Target.Line, RegexParser.Scope.Default);
      parser.AddPattern("Message", RegexParser.Presets.insideDoubleQuotes, RegexParser.Target.Line, RegexParser.Scope.Default);

      // Variable = operand
      parser.AddPattern("Assignment", RegexParser.Presets.assignment, RegexParser.Target.Tag, RegexParser.Scope.Group, OnParse);
      // Variable++
      string incrementPattern = RegexParser.Presets.ComposeUnaryOperation("Variable", "Count", '+');
      parser.AddPattern("Increment", incrementPattern, RegexParser.Target.Tag, RegexParser.Scope.Group, OnParse);
      // Variable += value
      string incrementAssignPattern = RegexParser.Presets.ComposeBinaryOperation("Variable", "Value", "+=");
      parser.AddPattern("AddAssignment", incrementAssignPattern, RegexParser.Target.Tag, RegexParser.Scope.Group, OnParse);

    }

    void OnParse(Parse parse)
    {
      Trace.Script(parse.ToString());
    }
    

  }
}
