using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileWalkerImmutable
{
    record Folder(string Name, Guid ID) : IFolder
    {
        public Folder(string name) : this(name, Guid.NewGuid()) { }
    }
}
