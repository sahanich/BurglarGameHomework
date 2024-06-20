﻿namespace BurglarGame
{
    public abstract class PinsController
    {
        protected IPinsModel _model;
        protected IPinsView _view;

        public PinsController(IPinsModel model, IPinsView view)
        {
            _model = model;
            _view = view;

            _view.Init(model.MinPossibleValue, model.MaxPossibleValue);
        }

        public abstract void SetStartState();
        public abstract void AddPinValues(int[] addingPinValues);

        public void RegisterListeners()
        {
            UnregisterListeners();
            if (_model != null)
            {
                _model.StateChanged += OnModelStateChanged;
            }
        }

        public void UnregisterListeners()
        {
            if (_model != null)
            {
                _model.StateChanged -= OnModelStateChanged;
            }
        }

        private void OnModelStateChanged()
        {
            _view.SetPinValues(_model.State);
        }
    }
}