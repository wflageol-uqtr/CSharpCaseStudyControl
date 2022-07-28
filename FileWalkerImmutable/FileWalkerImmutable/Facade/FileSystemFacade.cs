using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileWalkerImmutable
{
    public class FileSystemFacade
    {
        public Stack<string> NotificationLog { get; } = new();

        public IComponent CreateFolder(string name)
        {
            throw new NotImplementedException();
        }

        public IComponent CreateFile(string name, int size, string content)
        {
            throw new NotImplementedException();
        }

        public void AddChildren(IComponent root, params IComponent[] children)
        {
            throw new NotImplementedException();
        }

        public IComponent GetComponentByPath(IComponent root, params string[] componentNames)
        {
            throw new NotImplementedException();
        }

        public void Rename(IComponent component, string newName)
        {
            throw new NotImplementedException();
        }

        public void NotifyOnChange(IComponent component)
        {
            throw new NotImplementedException();
        }

        public void Delete(IComponent component)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IComponent> Collect(IComponent sourceComponent, bool includeFiles, bool includeFolders, bool recursive)
        {
            throw new NotImplementedException();
        }

        public void Undo()
        {
            throw new NotImplementedException();
        }

        public IComponent Duplicate(IComponent component)
        {
            throw new NotImplementedException();
        }
    }
}
