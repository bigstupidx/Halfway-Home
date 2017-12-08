/******************************************************************************/
/*!
@file   BehaviorTreeNodeEditor.cs
@author Christian Sagel
@par    email: ckpsm@live.com
All content � 2017 DigiPen (USA) Corporation, all rights reserved.
*/
/******************************************************************************/
using UnityEngine;
using UnityEditor;
using System;
using Stratus.Editors;

namespace Stratus
{
  namespace AI
  {
    public class BehaviorTreeNodeEditor : NodeBasedEditor<BehaviorTreeNode> 
    {
      //----------------------------------------------------------------------/
      // Interface
      //----------------------------------------------------------------------/
      protected override void OnContextMenu(GenericMenu menu, Vector2 mousePos)
      {
        menu.AddItem(new GUIContent("Add Simple"), false, () => AddNode(mousePos, OnAddSimple));
        menu.AddItem(new GUIContent("Add Composite"), false, ()=> AddNode(mousePos, OnAddComposite));
      }

      protected override void OnMultiSelectContextMenu(GenericMenu menu, Vector2 mousePos)
      {
        
      }

      //----------------------------------------------------------------------/
      // Nodes
      //----------------------------------------------------------------------/
      void OnAddSimple(BehaviorTreeNode node)
      {
        node.Name = "Simple";
        var texture = Resources.Load("gamepad") as Texture2D;
        node.AddContent(new Node.ContentElement(node.Name, "A simple node that likes you very much", texture, null));
        //node.AddContent(new Node.ContentElement(node.Name, "A simple node that likes you very much", texture, null));
      }

      void OnAddComposite(BehaviorTreeNode node)
      {
        node.Name = "Composite";
        //node.OutPoint.Enabled = true;
        node.AddContent(new Node.ContentElement(node.Name, "A composite node", null, null));
        AddDecorator(node, null);
      }

      void AddDecorator(BehaviorTreeNode node, Decorator decorator)
      {
        node.AddContent(new Node.ContentElement("Decorator", "One\nTwo\nThree", null, Woof));
      }

      void Woof()
      {
        Trace.Script("Woof!");
      }




    }

  } 
}