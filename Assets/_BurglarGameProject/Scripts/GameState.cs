using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

namespace BurglarGame
{

    public interface IGameModel
    {
        public event Action StateChanged;
        public bool IsWin { get; }
        public bool IsLose { get; }
        public int[] GetState();
        public void SetState(int[] pinValues);
        public void SetTargetState(int[] targetPinValues);
    }

    public interface IBurglarGameModel
    {
        public IReadOnlyReactiveProperty<int[]> PinValues { get; }
        public IReadOnlyReactiveProperty<int> SecondsToLoseLeft { get; }
        public IReadOnlyReactiveProperty<bool> IsWin { get; }
        public IReadOnlyReactiveProperty<bool> IsLose { get; }
        public void SetPinValues(int[] pinValues);
        public void SetSecondsToLoseLeft(int secondsToLoseLeft);
    }

    public sealed class BurglarGameModel : IBurglarGameModel
    {
        private ReactiveProperty<int[]> _pinValues;
        private ReactiveProperty<int> _secondsToLoseLeft;
        private ReactiveProperty<bool> _isWin;
        private ReactiveProperty<bool> _isLose;

        public IReadOnlyReactiveProperty<int[]> PinValues => _pinValues;

        public IReadOnlyReactiveProperty<int> SecondsToLoseLeft => _secondsToLoseLeft;

        public IReadOnlyReactiveProperty<bool> IsWin => _isWin;

        public IReadOnlyReactiveProperty<bool> IsLose => _isLose;

        public BurglarGameModel()
        {
            _pinValues = new ReactiveProperty<int[]>
            {
                Value = new int[0]
            };
            _secondsToLoseLeft = new ReactiveProperty<int>();
            _isWin.Value = false;
            _isLose.Value = false;
        }

        public void SetPinValues(int[] pinValues)
        {
            _pinValues.Value = (int[])pinValues.Clone();
            if (_pinValues.Value.Count(x => x == 5) == _pinValues.Value.Length)
            {
                _isWin.Value = true;
            }
        }

        public void SetSecondsToLoseLeft(int secondsToLoseLeft)
        {
            _secondsToLoseLeft.Value = secondsToLoseLeft;
            if (secondsToLoseLeft <= 0)
            {
                _isLose.Value = true;
            }
        }
    }

    public sealed class BurglarGameView : MonoBehaviour
    {
        [SerializeField]
        private PinSetView PinSetView;
        [SerializeField]
        private ToolSetView ToolSetView;
        [SerializeField]
        private GameOverView GameOverView;
        [SerializeField]
        private GameTimerView GameTimerView;
        [SerializeField]
        private SecondsToLoseView SecondsToLoseView;

        public void Init(IBurglarGameModel model, IBurglarGameModel model1)
        {
            //_model = model; 
            //_model.PinValues
            //    .ObserveEveryValueChanged(x => x.Value)
            //    .Subscribe(OnModelChanged)
            //    .AddTo(_disposables);
        }

        public void Deinit()
        {

        }

        private void OnModelChanged(int[] newPinValues)
        {
            throw new NotImplementedException();
        }
    }

    public abstract class BurglarGameViewModel
    {
        protected IBurglarGameModel _model;

        protected BurglarGameViewModel()
        {
            //_model.PinValues.ObserveReplace().Subscribe().AddTo(this);
        }

        private List<IDisposable> _disposables = new List<IDisposable>();

        public void Init(IBurglarGameModel model)
        {
            _model = model;
            _model.PinValues
                .ObserveEveryValueChanged(x => x.Value)
                .Subscribe(OnModelChanged)
                .AddTo(_disposables);
        }

        public void Dispose()
        {
            foreach (var disposable in _disposables)
            {
                disposable.Dispose();
            }
            _disposables.Clear();
        }

        private void OnModelChanged(int[] newPinValues)
        {
            throw new NotImplementedException();
        }
    }

    public class GameState
    {
        private int[] _pinValues;
        private int _secondsToLoseLeft;
        private ToolInfo[] _toolInfos;

        public int[] PinValues => _pinValues;
        public int SecondsToLoseLeft => _secondsToLoseLeft;
        public ToolInfo[] ToolInfos => _toolInfos;

        public void SetPinValues(int[] pinValues)
        {
            _pinValues = (int[])pinValues.Clone();
        }

        public void SetSecondsToLoseLeft(int secondsToLoseLeft)
        {
            _secondsToLoseLeft = secondsToLoseLeft;
        }

        public void SetToolInfos(ToolInfo[] toolInfos)
        {
            _toolInfos = toolInfos;
        }
    }
}