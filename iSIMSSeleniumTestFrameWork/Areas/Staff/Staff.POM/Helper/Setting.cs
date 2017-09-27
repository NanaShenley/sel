using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Staff.POM.Helper
{
    public class Setting
    {
        private static int _objecWait = 60;
        private static int _elementWait = 30;

        public static int ObjectWait
        {
            get
            {
                return _objecWait;
            }
            set
            {
                _objecWait = value;
            }
        }

        public static int ElementWait
        {
            get
            {
                return _elementWait;
            }
            set
            {
                _elementWait = value;
            }
        }

    }
}
