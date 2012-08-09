using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace paranothing
{
    interface Saveable
    {
        string saveData();
        void reset();
    }
}
