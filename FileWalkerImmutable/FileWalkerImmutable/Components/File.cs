using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileWalkerImmutable
{
    record File(string Name, Guid ID, int Size, string Content) : IFile
    {
        public File(string name, int size, string content) : this(name, Guid.NewGuid(), size, content) { }
    }
}
