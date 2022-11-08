using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileWalkerImmutable
{
    public class FileSystemFacade
    {
        private FileSystem currentFileSystem = new FileSystem();

        public Stack<string> NotificationLog { get; } = new();

        public IComponent CreateFolder(string name)
        {
            return new Folder(name);
        }

        public IComponent CreateFile(string name, int size, string content)
        {
            return new File(name, size, content);
        }

        public void AddChildren(IComponent root, params IComponent[] children)
        {
            currentFileSystem = currentFileSystem.AddList(root, children);
        }

        public IComponent GetComponentByPath(IComponent root, params string[] componentNames)
        {
            var currentComponent = root;
            foreach(string name in componentNames)
            {
                var children = currentFileSystem.Children(currentComponent);
                currentComponent = children.FirstOrDefault(c => c.Name == name);
            }

            return currentComponent;
        }

        public void Rename(IComponent component, string newName)
        {
            currentFileSystem = currentFileSystem.Rename(component, newName);
        }

        public void NotifyOnChange(IComponent component)
        {
            currentFileSystem = currentFileSystem.Attach(component, new FacadeNotificationObserver(this));
        }

        public void Delete(IComponent component)
        {
            currentFileSystem = currentFileSystem.Delete(component);
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
