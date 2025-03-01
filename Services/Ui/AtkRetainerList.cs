using System.Collections.Generic;
using System.Numerics;
using Dalamud.Interface.Colors;
using Dalamud.Logging;
using FFXIVClientStructs.FFXIV.Client.Game;
using FFXIVClientStructs.FFXIV.Component.GUI;

namespace CriticalCommonLib.Services.Ui
{
    public abstract unsafe class AtkRetainerList : AtkOverlay
    {
        public readonly uint ListComponent = 27;
        public readonly uint RetainerNameText = 3;
        public override WindowName WindowName { get; set; } = WindowName.RetainerList;
        public void SetName(ulong retainerId, string newName, Vector4? newColour)
        {
            var atkBaseWrapper = AtkUnitBase;
            if (atkBaseWrapper == null || atkBaseWrapper.AtkUnitBase == null) return;
            var atkUnitBase = atkBaseWrapper.AtkUnitBase;
            var listNode = (AtkComponentNode*)atkUnitBase->GetNodeById(ListComponent);
            if (listNode == null || (ushort)listNode->AtkResNode.Type < 1000) return;
            var retainerManager = RetainerManager.Instance();
            if (retainerManager != null)
            {
                for (uint i = 0; i < retainerManager->RetainerCount; i++)
                {
                    var retainer = retainerManager->GetRetainerBySortedIndex(i);
                    if (retainer != null && retainer->RetainerID == retainerId)
                    {
                        var renderer = GameUiManager.GetNodeByID<AtkComponentNode>(listNode->Component->UldManager,
                            i == 0 ? 4U : 41000U + i);
                        if (renderer == null || !renderer->AtkResNode.IsVisible) continue;
                        var retainerText =
                            (AtkTextNode*) renderer->Component->UldManager.SearchNodeById(RetainerNameText);
                        if (retainerText != null)
                        {
                            if (newColour.HasValue)
                            {
                                retainerText->TextColor = Utils.ColorFromVector4(newColour.Value);
                            }

                            retainerText->SetText(newName);
                        }

                        break;
                    }
                }
            }
        }
        
        public void SetNames(Dictionary<ulong, string> newNames,Dictionary<ulong, Vector4> newColours)
        {
            var atkBaseWrapper = AtkUnitBase;
            if (atkBaseWrapper == null) return;
            var atkUnitBase = atkBaseWrapper.AtkUnitBase;
            var listNode = (AtkComponentNode*)atkUnitBase->GetNodeById(ListComponent);
            if (listNode == null || (ushort) listNode->AtkResNode.Type < 1000)
            {
                PluginLog.Verbose("Couldn't find list node within retainer list.");
                return;
            };
            var retainerManager = RetainerManager.Instance();
            if (retainerManager != null)
            {
                var retainerCount = 10;
                for (uint i = 0; i < retainerCount; i++)
                {
                    var retainer = retainerManager->GetRetainerBySortedIndex(i);
                    if (retainer != null)
                    {
                        if (newNames.ContainsKey(retainer->RetainerID))
                        {
                            var renderer = GameUiManager.GetNodeByID<AtkComponentNode>(listNode->Component->UldManager,
                                i == 0 ? 4U : 41000U + i);
                            if (renderer == null || !renderer->AtkResNode.IsVisible) continue;
                            var retainerText =
                                (AtkTextNode*) renderer->Component->UldManager.SearchNodeById(RetainerNameText);
                            if (retainerText != null)
                            {
                                retainerText->SetText(newNames[retainer->RetainerID]);
                                if (newColours.ContainsKey(retainer->RetainerID))
                                {
                                    retainerText->TextColor = Utils.ColorFromVector4(newColours[retainer->RetainerID]);
                                }
                                else
                                {
                                    retainerText->TextColor =  Utils.ColorFromVector4(ImGuiColors.DalamudWhite);
                                }
                            }
                            else
                            {
                                PluginLog.Verbose("Couldn't find retainer text node.");
                            }
                        }
                        else
                        {
                            var renderer = GameUiManager.GetNodeByID<AtkComponentNode>(listNode->Component->UldManager,
                                i == 0 ? 4U : 41000U + i, (NodeType) 1011);
                            if (renderer == null || !renderer->AtkResNode.IsVisible) continue;
                            var retainerText =
                                (AtkTextNode*) renderer->Component->UldManager.SearchNodeById(RetainerNameText);
                            if (retainerText != null)
                            {
                                retainerText->SetText(retainer->Name);
                                if (newColours.ContainsKey(retainer->RetainerID))
                                {
                                    retainerText->TextColor = Utils.ColorFromVector4(newColours[retainer->RetainerID]);
                                }
                                else
                                {
                                    retainerText->TextColor =  Utils.ColorFromVector4(ImGuiColors.DalamudWhite);
                                }
                            }
                            else
                            {
                                PluginLog.Verbose("Couldn't find retainer text node.");
                            }
                        }
                    }
                    else
                    {
                        PluginLog.Verbose("Couldn't retrieve retainer by sorted index.");
                    }
                }
            }
            else
            {
                PluginLog.Verbose("Couldn't retrieve retainer manager.");
            }
        }
    }
}