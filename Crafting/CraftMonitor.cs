using System;
using CriticalCommonLib.Agents;
using CriticalCommonLib.Extensions;
using CriticalCommonLib.Services.Ui;
using Dalamud.Game;
using Dalamud.Logging;
using Lumina.Excel.GeneratedSheets;
using InventoryItem = FFXIVClientStructs.FFXIV.Client.Game.InventoryItem;

namespace CriticalCommonLib.Crafting
{
    public class CraftMonitor : IDisposable
    {
        private GameUiManager _gameUiManager;
        public CraftMonitor(GameUiManager gameUiManager)
        {
            _gameUiManager = gameUiManager;
            gameUiManager.UiVisibilityChanged += GameUiManagerOnUiVisibilityChanged;
            Service.Framework.Update += FrameworkOnUpdate;
        }
        
        public delegate void CraftStartedDelegate(uint itemId);
        public delegate void CraftFailedDelegate(uint itemId);
        public delegate void CraftCompletedDelegate(uint itemId, InventoryItem.ItemFlags flags, uint quantity);
        public event CraftStartedDelegate? CraftStarted;
        public event CraftFailedDelegate? CraftFailed;
        public event CraftCompletedDelegate? CraftCompleted;

        //For normal crafting
        private uint? _progressRequired;
        private uint? _progressMade;
        private bool? _completed = null;
        
        //For simple crafting
        private uint? _nqCompleted;
        private uint? _hqCompleted;
        private uint? _failed;

        //For both
        private uint? _currentItemId;

        private void FrameworkOnUpdate(Framework framework)
        {
            if (Agent != null && RecipeLevelTable != null)
            {
                if (_progressMade != Agent.Progress)
                {
                    _progressMade = (uint?) Agent.Progress;
                }
                if (_currentItemId != Agent.ResultItemId)
                {
                    _currentItemId = Agent.ResultItemId;
                }

                if (_progressRequired != RecipeLevelTable.ProgressRequired(CurrentRecipe))
                {
                    _progressRequired = RecipeLevelTable.ProgressRequired(CurrentRecipe);
                }

                if (_completed == null && _progressRequired != 0 && _progressMade == _progressRequired && _currentItemId != null)
                {
                    //Need to work out how to know if it was HQ output
                    CraftCompleted?.Invoke(_currentItemId.Value, InventoryItem.ItemFlags.HQ, 1);
                    _completed = true;
                }
            }
            if (SimpleAgent != null && RecipeLevelTable != null)
            {
                var simpleAgentNqCompleted = Math.Max(0,SimpleAgent.NqCompleted);
                var simpleAgentHqCompleted = Math.Max(0,SimpleAgent.HqCompleted);
                var simpleAgentFailed = Math.Max(0,SimpleAgent.TotalFailed);
                var itemId = SimpleAgent.ResultItemId;
                var finished = SimpleAgent.Finished;
                if (simpleAgentFailed >= 500)
                {
                    return;
                }
                if (_currentItemId != itemId)
                {
                    _currentItemId = itemId;
                }
                if (_nqCompleted != simpleAgentNqCompleted)
                {
                    if (_nqCompleted == null)
                    {
                        _nqCompleted = 0;
                    }
                    else
                    {
                        CraftCompleted?.Invoke(_currentItemId.Value,InventoryItem.ItemFlags.None, simpleAgentNqCompleted - _nqCompleted.Value);
                        _nqCompleted = simpleAgentNqCompleted;
                    }
                }
                if (_hqCompleted != simpleAgentHqCompleted)
                {
                    if (_hqCompleted == null)
                    {
                        _hqCompleted = 0;
                    }
                    else
                    {
                        CraftCompleted?.Invoke(_currentItemId.Value,InventoryItem.ItemFlags.HQ, simpleAgentHqCompleted - _hqCompleted.Value);
                        _hqCompleted = simpleAgentHqCompleted;
                    }
                }

                if (_failed != simpleAgentFailed)
                {
                    if (_failed == null)
                    {
                        _failed = 0;
                    }
                    else
                    {
                        CraftFailed?.Invoke(_currentItemId.Value);
                        _failed = simpleAgentFailed;
                    }
                }


                if (finished)
                {
                    _completed = true;
                }
            }
        }

        private void GameUiManagerOnUiVisibilityChanged(WindowName windowname, bool? windowstate)
        {
            if (windowname == WindowName.Synthesis || windowname == WindowName.SynthesisSimple)
            {
                _currentWindow = windowname;
                if (windowstate == true)
                {
                    WatchCraft();
                }
                else
                {
                    StopWatchingCraft();
                }
            }
        }

        public CraftingAgent? Agent => _agent;
        public SimpleCraftingAgent? SimpleAgent => _simpleCraftingAgent;

        public Recipe? CurrentRecipe => _currentRecipe;
        public RecipeLevelTable? RecipeLevelTable => _currentRecipeTable;

        private CraftingAgent? _agent;
        private SimpleCraftingAgent? _simpleCraftingAgent;

        private Recipe? _currentRecipe;
        private RecipeLevelTable? _currentRecipeTable;
        private WindowName? _currentWindow;

        private void WatchCraft()
        {
            if (_currentWindow == null || !_gameUiManager.IsWindowLoaded(_currentWindow.Value))
            {
                return;
            }

            if (_currentWindow == WindowName.Synthesis)
            {
                _agent = _gameUiManager.GetWindowAsPtr(_currentWindow.Value.ToString());
                var recipe = _agent.Recipe;
                _currentRecipe = Service.ExcelCache.GetRecipe(recipe);
                if (_currentRecipe != null)
                {
                    _currentRecipeTable = _currentRecipe.RecipeLevelTable.Value;
                }
                else
                {
                    PluginLog.Error("Could not find correct recipe for given synthesis. ");
                }

                CraftStarted?.Invoke(_agent.ResultItemId);
            }
            else
            {
                _simpleCraftingAgent = _gameUiManager.GetWindowAsPtr(_currentWindow.Value.ToString());
                var recipe = _simpleCraftingAgent.Recipe;
                _currentRecipe = Service.ExcelCache.GetRecipe(recipe);
                if (_currentRecipe != null)
                {
                    _currentRecipeTable = _currentRecipe.RecipeLevelTable.Value;
                }
                else
                {
                    PluginLog.Error("Could not find correct recipe for given synthesis. ");
                }

                CraftStarted?.Invoke(_simpleCraftingAgent.ResultItemId);
            }
        }

        private void StopWatchingCraft()
        {
            _currentRecipe = null;
            _currentRecipeTable = null;
            _agent = null;
            _simpleCraftingAgent = null;
            if (_currentWindow == WindowName.Synthesis)
            {
                if (_progressRequired != 0 && _progressMade != _progressRequired && _currentItemId != null)
                {
                    CraftFailed?.Invoke(_currentItemId.Value);
                    _progressMade = null;
                    _progressRequired = null;
                    _currentItemId = null;
                    _completed = null;
                }
                else
                {
                    _progressMade = null;
                    _progressRequired = null;
                    _currentItemId = null;
                    _completed = null;
                }
            }
            else if(_currentWindow == WindowName.SynthesisSimple)
            {
                _completed = null;
                _nqCompleted = null;
                _hqCompleted = null;
                _currentItemId = null;
                _failed = null;
            }

            _currentWindow = null;
        }

        private bool _disposed;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        
        private void Dispose(bool disposing)
        {
            if(!_disposed && disposing)
            {
                _gameUiManager.UiVisibilityChanged -= GameUiManagerOnUiVisibilityChanged;
                Service.Framework.Update -= FrameworkOnUpdate;
            }
            _disposed = true;         
        }
    }
}