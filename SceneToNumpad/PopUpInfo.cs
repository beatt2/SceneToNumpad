namespace Plugins.SceneToNumpad
{
    public class PopUpInfo
    {
        private readonly string[] _contents0;
        private readonly string[] _contents1;
        private readonly string[] _contents2;
        private readonly string[][] _contents;

        private const string Ctrl = "CTRL";
        private const string Alt = "ALT";


        public PopUpInfo()
        {
            _contents0 = new[] {Ctrl, Alt};
            _contents1 = new[] {Ctrl};
            _contents2 = new[] {Alt};

            _contents = new string[3][];
            _contents[0] = _contents0;
            _contents[1] = _contents1;
            _contents[2] = _contents2;
        }
        private int _calcIndexTwo;


        //it used to be modular I promise
        private int CalcIndexTwo(int contentIndex)
        {
            switch (contentIndex)
            {
                case 0:_calcIndexTwo = 2; break;
                case 1:_calcIndexTwo = 1; break;
            }
            return _calcIndexTwo;
        }


        public string[] ArrayGetTwo(int contentIndex)
        {
            return _contents[CalcIndexTwo(contentIndex)];
        }

        public string GetStringOne(int contentIndex)
        {
            return _contents0[contentIndex];
        }

        public string GetStringTwo(int contentIndex)
        {
            return ArrayGetTwo(contentIndex)[contentIndex];
        }

        public string[] ArrayOne
        {
            get { return _contents0; }
        }
    }
}
