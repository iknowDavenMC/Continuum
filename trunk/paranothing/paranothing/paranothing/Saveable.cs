using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace paranothing
{
    public interface Saveable
    {
        string saveData();
        void reset();
    }
}
