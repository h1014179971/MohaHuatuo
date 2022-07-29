namespace MHSpace
{
    [System.Serializable]
    public class PowerInfo
    {
        public readonly PowerType type;
        public readonly int lv;
        public readonly float num;
        public readonly string price;
        public readonly int nameId;
    }

    [System.Serializable]
    public enum PowerType
    {
        none,
        power,//威力
        interval,//间隔
        gravity,//重力
        accuracy,//准确度
    }
}
