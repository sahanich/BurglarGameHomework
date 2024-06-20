namespace BurglarGame
{
    public sealed class OneTargetValuePinsModel : PinsModel
    {
        public OneTargetValuePinsModel(int pinCount, int minPossibleValue, int maxPossibleValue,
            int targetPinValue) : base(pinCount, minPossibleValue, maxPossibleValue)
        {
            for(int i = 0; i < TargetState.Length; i++)
            {
                TargetState[i] = targetPinValue;
            }
        }
    }
}