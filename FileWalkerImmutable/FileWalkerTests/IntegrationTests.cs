using System.Linq;
using FileWalkerImmutable;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FileWalkerTests
{
    [TestClass]
    public class IntegrationTests
    {
        FileSystemFacade facade = new();
        IComponent root;

        [TestInitialize]
        public void Setup()
        {
            root = facade.CreateFolder("Root");
            var file1 = facade.CreateFile("File1", 100, "Content of File 1.");
            var file2 = facade.CreateFile("File2", 200, "Content of File 2.");
            var file3 = facade.CreateFile("File3", 300, "Content of File 3.");
            var file4 = facade.CreateFile("File4", 400, "Content of File 4.");
            var folder1 = facade.CreateFolder("Folder1");
            var folder2 = facade.CreateFolder("Folder2");

            facade.AddChildren(root, file1, folder1);
            facade.AddChildren(folder1, file2, file3, file4, folder2);
        }

        [TestMethod]
        public void TestGetFileByPath()
        {
            // Should return a component by looking it up from the names in the hierarchy.
            var file = (IFile)facade.GetComponentByPath(root, "Folder1", "File2");
            Assert.AreEqual(file.Size, 200);
        }

        [TestMethod]
        public void TestGetFileByPathNotFound()
        {
            // A wrong path or not existing file should return null.
            var file = facade.GetComponentByPath(root, "Folder1", "File1");
            Assert.IsNull(file);
        }

        [TestMethod]
        public void TestRename()
        {
            // Pick a file and rename it.
            var originalFile = facade.GetComponentByPath(root, "File1");
            facade.Rename(originalFile, "RenamedFile1");

            // Check that original file object was not changed, but does not exist anymore in the hierarchy.
            Assert.AreEqual(originalFile.Name, "File1");
            originalFile = facade.GetComponentByPath(root, "File1");
            Assert.IsNull(originalFile);

            // Check that there's a file with the new name in its place.
            var renamedFile = (IFile)facade.GetComponentByPath(root, "RenamedFile1");
            Assert.AreEqual(renamedFile.Name, "RenamedFile1");
            Assert.AreEqual(renamedFile.Size, 100);
            Assert.AreEqual(renamedFile.Content, "Content of File 1.");
        }

        [TestMethod]
        public void TestRenameNotify()
        {
            // Pick a file, observe it, and then rename it.
            var file = facade.GetComponentByPath(root, "File1");
            facade.NotifyOnChange(file);
            facade.Rename(file, "Temp");

            // Check that the rename has been logged.
            Assert.AreEqual("File1 was renamed to Temp.", facade.NotificationLog.Peek());

            // Rename it back and check that the rename has been logged again.
            file = facade.GetComponentByPath(root, "Temp");
            facade.Rename(file, "File1");
            Assert.AreEqual("Temp was renamed to File1.", facade.NotificationLog.Peek());
        }

        [TestMethod]
        public void TestDelete()
        {
            // Pick a file and delete it.
            var file = facade.GetComponentByPath(root, "File1");
            facade.Delete(file);

            // Check that the file no longer exists.
            file = facade.GetComponentByPath(root, "File1");
            Assert.IsNull(file);
        }

        [TestMethod]
        public void TestDeleteNotify()
        {
            // Observe a folder and a file inside it, then delete the folder.
            var folder = facade.GetComponentByPath(root, "Folder1");
            var file = facade.GetComponentByPath(folder, "File2");
            facade.NotifyOnChange(folder);
            facade.NotifyOnChange(file);
            facade.Delete(folder);

            // Check that both nofications were logged.
            Assert.AreEqual(facade.NotificationLog.Pop(), "File2 was deleted.");
            Assert.AreEqual(facade.NotificationLog.Pop(), "Folder1 was deleted.");
        }

        [TestMethod]
        public void TestCollectEverything()
        {
            // Collect everything under root and count it.
            var collection = facade.Collect(root, true, true, true);
            Assert.AreEqual(6, collection.Count());
        }

        [TestMethod]
        public void TestCollectFiles()
        {
            // Collect only files under root and count them.
            var collection = facade.Collect(root, true, false, true);
            Assert.AreEqual(4, collection.Count());
        }

        [TestMethod]
        public void TestCollectFolders()
        {
            // Collect only folders under root and count them.
            var collection = facade.Collect(root, false, true, true);
            Assert.AreEqual(2, collection.Count());
        }

        [TestMethod]
        public void TestCollectNoRecurse()
        {
            // Collect the content of root non-recursively.
            var collection = facade.Collect(root, true, true, false);
            Assert.AreEqual(2, collection.Count());
        }

        [TestMethod]
        public void TestUndoRename()
        {
            // Rename a file and undo it.
            var file = facade.GetComponentByPath(root, "File1");
            facade.Rename(file, "RenamedFile1");
            facade.Undo();

            // Check that the name reverted back.
            file = facade.GetComponentByPath(root, "File1");
            Assert.AreEqual(file.Name, "File1");
        }

        [TestMethod]
        public void TestUndoDelete()
        {
            // Delete a folder and undo it.
            var folder = facade.GetComponentByPath(root, "Folder1");
            facade.Delete(folder);
            facade.Undo();

            // Check that the folder and its contents are still there.
            folder = facade.GetComponentByPath(root, "Folder1");
            var file2 = facade.GetComponentByPath(folder, "File2");
            var file3 = facade.GetComponentByPath(folder, "File3");

            Assert.IsNotNull(folder);
            Assert.IsNotNull(file2);
            Assert.IsNotNull(file3);
        }

        [TestMethod]
        public void TestDuplicate()
        {
            // Duplicate root and verify that the content of the new root are the same, but different objects.
            var newRoot = facade.Duplicate(root);

            var oldCollection = facade.Collect(root, true, true, true);
            var newCollection = facade.Collect(newRoot, true, true, true);

            Assert.AreEqual(oldCollection.Count(), newCollection.Count());
            for(int i = 0; i < oldCollection.Count(); i++)
            {
                var oldComponent = oldCollection.ElementAt(i);
                var newComponent = newCollection.ElementAt(i);

                Assert.AreEqual(oldComponent.Name, newComponent.Name);
                if(oldComponent is IFile)
                {
                    Assert.AreEqual((oldComponent as IFile).Size, (newComponent as IFile).Size);
                    Assert.AreEqual((oldComponent as IFile).Content, (newComponent as IFile).Content);
                }
                Assert.AreNotSame(oldComponent, newComponent);
            }
        }
    }
}
