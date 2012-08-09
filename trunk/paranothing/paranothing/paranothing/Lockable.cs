using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace paranothing
{
    interface Lockable
    {
        void lockObj(); // Can't use "lock" as the name of the method.
        void unlockObj();
        bool isLocked();
        void setKeyName(string keyName);
    }
}
